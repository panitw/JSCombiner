using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace JSCombiner.Engine {

    public class CombinerEngine {

        private class FileEntry {
            public int ReferencedCount = 0;
            public string Path = null;
            public List<FileEntry> Referenced = new List<FileEntry>();
        }

        public static string Execute (EngineParameter param) {

            Dictionary<string, FileEntry> _entryTable = new Dictionary<string, FileEntry>();
            List<string> allInputFiles = PathUtil.CheckAndConvertDirectoryToFiles(param.InputFiles,param.FilePattern);

            //Create Graph
            foreach (string inputFile in allInputFiles) {
                CreateReferenceGraph(inputFile, _entryTable);
            }

            //Analyze Graph

            //Sort File Entry

            //Generate Output

            return null;
        }

        private static void CreateReferenceGraph (string file, Dictionary<string, FileEntry> entryTable) {

        }
    }

}
