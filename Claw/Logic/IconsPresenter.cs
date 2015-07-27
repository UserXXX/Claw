using Claw.Blasts;
using Claw.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
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
        private const string ERROR_MESSAGE_INVALID_IMAGE_FORMAT = "InvalidImageFormat";
        private const string QUESTION_SURE_TO_REMOVE_ICONS = "SureToRemoveIcons";

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

        public void RemoveIconsRequested(LinkedList<Blast> blasts)
        {
            if (blasts == null)
            {
                throw new ArgumentNullException("blasts");
            }

            if (!mainPresenter.ForwardYesNoQuestion((string)Application.Current.FindResource(QUESTION_SURE_TO_REMOVE_ICONS)))
            {
                return;
            }

            MadCatzProfile activeProfile = mainPresenter.ActiveProfile;
            foreach (Blast blast in blasts)
            {
                mainPresenter.Model.RemoveIcon(activeProfile, blast);
            }

            view.ActiveProfileChanged(activeProfile);
        }

        public void ExtractIconRequested(Blast blast)
        {
            if (blast == null)
            {
                throw new ArgumentNullException("blast");
            }

            FileInfo saveFile = view.SelectImageSaveFile();

            if (saveFile == null)
            {
                return;
            }

            switch (saveFile.Extension.ToUpperInvariant())
            {
                case ".BMP":
                    SaveToFile(saveFile, blast, new BmpBitmapEncoder());
                    break;

                case ".GIF":
                    SaveToFile(saveFile, blast, new GifBitmapEncoder());
                    break;

                case ".JPEG":
                case ".JPG":
                    SaveToFile(saveFile, blast, new JpegBitmapEncoder());
                    break;

                case ".PNG":
                    SaveToFile(saveFile, blast.GetData());
                    break;

                case ".TIF":
                case ".TIFF":
                    SaveToFile(saveFile, blast, new TiffBitmapEncoder());
                    break;

                default:
                    mainPresenter.ForwardError((string)Application.Current.FindResource(ERROR_MESSAGE_INVALID_IMAGE_FORMAT));
                    break;
            }
        }

        /// <summary>
        /// Saves the icon using the given encoder to the given file.
        /// </summary>
        /// <param name="file">File to save to.</param>
        /// <param name="blast">Icon to save.</param>
        /// <param name="encoder">Encoder to use.</param>
        private static void SaveToFile(FileInfo file, Blast blast, BitmapEncoder encoder)
        {
            var image = new BitmapImage();
            using (var stream = new MemoryStream(blast.GetData()))
            {
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = stream;
                image.EndInit();
            }
            image.Freeze();

            BitmapFrame frame = BitmapFrame.Create(image);
            encoder.Frames.Add(frame);

            using (FileStream stream = OpenFileStream(file))
            {
                encoder.Save(stream);
            }
        }

        /// <summary>
        /// Saves the given data to the given file.
        /// </summary>
        /// <param name="file">The file to save to.</param>
        /// <param name="data">The data to write to the file.</param>
        private static void SaveToFile(FileInfo file, byte[] data)
        {
            using (FileStream stream = OpenFileStream(file))
            {
                stream.Write(data, 0, data.Length);
            }
        }

        /// <summary>
        /// Opens a stream to the given file. If the file already exists it is overwritten.
        /// </summary>
        /// <param name="file">The file to write to.</param>
        /// <returns>The opened stream.</returns>
        private static FileStream OpenFileStream(FileInfo file)
        {
            if (file.Exists)
            {
                file.Delete();
            }

            return file.OpenWrite();
        }
    }
}
