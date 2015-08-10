using Claw.Commands;
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
using ClawControl = Claw.Controllers.Controls.Control;

namespace Claw.UI.Controls
{
    /// <summary>
    /// Logic for AssignmentListBoxItemContent.xaml
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1036:OverrideMethodsOnComparableTypes")]
    public partial class AssignmentListBoxItemContent : UserControl, IComparable
    {
        private Command command;
        /// <summary>
        /// The command displayed by this item.
        /// </summary>
        public Command Command
        {
            get { return command; }
            set
            {
                command = value;
                CommandNameChanged();
            }
        }

        private ClawControl control;
        /// <summary>
        /// The control this item represents.
        /// </summary>
        public ClawControl Control
        {
            get { return control; }
        }
        /// <summary>
        /// The name of the device control.
        /// </summary>
        public string ControlName
        {
            get { return tbControl.Text; }
        }

        /// <summary>
        /// Creates a new AssignmentListBoxItemContent.
        /// </summary>
        /// <param name="ctrl">The control.</param>
        /// <param name="cmd">The command.</param>
        public AssignmentListBoxItemContent(ClawControl ctrl, Command cmd)
        {
            if (ctrl == null)
            {
                throw new ArgumentNullException("ctrl");
            }

            InitializeComponent();

            tbControl.Text = ctrl.Name;
            tbCommand.Text = cmd == null ? "" : cmd.Name;
            command = cmd;
            control = ctrl;
        }

        /// <summary>
        /// Tells this item, that it's command changed and needs to be reloaded.
        /// </summary>
        public void CommandNameChanged()
        {
            tbCommand.Text = command == null ? "" : command.Name;
        }

        public int CompareTo(AssignmentListBoxItemContent other)
        {
            if (other == null)
            {
                return 0;
            }

            return string.Compare(tbControl.Text, other.tbControl.Text, StringComparison.Ordinal);
        }

        public int CompareTo(object obj)
        {
            AssignmentListBoxItemContent content = obj as AssignmentListBoxItemContent;
            if (content != null)
            {
                return CompareTo(content);
            }
            else
            {
                return 0;
            }
        }
    }
}
