using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Claw.UI
{
    /// <summary>
    /// Content panel for the profile buttons.
    /// </summary>
    public partial class ProfileButtonContentPanel : UserControl
    {
        /// <summary>
        /// Event handler for clicking on the close button. The event will pass the RadioButton hosting this control.
        /// </summary>
        public event EventHandler<EventArgs> OnCloseClick;

        private RadioButton parent;

        /// <summary>
        /// Creates a new ProfileButtonContentPanel.
        /// </summary>
        /// <param name="parentButton">The button containing this panel.</param>
        /// <param name="profileName">The name of the profile.</param>
        public ProfileButtonContentPanel(RadioButton parentButton, string profileName)
        {
            parent = parentButton;

            InitializeComponent();

            tbName.Text = profileName;
        }

        /// <summary>
        /// Handles clicks on the close button.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            if (OnCloseClick != null)
            {
                // Pass the original button, as it holds the context information.
                OnCloseClick(parent, e);
            }
        }
    }
}
