using Claw.Blasts;
using Claw.Commands;
using Claw.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Media;
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

            SortCommandList();
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

        /// <summary>
        /// Event handler for the preview text input event of the name text box.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void OnNamePreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // If the user typed a ', we stop calling other events.
            e.Handled = e.Text == "\'";
        }

        /// <summary>
        /// Event handler for the pasting event of the name text box.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void OnNamePasting(object sender, DataObjectPastingEventArgs e)
        {
            // If there was a ' pasted into the textbox, we cancel that action.
            if (e.DataObject.GetData(typeof(string)).ToString().Contains('\''))
            {
                e.CancelCommand();
                SystemSounds.Exclamation.Play();
            }
        }

        /// <summary>
        /// Event handler for the text changed event of the name text box.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void OnNameTextChanged(object sender, TextChangedEventArgs e)
        {
            presenter.OnNameChangeRequested((Command)((ListBoxItem)lbCommands.SelectedItem).Tag, tbName.Text);
        }
        
        public void CommandNameChanged(Command command)
        {
            ListBoxItem item = GetItemByCommand(command);
            item.Content = command.Name;
            item.InvalidateVisual();
            SortCommandList();
        }

        /// <summary>
        /// Sorts the items in the list box for commands.
        /// </summary>
        private void SortCommandList()
        {
            lbCommands.Items.SortDescriptions.Clear();
            lbCommands.Items.SortDescriptions.Add(new SortDescription("Content", ListSortDirection.Ascending));
        }

        /// <summary>
        /// Searches the list box item associated with the given command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>The list box item or null if none was found.</returns>
        private ListBoxItem GetItemByCommand(Command command)
        {
            foreach (object obj in lbCommands.Items)
            {
                if (command == ((ListBoxItem)obj).Tag)
                {
                    return (ListBoxItem)obj;
                }
            }
            return null;
        }
    }
}
