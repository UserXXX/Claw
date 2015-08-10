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
    /// Logic for EditorPanel.xaml.
    /// </summary>
    public partial class EditorPanel : UserControl
    {
        /// <summary>
        /// The icons panel.
        /// </summary>
        public IIconsView IconsPanel
        {
            get { return pIcons; }
        }

        /// <summary>
        /// The commands panel.
        /// </summary>
        public ICommandsView CommandsPanel
        {
            get { return pCommands; }
        }

        /// <summary>
        /// The assignments panel.
        /// </summary>
        public IAssignmentView AssignPanel
        {
            get { return pAssignments; }
        }

        public EditorPanel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Event handler for clicks on the icons button.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void OnEditIconsClick(object sender, RoutedEventArgs e)
        {
            pIcons.Visibility = Visibility.Visible;
            pCommands.Visibility = Visibility.Hidden;
            pAssignments.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Event handler for clicks on the commands button.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void OnEditCommandsClick(object sender, RoutedEventArgs e)
        {
            pIcons.Visibility = Visibility.Hidden;
            pCommands.Visibility = Visibility.Visible;
            pAssignments.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Event handler for clicks on the assignments button.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void OnAssignCommandsClick(object sender, RoutedEventArgs e)
        {
            pIcons.Visibility = Visibility.Hidden;
            pCommands.Visibility = Visibility.Hidden;
            pAssignments.Visibility = Visibility.Visible;
        }
    }
}
