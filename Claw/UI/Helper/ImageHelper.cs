using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Claw.UI.Helper
{
    /// <summary>
    /// Helper class for image interaction with ClawLib for view components.
    /// </summary>
    public static class ImageHelper
    {
        /// <summary>
        /// Creates a BitmapImage from the data array.
        /// </summary>
        /// <param name="data">The base data.</param>
        /// <returns>The created BitmapImage.</returns>
        public static BitmapImage CreateImage(byte[] data)
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

        /// <summary>
        /// Creates a new Image control with the image extracted from data.
        /// </summary>
        /// <param name="data">The image data.</param>
        /// <returns>The created image.</returns>
        public static Image CreateImageControl(byte[] data)
        {
            BitmapImage image = ImageHelper.CreateImage(data);
            var imageControl = new Image();
            imageControl.Source = image;
            imageControl.Margin = new Thickness(5,5,7,5);
            imageControl.Width = 100;
            imageControl.Height = 100;
            return imageControl;
        }
    }
}
