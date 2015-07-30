using Claw.Blasts;
using Claw.Commands;
using Claw.Interfaces;
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
    /// Logic for CommandsPanel.xaml
    /// </summary>
    public partial class CommandsPanel : UserControl, ICommandsView
    {
        private ICommandsPresenter presenter;

        public CommandsPanel()
        {
            InitializeComponent();
        }

        public void SetPresenter(ICommandsPresenter newPresenter)
        {
            presenter = newPresenter;
        }

        public void ActiveProfileChanged(MadCatzProfile activeProfile)
        {
            lbCommands.Items.Clear();

            if (activeProfile == null)
            {
                btCreate.IsEnabled = false;
                return;
            }

            btCreate.IsEnabled = true;
            foreach (Command command in activeProfile.Commands)
            {
                var item = new ListBoxItem();
                item.Content = command.Name;
                item.Tag = command;
                lbCommands.Items.Add(item);
            }
        }

        /// <summary>
        /// Event handler for the change event of the commands list box.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void OnCommandSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetSingleSelectControlsEnabled(lbCommands.SelectedItems.Count == 1);
            SetMultiSelectControlsEnabled(lbCommands.SelectedItems.Count > 0);

            if (lbCommands.SelectedItems.Count == 1)
            {
                ListBoxItem item = (ListBoxItem)lbCommands.SelectedItem;
                ShowCommandDetails((Command)item.Tag);
            }
            else
            {
                ShowCommandDetails(null);
            }
        }

        /// <summary>
        /// Sets the controls properties to the values in the given command.
        /// </summary>
        /// <param name="command">The command, if null is passed, the UI is cleared.</param>
        private void ShowCommandDetails(Command command)
        {
            if (command == null)
            {
                tbName.Text = "";
                iIcon.Source = null;
                tbNoImage.Visibility = Visibility.Hidden;
                return;
            }

            tbName.Text = command.Name;
            MadCatzProfile profile = presenter.ActiveProfile;
            Blast blast = profile.Blasts.GetBlastForCommand(command);
            if (blast != null)
            {
                iIcon.Source = ImageHelper.CreateImage(blast.GetData());
                tbNoImage.Visibility = Visibility.Hidden;
                iIcon.Visibility = Visibility.Visible;
            }
            else
            {
                tbNoImage.Visibility = Visibility.Visible;
                iIcon.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// Sets the enabled state for controls that only work if at least one item is selected.
        /// </summary>
        /// <param name="enabled">Whether to enable or disable the controls.</param>
        private void SetMultiSelectControlsEnabled(bool enabled)
        {
            btDelete.IsEnabled = enabled;
        }

        /// <summary>
        /// Sets the enabled state for controls that only work if exactly one item is selected.
        /// </summary>
        /// <param name="enabled">Whether to enable or disable the controls.</param>
        private void SetSingleSelectControlsEnabled(bool enabled)
        {
            tbName.IsEnabled = enabled;
            iIcon.IsEnabled = enabled;

            if (enabled)
            {
                bImage.SetResourceReference(Border.BorderBrushProperty, "MidBrush");
            }
            else
            {
                bImage.SetResourceReference(Border.BorderBrushProperty, "DisabledMidBrush");
            }
        }
    }
}
