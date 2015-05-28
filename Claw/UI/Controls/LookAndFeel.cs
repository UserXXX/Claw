using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Claw.UI.Controls
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
        // Constant definitions for interaction with the resource dictionary.
        private const string FORE_COLOR = "ForeColor";
        private const string MID_COLOR = "MidColor";
        private const string BACK_COLOR = "BackColor";
        private const string INTERACT_COLOR = "InteractColor";
        private const string HOVER_COLOR = "HoverColor";
        private const string DISABLED_MID_COLOR = "DisabledMidColor";

        private const string TILE_IMAGE = "TileImage";
        private const string TOP_LEFT_WINDOW_EDGE_IMAGE = "WETopLeftImage";
        private const string TOP_RIGHT_WINDOW_EDGE_IMAGE = "WETopRightImage";
        private const string BOTTOM_LEFT_WINDOW_EDGE_IMAGE = "WEBotLeftImage";
        private const string BOTTOM_RIGHT_WINDOW_EDGE_IMAGE = "WEBotRightImage";
        private const string SOLID_IMAGE = "SolidImage";

        private static LookAndFeel instance = new LookAndFeel();

        /// <summary>
        /// The singleton instance of LookAndFeel.
        /// </summary>
        public static LookAndFeel Instance
        {
            get { return instance; }
        }

        /*private ImageSource closeImage;
        private ImageSource maximizeImage;
        private ImageSource normalizeImage;
        private ImageSource minimizeImage;
        private ImageSource extenderBackImage;
        private ImageSource extenderDownImage;
        private ImageSource extenderLeftImage;
        private ImageSource extenderRightImage;
        private ImageSource extenderUpImage;*/

        /// <summary>
        /// The default foreground color for all controls.
        /// </summary>
        public Color ForeColor
        {
            get { return (Color)App.Current.Resources[FORE_COLOR]; }
            set
            {
                App.Current.Resources[FORE_COLOR] = value;
                FireChanged();
            }
        }

        /// <summary>
        /// The mid color functions as an accent for the background color.
        /// </summary>
        public Color MidColor
        {
            get { return (Color)App.Current.Resources[MID_COLOR]; }
            set
            {
                App.Current.Resources[MID_COLOR] = value;
                FireChanged();
            }
        }

        /// <summary>
        /// The default background color for all controls.
        /// </summary>
        public Color BackColor
        {
            get { return (Color)App.Current.Resources[BACK_COLOR]; }
            set
            {
                App.Current.Resources[BACK_COLOR] = value;
                FireChanged();
            }
        }

        /// <summary>
        /// Color used when hovering over a control (i.e. a Button).
        /// </summary>
        public Color HoverColor
        {
            get { return (Color)App.Current.Resources[HOVER_COLOR]; }
            set
            {
                App.Current.Resources[HOVER_COLOR] = value;
                FireChanged();
            }
        }

        /// <summary>
        /// A color used for interaction events, such as mouse button presses.
        /// </summary>
        public Color InteractColor
        {
            get { return (Color)App.Current.Resources[INTERACT_COLOR]; }
            set
            {
                App.Current.Resources[INTERACT_COLOR] = value;
                FireChanged();
            }
        }

        /// <summary>
        /// A color used for accents of disabled controls.
        /// </summary>
        public Color DisabledMidColor
        {
            get { return (Color)App.Current.Resources[DISABLED_MID_COLOR]; }
            set
            {
                App.Current.Resources[DISABLED_MID_COLOR] = value;
                FireChanged();
            }
        }

        /*
        /// <summary>
        /// The image used for the close button in forms.
        /// </summary>
        public ImageSource CloseImage
        {
            get { return closeImage; }
            set
            {
                closeImage = value;
                FireChanged();
            }
        }

        /// <summary>
        /// The image used for the maximize button in forms.
        /// </summary>
        public ImageSource MaximizeImage
        {
            get { return maximizeImage; }
            set
            {
                maximizeImage = value;
                FireChanged();
            }
        }

        /// <summary>
        /// The image used for the normalize button in forms.
        /// </summary>
        public ImageSource NormalizeImage
        {
            get { return normalizeImage; }
            set
            {
                normalizeImage = value;
                FireChanged();
            }
        }

        /// <summary>
        /// The image used for the minimize button in forms.
        /// </summary>
        public ImageSource MinimizeImage
        {
            get { return minimizeImage; }
            set
            {
                minimizeImage = value;
                FireChanged();
            }
        }

        /// <summary>
        /// The image used for the background of extender controls.
        /// </summary>
        public ImageSource ExtenderBackImage
        {
            get { return extenderBackImage; }
            set
            {
                extenderBackImage = value;
                FireChanged();
            }
        }

        /// <summary>
        /// The image used for displaying the down state of extender controls.
        /// </summary>
        public ImageSource ExtenderDownImage
        {
            get { return extenderDownImage; }
            set
            {
                extenderDownImage = value;
                FireChanged();
            }
        }

        /// <summary>
        /// The image used for displaying the left state of extender controls.
        /// </summary>
        public ImageSource ExtenderLeftImage
        {
            get { return extenderLeftImage; }
            set
            {
                extenderLeftImage = value;
                FireChanged();
            }
        }

        /// <summary>
        /// The image used for displaying the right state of extender controls.
        /// </summary>
        public ImageSource ExtenderRightImage
        {
            get { return extenderRightImage; }
            set
            {
                extenderRightImage = value;
                FireChanged();
            }
        }

        /// <summary>
        /// The image used for displaying the up state of extender controls.
        /// </summary>
        public ImageSource ExtenderUpImage
        {
            get { return extenderUpImage; }
            set
            {
                extenderUpImage = value;
                FireChanged();
            }
        }
        */

        /// <summary>
        /// The tile image that is used for form backgrounds.
        /// </summary>
        public ImageSource TileImage
        {
            get { return (ImageSource)App.Current.Resources[TILE_IMAGE]; }
        }

        /// <summary>
        /// The mask image for the top left window edge.
        /// </summary>
        public ImageSource TopLeftWindowEdgeImage
        {
            get { return (ImageSource)App.Current.Resources[TOP_LEFT_WINDOW_EDGE_IMAGE]; }
        }

        /// <summary>
        /// The mask image for the top right window edge.
        /// </summary>
        public ImageSource TopRightWindowEdgeImage
        {
            get { return (ImageSource)App.Current.Resources[TOP_RIGHT_WINDOW_EDGE_IMAGE]; }
        }

        /// <summary>
        /// The mask image for the bottom left window edge.
        /// </summary>
        public ImageSource BottomLeftWindowEdgeImage
        {
            get { return (ImageSource)App.Current.Resources[BOTTOM_LEFT_WINDOW_EDGE_IMAGE]; }
        }

        /// <summary>
        /// The mask image for the bottom right window edge.
        /// </summary>
        public ImageSource BottomRightWindowEdgeImage
        {
            get { return (ImageSource)App.Current.Resources[BOTTOM_RIGHT_WINDOW_EDGE_IMAGE]; }
        }

        /// <summary>
        /// The mask image for the solid part of the window.
        /// </summary>
        public ImageSource SolidImage
        {
            get { return (ImageSource)App.Current.Resources[SOLID_IMAGE]; }
        }

        /// <summary>
        /// Event for a change of the LookAndFeel.
        /// This is fired whenever there is a change of an attribute of this LookAndFeel.
        /// </summary>
        public event ChangedEventHandler Changed;

        /// <summary>
        /// Creates a new default LookAndFeel.
        /// </summary>
        protected LookAndFeel() { }

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
