using Claw.UI.Helper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Claw.UI.Controls
{
    /// <summary>
    /// Base class of all forms used by Claw.
    /// This class is not allowed to be abstract because the designer doesn't allo
    /// </summary>
    public class ClawForm : Form
    {
        private ClawFormPainter painter;

        private Image tileImage;
        private Image processedTileImage;

        private int maxScreenWidth = 0;
        private int maxScreenHeight = 0;

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

        private ClawButton btClose;
        private ClawButton btMaximize;
        private ClawButton btNormalize;
        private ClawButton btMinimize;

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
            set { base.MinimumSize = new Size(Math.Max(value.Width, 2 * painter.EdgeWidth), Math.Max(value.Height, 2 * painter.EdgeHeight)); }
        }

        /// <summary>
        /// The processed tile image, prepared for rendering.
        /// </summary>
        public Image ProcessedTileImage
        {
            get { return processedTileImage; }
        }

        /// <summary>
        /// Creates a new ClawForm.
        /// </summary>
        protected ClawForm()
        {
            painter = new ClawFormPainter();

            // Create a dummy tile image
            tileImage = new Bitmap(100, 100, PixelFormat.Format32bppArgb);
            using (Graphics graph = Graphics.FromImage(tileImage))
            {
                graph.FillEllipse(new SolidBrush(Color.White), new Rectangle(0, 0, 100, 100));
            }

            foreach (Screen screen in Screen.AllScreens)
            {
                if (screen.WorkingArea.Width > maxScreenWidth)
                    maxScreenWidth = screen.WorkingArea.Width;
                if (screen.WorkingArea.Height > maxScreenHeight)
                    maxScreenHeight = screen.WorkingArea.Height;
            }

            MouseDown += new MouseEventHandler(ClawFormMouseDown);
            MouseMove += new MouseEventHandler(ClawFormMouseMove);
            base.FormBorderStyle = FormBorderStyle.None;
            MinimumSize = new Size(2 * painter.EdgeWidth, 2 * painter.EdgeHeight);
            MaximumSize = new Size(maxScreenWidth, maxScreenHeight);
            DoubleBuffered = true;

            CreateFormButtons();
            LookChanged(this, new EventArgs());
            TransparencyKey = painter.GetTransparentColor(this);

            LookAndFeel.Instance.Changed += LookChanged;
        }

        /// <summary>
        /// Creates the buttons for form control, such as close, maximize, minimize etc.
        /// </summary>
        private void CreateFormButtons()
        {
            LookAndFeel lAndF = LookAndFeel.Instance;

            btClose = ClawFormFactory.CreateButton(lAndF.CloseImage, ForeColor);
            btClose.TabIndex = int.MaxValue;
            btClose.Location = new Point(Width - 45, 10);
            btClose.Click += btCloseClick;
            Controls.Add(btClose);

            btMaximize = ClawFormFactory.CreateButton(lAndF.MaximizeImage, ForeColor);
            btMaximize.TabIndex = int.MaxValue - 1;
            btMaximize.Location = new Point(Width - 70, 10);
            btMaximize.Click += btMaximizeClick;
            Controls.Add(btMaximize);

            btNormalize = ClawFormFactory.CreateButton(lAndF.NormalizeImage, ForeColor);
            btNormalize.TabIndex = int.MaxValue - 1;
            btNormalize.Location = new Point(Width - 55, 10);
            btNormalize.Visible = false;
            btNormalize.Click += btNormalizeClick;
            Controls.Add(btNormalize);

            btMinimize = ClawFormFactory.CreateButton(lAndF.MinimizeImage, ForeColor);
            btMinimize.TabIndex = int.MaxValue - 2;
            btMinimize.Location = new Point(Width - 95, 10);
            btMinimize.Click += btMinimizeClick;
            Controls.Add(btMinimize);
        }

        /// <summary>
        /// Called when the minimize button is clicked. Event handler method for Button.Click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event.</param>
        void btMinimizeClick(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        /// <summary>
        /// Called when the normalize button is clicked. Event handler method for Button.Click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event.</param>
        void btNormalizeClick(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Normal;
            Invalidate();
        }

        /// <summary>
        /// Called when the maximize button is clicked. Event handler method for Button.Click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event.</param>
        private void btMaximizeClick(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;
            Invalidate();
        }

        /// <summary>
        /// Called when the close button is clicked. Event handler method for Button.Click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event.</param>
        private void btCloseClick(object sender, EventArgs e)
        {
            Close();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
            {
                btClose.Location = new Point(Width - 30, 10);
                btMaximize.Visible = false;
                btNormalize.Location = new Point(Width - 55, 10);
                btNormalize.Visible = true;
                btMinimize.Location = new Point(Width - 80, 10);
            }
            else
            {
                btClose.Location = new Point(Width - 45, 10);
                btMaximize.Location = new Point(Width - 70, 10);
                btMaximize.Visible = true;
                btNormalize.Visible = false;
                btMinimize.Location = new Point(Width - 95, 10);
            }

            base.OnSizeChanged(e);
        }

        private void LookChanged(object sender, EventArgs e)
        {
            LookAndFeel lAndF = LookAndFeel.Instance;
            base.BackColor = lAndF.BackColor;
            base.ForeColor = lAndF.MidColor;
            tileImage = lAndF.TileImage;
            ProcessTileImage();

            ClawFormFactory.CreateImageForButton(btClose, lAndF.CloseImage, ForeColor);
            ClawFormFactory.CreateImageForButton(btMaximize, lAndF.MaximizeImage, ForeColor);
            ClawFormFactory.CreateImageForButton(btNormalize, lAndF.NormalizeImage, ForeColor);
            ClawFormFactory.CreateImageForButton(btMinimize, lAndF.MinimizeImage, ForeColor);
        }

        private void ClawFormMouseMove(object sender, MouseEventArgs e)
        {
            if (WindowState != FormWindowState.Maximized)
            {
                if (e.Button == MouseButtons.Left && cursorLocation != CursorLocation.Default)
                {
                    ResizeForm(e.X, e.Y);
                }
                else
                {
                    if (e.X > painter.EdgeWidth && e.X < Width - painter.EdgeHeight)
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
                    if (e.Y > painter.EdgeHeight && e.Y < Height - painter.EdgeHeight)
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
                    if (e.X < painter.EdgeWidth && e.Y < painter.EdgeHeight && e.Y >= painter.EdgeHeight - (painter.EdgeHeight / painter.EdgeWidth) * e.X && e.Y <= painter.EdgeHeight - (painter.EdgeHeight / painter.EdgeWidth) * e.X + RESIZE_BORDER)
                    {
                        cursorLocation = CursorLocation.TopLeft;
                        Cursor = Cursors.SizeNWSE;
                        return;
                    }
                    if (e.X > Width - painter.EdgeWidth && e.Y < painter.EdgeHeight && e.Y >= (painter.EdgeHeight / painter.EdgeWidth) * e.X + painter.EdgeHeight - (painter.EdgeHeight / painter.EdgeWidth) * Width && e.Y <= (painter.EdgeHeight / painter.EdgeWidth) * e.X + painter.EdgeHeight - (painter.EdgeHeight / painter.EdgeWidth) * Width + RESIZE_BORDER)
                    {
                        cursorLocation = CursorLocation.TopRight;
                        Cursor = Cursors.SizeNESW;
                        return;
                    }
                    if (e.X < painter.EdgeWidth && e.Y > Height - painter.EdgeHeight && e.Y <= Height - (painter.EdgeHeight - (painter.EdgeHeight / painter.EdgeWidth) * e.X) && e.Y >= Height - (painter.EdgeHeight - (painter.EdgeHeight / painter.EdgeWidth) * e.X) - RESIZE_BORDER)
                    {
                        cursorLocation = CursorLocation.BottomLeft;
                        Cursor = Cursors.SizeNESW;
                        return;
                    }
                    if (e.X > Width - painter.EdgeWidth && e.Y > Height - painter.EdgeHeight && e.Y <= Height - ((painter.EdgeHeight / painter.EdgeWidth) * e.X + painter.EdgeHeight - (painter.EdgeHeight / painter.EdgeWidth) * Width) && e.Y >= Height - ((painter.EdgeHeight / painter.EdgeWidth) * e.X + painter.EdgeHeight - (painter.EdgeHeight / painter.EdgeWidth) * Width) - RESIZE_BORDER)
                    {
                        cursorLocation = CursorLocation.BottomRight;
                        Cursor = Cursors.SizeNWSE;
                        return;
                    }

                    cursorLocation = CursorLocation.Default;
                    Cursor = Cursors.Default;
                }
            }
        }

        /// <summary>
        /// Handles the resize logic.
        /// </summary>
        /// <param name="x">Mouse x coordinate.</param>
        /// <param name="y">Mouse y coordinate.</param>
        private void ResizeForm(int x, int y)
        {
            int newLeft = Left;
            int newTop = Top;
            int newWidth = Width;
            int newHeight = Height;
            switch (cursorLocation)
            {
                case CursorLocation.Bottom:
                    newHeight = y;
                    break;

                case CursorLocation.Left:
                    newWidth -= x;
                    newLeft += x;
                    break;

                case CursorLocation.Right:
                    newWidth = x;
                    break;

                case CursorLocation.Top:
                    newHeight -= y;
                    newTop += y;
                    break;

                case CursorLocation.BottomLeft:
                    newHeight = y;
                    newWidth -= x;
                    newLeft += x;
                    break;

                case CursorLocation.BottomRight:
                    newHeight = y;
                    newWidth = x;
                    break;

                case CursorLocation.TopLeft:
                    newWidth -= x;
                    newLeft += x;
                    newHeight -= y;
                    newTop += y;
                    break;

                case CursorLocation.TopRight:
                    newHeight -= y;
                    newTop += y;
                    newWidth = x;
                    break;
            }

            if (newWidth >= MinimumSize.Width && newWidth <= MaximumSize.Width &&
                newHeight >= MinimumSize.Height && newHeight <= MaximumSize.Height)
            {
                Left = newLeft;
                Top = newTop;
                Width = newWidth;
                Height = newHeight;
                Invalidate();
            }
        }

        private void ClawFormMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (cursorLocation == CursorLocation.Default && IsFormVisibleAt(e.Location) && WindowState != FormWindowState.Maximized)
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
            return (point.X > painter.EdgeWidth && point.X < Width - painter.EdgeWidth) ||
                (point.Y > painter.EdgeHeight && point.Y < Height - painter.EdgeHeight) ||
                (point.X <= painter.EdgeWidth && point.Y > painter.EdgeHeight - (painter.EdgeHeight / painter.EdgeWidth) * point.X && point.Y < Height - (painter.EdgeHeight - (painter.EdgeHeight / painter.EdgeWidth) * point.X)) ||
                (point.X >= Width - painter.EdgeWidth && point.Y > (painter.EdgeHeight / painter.EdgeWidth) * point.X + painter.EdgeHeight - (painter.EdgeHeight / painter.EdgeWidth) * Width && point.Y < Height - ((painter.EdgeHeight / painter.EdgeWidth) * point.X + painter.EdgeHeight - (painter.EdgeHeight / painter.EdgeWidth) * Width));
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
            painter.Paint(this, e.Graphics);
        }
    }
}
