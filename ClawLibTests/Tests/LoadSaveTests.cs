using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Claw;
using System.IO;
using System.Text;

namespace ClawLibTests.Tests
{
    [TestClass]
    public class LoadSaveTests
    {
        /// <summary>
        /// Loads a medium sized profile, saves it and compares the generated file to the original.
        /// </summary>
        [TestMethod]
        public void LoadSaveTestMedium()
        {
            DoLoadSaveTest(TestConstants.VALID_TESTDATA_DIR + "MediumTestProfile.pr0");
        }

        /// <summary>
        /// Loads a large profile, saves it and compares the generated file to the original.
        /// </summary>
        [TestMethod]
        public void LoadSaveTestLarge()
        {
            DoLoadSaveTest(TestConstants.VALID_TESTDATA_DIR + "LargeTestProfile.pr0");
        }

        /// <summary>
        /// Does a test which should cause the API to optimize the profile.
        /// </summary>
        [TestMethod]
        public void AssymetricTest01()
        {
            DoLoadSaveTest(TestConstants.VALID_TESTDATA_IN_DIR + "TP01.pr0", TestConstants.VALID_TESTDATA_OUT_DIR + "TP01.pr0");
        }

        /// <summary>
        /// Does a test which should cause the API to optimize the profile.
        /// </summary>
        [TestMethod]
        public void AssymetricTest02()
        {
            DoLoadSaveTest(TestConstants.VALID_TESTDATA_IN_DIR + "TP02.pr0", TestConstants.VALID_TESTDATA_OUT_DIR + "TP02.pr0");
        }

        /// <summary>
        /// Does a test which should cause the API to optimize the profile.
        /// </summary>
        [TestMethod]
        public void AssymetricTest03()
        {
            DoLoadSaveTest(TestConstants.VALID_TESTDATA_IN_DIR + "TP03.pr0", TestConstants.VALID_TESTDATA_OUT_DIR + "TP03.pr0");
        }

        /// <summary>
        /// Tests the API by loading the input file, saving it to memory and comparing it to the same file.
        /// </summary>
        /// <param name="file">File to load and compare against.</param>
        private static void DoLoadSaveTest(string file)
        {
            DoLoadSaveTest(file, file);
        }

        /// <summary>
        /// Tests the API by loading the input file, saving it to memory and comparing it to the wished result file.
        /// </summary>
        /// <param name="inFile">The file to load and save.</param>
        /// <param name="resultFile">The file to compare to.</param>
        private static void DoLoadSaveTest(string inFile, string resultFile)
        {
            MadCatzProfile profile = ProfileFactory.Load(inFile);
            byte[] data = null;
            using (var stream = new MemoryStream())
            {
                ProfileFactory.Save(stream, profile);
                data = stream.GetBuffer();
            }

            StreamReader writtenReader = null;
            MemoryStream memStream = null;
            string written = null;
            try
            {
                memStream = new MemoryStream(data);
                writtenReader = new StreamReader(memStream);
                written = writtenReader.ReadToEnd().Trim().Replace("\0", string.Empty);
                writtenReader.Close();
                writtenReader = null;
                memStream = null;
            }
            finally
            {
                if (writtenReader != null)
                {
                    writtenReader.Close();
                    writtenReader = null;
                    memStream = null;
                }
                if (memStream != null)
                {
                    memStream.Close();
                }
            }

            StreamReader originalReader = null;
            string original = null;
            try
            {
                originalReader = new StreamReader(resultFile);
                original = originalReader.ReadToEnd().Trim();
                originalReader.Close();
                originalReader = null;
            }
            finally
            {
                if (originalReader != null)
                {
                    originalReader.Close();
                }
            }

            Assert.AreEqual(original, written);
        }
    }
}
