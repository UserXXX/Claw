using Claw.Blasts;
using Claw.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Claw.Logic
{
    /// <summary>
    /// Presenter implementation for the icons display.
    /// </summary>
    public class IconsPresenter : IIconsPresenter
    {
        private const string ERROR_MESSAGE_COULD_NOT_LOAD_IMAGES = "CouldNotLoadImages";

        private IMainPresenter mainPresenter;
        private IIconsView view;

        /// <summary>
        /// Creates a new IconsPresenter.
        /// </summary>
        /// <param name="mainPres">The main presenter.</param>
        public IconsPresenter(IMainPresenter mainPres)
        {
            mainPresenter = mainPres;
        }

        public void SetView(IIconsView newView)
        {
            view = newView;
        }

        public void ActiveProfileChanged(MadCatzProfile profile)
        {
            view.ActiveProfileChanged(profile);
        }

        public void AddIconsRequested()
        {
            FileInfo[] imageFiles = view.SelectImageFiles();

            if (imageFiles == null)
            {
                return;
            }
            string error = null;

            var validExtensions = new List<string>(new string[] {
                ".BMP",
                ".GIF",
                ".JPEG",
                ".JPG",
                ".PNG",
                ".TIF",
                ".TIFF",
            });

            foreach (FileInfo info in imageFiles)
            {
                if (validExtensions.Contains(info.Extension.ToUpperInvariant()))
                {
                    LoadImage(info);
                }
                else
                {
                    if (error == null)
                    {
                        error = info.Name;
                    }
                    else
                    {
                        error += "\n" + info.Name;
                    }
                }
            }

            if (error != null)
            {
                mainPresenter.ForwardError((string)App.Current.FindResource(ERROR_MESSAGE_COULD_NOT_LOAD_IMAGES) + "\n" + error);
            }
        }

        /// <summary>
        /// Loads an image from a file to the model.
        /// </summary>
        /// <param name="imageFile">The image file info.</param>
        private void LoadImage(FileInfo imageFile)
        {
            byte[] pngData = null;
            if (imageFile.Extension.ToUpperInvariant() != ".PNG")
            {
                pngData = LoadImageToPngData(imageFile);
            }
            else
            {
                pngData = LoadFileContents(imageFile);
            }

            MadCatzProfile activeProfile = mainPresenter.ActiveProfile;
            mainPresenter.Model.AddIcon(activeProfile, pngData);

            view.ActiveProfileChanged(activeProfile);
        }

        /// <summary>
        /// Loads the contents of the given file.
        /// </summary>
        /// <param name="imageFile">The file to read from.</param>
        /// <returns>The files content.</returns>
        private static byte[] LoadFileContents(FileInfo imageFile)
        {
            var result = new byte[imageFile.Length];
            using (FileStream stream = imageFile.OpenRead())
            {
                int position = 0;
                while (position < imageFile.Length)
                {
                    position += stream.Read(result, position, result.Length - position);
                }
                return result;
            }
        }

        /// <summary>
        /// Loads an images data as the bytes that would be written to a png file.
        /// </summary>
        /// <param name="imageFile">The file to load from.</param>
        /// <returns>The image data in png format.</returns>
        private static byte[] LoadImageToPngData(FileInfo imageFile)
        {
            var image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(imageFile.FullName, UriKind.Absolute);
            image.EndInit();

            var frame = BitmapFrame.Create(image);
            
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(frame);

            using (var stream = new MemoryStream())
            {
                encoder.Save(stream);
                return stream.ToArray();
            }
        }
    }
}
