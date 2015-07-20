using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Claw;
using ClawLibTests.Dummy;
using System.Collections.Generic;

namespace ClawLibTests.Tests
{
    /// <summary>
    /// Test whether loading fails with the expected messages when loading corrupted profiles.
    /// </summary>
    [TestClass]
    public class CorruptedTests
    {
        /// <summary>
        /// Tests a profile with a corrupt version.
        /// </summary>
        [TestMethod]
        public void CorruptVersionTest()
        {
            var report = new TestReport();
            ProfileFactory.Load(TestConstants.CORRUPTED_TESTDATA_DIR + "CorruptVersion.pr0", report);
            report.AssertEquals(
                new List<string>(),
                new List<string>(),
                new List<string>(new string[] { "Unsupported version: 6" }));
        }

        /// <summary>
        /// Tests a profile with missing required attributes and tags.
        /// </summary>
        [TestMethod]
        public void CorruptAttributesAndTags()
        {
            var report = new TestReport();
            ProfileFactory.Load(TestConstants.CORRUPTED_TESTDATA_DIR + "CorruptAttributesAndTags.pr0", report);
            report.AssertEquals(
                new List<string>(),
                new List<string>(),
                new List<string>(new string[] { "Missing required tag in \"controller\" node.",
                    "Missing required attribute in \"controller\" node: group", 
                    "Missing required tag in \"shift\" node.",
                    "Missing required tag in \"mousepointer\" node.", 
                    "Missing required attribute in \"member\" node: name",
                    "Missing required tag in \"actioncommand\" node.",
                    "Missing required attribute in \"actioncommand\" node: name", 
                    "Missing required attribute in \"action\" node: device", 
                    "Missing required attribute in \"action\" node: page",
                    "Missing required attribute in \"action\" node: usage", }));
        }
    }
}
