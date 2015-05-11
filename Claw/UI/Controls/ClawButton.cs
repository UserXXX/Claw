using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Claw.UI.Controls
{
    /// <summary>
    /// Button with Claw look and feel.
    /// </summary>
    public class ClawButton : Button
    {
        private bool mouseOver = false;
        private bool mouseDown = false;

        public override Color ForeColor
        {
            get { return base.ForeColor; }
            set
            {
                // Do nothing to prohibit setting this value.
            }
        }

        public override Color BackColor
        {
            get { return base.BackColor; }
            set
            {
                // Do nothing to prohibit setting this value.
            }
        }

        /// <summary>
        /// Creates a new ClawButton.
        /// </summary>
        public ClawButton()
            : base()
        {
            LookAndFeel.Instance.Changed += LookChanged;
            LookChanged(null, null);
        }

        /// <summary>
        /// Event handler method for the L&F.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="evt">The event.</param>
        private void LookChanged(object sender, EventArgs evt)
        {
            LookAndFeel lAndF = LookAndFeel.Instance;
            base.ForeColor = lAndF.ForeColor;
            base.BackColor = lAndF.BackColor;
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            mouseOver = true;
            base.OnMouseEnter(e);
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            if (mouseOver)
            {
                mouseDown = true;
            }
            base.OnMouseDown(mevent);
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            if (mouseOver)
            {
                mouseDown = false;
            }
            base.OnMouseUp(mevent);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            mouseOver = false;
            mouseDown = false;
            base.OnMouseLeave(e);
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            Graphics graphics = pevent.Graphics;

            DrawBackgroundColor(graphics);
            DrawBackgroundImage(graphics);
            DrawBorder(graphics);
            DrawImage(graphics);
            DrawText(graphics);
            DrawFocusBorder(graphics);
        }

        /// <summary>
        /// Draws the background image.
        /// </summary>
        /// <param name="graphics">Graphics to draw to.</param>
        private void DrawBackgroundImage(Graphics graphics)
        {
            if (BackgroundImage != null)
            {
                Rectangle[] rect = null;
                switch (BackgroundImageLayout)
                {
                    case ImageLayout.Center:
                        rect = new Rectangle[1];
                        rect[0] = new Rectangle((Width - BackgroundImage.Width) / 2, (Height - BackgroundImage.Height) / 2, BackgroundImage.Width, BackgroundImage.Height);
                        break;

                    case ImageLayout.None:
                        rect = new Rectangle[1];
                        rect[0] = new Rectangle(0, 0, BackgroundImage.Width, BackgroundImage.Height);
                        break;

                    case ImageLayout.Stretch:
                        rect = new Rectangle[1];
                        rect[0] = new Rectangle(0, 0, Width - 1, Height - 1);
                        break;

                    case ImageLayout.Tile:
                        int sizeX = Width / BackgroundImage.Width + 1;
                        int sizeY = Height / BackgroundImage.Height + 1;
                        rect = new Rectangle[sizeX * sizeY];
                        for (var x = 0; x < sizeX; x++)
                        {
                            for (var y = 0; y < sizeY; y++)
                            {
                                rect[x + y * sizeX] = new Rectangle(x * BackgroundImage.Width, y * BackgroundImage.Height, BackgroundImage.Width, BackgroundImage.Height);
                            }
                        }
                        break;

                    case ImageLayout.Zoom:
                        rect = new Rectangle[1];
                        if (BackgroundImage.Width == BackgroundImage.Height)
                        {
                            rect[0] = new Rectangle(0, 0, Width, Height);
                        }
                        else if (BackgroundImage.Width > BackgroundImage.Height)
                        {
                            double scaleFactor = ((double)Height) / ((double)BackgroundImage.Height);
                            double width = BackgroundImage.Width * scaleFactor;
                            double xPos = (Width - width) / 2;
                            rect[0] = new Rectangle((int)xPos, 0, (int)width, Height - 1);
                        }
                        else
                        {
                            double scaleFactor = ((double)Width) / ((double)BackgroundImage.Width);
                            double height = BackgroundImage.Height * scaleFactor;
                            double yPos = (Height - height) / 2;
                            rect[0] = new Rectangle(0, (int)yPos, Width - 1, (int)height);
                        }
                        break;
                }
                for (var i = 0; i < rect.Length; i++)
                {
                    graphics.DrawImage(BackgroundImage, rect[i]);
                }
            }
        }

        /// <summary>
        /// If the button has the focus and is not pressed, this draws a rectangle that is set 2px into the button, indicating it is focused.
        /// </summary>
        /// <param name="graphics">Graphics to draw to.</param>
        private void DrawFocusBorder(Graphics graphics)
        {
            if (Focused && !mouseDown)
            {
                using (var pen = new Pen(Color.FromArgb(100, ForeColor), 3))
                {
                    graphics.DrawRectangle(pen, new Rectangle(2, 2, Width - 5, Height - 5));
                }
            }
        }

        /// <summary>
        /// Draws the text on the button.
        /// </summary>
        /// <param name="graphics">Graphics to draw to.</param>
        private void DrawText(Graphics graphics)
        {
            using (var brush = new SolidBrush(ForeColor))
            {
                SizeF size = graphics.MeasureString(Text, Font);
                graphics.DrawString(Text, Font, brush, new PointF((Size.Width - size.Width) / 2, (Size.Height - size.Height) / 2));
            }
        }

        /// <summary>
        /// Draws the border of the button.
        /// </summary>
        /// <param name="graphics">The graphics to draw to.</param>
        private void DrawBorder(Graphics graphics)
        {
            using (var pen = new Pen(ForeColor))
            {
                graphics.DrawRectangle(pen, new Rectangle(0, 0, Width - 1, Height - 1));
            }
        }

        /// <summary>
        /// Draws the background image if one is set.
        /// </summary>
        /// <param name="graphics">The graphics to draw to.</param>
        private void DrawImage(Graphics graphics)
        {
            if (Image != null)
            {
                int imageX = 0;
                int imageY = 0;
                switch (ImageAlign)
                {
                    case ContentAlignment.BottomCenter:
                        imageX = (Width - Image.Width) / 2;
                        imageY = Height - Image.Height;
                        break;

                    case ContentAlignment.BottomLeft:
                        imageY = Height - Image.Height;
                        break;

                    case ContentAlignment.BottomRight:
                        imageX = Width - Image.Width;
                        imageY = Height - Image.Height;
                        break;

                    case ContentAlignment.MiddleCenter:
                        imageX = (Width - Image.Width) / 2;
                        imageY = (Height - Image.Height) / 2;
                        break;

                    case ContentAlignment.MiddleLeft:
                        imageY = (Height - Image.Height) / 2;
                        break;

                    case ContentAlignment.MiddleRight:
                        imageX = Width - Image.Width;
                        imageY = (Height - Image.Height) / 2;
                        break;

                    case ContentAlignment.TopCenter:
                        imageX = (Width - Image.Width) / 2;
                        break;

                    case ContentAlignment.TopLeft:
                        break;

                    case ContentAlignment.TopRight:
                        imageX = Width - Image.Width;
                        break;
                }

                graphics.DrawImageUnscaled(Image, new Point(imageX, imageY));
            }
        }

        /// <summary>
        /// Draws the single colored bakground.
        /// </summary>
        /// <param name="graphics">Graphics to use for drawing.</param>
        private void DrawBackgroundColor(Graphics graphics)
        {
            Color brushColor = BackColor;
            if (mouseOver)
                brushColor = LookAndFeel.Instance.HoverColor;
            if (mouseDown)
                brushColor = LookAndFeel.Instance.InteractColor;

            using (var brush = new SolidBrush(brushColor))
            {
                graphics.FillRectangle(brush, new Rectangle(0, 0, Width, Height));
            }
        }
    }
}
