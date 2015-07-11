using Claw.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claw.Model
{
    /// <summary>
    /// ValidationReport implementation that checks whether errors occured during loading.
    /// </summary>
    public class ErrorCatcherReport : ValidationReport
    {
        private bool error = false;

        /// <summary>
        /// Whether an error occured.
        /// </summary>
        public bool ErrorOccured
        {
            get { return error; }
        }

        public override void AddError(string message)
        {
            error = true;
        }

        public override void AddInfo(string message) { }

        public override void AddWarning(string message) { }
    }
}
