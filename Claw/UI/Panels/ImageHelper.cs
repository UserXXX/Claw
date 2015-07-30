using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace Claw.UI.Panels
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
    }
}
