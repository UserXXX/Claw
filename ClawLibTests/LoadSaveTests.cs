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
        private void DoLoadSaveTest(string file)
        {
            Profile profile = ProfileFactory.Load(file);
            byte[] data = null;
            using (var stream = new MemoryStream())
            {
                ProfileFactory.Save(stream, profile);
                data = stream.GetBuffer();
            }
            string written = null;
            using (var reader = new StreamReader(new MemoryStream(data)))
            {
                written = reader.ReadToEnd().Trim().Replace("\0", string.Empty);
            }
            string original = null;
            using (var reader = new StreamReader(file))
            {
                original = reader.ReadToEnd().Trim();
            }
            Assert.AreEqual(original, written);
        }
    }
}
