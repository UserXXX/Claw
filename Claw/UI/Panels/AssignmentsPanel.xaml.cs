using Claw.Commands;
using Claw.Controllers;
using Claw.Controllers.Assignments;
using Claw.Controllers.Controls;
using Claw.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using ClawControl = Claw.Controllers.Controls.Control;

namespace Claw.UI.Panels
{
    /// <summary>
    /// Logic for AssignmentsPanel.xaml
    /// </summary>
    public partial class AssignmentsPanel : UserControl, IAssignmentView
    {
        private const string NO_COMMAND_ASSIGNED_TEXT = "NoCommandAssignedText";
        private const string CONTROL_TEXT = "ControlText";

        private IAssignmentPresenter presenter;

        private MadCatzProfile activeProfile;

        private bool blockChangeEvent = false;

        public AssignmentsPanel()
        {
            InitializeComponent();

            cbiRATMice.Tag = Controller.RatMiceIdentifier;
            cbiStrike7.Tag = Controller.Strike7Identifier;
            cbiStrike5.Tag = Controller.Strike5Identifier;
            cbiRATM.Tag = Controller.RatMIdentifier;
            cbiMOUS9.Tag = Controller.Mous9Identifier;
        }

        public void SetPresenter(IAssignmentPresenter assignPresenter)
        {
            presenter = assignPresenter;
        }

        public void ActiveProfileChanged(MadCatzProfile profile)
        {
            activeProfile = profile;

            lbControls.Items.Clear();
            if (profile == null)
            {
                cbDevice.SelectedItem = null;
                cbMode.SelectedItem = null;
                blockChangeEvent = true;
                cbAssigned.SelectedItem = null;
                blockChangeEvent = false;
                SetControlsEnabled(false);
                return;
            }

            SetControlsEnabled(true);
            cbDevice.Items.Clear();
            cbMode.Items.Clear();
            LoadActiveProfileData();
        }

        /// <summary>
        /// Loads the data from the active profile and displays it.
        /// </summary>
        private void LoadActiveProfileData()
        {
            LoadDevices();
            LoadCommands();
        }

        /// <summary>
        /// Loads the commands data from the active profile into the commands.
        /// </summary>
        private void LoadCommands()
        {
            blockChangeEvent = true;
            cbAssigned.Items.Clear();
            var emptyItem = new ComboBoxItem();
            emptyItem.Content = (string)Application.Current.FindResource(NO_COMMAND_ASSIGNED_TEXT);
            cbAssigned.Items.Add(emptyItem);

            foreach (Command command in activeProfile.Commands)
            {
                var item = new ComboBoxItem();
                item.Content = command.Name;
                item.Tag = command;
                cbAssigned.Items.Add(item);
            }

            cbAssigned.Items.SortDescriptions.Clear();
            cbAssigned.Items.SortDescriptions.Add(new SortDescription("Content", ListSortDirection.Ascending));
            blockChangeEvent = false;
        }

        /// <summary>
        /// Loads the device data from the active profile into the controls.
        /// </summary>
        private void LoadDevices()
        {
            cbDevice.Items.Add(cbiRATMice);
            cbDevice.Items.Add(cbiStrike7);
            cbDevice.Items.Add(cbiStrike5);
            cbDevice.Items.Add(cbiRATM);
            cbDevice.Items.Add(cbiMOUS9);

            foreach (Controller controller in activeProfile.Controllers)
            {
                if (controller.IsKnown())
                {
                    continue;
                }

                string name = "";
                foreach (Member member in controller.Members)
                {
                    name += "/" + member.Name;
                }

                var item = new ComboBoxItem();
                item.Content = name.Substring(1);
                item.Tag = controller.Uuid;
                cbDevice.Items.Add(item);
            }
            cbDevice.SelectedIndex = 0;
        }

        /// <summary>
        /// Sets whether user interaction controls are enabled.
        /// </summary>
        /// <param name="enabled">Whether the controls shall be enabled.</param>
        private void SetControlsEnabled(bool enabled)
        {
            cbDevice.IsEnabled = enabled;
            cbMode.IsEnabled = enabled;
        }

        /// <summary>
        /// Event handler for the selection changed event of the device combobox.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void OnSelectedDeviceChanged(object sender, SelectionChangedEventArgs e)
        {
            Guid selectedId = (Guid)((ComboBoxItem)cbDevice.SelectedItem).Tag;
            Controller selected = activeProfile.Controllers.GetControllerByIdentifier(selectedId);
            if (selected == null)
            {
                selected = presenter.RequestInsertController(activeProfile, selectedId);
            }

            cbMode.Items.Clear();
            foreach (Shift shift in selected.Shifts)
            {
                var item = new ComboBoxItem();
                item.Content = shift.Name;
                item.Tag = shift;
                cbMode.Items.Add(item);
            }
            cbMode.SelectedIndex = 0;
        }

