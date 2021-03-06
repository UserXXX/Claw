﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Claw.Validation
{
    /// <summary>
    /// Stellt einen ValidationReport dar, der alle Nachrichten verwirft.
    /// </summary>
    public class EmptyReport : ValidationReport
    {
        public override void AddInfo(string message) { }

        public override void AddWarning(string message) { }

        public override void AddError(string message) { }
    }
}
