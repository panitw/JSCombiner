using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JSCombiner.Engine;

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
                        throw new Exception("Include path is not specified");
                    }
                } else
                    if (args[i] == "-o") {
                        i++;
                        if (args.Length > i) {
                            outputFile = args[i];
                        } else {
                            throw new Exception("Output file is not specified");
                        }
                    } else {
                        for (int j = i; j < args.Length; j++) {
                            inputFileList.Add(args[j]);
                        }
                    }
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
            Console.WriteLine("Usage:\n");
            Console.WriteLine("jscombiner [-?] [-i <include_path>] -o <output_file> <script1.js> [<script2.js> <script3.js> ...]");
            Console.WriteLine("Parameters:\n");
            Console.WriteLine("  -?    Show this help");
            Console.WriteLine("  -i    Specify the include path which will be use when script is referred in");
            Console.WriteLine("        bracket format (<script.js>). This option is optional.");
            Console.WriteLine("  -o    Specify the output file. This option is mandatory.\n\n");
            Console.WriteLine("There has to be at least one input file.");
        }

    }
}
