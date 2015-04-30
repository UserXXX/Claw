using Claw.UI.Helper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Claw.UI
{
    public class ClawForm : Form
    {
        private readonly Color[] TransparentColors = {
                                                         Color.Pink,
                                                         Color.Violet,
                                                         Color.LightGreen,
                                                     };

        private Image tileImage;
        private Image processedTileImage;

        private int maxScreenWidth = 0;
        private int maxScreenHeight = 0;

        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        private static extern bool ReleaseCapture();

        public Image TileImage
        {
            get { return tileImage; }
            set
            {
                tileImage = value;
                ProcessTileImage();
            }
        }

        public sealed override Color ForeColor
        {
            get { return base.ForeColor; }
            set
            {
                base.ForeColor = value;
                ProcessTileImage();
            }
        }

        public new FormBorderStyle FormBorderStyle
        {
            get { return base.FormBorderStyle; }
            set { }
        }

        protected ClawForm()
        {
            tileImage = new Bitmap(100, 100, PixelFormat.Format32bppArgb);
            using (Graphics graph = Graphics.FromImage(tileImage))
            {
                graph.FillEllipse(new SolidBrush(Color.White), new Rectangle(0, 0, 100, 100));
            }

            MouseDown += new MouseEventHandler(ClawFormMouseDown);
            base.FormBorderStyle = FormBorderStyle.None;

            foreach (Screen screen in Screen.AllScreens)
            {
                if (screen.WorkingArea.Width > maxScreenWidth)
                    maxScreenWidth = screen.WorkingArea.Width;
                if (screen.WorkingArea.Height > maxScreenHeight)
                    maxScreenHeight = screen.WorkingArea.Height;
            }

            BackColor = Color.Black;
            ForeColor = Color.Red;
        }

        private void ClawFormMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void ProcessTileImage()
        {
            using (var imageCopy = new Bitmap(tileImage))
            {
                BitmapHelper.Multiply(imageCopy, ForeColor);

                var newImage = new Bitmap(maxScreenWidth, maxScreenHeight, PixelFormat.Format32bppArgb);
                using (Graphics newImageGraphics = Graphics.FromImage(newImage))
                {
                    newImageGraphics.Clear(Color.Transparent);

                    // Draw the re-colored image in tiles to the new image
                    for (var x = 0; x < (maxScreenWidth / tileImage.Width) + 1; x++)
                    {
                        for (var y = 0; y < (maxScreenHeight / tileImage.Height) + 1; y++)
                        {
                            newImageGraphics.DrawImageUnscaled(imageCopy, new Point(x * tileImage.Width, y * tileImage.Height));
                        }
                    }
                }

                if (processedTileImage != null)
                {
                    processedTileImage.Dispose();
                }
                processedTileImage = newImage;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            using (var background = new Bitmap(Width, Height, PixelFormat.Format32bppArgb))
            {
                Color transparentColor = GetTransparentColor();

                using (Graphics graph = Graphics.FromImage(background))
                {
                    graph.Clear(BackColor);

                    Rectangle srcRectangle = new Rectangle((processedTileImage.Width - Width) / 2, (processedTileImage.Height - Height) / 2, Width, Height);
                    Rectangle destRectangle = new Rectangle(0, 0, Width, Height);
                    graph.DrawImage(processedTileImage, destRectangle, srcRectangle, GraphicsUnit.Pixel);

                    Rectangle gradientRectangle = new Rectangle(0, 0, Width, Height);
                    Color gradientSrcColor = Color.FromArgb(50, BackColor);
                    // 191 is about 3/4 of 255.
                    Color gradientDestColor = Color.FromArgb(191, BackColor);

                    using (var gradientBrush = new LinearGradientBrush(gradientRectangle, gradientSrcColor, gradientDestColor, LinearGradientMode.Horizontal))
                    {
                        graph.FillRectangle(gradientBrush, gradientRectangle);
                    }

                    // Draw the edges
                    float widthDiff = Width * 0.01f;
                    float heightDiff = Height * 0.1f;
                    using (var brush = new SolidBrush(transparentColor))
                    {
                        graph.FillPolygon(brush, new PointF[] {
                            new PointF(0, 0),
                            new PointF(widthDiff, 0),
                            new PointF(0, heightDiff),
                        });
                        graph.FillPolygon(brush, new PointF[] {
                            new PointF(Width, 0),
                            new PointF(Width - widthDiff, 0),
                            new PointF(Width, heightDiff),
                        });
                        graph.FillPolygon(brush, new PointF[] {
                            new PointF(0, Height),
                            new PointF(widthDiff, Height),
                            new PointF(0, Height - heightDiff),
                        });
                        graph.FillPolygon(brush, new PointF[] {
                            new PointF(Width, Height),
                            new PointF(Width - widthDiff, Height),
                            new PointF(Width, Height - heightDiff),
                        });
                    }

                    // Draw the border
                    using (var pen = new Pen(ForeColor))
                    {
                        graph.DrawPolygon(pen, new PointF[] {
                            new PointF(widthDiff, 0),
                            new PointF(Width - widthDiff, 0),
                            new PointF(Width - 1, heightDiff),
                            new PointF(Width - 1, Height - heightDiff),
                            new PointF(Width - widthDiff, Height- 1),
                            new PointF(widthDiff, Height - 1),
                            new PointF(0, Height - heightDiff),
                            new PointF(0, heightDiff),
                        });
                    }
                }

                // Set the transparency key, so everything drawn in that color will be completely transparent.
                TransparencyKey = transparentColor;
                e.Graphics.DrawImageUnscaled(background, new Point(0, 0));
            }
        }

        /// <summary>
        /// Gets the best transparent color based on the fore and back colors.
        /// </summary>
        /// <returns>The selected color.</returns>
        private Color GetTransparentColor()
        {
            var ret = Color.Pink;
            var found = false;
            for (var i = 0; i < TransparentColors.Length && !found; i++)
            {
                if (TransparentColors[i] != ForeColor && TransparentColors[i] != BackColor)
                {
                    found = true;
                    ret = TransparentColors[i];
                }
            }
            return ret;
        }
    }
}
