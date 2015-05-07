using Claw.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;

namespace Claw.UI
{
    /// <summary>
    /// Delegate for changed events.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="evt">The event instance.</param>
    public delegate void ChangedEventHandler(object sender, EventArgs evt);

    /// <summary>
    /// Look and feel for all visual components. This is a singleton.
    /// </summary>
    public class LookAndFeel
    {
        private static LookAndFeel instance = new LookAndFeel();
        /// <summary>
        /// The singleton instance of LookAndFeel.
        /// </summary>
        public static LookAndFeel Instance
        {
            get { return instance; }
        }

        private Color foreColor;
        private Color backColor;
        private Image tileImage;
        private Color hoverColor;
        private Color interactColor;

        /// <summary>
        /// The default foreground color for all controls.
        /// </summary>
        public Color ForeColor
        {
            get { return foreColor; }
            set
            {
                foreColor = value;
                FireChanged();
            }
        }

        /// <summary>
        /// The default background color for all controls.
        /// </summary>
        public Color BackColor
        {
            get { return backColor; }
            set
            {
                backColor = value;
                FireChanged();
            }
        }

        /// <summary>
        /// The tile image that is used for form backgrounds.
        /// </summary>
        public Image TileImage
        {
            get { return tileImage; }
            set
            {
                tileImage = value;
                FireChanged();
            }
        }

        /// <summary>
        /// Color used when hovering over a control (i.e. a Button).
        /// </summary>
        public Color HoverColor
        {
            get { return hoverColor; }
            set
            {
                hoverColor = value;
                FireChanged();
            }
        }

        /// <summary>
        /// A color used for interaction events, such as mouse button presses.
        /// </summary>
        public Color InteractColor
        {
            get { return interactColor; }
            set
            {
                interactColor = value;
                FireChanged();
            }
        }

        /// <summary>
        /// Event for a change of the LookAndFeel.
        /// This is fired whenever there is a change of an attribute of this LookAndFeel.
        /// </summary>
        public event ChangedEventHandler Changed;

        /// <summary>
        /// Creates a new, default LookAndFeel.
        /// </summary>
        private LookAndFeel()
        {
            foreColor = Color.Red;
            backColor = Color.FromArgb(255, 30, 30, 30);
            tileImage = (Image)(Resources.ResourceManager.GetObject("Tile"));
            hoverColor = Color.FromArgb(255, 60, 60, 60);
            interactColor = Color.Black;
        }

        /// <summary>
        /// Fires the changed event.
        /// </summary>
        private void FireChanged()
        {
            if (Changed != null)
                Changed(this, new EventArgs());
        }
    }
}
