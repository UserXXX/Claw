using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClawLibTests
{
    /// <summary>
    /// Contains globally used test properties.
    /// </summary>
    internal static class TestConstants
    {
        internal readonly static string VALID_TESTDATA_DIR = AppDomain.CurrentDomain.BaseDirectory + "\\Testdata\\Valid\\";
        internal readonly static string VALID_TESTDATA_IN_DIR = AppDomain.CurrentDomain.BaseDirectory + "\\Testdata\\Valid\\In\\";
        internal readonly static string VALID_TESTDATA_OUT_DIR = AppDomain.CurrentDomain.BaseDirectory + "\\Testdata\\Valid\\Out\\";
        internal readonly static string CORRUPTED_TESTDATA_DIR = AppDomain.CurrentDomain.BaseDirectory + "\\Testdata\\Corrupted\\";
    }
}
