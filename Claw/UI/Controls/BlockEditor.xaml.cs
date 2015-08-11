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

namespace Claw.UI.Controls
{
    /// <summary>
    /// Logic for BlockEditor.xaml
    /// </summary>
    public partial class BlockEditor : UserControl
    {
        /// <summary>
        /// Dependencyproperty for the BlockEditor title.
        /// </summary>
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(BlockEditor), new PropertyMetadata(""));

        /// <summary>
        /// Title of the BlockEditor.
        /// </summary>
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public BlockEditor()
        {
            InitializeComponent();
        }


    }
}
