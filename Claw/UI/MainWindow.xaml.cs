﻿using Claw.Interfaces;
using Claw.UI.Controls;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Claw.UI
{
    /// <summary>
    /// The applications main window.
    /// </summary>
    public partial class MainWindow : ClawWindow, IMainView
    {
        private const string BASE_PROFILES_DIRECTORY = "C:\\Users\\Public\\Documents\\SmartTechnology Profiles";
        private const string MESSAGE_TITLE_CLAW = "MessageTitleClaw";
        private const string MESSAGE_TITLE_CLAW_ERROR = "MessageTitleClawError";
        private const string FILTER_PROFILE_FILES = "MadCatzProfileFiles";
        private const string FILTER_ALL_FILES = "AllFiles";

        private IMainPresenter presenter;

        private OpenFileDialog openProfileDialog;

        protected override Panel BaseComponent
        {
            get { return baseGrid; }
        }

        public IIconsView IconsView
        {
            get { return pEditor.IconsPanel; }
        }

        public MainWindow()
        {
            InitializeComponent();

            openProfileDialog = new OpenFileDialog();
            openProfileDialog.Filter = (string)App.Current.FindResource(FILTER_PROFILE_FILES) + " (*.pr0)|*.pr0|" + (string)App.Current.FindResource(FILTER_ALL_FILES) + " (*.*)|*.*";
            if (Directory.Exists(BASE_PROFILES_DIRECTORY))
            {
                openProfileDialog.InitialDirectory = BASE_PROFILES_DIRECTORY;
            }
        }

        public void SetPresenter(IMainPresenter mainPresenter)
        {
            this.presenter = mainPresenter;
        }

        public FileInfo SelectProfileFile()
        {
            bool? success = openProfileDialog.ShowDialog();
            if (success.HasValue && success.Value)
            {
                return new FileInfo(openProfileDialog.FileName);
            }
            else
            {
                return null;
            }
        }

        public void ShowErrorMessage(string message)
        {
            MessageBox.Show(this, message, (string)App.Current.FindResource(MESSAGE_TITLE_CLAW_ERROR), MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(this, message, (string)App.Current.FindResource(MESSAGE_TITLE_CLAW_ERROR), MessageBoxButton.OK);
        }

        public void AddProfile(MadCatzProfile profile)
        {
            if (profile == null)
            {
                throw new ArgumentNullException("profile");
            }

            RadioButton profileButton = new RadioButton();
            profileButton.GroupName = "rbgProfiles";
            profileButton.Content = profile.Name;
            profileButton.Tag = profile;
            profileButton.SetResourceReference(RadioButton.StyleProperty, typeof(ToggleButton));
            profileButton.Height = 30;
            profileButton.MinWidth = 100;
            profileButton.Margin = new Thickness(2, 0, 3, 0);
            profileButton.Checked += OnProfileSelected;
            spProfiles.Children.Add(profileButton);

            profileButton.IsChecked = true;
            InvalidateVisual();
        }

        /// <summary>
        /// Callback for the selection of a profile.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void OnProfileSelected(object sender, RoutedEventArgs e)
        {
            btSave.IsEnabled = true;
            btSaveAs.IsEnabled = true;
            RadioButton button = (RadioButton)sender;
            MadCatzProfile profile = (MadCatzProfile)button.Tag;
            presenter.ActiveProfileChanged(profile);
        }

        /// <summary>
        /// Initialization handler for this window.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">Event arguments.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification="This method is an event handler for the WPF UI, so it is not recognized to be called by the analyzer.")]
        private void OnInitialized(object sender, EventArgs e)
        {
            exFile.IsExpanded = true;
            App.OnStartup(this);
        }

        #region Expander management

        /// <summary>
        /// Handles the Expanded event for the expander on the right side of the window.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void OnExFileExpanded(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
            {
                Width = ActualWidth + 100;
            }

            SetDistToRight(100);
        }

        /// <summary>
        /// Handles the Collapsed event for the expander on the right side of the window.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void OnExFileCollapsed(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
            {
                Width = ActualWidth - 100;
            }

            SetDistToRight(0);
        }

        /// <summary>
        /// Sets the distance of the controls in the center of the window relative to their original position.
        /// </summary>
        /// <param name="dist">Relative distance to the right window border.</param>
        private void SetDistToRight(double dist)
        {
            exFile.Margin = new Thickness(0, 60, -94 + dist, 0);
            pCentral.Margin = new Thickness(0, 40, 23 + dist, 60);
            InvalidateMeasure();
        }

        #endregion

        #region Resize management

        /// <summary>
        /// Handles the StateChanged event.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "This method is an event handler for the WPF UI, so it is not recognized to be called by the analyzer.")]
        private void OnStateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                btOptions.Margin = new Thickness(10, 10, 85, 10);
            }
            else
            {
                btOptions.Margin = new Thickness(10, 10, 100, 10);
            }
        }

        #endregion

        /// <summary>
        /// Click event handler for open file button.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void OnOpenFileClick(object sender, RoutedEventArgs e)
        {
            presenter.OpenFileRequested();
        }
    }
}
