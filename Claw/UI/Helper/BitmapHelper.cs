using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Claw.UI.Helper
{
    /// <summary>
    /// Helper class for Bitmap handling.
    /// </summary>
    public static class BitmapHelper
    {
        /// <summary>
        /// Multiplies the color of each pixel on the bitmap with the given color.
        /// </summary>
        /// <param name="bitmap">The bitmap to edit.</param>
        /// <param name="color">The color to multiply with.</param>
        public static void Multiply(Bitmap bitmap, Color color)
        {
            for (var x = 0; x < bitmap.Width; x++)
            {
                for (var y = 0; y < bitmap.Height; y++)
                {
                    Color original = bitmap.GetPixel(x, y);
                    Color newColor = Color.FromArgb(
                        Multiply(original.A, color.A),
                        Multiply(original.R, color.R),
                        Multiply(original.G, color.G),
                        Multiply(original.B, color.B));
                    bitmap.SetPixel(x, y, newColor);
                }
            }
        }

        /// <summary>
        /// Multiplies two bytes to a color attribute.
        /// </summary>
        /// <param name="attr1">The first byte.</param>
        /// <param name="attr2">The second byte.</param>
        /// <returns>The logical color multiplication of both.</returns>
        public static int Multiply(byte attr1, byte attr2)
        {
            float val1 = ((float)attr1) / 255.0f;
            float val2 = ((float)attr2) / 255.0f;
            float result = val1 * val2 * 255.0f;
            // Min / Max check is for numeric stability.
            return Math.Min(255, Math.Max(0, (int)result));
        }
    }
}
