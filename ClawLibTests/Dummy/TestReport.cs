using Claw.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClawLibTests.Dummy
{
    /// <summary>
    /// Test report for storing report information.
    /// </summary>
    internal class TestReport : ValidationReport
    {
        private List<string> errors = new List<string>();
        private List<string> warnings = new List<string>();
        private List<string> infos = new List<string>();

        public override void AddError(string message)
        {
            errors.Add(message);
        }

        public override void AddInfo(string message)
        {
            infos.Add(message);
        }

        public override void AddWarning(string message)
        {
            warnings.Add(message);
        }

        /// <summary>
        /// Tests whether the output of the validation meets the given expected results.
        /// </summary>
        /// <param name="expectedInfos">Expected infos.</param>
        /// <param name="expectedWarnings">Expected warnings.</param>
        /// <param name="expectedErrors">Expected errors.</param>
        public void AssertEquals(List<string> expectedInfos, List<string> expectedWarnings, List<string> expectedErrors)
        {
            AssertEquals(infos, expectedInfos);
            AssertEquals(warnings, expectedWarnings);
            AssertEquals(errors, expectedErrors);
        }

        /// <summary>
        /// Tests whether the given list and the expected are equal.
        /// </summary>
        /// <param name="result">The test output.</param>
        /// <param name="expected">The expected result.</param>
        private static void AssertEquals(List<string> result, List<string> expected)
        {
            Assert.AreEqual<int>(expected.Count, result.Count, "Result length differs from expected length. Result was " + result.Count + " but expected " + expected.Count + ".");
            foreach (string exp in expected)
            {
                Assert.IsTrue(result.Contains(exp), "Result doesn't contain \"" + exp +"\".");
            }
        }
    }
}
