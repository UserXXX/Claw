using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Claw.UI.Controls
{
    /// <summary>
    /// Custom ListViewItem which gets it's visual data from the look and feel.
    /// </summary>
    public class ClawListViewItem : ListViewItem
    {
        public override Color ForeColor
        {
            get { return base.ForeColor; }
            set
            {
                // Ignore, as this is set by the look and feel.
            }
        }

        public override Color BackColor
        {
            get { return base.BackColor; }
            set
            {
                // Ignore, as this is set by the look and feel.
            }
        }

        /// <summary>
        /// Creates a new ClawListViewItem.
        /// </summary>
        public ClawListViewItem()
            : base()
        {
            LookAndFeel.Instance.Changed += LookChanged;
        }

        /// <summary>
        /// Called when the look and feel changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="evt">The event arguments.</param>
        private void LookChanged(object sender, EventArgs evt)
        {
            LookAndFeel lookAndFeel = LookAndFeel.Instance;
            base.ForeColor = lookAndFeel.ForeColor;
            base.BackColor = lookAndFeel.BackColor;
        }
    }
}