        /// <summary>
        /// Event handler for the selection changed event of the mode combobox.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void OnSelectedModeChanged(object sender, SelectionChangedEventArgs e)
        {
            lbControls.Items.Clear();

            if (cbMode.SelectedItem == null)
            {
                return;
            }

            Shift shift = (Shift)((ComboBoxItem)cbMode.SelectedItem).Tag;

            Controller controller = activeProfile.Controllers.GetControllerByIdentifier((Guid)((ComboBoxItem)cbDevice.SelectedItem).Tag);
            foreach (ClawControl control in controller.Controls)
            {
                if (!(control is ButtonControl))
                {
                    continue;
                }

                Assignment assignment = shift.Assignments.GetAssignmentForControl(control);
                AssignmentListBoxItemContent content = CreateContent(control, assignment);

                var item = new ListBoxItem();
                item.Content = content;
                lbControls.Items.Add(item);
            }

            lbControls.Items.SortDescriptions.Clear();
            lbControls.Items.SortDescriptions.Add(new SortDescription("Content", ListSortDirection.Ascending));
        }

        /// <summary>
        /// Creates the content for an item that shall be inserted into the Control, Command listbox.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="assignment">The assignment for the control.</param>
        /// <returns>The created AssignmentListBoxItemContent.</returns>
        private AssignmentListBoxItemContent CreateContent(ClawControl control, Assignment assignment)
        {
            if (assignment == null)
            {
                return new AssignmentListBoxItemContent(control, null);
            }

            ButtonAssignment buttonAssignment = assignment as ButtonAssignment;

            if (buttonAssignment == null)
            {
                throw new InvalidOperationException("An assignment to a ButtonControl is not allowed to be something else than a ButtonAssignment! Got: " + assignment.GetType());
            }

            if (buttonAssignment.Bands.Count == 0)
            {
                return new AssignmentListBoxItemContent(control, null);
            }

            // Currently there is alway only one node bound so looking at the first item is sufficient.
            Band band = buttonAssignment.Bands.First();
            Command command = activeProfile.Commands.GetCommandForBand(band);
            return new AssignmentListBoxItemContent(control, command);
        }

        public void CommandNameChanged(Command command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            foreach (object item in lbControls.Items)
            {
                AssignmentListBoxItemContent content = (AssignmentListBoxItemContent)((ListBoxItem)item).Content;
                Command shown = content.Command;
                if (command.Equals(shown))
                {
                    content.CommandNameChanged();
                }
            }

            OnSelectedControlChanged(lbControls, null);
        }

        /// <summary>
        /// Event handler for the selection changed event of the control, command listbox.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Literale nicht als lokalisierte Parameter übergeben", MessageId = "System.Windows.Controls.TextBlock.set_Text(System.String)")]
        private void OnSelectedControlChanged(object sender, SelectionChangedEventArgs e)
        {
            blockChangeEvent = true;
            if (lbControls.SelectedItem == null)
            {
                cbAssigned.IsEnabled = false;
                cbAssigned.SelectedItem = null;
                tbSelectedControl.Text = (string)Application.Current.FindResource(CONTROL_TEXT);
                blockChangeEvent = false;
                return;
            }

            cbAssigned.IsEnabled = true;

            AssignmentListBoxItemContent content = (AssignmentListBoxItemContent)((ListBoxItem)lbControls.SelectedItem).Content;
            if (content.Command == null)
            {
                // The index 0 is always the empty item.
                cbAssigned.SelectedIndex = 0;
                tbSelectedControl.Text = (string)Application.Current.FindResource(CONTROL_TEXT);
                blockChangeEvent = false;
                return;
            }

            tbSelectedControl.Text = (string)Application.Current.FindResource(CONTROL_TEXT) + " " + content.ControlName;

            foreach (ComboBoxItem item in cbAssigned.Items)
            {
                Command tag = (Command)item.Tag;
                if (content.Command.Equals(tag))
                {
                    cbAssigned.SelectedItem = item;
                    break;
                }
            }
            blockChangeEvent = false;
        }

        /// <summary>
        /// Event handler for the selection changed event of the assigned command combobox.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void OnAssignedCommandChanged(object sender, SelectionChangedEventArgs e)
        {
            if (blockChangeEvent)
            {
                return;
            }

            if (lbControls.SelectedItem == null)
            {
                return;
            }

            if (cbMode.SelectedItem == null)
            {
                return;
            }

            object tag = ((ComboBoxItem)cbAssigned.SelectedItem).Tag;
            Command selected = tag == null ? null : (Command)tag;
            ClawControl control = ((AssignmentListBoxItemContent)((ListBoxItem)lbControls.SelectedItem).Content).Control;
            Shift shift = (Shift)((ListBoxItem)cbMode.SelectedItem).Tag;
            presenter.OnAssociateCommandRequested(shift, control, selected);
        }

        public void ControlAssociationChanged(Shift shift, ClawControl control, Command command)
        {
            if (cbMode.SelectedItem == null)
            {
                return;
            }

            if (!((ComboBoxItem)cbMode.SelectedItem).Tag.Equals(shift))
            {
                return;
            }

            foreach (ListBoxItem item in lbControls.Items)
            {
                AssignmentListBoxItemContent content = (AssignmentListBoxItemContent)item.Content;
                if (content.Control.Equals(control))
                {
                    content.Command = command;
                }
            }

            if (((AssignmentListBoxItemContent)(((ListBoxItem)lbControls.SelectedItem).Content)).Control.Equals(control))
            {
                OnSelectedControlChanged(lbControls, null);
            }
        }
    }
}
