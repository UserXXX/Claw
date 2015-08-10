using Claw.Blasts;
using Claw.UI.Controls;
using Claw.UI.Helper;
using Claw.UI.Panels;
using Claw.UI.Style;
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

namespace Claw.UI.Windows
{
    /// <summary>
    /// Logic for ChooseIconWindow.xaml
    /// </summary>
    public partial class ChooseIconWindow : ClawWindow
    {
        /// <summary>
        /// Shows a selection dialog for the given blasts.
        /// </summary>
        /// <param name="options">The blasts the user can select from.</param>
        /// <param name="selected">The initially selected blast.</param>
        /// <returns>An instance of ChooseIconWindowResult representing the dialogs result.</returns>
        public static ChooseIconWindowResult ShowDialog(IList<Blast> options, Blast selected)
        {
            var window = new ChooseIconWindow(options, selected);
            window.ShowDialog();
            Blast sel = null;
            ListViewItem selItem = (ListViewItem)window.lvBlasts.SelectedItem;
            if (selItem.Tag != null)
            {
                sel = (Blast)selItem.Tag;
            }
            return new ChooseIconWindowResult(!window.success, sel);
        }

        protected override Panel BaseComponent
        {
            get { return baseGrid; }
        }

        private bool success = false;

        private ChooseIconWindow(IList<Blast> options, Blast selected)
        {
            InitializeComponent();

            foreach (Blast blast in options)
            {
                var imageControl = ImageHelper.CreateImageControl(blast.GetData());
                var grid = new Grid();
                grid.Children.Add(imageControl);
                var item = new ListViewItem();
                item.Content = grid;
                item.Tag = blast;
                lvBlasts.Items.Add(item);

                if (blast == selected)
                {
                    lvBlasts.SelectedItem = item;
                }
            }
        }

        private void OnOKClick(object sender, RoutedEventArgs e)
        {
            success = true;
            Close();
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
