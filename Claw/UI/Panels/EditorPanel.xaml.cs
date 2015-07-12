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
        public EditorPanel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Notifies this panel that the selected profile changed.
        /// </summary>
        /// <param name="activeProfile">The new active profile.</param>
        public void ActiveProfileChanged(MadCatzProfile activeProfile)
        {
            pIcons.ActiveProfileChanged(activeProfile);
        }
    }
}
