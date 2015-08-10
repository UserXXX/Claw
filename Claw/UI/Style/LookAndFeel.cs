using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Claw.UI.Style
{
    /// <summary>
    /// Look and feel for all visual components.
    /// </summary>
    public static class LookAndFeel
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

        /// <summary>
        /// The default foreground color for all controls.
        /// </summary>
        public static Color ForeColor
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
        public static Color MidColor
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
        public static Color BackColor
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
        public static Color HoverColor
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
        public static Color InteractColor
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
        public static Color DisabledMidColor
        {
            get { return (Color)App.Current.Resources[DISABLED_MID_COLOR]; }
            set
            {
                App.Current.Resources[DISABLED_MID_COLOR] = value;
                FireChanged();
            }
        }

        /// <summary>
        /// The tile image that is used for form backgrounds.
        /// </summary>
        public static ImageSource TileImage
        {
            get { return (ImageSource)App.Current.Resources[TILE_IMAGE]; }
        }

        /// <summary>
        /// The mask image for the top left window edge.
        /// </summary>
        public static ImageSource TopLeftWindowEdgeImage
        {
            get { return (ImageSource)App.Current.Resources[TOP_LEFT_WINDOW_EDGE_IMAGE]; }
        }

        /// <summary>
        /// The mask image for the top right window edge.
        /// </summary>
        public static ImageSource TopRightWindowEdgeImage
        {
            get { return (ImageSource)App.Current.Resources[TOP_RIGHT_WINDOW_EDGE_IMAGE]; }
        }

        /// <summary>
        /// The mask image for the bottom left window edge.
        /// </summary>
        public static ImageSource BottomLeftWindowEdgeImage
        {
            get { return (ImageSource)App.Current.Resources[BOTTOM_LEFT_WINDOW_EDGE_IMAGE]; }
        }

        /// <summary>
        /// The mask image for the bottom right window edge.
        /// </summary>
        public static ImageSource BottomRightWindowEdgeImage
        {
            get { return (ImageSource)App.Current.Resources[BOTTOM_RIGHT_WINDOW_EDGE_IMAGE]; }
        }

        /// <summary>
        /// The mask image for the solid part of the window.
        /// </summary>
        public static ImageSource SolidImage
        {
            get { return (ImageSource)App.Current.Resources[SOLID_IMAGE]; }
        }

        /// <summary>
        /// Event for a change of the LookAndFeel.
        /// This is fired whenever there is a change of an attribute of this LookAndFeel.
        /// </summary>
        public static event EventHandler<EventArgs> Changed;

        /// <summary>
        /// Fires the changed event.
        /// </summary>
        private static void FireChanged()
        {
            if (Changed != null)
                Changed(null, new EventArgs());
        }
    }
}
