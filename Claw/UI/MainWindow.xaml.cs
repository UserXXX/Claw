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
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ClawWindow
    {
        public MainWindow()
        {
            InitializeComponent();
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
    }
}
