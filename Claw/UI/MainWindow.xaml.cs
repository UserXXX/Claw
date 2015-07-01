using Claw.UI.Controls;
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
using System.Windows.Shapes;

namespace Claw.UI
{
    /// <summary>
    /// The applications main window.
    /// </summary>
    public partial class MainWindow : ClawWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialization handler for this window.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void OnInitialized(object sender, EventArgs e)
        {
            exFile.IsExpanded = true;
        }

        protected override Panel GetBaseComponent()
        {
            return baseGrid;
        }

        /// <summary>
        /// Handles the StateChanged event.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
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
    }
}
