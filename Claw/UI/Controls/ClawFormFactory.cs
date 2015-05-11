using Claw.UI.Helper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Claw.UI.Controls
{
    /// <summary>
    /// Class containing some factory methods used by ClawForm. This is NOT an abstract factory.
    /// </summary>
    public static class ClawFormFactory
    {
        /// <summary>
        /// Creates a button for this form.
        /// </summary>
        /// <param name="image">The image for the button.</param>
        /// <param name="foreColor">Fore color the buttons image shall use.</param>
        /// <returns>The created button.</returns>
        public static ClawButton CreateButton(Image image, Color foreColor)
        {
            var button = new ClawButton();
            CreateImageForButton(button, image, foreColor);
            button.BackgroundImageLayout = ImageLayout.Stretch;
            button.Size = new Size(20, 20);
            return button;
        }

        /// <summary>
        /// Creates the image for a button and sets it to it.
        /// </summary>
        /// <param name="button">The button to set the image to.</param>
        /// <param name="image">The image to process and set to the button.</param>
        /// <param name="foreColor">Fore color the image shall use.</param>
        public static void CreateImageForButton(ClawButton button, Image image, Color foreColor)
        {
            if (button.BackgroundImage != null)
            {
                button.BackgroundImage.Dispose();
            }

            var bitmap = new Bitmap(image);
            BitmapHelper.Multiply(bitmap, foreColor);
            button.BackgroundImage = bitmap;
        }
    }
}
