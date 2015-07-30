using Claw.Interfaces;
using Claw.UI.Controls;
using Claw.UI.Windows;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private const string DIALOG_TITLE_OPEN_PROFILES = "DialogTitleOpenProfiles";
        private const string DIALOG_TITLE_SAVE_PROFILE = "DialogTitleSaveProfile";
        private const string FILTER_PROFILE_FILES = "MadCatzProfileFiles";
        private const string FILTER_ALL_FILES = "AllFiles";
        private const string OK_TEXT = "OKText";
        private const string CANCEL_TEXT = "CancelText";

        private IMainPresenter presenter;

        private OpenFileDialog openProfileDialog;
        private SaveFileDialog saveProfileDialog;

        protected override Panel BaseComponent
        {
            get { return baseGrid; }
        }

        public IIconsView IconsView
        {
            get { return pEditor.IconsPanel; }
        }

        public ICommandsView CommandsView
        {
            get { return pEditor.CommandsPanel; }
        }

        public MainWindow()
        {
            InitializeComponent();

            openProfileDialog = new OpenFileDialog();
            openProfileDialog.Multiselect = true;
            openProfileDialog.Title = (string)App.Current.FindResource(DIALOG_TITLE_OPEN_PROFILES);
            openProfileDialog.Filter = (string)App.Current.FindResource(FILTER_PROFILE_FILES) + " (*.pr0)|*.pr0|" + (string)App.Current.FindResource(FILTER_ALL_FILES) + " (*.*)|*.*";

            saveProfileDialog = new SaveFileDialog();
            saveProfileDialog.Title = (string)App.Current.FindResource(DIALOG_TITLE_SAVE_PROFILE);
            saveProfileDialog.Filter = (string)App.Current.FindResource(FILTER_PROFILE_FILES) + " (*.pr0)|*.pr0|" + (string)App.Current.FindResource(FILTER_ALL_FILES) + " (*.*)|*.*";
            
            if (Directory.Exists(BASE_PROFILES_DIRECTORY))
            {
                openProfileDialog.InitialDirectory = BASE_PROFILES_DIRECTORY;
                saveProfileDialog.InitialDirectory = BASE_PROFILES_DIRECTORY;
            }
        }

        /// <summary>
        /// Initialization handler for this window.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">Event arguments.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "This method is an event handler for the WPF UI, so it is not recognized to be called by the analyzer.")]
        private void OnInitialized(object sender, EventArgs e)
        {
            exFile.IsExpanded = true;
            App.OnStartup(this);
        }

        public void SetPresenter(IMainPresenter mainPresenter)
        {
            this.presenter = mainPresenter;
        }

        public FileInfo[] SelectProfileFiles()
        {
            bool? success = openProfileDialog.ShowDialog();
            if (!success.HasValue || !success.Value)
            {
                return null;
            }

            var files = new FileInfo[openProfileDialog.FileNames.Length];

            for (int i = 0; i < files.Length; i++)
            {
                files[i] = new FileInfo(openProfileDialog.FileNames[i]);
            }

            return files;
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
            var content  = new ProfileButtonContentPanel(profileButton, profile.Name);
            content.OnCloseClick += OnProfileCloseClick;
            profileButton.Content = content;
            profileButton.Tag = profile;
            profileButton.SetResourceReference(RadioButton.StyleProperty, typeof(ToggleButton));
            profileButton.Height = 30;
            profileButton.Margin = new Thickness(2, 0, 3, 0);
            profileButton.Checked += OnProfileSelected;
            spProfiles.Children.Add(profileButton);

            profileButton.IsChecked = true;
            InvalidateVisual();
        }

        /// <summary>
        /// Callback for a close request of a profile.
        /// </summary>
        /// <param name="sender">Event sender, this is the RadioButton associated with the profile.</param>
        /// <param name="e">Event arguments.</param>
        private void OnProfileCloseClick(object sender, EventArgs e)
        {
            MadCatzProfile profile = (MadCatzProfile)((RadioButton)sender).Tag;
            presenter.CloseProfileRequested(profile);
        }

        /// <summary>
        /// Callback for the selection of a profile.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void OnProfileSelected(object sender, RoutedEventArgs e)
        {
            SetProfileDependingControlsEnabled(true);
            RadioButton button = (RadioButton)sender;
            MadCatzProfile profile = (MadCatzProfile)button.Tag;
            presenter.ActiveProfileChanged(profile);
        }

        /// <summary>
        /// Sets the enabled state of the controls that are only enabled if a profile is selected.
        /// </summary>
        /// <param name="enabled">Whether the controls shall be enabled.</param>
        private void SetProfileDependingControlsEnabled(bool enabled)
        {
            btSave.IsEnabled = enabled;
            btSaveAs.IsEnabled = enabled;
        }

        public void SetActiveProfile(MadCatzProfile activeProfile)
        {
            if (activeProfile == null)
            {
                pEditor.IconsPanel.ActiveProfileChanged(null);
                pEditor.CommandsPanel.ActiveProfileChanged(null);
                return;
            }

            GetButtonByProfile(activeProfile).IsChecked = true;
        }

        public void ProfileClosed(MadCatzProfile profile)
        {
            spProfiles.Children.Remove(GetButtonByProfile(profile));
            if (spProfiles.Children.Count == 0)
            {
                SetProfileDependingControlsEnabled(false);
            }
        }

        /// <summary>
        /// Gets the RadioButton associated with the given profile.
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <returns>The button associated with the profile.</returns>
        private RadioButton GetButtonByProfile(MadCatzProfile profile)
        {
            foreach (UIElement elem in spProfiles.Children)
            {
                RadioButton button = (RadioButton)elem;
                if (button.Tag == profile)
                {
                    return button;
                }
            }
            return null;
        }

        /// <summary>
        /// Event handler for a click on the save button.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void OnSaveClick(object sender, RoutedEventArgs e)
        {
            presenter.SaveActiveProfileRequested();
        }

        /// <summary>
        /// Event handler for a click on the save as button.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void OnSaveAsClick(object sender, RoutedEventArgs e)
        {
            presenter.SaveActiveProfileAsRequested();
        }

        public FileInfo SelectProfileSaveFile(MadCatzProfile profile, FileInfo currentSaveFile)
        {
            if (currentSaveFile != null)
            {
                saveProfileDialog.InitialDirectory = currentSaveFile.Directory.FullName;
                saveProfileDialog.FileName = currentSaveFile.Name;
            }
            bool? result = saveProfileDialog.ShowDialog();

            if (!result.HasValue || !result.Value)
            {
                return null;
            }

            return new FileInfo(saveProfileDialog.FileName);
        }

        public bool? ShowYesNoAbortQuestion(string message)
        {
            MessageBoxResult result = MessageBox.Show(message, "Claw", MessageBoxButton.YesNoCancel, MessageBoxImage.Question, MessageBoxResult.Cancel);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    return true;

                case MessageBoxResult.No:
                    return false;

                default:
                    return null;
            }
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

        private void OnExitClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Handles the OnClosing event to prevent losing changes made to profiles.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected override void OnClosing(CancelEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("e");
            }

            base.OnClosing(e);

            e.Cancel = e.Cancel || presenter.ExitApplicationRequested();
        }

        /// <summary>
        /// Handles clicks on the "New" button.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void OnNewClick(object sender, RoutedEventArgs e)
        {
            presenter.CreateNewProfileRequested();
        }

        public string ShowTextQuestion(string title, string message, char[] forbiddenChars)
        {
            return TextQuestionWindow.Show(title, message,
                (string)App.Current.FindResource(OK_TEXT),
                (string)App.Current.FindResource(CANCEL_TEXT),
                forbiddenChars);
        }

        public bool ShowYesNoQuestion(string question)
        {
            MessageBoxResult result = MessageBox.Show(question, "Claw", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            if (result == MessageBoxResult.Yes)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
