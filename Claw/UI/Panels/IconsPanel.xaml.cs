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

namespace Claw.UI.Panels
{
    /// <summary>
    /// Contains the control logic for IconsPanel.xaml.
    /// </summary>
    public partial class IconsPanel : UserControl
    {
        /// <summary>
        /// Creates a new IconsPanel.
        /// </summary>
        public IconsPanel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Selection change event handler for the selection of the main ListView within the IconsPanel.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void OnLvIconsSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btRemove.IsEnabled = lvIcons.SelectedIndex != -1;
            btExtract.IsEnabled = lvIcons.SelectedIndex != -1;
            btExportToDB.IsEnabled = lvIcons.SelectedIndex != -1;
        }
    }
}
