using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Claw.UI.Controls
{
    /// <summary>
    /// Custom ListView that loads its settings directly from the LookAndFeel.
    /// </summary>
    public class ClawListView : ListView
    {
        private const int WM_REFLECT_NOTIFY = 0x204E;
        private const int NM_CUSTOMDRAW = -12;
        private const int CDDS_POSTPAINT = 2;

        [StructLayout(LayoutKind.Sequential)]
        private struct NMHDR
        {
            public IntPtr hwndFrom;
            public IntPtr idFrom;
            public int code;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct NMCUSTOMDRAW
        {
            public NMHDR nmcd;
            public int dwDrawStage;
            public IntPtr hdc;
            public RECT rc;
            public IntPtr dwItemSpec;
            public int uItemState;
            public IntPtr lItemlParam;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct NMLVCUSTOMDRAW
        {
            public NMCUSTOMDRAW nmcd;
            public int clrText;
            public int clrTextBk;
            public int iSubItem;
            public int dwItemType;
            public int clrFace;
            public int iIconEffect;
            public int iIconPhase;
            public int iPartId;
            public int iStateId;
            public RECT rcText;
            public uint uAlign;
        }

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
        /// Creates a new ClawListView.
        /// </summary>
        public ClawListView()
            : base()
        {
            LookAndFeel.Instance.Changed += LookChanged;
            LookChanged(this, new EventArgs());
        }

        /// <summary>
        /// Called when the LookAndFeel changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void LookChanged(object sender, EventArgs e)
        {
            LookAndFeel lookAndFeel = LookAndFeel.Instance;
            base.BackColor = lookAndFeel.BackColor;
            base.ForeColor = lookAndFeel.ForeColor;
        }

        protected override void WndProc(ref Message m)
        {
            bool drawn = false;

            if (m.Msg == WM_REFLECT_NOTIFY)
            {
                NMHDR nmhdr = (NMHDR)m.GetLParam(typeof(NMHDR));
                if (nmhdr.code == NM_CUSTOMDRAW)
                {
                    NMLVCUSTOMDRAW nmcustomdraw = (NMLVCUSTOMDRAW)m.GetLParam(typeof(NMLVCUSTOMDRAW));
                    using (Graphics graphics = Graphics.FromHdc(nmcustomdraw.nmcd.hdc))
                    {
                        OnPostPaint(graphics);
                        drawn = true;
                    }
                }
            }

            if (!drawn)
            {
                base.WndProc(ref m);
            }
        }

        /// <summary>
        /// Called after the component has been drawed.
        /// </summary>
        /// <param name="graphics"></param>
        private void OnPostPaint(Graphics graphics)
        {
            using (var pen = new Pen(LookAndFeel.Instance.MidColor))
            {
                graphics.DrawRectangle(pen, new Rectangle(0, 0, Width - 5, Height - 5));
            }
        }
    }
}
