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
    /// <summary>
    /// Base class of all forms used by Claw.
    /// This class is not allowed to be abstract because the designer doesn't allo
    /// </summary>
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

        private const int EDGE_WIDTH = 20;
        private const int EDGE_HEIGHT = 60;

        private const int RESIZE_BORDER = 4;

        private CursorLocation cursorLocation = CursorLocation.Default;
        private enum CursorLocation
        {
            Top,
            Left,
            Right,
            Bottom,
            TopLeft,
            TopRight,
            BottomLeft,
            BottomRight,
            Default,
        }

        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        private static extern bool ReleaseCapture();

        /// <summary>
        /// Image that will get tiled over the background.
        /// </summary>
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

        /// <summary>
        /// The style of the window borders.
        /// </summary>
        public new FormBorderStyle FormBorderStyle
        {
            get { return base.FormBorderStyle; }
            set { }
        }

        public override Size MaximumSize
        {
            get { return base.MaximumSize; }
            set { base.MaximumSize = new Size(Math.Min(value.Width, maxScreenWidth), Math.Min(value.Height, maxScreenHeight)); }
        }

        public override Size MinimumSize
        {
            get { return base.MinimumSize; }
            set { base.MinimumSize = new Size(Math.Max(value.Width, 2 * EDGE_WIDTH), Math.Max(value.Height, 2 * EDGE_HEIGHT)); }
        }

        /// <summary>
        /// Creates a new ClawForm.
        /// </summary>
        protected ClawForm()
        {
            // Create a dummy tile image
            tileImage = new Bitmap(100, 100, PixelFormat.Format32bppArgb);
            using (Graphics graph = Graphics.FromImage(tileImage))
            {
                graph.FillEllipse(new SolidBrush(Color.White), new Rectangle(0, 0, 100, 100));
            }

            MouseDown += new MouseEventHandler(ClawFormMouseDown);
            MouseMove += new MouseEventHandler(ClawFormMouseMove);
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
            TransparencyKey = GetTransparentColor();
            DoubleBuffered = true;
        }

        private void ClawFormMouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && cursorLocation != CursorLocation.Default)
            {
                ResizeForm(e.X, e.Y);
            }
            else
            {
                if (e.X > EDGE_WIDTH && e.X < Width - EDGE_WIDTH)
                {
                    if (e.Y < RESIZE_BORDER)
                    {
                        cursorLocation = CursorLocation.Top;
                        Cursor = Cursors.SizeNS;
                        return;
                    }
                    if (e.Y > Height - (RESIZE_BORDER + 1))
                    {
                        cursorLocation = CursorLocation.Bottom;
                        Cursor = Cursors.SizeNS;
                        return;
                    }
                }
                if (e.Y > EDGE_HEIGHT && e.Y < Height - EDGE_HEIGHT)
                {
                    if (e.X < RESIZE_BORDER)
                    {
                        cursorLocation = CursorLocation.Left;
                        Cursor = Cursors.SizeWE;
                        return;
                    }
                    if (e.X > Width - (RESIZE_BORDER + 1))
                    {
                        cursorLocation = CursorLocation.Right;
                        Cursor = Cursors.SizeWE;
                        return;
                    }
                }
                if (e.X < EDGE_WIDTH && e.Y < EDGE_HEIGHT && e.Y >= EDGE_HEIGHT - (EDGE_HEIGHT / EDGE_WIDTH) * e.X && e.Y <= EDGE_HEIGHT - (EDGE_HEIGHT / EDGE_WIDTH) * e.X + RESIZE_BORDER)
                {
                    cursorLocation = CursorLocation.TopLeft;
                    Cursor = Cursors.SizeNWSE;
                    return;
                }
                if (e.X > Width - EDGE_WIDTH && e.Y < EDGE_HEIGHT && e.Y >= (EDGE_HEIGHT / EDGE_WIDTH) * e.X + EDGE_HEIGHT - (EDGE_HEIGHT / EDGE_WIDTH) * Width && e.Y <= (EDGE_HEIGHT / EDGE_WIDTH) * e.X + EDGE_HEIGHT - (EDGE_HEIGHT / EDGE_WIDTH) * Width + RESIZE_BORDER)
                {
                    cursorLocation = CursorLocation.TopRight;
                    Cursor = Cursors.SizeNESW;
                    return;
                }
                if (e.X < EDGE_WIDTH && e.Y > Height - EDGE_HEIGHT && e.Y <= Height - (EDGE_HEIGHT - (EDGE_HEIGHT / EDGE_WIDTH) * e.X) && e.Y >= Height - (EDGE_HEIGHT - (EDGE_HEIGHT / EDGE_WIDTH) * e.X) - RESIZE_BORDER)
                {
                    cursorLocation = CursorLocation.BottomLeft;
                    Cursor = Cursors.SizeNESW;
                    return;
                }
                if (e.X > Width - EDGE_WIDTH && e.Y > Height - EDGE_HEIGHT && e.Y <= Height - ((EDGE_HEIGHT / EDGE_WIDTH) * e.X + EDGE_HEIGHT - (EDGE_HEIGHT / EDGE_WIDTH) * Width) && e.Y >= Height - ((EDGE_HEIGHT / EDGE_WIDTH) * e.X + EDGE_HEIGHT - (EDGE_HEIGHT / EDGE_WIDTH) * Width) - RESIZE_BORDER)
                {
                    cursorLocation = CursorLocation.BottomRight;
                    Cursor = Cursors.SizeNWSE;
                    return;
                }

                cursorLocation = CursorLocation.Default;
                Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// Handles the resize logic.
        /// </summary>
        /// <param name="x">Mouse x coordinate.</param>
        /// <param name="y">Mouse y coordinate.</param>
        private void ResizeForm(int x, int y)
        {
            switch (cursorLocation)
            {
                case CursorLocation.Bottom:
                    Height = y;
                    break;

                case CursorLocation.Left:
                    Width -= x;
                    Left += x;
                    break;

                case CursorLocation.Right:
                    Width = x;
                    break;

                case CursorLocation.Top:
                    Height -= y;
                    Top += y;
                    break;

                case CursorLocation.BottomLeft:
                    Height = y;
                    Width -= x;
                    Left += x;
                    break;

                case CursorLocation.BottomRight:
                    Height = y;
                    Width = x;
                    break;

                case CursorLocation.TopLeft:
                    Width -= x;
                    Left += x;
                    Height -= y;
                    Top += y;
                    break;

                case CursorLocation.TopRight:
                    Height -= y;
                    Top += y;
                    Width = x;
                    break;
            }
            Invalidate();
        }

        private void ClawFormMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (cursorLocation == CursorLocation.Default && IsFormVisibleAt(e.Location))
                {
                    ReleaseCapture();
                    SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
                }
            }
        }

        /// <summary>
        /// Checks whether the form is visible at the given point.
        /// </summary>
        /// <param name="point">The location to check. This is assumed to be the position relative to the top left corner of the form.</param>
        /// <returns>Whether the form is visible at <paramref name="point"/></returns>
        private bool IsFormVisibleAt(Point point)
        {
            return (point.X > EDGE_WIDTH && point.X < Width - EDGE_WIDTH) ||
                (point.Y > EDGE_HEIGHT && point.Y < Height - EDGE_HEIGHT) ||
                (point.X <= EDGE_WIDTH && point.Y > EDGE_HEIGHT - (EDGE_HEIGHT / EDGE_WIDTH) * point.X && point.Y < Height - (EDGE_HEIGHT - (EDGE_HEIGHT / EDGE_WIDTH) * point.X)) ||
                (point.X >= Width - EDGE_WIDTH && point.Y > (EDGE_HEIGHT / EDGE_WIDTH) * point.X + EDGE_HEIGHT - (EDGE_HEIGHT / EDGE_WIDTH) * Width && point.Y < Height - ((EDGE_HEIGHT / EDGE_WIDTH) * point.X + EDGE_HEIGHT - (EDGE_HEIGHT / EDGE_WIDTH) * Width));
        }

        /// <summary>
        /// Processes the tile image. This method creates a back buffer in size of the maximum screen components the tile image is tiled over.
        /// Because of the size restrictions of the form, it gets quite simple to draw the background: Select a rectangle from this huge background and draw it.
        /// </summary>
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
                    using (var brush = new SolidBrush(transparentColor))
                    {
                        graph.FillPolygon(brush, new PointF[] {
                            new PointF(0, 0),
                            new PointF(EDGE_WIDTH, 0),
                            new PointF(0, EDGE_HEIGHT),
                        });
                        graph.FillPolygon(brush, new PointF[] {
                            new PointF(Width, 0),
                            new PointF(Width - EDGE_WIDTH, 0),
                            new PointF(Width, EDGE_HEIGHT),
                        });
                        graph.FillPolygon(brush, new PointF[] {
                            new PointF(0, Height),
                            new PointF(EDGE_WIDTH, Height),
                            new PointF(0, Height - EDGE_HEIGHT),
                        });
                        graph.FillPolygon(brush, new PointF[] {
                            new PointF(Width, Height),
                            new PointF(Width - EDGE_WIDTH, Height),
                            new PointF(Width, Height - EDGE_HEIGHT),
                        });
                    }

                    // Draw the border
                    int borderWidth = 3;
                    using (var pen = new Pen(ForeColor, borderWidth))
                    {
                        graph.DrawPolygon(pen, new PointF[] {
                            new PointF(EDGE_WIDTH, 0),
                            new PointF(Width - EDGE_WIDTH, 0),
                            new PointF(Width - 1, EDGE_HEIGHT),
                            new PointF(Width - 1, Height - EDGE_HEIGHT),
                            new PointF(Width - EDGE_WIDTH, Height- 1),
                            new PointF(EDGE_WIDTH, Height - 1),
                            new PointF(0, Height - EDGE_HEIGHT),
                            new PointF(0, EDGE_HEIGHT),
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
