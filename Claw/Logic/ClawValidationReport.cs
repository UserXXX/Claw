using Claw.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claw.Logic
{
    /// <summary>
    /// ValidationReport implementation for Claw.
    /// </summary>
    public class ClawValidationReport : ValidationReport
    {
        private string result = "";

        /// <summary>
        /// Whether some message was given while loading.
        /// </summary>
        public bool SomethingHappened
        {
            get { return !string.IsNullOrEmpty(result); }
        }

        /// <summary>
        /// The (combined) message of what happened during loading.
        /// </summary>
        public string Message
        {
            get { return result; }
        }

        public override void AddError(string message)
        {
            AddToMessage("Error: ", message);
        }

        public override void AddInfo(string message)
        {
            AddToMessage("Info: ", message);
        }

        public override void AddWarning(string message)
        {
            AddToMessage("Warning: ", message);
        }

        /// <summary>
        /// Adds a line to the message.
        /// </summary>
        /// <param name="title">Title of the line.</param>
        /// <param name="msg">Original message.</param>
        private void AddToMessage(string title, string msg)
        {
            if (string.IsNullOrEmpty(result))
            {
                result = title + msg;
            }
            else
            {
                result += "\n" + title + msg;
            }
        }
    }
}
