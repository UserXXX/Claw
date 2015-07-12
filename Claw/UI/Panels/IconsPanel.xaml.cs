using Claw.Blasts;
using Claw.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
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

        /// <summary>
        /// Notifies this panel that the selected profile changed.
        /// </summary>
        /// <param name="activeProfile">The new active profile.</param>
        public void ActiveProfileChanged(MadCatzProfile activeProfile)
        {
            if (activeProfile == null)
            {
                throw new ArgumentNullException("activeProfile");
            }

            lvIcons.Items.Clear();
            foreach (Blast blast in activeProfile.Blasts)
            {
                BitmapImage image = CreateImage(blast.Data);
                var imageControl = new Image();
                imageControl.Source = image;
                lvIcons.Items.Add(imageControl);
            }
        }

        /// <summary>
        /// Creates a BitmapImage from the data array.
        /// </summary>
        /// <param name="data">The base data.</param>
        /// <returns></returns>
        private static BitmapImage CreateImage(byte[] data)
        {
            var image = new BitmapImage();
            using (var stream = new MemoryStream(data))
            {
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = stream;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }
    }
}
