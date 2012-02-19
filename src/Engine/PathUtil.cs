using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace JSCombiner.Engine {

    public class PathUtil {

        public static string GetFullPath (string originFile, string referencedFile, string includePath) {
            if (referencedFile.StartsWith("<") && referencedFile.EndsWith(">")) {
                DirectoryInfo dirInfo = new DirectoryInfo(includePath);
                string path = Path.Combine(dirInfo.FullName, referencedFile.TrimStart('<').TrimEnd('>'));
                return Path.GetFullPath(path);
            } else
            if (referencedFile.StartsWith("\"") && referencedFile.EndsWith("\"")) {
                FileInfo fileInfo = new FileInfo(originFile);
                string path = Path.Combine(fileInfo.Directory.FullName, referencedFile.Trim('\"'));
                return Path.GetFullPath(path);
            } else {
                throw new Exception("Invalid reference file pattern");
            }
        }

        public static void CrawlDirectory (string directory, string filePattern, List<string> output) {
            DirectoryInfo dirInfo = new DirectoryInfo(directory);
            if (dirInfo.Exists) {
                DirectoryInfo[] subDirList = dirInfo.GetDirectories();
                foreach (DirectoryInfo subDir in subDirList) {
                    CrawlDirectory(subDir.FullName, filePattern, output);
                }
                FileInfo[] fileInfoList = dirInfo.GetFiles(filePattern);
                foreach (FileInfo fileInfo in fileInfoList) {
                    output.Add(fileInfo.FullName);
                }
            } else {
                throw new Exception(string.Format("Directory '{0}' does not exist.",directory));
            }
        }

        public static List<string> CheckAndConvertDirectoryToFiles (List<string> input, string filePattern) {
            List<string> output = new List<string>();
            foreach (string entry in input) {
                if ((File.GetAttributes(entry) & FileAttributes.Directory) == FileAttributes.Directory) {
                    PathUtil.CrawlDirectory(entry, filePattern, output);
                } else
                    if (File.Exists(entry)) {
                        FileInfo info = new FileInfo(entry);
                        output.Add(info.FullName);
                    }
            }
            return output;
        }

        public static List<string> GrabAllReferenced (string file, string referencePrefix) {
            List<string> output = new List<string>();
            StreamReader reader = null;
            try {
                reader = new StreamReader(file);
                while (reader.Peek() != -1) {
                    string textLine = reader.ReadLine();
                    if (textLine.StartsWith(referencePrefix)) {
                        string referenced = textLine.Substring(referencePrefix.Length).Trim();
                        output.Add(referenced);
                    } else {
                        break;
                    }
                }
            }
            finally {
                reader.Close();
            }
            return output;
        }
    
    }

}
