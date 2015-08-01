using Claw.Blasts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claw.UI.Windows
{
    /// <summary>
    /// Container for the result of an ChooseIconWindow.ShowDialog call.
    /// </summary>
    public class ChooseIconWindowResult
    {
        private bool canceled;
        /// <summary>
        /// True, if the dialog was canceled, otherwise false.
        /// </summary>
        public bool Canceled
        {
            get { return canceled; }
        }

        private Blast selected;
        /// <summary>
        /// The selected blast. Null if the "None" option was chosen.
        /// </summary>
        public Blast Selected
        {
            get { return selected; }
        }

        /// <summary>
        /// Creates a new container with the given data.
        /// </summary>
        /// <param name="wasCanceled">Whether the dialog was canceled.</param>
        /// <param name="selectedBlast">The selected blast when the dialog closed.</param>
        public ChooseIconWindowResult(bool wasCanceled, Blast selectedBlast)
        {
            canceled = wasCanceled;
            selected = selectedBlast;
        }
    }
}
