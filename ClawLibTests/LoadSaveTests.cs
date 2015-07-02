using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Claw;
using System.IO;
using System.Text;

namespace ClawLibTests
{
    [TestClass]
    public class LoadSaveTests
    {
        private readonly string TESTDATA_DIR = AppDomain.CurrentDomain.BaseDirectory + "\\Testdata\\";

        /// <summary>
        /// Loads a large profile, saves it and compares the generated file to the original.
        /// </summary>
        [TestMethod]
        public void LoadSaveTestLarge()
        {
            DoLoadSaveTest(TESTDATA_DIR + "LargeTestProfile.pr0");
        }

        /// <summary>
        /// Loads a medium sized profile, saves it and compares the generated file to the original.
        /// </summary>
        [TestMethod]
        public void LoadSaveTestMedium()
        {
            DoLoadSaveTest(TESTDATA_DIR + "MediumTestProfile.pr0");
        }

        /// <summary>
        /// Tests the given file by loading it, saving it and compare the gathered save file to the original file.
        /// </summary>
        /// <param name="file">File to load.</param>
        private static void DoLoadSaveTest(string file)
        {
            MadCatzProfile profile = ProfileFactory.Load(file);
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
                originalReader = new StreamReader(file);
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
