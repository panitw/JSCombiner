using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JSCombiner.Engine;
using System.IO;

namespace JSCombiner {
    class Program {

        static void Main (string[] args) {

            EngineParameter param = null;

            //Check parameters
            if (args.Length == 0 || args[0] == "-?") {
                PrintUsage();
            } else {
                try {
                    param = ParseParameter(args);
                    param.WorkingPath = Environment.CurrentDirectory;
                }
                catch (Exception ex) {
                    PrintError(ex.Message);
                }
            }

            //Pass all parameter to the combiner engine
            if (param != null) {
                try {
                    string output = CombinerEngine.Execute(param);
                    File.WriteAllText(param.OutputFile, output);
                }
                catch (Exception ex) {
                    PrintError(ex.Message);
                }
            }

        }

        public static EngineParameter ParseParameter (string[] args) {

            string includePath = null;
            string outputFile = null;
            List<string> inputFileList = new List<string>();
            
            for (int i = 0; i < args.Length; i++) {
                if (args[i] == "-i") {
                    i++;
                    if (args.Length > i) {
                        includePath = args[i];
                    } else {
                        throw new Exception("Include path is not specified with switch -i");
                    }
                } else
                if (args[i] == "-o") {
                    i++;
                    if (args.Length > i) {
                        outputFile = args[i];
                    } else {
                        throw new Exception("Output file is not specified with switch -o");
                    }
                }  else
                if (args[i] == "-p") {
                    i++;
                    if (args.Length > i) {
                        outputFile = args[i];
                    } else {
                        throw new Exception("Source File scan pattern is not specified with switch -p");
                    }
                } else {
                    for (int j = i; j < args.Length; j++) {
                        inputFileList.Add(args[j]);
                    }
                }
            }

            if (outputFile == null) {
                throw new Exception("No output file specified.");
            }

            if (inputFileList.Count == 0) {
                throw new Exception("No input file specified.");
            }

            EngineParameter param = new EngineParameter();
            param.IncludePath = includePath;
            param.OutputFile = outputFile;
            param.InputFiles = inputFileList;

            return param;
        }

        public static void PrintError (string msg) {
            Console.WriteLine("Error: " + msg);
        }

        public static void PrintUsage() {
            Console.WriteLine("\nUsage:\n");
            Console.WriteLine("jscombiner [-?] [-i <includePath>] [-p <sourceFilePattern>] -o <outputFile> <inputFile/dir> [<inputFile/dir> <inputFile/dir> ...]\n");
            Console.WriteLine("Parameters:\n");
            Console.WriteLine("  -?    Show this help");
            Console.WriteLine("  -i    Specify the include path which will be use when script is referred in");
            Console.WriteLine("        bracket format (<script.js>).");
            Console.WriteLine("  -o    Specify the output file. This option is mandatory.");
            Console.WriteLine("  -p    Specify the source file scan pattern. Default is *.js\n\n");
            Console.WriteLine("There has to be at least one input file or directory.");
        }

    }
}
