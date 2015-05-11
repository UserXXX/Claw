using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;

namespace Claw.UI.Controls
{
    /// <summary>
    /// Renderer for the ClawForm.
    /// </summary>
    public class ClawFormPainter
    {
        /// <summary>
        /// Width of the cut out edges.
        /// </summary>
        private const int EDGE_WIDTH = 20;
        /// <summary>
        /// Height of the cut out edges.
        /// </summary>
        private const int EDGE_HEIGHT = 60;

        /// <summary>
        /// Possible colors for transparency keys. Need to be at least four, because three could be used by Fore-, Mid- and BackColor.
        /// </summary>
        private static readonly Color[] TransparentColors = {
                                                         Color.Pink,
                                                         Color.Violet,
                                                         Color.LightGreen,
                                                         Color.YellowGreen,
                                                     };

        /// <summary>
        /// Width of the cut out edges.
        /// </summary>
        public int EdgeWidth
        {
            get { return EDGE_WIDTH; }
        }

        /// <summary>
        /// Height of the cut out edges.
        /// </summary>
        public int EdgeHeight
        {
            get { return EDGE_HEIGHT; }
        }

        /// <summary>
        /// Paints the ClawForm layout. This will change the Form.TransparencyKey setting, because the transparency key needs not to be Fore- or BackColor.
        /// This method will choose the best fitting transparency key and set it to the form.
        /// </summary>
        /// <param name="form">Form to paint.</param>
        /// <param name="graphics">Graphics to use for painting.</param>
        public void Paint(ClawForm form, Graphics graphics)
        {
            using (var background = new Bitmap(form.Width, form.Height, PixelFormat.Format32bppArgb))
            {
                Color transparentColor = GetTransparentColor(form);

                PaintBackground(form, background, transparentColor);

                // Set the transparency key, so everything drawn in that color will be completely transparent.
                form.TransparencyKey = transparentColor;
                graphics.DrawImageUnscaled(background, new Point(0, 0));
            }
        }

        /// <summary>
        /// Paints the forms background.
        /// </summary>
        /// <param name="form">Form whichs background shall be painted.</param>
        /// <param name="background">The background image to paint to.</param>
        /// <param name="transparentColor">The color to use for transparent areas.</param>
        private void PaintBackground(ClawForm form, Bitmap background, Color transparentColor)
        {
            Color formBackColor = form.BackColor;
            Image formTileImage = form.ProcessedTileImage;
            int formWidth = form.Width;
            int formHeight = form.Height;

            using (Graphics graph = Graphics.FromImage(background))
            {
                graph.Clear(formBackColor);

                Rectangle srcRectangle = new Rectangle((formTileImage.Width - form.Width) / 2, (formTileImage.Height - formHeight) / 2, formWidth, formHeight);
                Rectangle destRectangle = new Rectangle(0, 0, form.Width, form.Height);
                graph.DrawImage(formTileImage, destRectangle, srcRectangle, GraphicsUnit.Pixel);

                Rectangle gradientRectangle = new Rectangle(0, 0, formWidth, formHeight);
                Color gradientSrcColor = Color.FromArgb(50, formBackColor);
                // 191 is about 3/4 of 255.
                Color gradientDestColor = Color.FromArgb(191, formBackColor);

                using (var gradientBrush = new LinearGradientBrush(gradientRectangle, gradientSrcColor, gradientDestColor, LinearGradientMode.Horizontal))
                {
                    graph.FillRectangle(gradientBrush, gradientRectangle);
                }

                if (form.WindowState != FormWindowState.Maximized)
                {
                    DrawMissingEdges(form, graph, transparentColor);
                }
                DrawBorder(form, graph);
            }
        }

        /// <summary>
        /// Draws the border of the form.
        /// </summary>
        /// <param name="form">Form whichs border shall be drawn.</param>
        /// <param name="graph">Graphics to use.</param>
        private void DrawBorder(ClawForm form, Graphics graph)
        {
            int formWidth = form.Width;
            int formHeight = form.Height;

            int borderWidth = 3;
            using (var pen = new Pen(form.ForeColor, borderWidth))
            {
                if (form.WindowState == FormWindowState.Normal)
                {
                    graph.DrawPolygon(pen, new PointF[] {
                            new PointF(EDGE_WIDTH, 0),
                            new PointF(formWidth - EDGE_WIDTH, 0),
                            new PointF(formWidth - 1, EDGE_HEIGHT),
                            new PointF(formWidth - 1, formHeight - EDGE_HEIGHT),
                            new PointF(formWidth - EDGE_WIDTH, formHeight- 1),
                            new PointF(EDGE_WIDTH, formHeight - 1),
                            new PointF(0, formHeight - EDGE_HEIGHT),
                            new PointF(0, EDGE_HEIGHT),
                        });
                }
                else
                {
                    graph.DrawRectangle(pen, new Rectangle(0, 0, formWidth - 1, formHeight - 1));
                }
            }
        }

        /// <summary>
        /// Draws the transparent "missing" edges of the form.
        /// </summary>
        /// <param name="form">Form that edges shall be drawn.</param>
        /// <param name="graph">Graphics to use.</param>
        /// <param name="transparentColor">Color to use for transparency.</param>
        private void DrawMissingEdges(ClawForm form, Graphics graph, Color transparentColor)
        {
            int formWidth = form.Width;
            int formHeight = form.Height;

            using (var brush = new SolidBrush(transparentColor))
            {
                graph.FillPolygon(brush, new PointF[] {
                            new PointF(0, 0),
                            new PointF(EDGE_WIDTH, 0),
                            new PointF(0, EDGE_HEIGHT),
                        });
                graph.FillPolygon(brush, new PointF[] {
                            new PointF(formWidth, 0),
                            new PointF(formWidth - EDGE_WIDTH, 0),
                            new PointF(formWidth, EDGE_HEIGHT),
                        });
                graph.FillPolygon(brush, new PointF[] {
                            new PointF(0, formHeight),
                            new PointF(EDGE_WIDTH, formHeight),
                            new PointF(0, formHeight - EDGE_HEIGHT),
                        });
                graph.FillPolygon(brush, new PointF[] {
                            new PointF(formWidth, formHeight),
                            new PointF(formWidth - EDGE_WIDTH, formHeight),
                            new PointF(formWidth, formHeight - EDGE_HEIGHT),
                        });
            }
        }

        /// <summary>
        /// Gets the best transparent color based on the fore and back colors of the given form.
        /// </summary>
        /// <returns>The selected color.</returns>
        public Color GetTransparentColor(ClawForm form)
        {
            var ret = Color.Pink;
            var found = false;
            for (var i = 0; i < TransparentColors.Length && !found; i++)
            {
                if (TransparentColors[i] != form.ForeColor && TransparentColors[i] != form.BackColor)
                {
                    found = true;
                    ret = TransparentColors[i];
                }
            }
            return ret;
        }
    }
}
