using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JSCombiner.Engine {

    public class EngineParameter {
        public string ReferencePrefix = "//#include";
        public string FilePattern = "*.js";
        public string IncludePath = null;
        public string WorkingPath = null;
        public string OutputFile = null;
        public List<string> InputFiles = null;
    }

}
