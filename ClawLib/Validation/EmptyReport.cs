using System;
using System.Collections.Generic;
using System.Text;

namespace Claw.Validation
{
    /// <summary>
    /// Stellt einen ValidationReport dar, der alle Nachrichten verwirft.
    /// </summary>
    public class EmptyReport : ValidationReport
    {
        internal override void AddInfo(string message) { }

        internal override void AddWarning(string message) { }

        internal override void AddError(string message) { }
    }
}
