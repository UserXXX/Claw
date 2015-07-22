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

namespace Claw.UI.Windows
{
    /// <summary>
    /// Logic for TextQuestionWindow.xaml
    /// </summary>
    public partial class TextQuestionWindow : ClawWindow
    {
        /// <summary>
        /// Shows a question dialog with a text field that asks the user to input something.
        /// </summary>
        /// <param name="title">Dialog title.</param>
        /// <param name="message">Message displayed to the user.</param>
        /// <param name="okButtonText">Text displayed on the OK button.</param>
        /// <param name="cancelButtonText">Text displayed on the cancel button.</param>
        /// <returns>The user input or null if the dialog was aborted.</returns>
        public static string Show(string title, string message, string okButtonText, string cancelButtonText)
        {
            var window = new TextQuestionWindow(title, message, okButtonText, cancelButtonText);
            window.Owner = Application.Current.MainWindow;
            window.ShowDialog();
            
            if (!window.success)
            {
                return null;
            }

            return window.tbAnswer.Text;
        }

        private bool success = false;

        protected override Panel BaseComponent
        {
            get { return baseGrid; }
        }

        private TextQuestionWindow(string title, string message, string okButtonText, string cancelButtonText)
        {
            InitializeComponent();

            Title = title;
            tbQuestion.Text = message;
            btOK.Content = okButtonText;
            btCancel.Content = cancelButtonText;
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

        private void OnKeyPress(object sender, KeyEventArgs e)
        {
            if (e.IsDown && e.Key == Key.Enter)
            {
                OnOKClick(sender, e);
            }
        }
    }
}
