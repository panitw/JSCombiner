using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace JSCombiner.Engine {

    public class CombinerEngine {

        private class FileEntry : IComparable {
            public int Weight = 0;
            public string Path = null;
            public List<FileEntry> ReferTo = new List<FileEntry>();

            public int CompareTo (object obj) {
                FileEntry theOther = (FileEntry)obj;
                return (theOther.Weight > this.Weight) ? 1 : (theOther.Weight == this.Weight) ? 0 : -1;
            }
        }

        public static string Execute (EngineParameter param) {

            Dictionary<string, FileEntry> _entryTable = new Dictionary<string, FileEntry>();
            List<string> allInputFiles = PathUtil.CheckAndConvertDirectoryToFiles(param.InputFiles,param.FilePattern);

            //Create Graph
            foreach (string inputFile in allInputFiles) {
                CreateReferenceGraph(inputFile, param, null, _entryTable);
            }

            //Loop Reference Check
            string foundLoopAt = null;
            Dictionary<string,int> traverseBag = new Dictionary<string,int>();
            foreach (string inputFile in allInputFiles) {
                foundLoopAt = CheckLoopReference(inputFile, _entryTable, null, traverseBag);
                if (foundLoopAt != null) {
                    break;
                }
                traverseBag.Clear();
            }
            if (foundLoopAt != null) {
                throw new Exception("Found Loop Reference at " + foundLoopAt);
            }

            //Analyze Link By Adding Weight
            foreach (string inputFile in allInputFiles) {
                CalculateWeight(inputFile, 1, _entryTable);
            }

            //Sort File Entry
            List<FileEntry> allEntry = new List<FileEntry>(_entryTable.Values);
            allEntry.Sort();

            //Generate Output
            StringBuilder outputBuilder = new StringBuilder();
            foreach (FileEntry entry in allEntry) {
                string content = File.ReadAllText(entry.Path);
                outputBuilder.AppendLine(content);
            }

            return outputBuilder.ToString();
        }

        private static void CreateReferenceGraph (string file, EngineParameter param, FileEntry referer, Dictionary<string, FileEntry> entryTable) {
            
            if (entryTable.ContainsKey(file)) {
                if (referer != null) {
                    FileEntry referencedEntry = entryTable[file];
                    referer.ReferTo.Add(referencedEntry);
                }
            } else {
                FileEntry entry = new FileEntry();
                entry.Path = file;
                entryTable[file] = entry;

                if (referer != null) {
                    referer.ReferTo.Add(entry);
                }

                List<string> allReferenced = PathUtil.GrabAllReferenced(file, param.ReferencePrefix);
                foreach (string aRef in allReferenced) {
                    string aRefFullPath = PathUtil.GetFullPath(file, aRef, param.IncludePath);
                    CreateReferenceGraph(aRefFullPath, param, entry, entryTable);
                }
            }
        }

        private static string CheckLoopReference (string file, Dictionary<string, FileEntry> entryTable, string referencedBy, Dictionary<string,int> traverseBag) {
            if (entryTable.ContainsKey(file)) {
                FileEntry entry = entryTable[file];
                if (traverseBag.ContainsKey(entry.Path)) {
                    return referencedBy + " --> " + file;
                } else {
                    traverseBag[entry.Path] = 1;
                    foreach (FileEntry referTo in entry.ReferTo) {
                        string foundLoopAt = CheckLoopReference(referTo.Path, entryTable, entry.Path, traverseBag);
                        if (foundLoopAt != null) {
                            return foundLoopAt;
                        }
                    }
                    traverseBag.Remove(entry.Path);
                    return null;
                }
            } else {
                return null;
            }
        }

        private static void CalculateWeight (string file, int level, Dictionary<string, FileEntry> entryTable) {
            if (entryTable.ContainsKey(file)) {
                FileEntry entry = entryTable[file];
                entry.Weight += level;
                foreach (FileEntry referTo in entry.ReferTo) {
                    CalculateWeight(referTo.Path, (level+1), entryTable);
                }
            }
        }
    }

}
