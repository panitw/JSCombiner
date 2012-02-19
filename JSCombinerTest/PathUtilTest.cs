using JSCombiner.Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace JSCombinerTest
{
    
    
    /// <summary>
    ///This is a test class for PathUtilTest and is intended
    ///to contain all PathUtilTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PathUtilTest {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext {
            get {
                return testContextInstance;
            }
            set {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        [TestMethod()]
        public void GetFullPathTest_bracket () {
            string originFile = "c:\\folder1\\script1.js";
            string referencedFile = "<script2.js>";
            string includePath = "c:\\includes";
            string expected = "c:\\includes\\script2.js";
            string actual;
            actual = PathUtil.GetFullPath(originFile, referencedFile, includePath);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetFullPathTest_DoubleQuote () {
            string originFile = "c:\\folder1\\script1.js";
            string referencedFile = "\"script2.js\"";
            string includePath = "c:\\includes";
            string expected = "c:\\folder1\\script2.js";
            string actual;
            actual = PathUtil.GetFullPath(originFile, referencedFile, includePath);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetFullPathTest_DoubleQuote_Referenced () {
            string originFile = "c:\\folder1\\script1.js";
            string referencedFile = "\"..\\folder2\\script2.js\"";
            string includePath = "c:\\includes";
            string expected = "c:\\folder2\\script2.js"; 
            string actual;
            actual = PathUtil.GetFullPath(originFile, referencedFile, includePath);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetFullPathTest_DoubleQuote_Referenced_2Level () {
            string originFile = "c:\\folder1\\subfolder1\\script1.js";
            string referencedFile = "\"..\\..\\folder2\\script2.js\"";
            string includePath = "c:\\includes";
            string expected = "c:\\folder2\\script2.js";
            string actual;
            actual = PathUtil.GetFullPath(originFile, referencedFile, includePath);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetFullPathTest_DoubleQuote_Referenced_FullPath () {
            string originFile = "c:\\folder1\\subfolder1\\script1.js";
            string referencedFile = "\"d:\\folder2\\script2.js\"";
            string includePath = "c:\\includes";
            string expected = "d:\\folder2\\script2.js";
            string actual;
            actual = PathUtil.GetFullPath(originFile, referencedFile, includePath);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetFullPathTest_Origin_NotFullPath () {
            string currentDir = Directory.GetCurrentDirectory();
            string originFile = "script1.js";
            string referencedFile = "\"script2.js\"";
            string includePath = "c:\\includes";
            string expected = Path.Combine(currentDir,"script2.js");
            string actual;
            actual = PathUtil.GetFullPath(originFile, referencedFile, includePath);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetFullPathTest_Origin_NotFullPath_2Level () {
            string currentDir = Directory.GetCurrentDirectory();
            string originFile = "script1.js";
            string referencedFile = "\"subfolder\\script2.js\"";
            string includePath = "c:\\includes";
            string expected = Path.Combine(currentDir, "subfolder\\script2.js");
            string actual;
            actual = PathUtil.GetFullPath(originFile, referencedFile, includePath);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        [ExpectedException(typeof(Exception))]
        public void GetFullPathTest_InvalidReferenceFormat () {
            string originFile = "c:\\folder1\\subfolder1\\script1.js";
            string referencedFile = "script2.js";
            string includePath = "c:\\includes";
            string actual;
            actual = PathUtil.GetFullPath(originFile, referencedFile, includePath);
        }
    
    }
}
