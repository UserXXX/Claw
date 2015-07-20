﻿using Claw.Blasts;
using Claw.Interfaces;
using Microsoft.Win32;
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
    public partial class IconsPanel : UserControl, IIconsView
    {
        private const string FILTER_IMAGE_FILES = "ImageFiles";
        private const string FILTER_ALL_FILES = "AllFiles";
        private const string DIALOG_TITLE_OPEN_IMAGES = "DialogTitleOpenImages";

        private IIconsPresenter presenter;

        private OpenFileDialog openImagesDialog;

        /// <summary>
        /// Creates a new IconsPanel.
        /// </summary>
        public IconsPanel()
        {
            InitializeComponent();
            
            openImagesDialog = new OpenFileDialog();
            openImagesDialog.Title = (string)App.Current.FindResource(DIALOG_TITLE_OPEN_IMAGES);
            openImagesDialog.Multiselect = true;
            openImagesDialog.Filter = (string)App.Current.FindResource(FILTER_IMAGE_FILES) + " (*.bmp;*.gif;*.jpeg;*.jpg;*.png;*.tif;*.tiff)|*.bmp;*.gif;*.jpeg;*.jpg;*.png;*.tif;*.tiff|" + (string)App.Current.FindResource(FILTER_ALL_FILES) + " (*.*)|*.*";
            openImagesDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        }

        public void SetPresenter(IIconsPresenter newPresenter)
        {
            presenter = newPresenter;
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
            lvIcons.Items.Clear();
            if (activeProfile == null)
            {
                return;
            }

            foreach (Blast blast in activeProfile.Blasts)
            {
                BitmapImage image = CreateImage(blast.GetData());
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

        /// <summary>
        /// Event handler for clicks on the add icon button.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void OnAddIconClick(object sender, RoutedEventArgs e)
        {
            presenter.AddIconsRequested();
        }

        public FileInfo[] SelectImageFiles()
        {
            bool? success = openImagesDialog.ShowDialog();
            if (success.HasValue && success.Value)
            {
                var files = new FileInfo[openImagesDialog.FileNames.Length];
                for (int i = 0; i < files.Length; i++)
                {
                    files[i] = new FileInfo(openImagesDialog.FileNames[i]);
                }
                return files;
            }
            else
            {
                return null;
            }
        }
    }
}
