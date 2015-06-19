using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace Claw.UI.Controls
{
    /// <summary>
    /// Renderer for ClawWindows.
    /// </summary>
    public class ClawWindowRenderer
    {
        private ClawWindow window;

        private DrawingBrush windowMaskBrush;

        #region Static render data

        private static double maxScreenWidth;
        private static double maxScreenHeight;
        private static ImageSource processedTileImage;
        private static ImageBrush processedTileBrush;

        private static SolidColorBrush midColorBrush;
        private static SolidColorBrush backColorBrush;
        private static Pen borderPen;

        private static LinearGradientBrush fadeOutBrush;

        private static readonly ImageSource[] windowImages = new ImageSource[] {
                LookAndFeel.Instance.SolidImage,
                LookAndFeel.Instance.SolidImage,
                LookAndFeel.Instance.TopLeftWindowEdgeImage,
                LookAndFeel.Instance.TopRightWindowEdgeImage,
                LookAndFeel.Instance.BottomLeftWindowEdgeImage,
                LookAndFeel.Instance.BottomRightWindowEdgeImage,
                LookAndFeel.Instance.SolidImage,
                LookAndFeel.Instance.SolidImage,
            };

        private static Color transparentForeColor;

        /// <summary>
        /// Static initializer for ClawWindowRenderer.
        /// </summary>
        static ClawWindowRenderer()
        {
            maxScreenWidth = SystemParameters.PrimaryScreenWidth;
            maxScreenHeight = SystemParameters.PrimaryScreenHeight;

            LookAndFeel.Instance.Changed += StaticLookChanged;
            StaticLookChanged(null, new EventArgs());
        }
        
        /// <summary>
        /// Called when the look and feel changed. This is the static callback for the shared background image.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="evt">The event arguments.</param>
        private static void StaticLookChanged(object sender, EventArgs evt)
        {
            LookAndFeel lAndF = LookAndFeel.Instance;

            midColorBrush = new SolidColorBrush(lAndF.MidColor);
            midColorBrush.Freeze();

            ImageBrush mask = CreateTileImageMask();

            var area = new Rect(0, 0, maxScreenWidth, maxScreenHeight);

            var drawingVisual = new DrawingVisual();
            using (DrawingContext drawingContext = drawingVisual.RenderOpen())
            {
                drawingContext.PushOpacityMask(mask);
                drawingContext.DrawRectangle(midColorBrush, null, area);
                drawingContext.Pop();
            }

            var bitmap = new RenderTargetBitmap((int)maxScreenWidth, (int)maxScreenHeight, 96, 96, PixelFormats.Default);
            bitmap.Render(drawingVisual);
            bitmap.Freeze();

            processedTileImage = bitmap;
            processedTileBrush = new ImageBrush(processedTileImage);
            processedTileBrush.Freeze();

            borderPen = new Pen(new SolidColorBrush(lAndF.MidColor), 2);
            borderPen.Freeze();
            backColorBrush = new SolidColorBrush(lAndF.BackColor);
            backColorBrush.Freeze();

            Color backColor = lAndF.BackColor;
            var gradientStartColor = Color.FromArgb(50, backColor.R, backColor.G, backColor.B);
            var gradientEndColor = Color.FromArgb(191, backColor.R, backColor.G, backColor.B);
            fadeOutBrush = new LinearGradientBrush(gradientStartColor, gradientEndColor, 0);
            fadeOutBrush.Freeze();

            transparentForeColor = Color.FromArgb(0, lAndF.ForeColor.R, lAndF.ForeColor.G, lAndF.ForeColor.B);
        }

        /// <summary>
        /// Creates the alpha mask with the tiled image.
        /// </summary>
        /// <returns>The brush to use for masking.</returns>
        private static ImageBrush CreateTileImageMask()
        {
            ImageSource tileImage = LookAndFeel.Instance.TileImage;
            var drawings = new DrawingGroup();
            // TODO: Get from image.
            var imageWidth = 172;
            var imageHeight = 100;
            for (var x = 0; x < (maxScreenWidth / imageWidth) + 1; x++)
            {
                for (var y = 0; y < (maxScreenHeight / imageHeight) + 1; y++)
                {
                    var drawing = new ImageDrawing();
                    drawing.Rect = new Rect(x * imageWidth, y * imageHeight, imageWidth, imageHeight);
                    drawing.ImageSource = tileImage;
                    drawing.Freeze();
                    drawings.Children.Add(drawing);
                }
            }
            drawings.Freeze();

            var mask = new DrawingImage(drawings);
            mask.Freeze();

            var maskBrush = new ImageBrush(mask);
            maskBrush.Freeze();
            return maskBrush;
        }

        #endregion

        /// <summary>
        /// Creates a new ClawWindowRenderer.
        /// </summary>
        /// <param name="window">The window to render.</param>
        public ClawWindowRenderer(ClawWindow window)
        {
            this.window = window;

            window.LocationChanged += OnLocationChanged;
            window.SizeChanged += OnSizeChanged;

            RecreateWindowMask();
        }

        /// <summary>
        /// Called when the windows location changed.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Event arguments.</param>
        private void OnLocationChanged(object sender, EventArgs e)
        {
            RecreateWindowMask();
        }

        /// <summary>
        /// Called when the windows size changed.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Event arguments.</param>
        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            RecreateWindowMask();
        }

        /// <summary>
        /// Gets the rectangle describing the windows size in a normalized location.
        /// </summary>
        /// <returns>The rectangle.</returns>
        private Rect GetWindowShape()
        {
            double width = window.ActualWidth;
            double height = window.ActualHeight;
            if (window.Opening)
            {
                width = window.Width;
                height = window.Height;
            }

            if (window.WindowState == WindowState.Maximized)
            {
                width -= 20;
                height -= 20;
            }
            double left = Math.Max(0, (maxScreenWidth - width) / 2);
            double top = Math.Max(0, (maxScreenHeight - height) / 2);
            return new Rect(left, top, width, height);
        }

        /// <summary>
        /// Gets the rectangle describing the windows size and virtual location.
        /// </summary>
        /// <returns>The rectangle.</returns>
        private Rect GetWindowRect()
        {
            double width = window.ActualWidth;
            double height = window.ActualHeight;
            double left = 0;
            double top = 0;
            if (window.Opening)
            {
                width = window.Width;
                height = window.Height;
            }

            return new Rect(left, top, width, height);
        }

        /// <summary>
        /// Recreates the window mask.
        /// </summary>
        private void RecreateWindowMask()
        {
            Rect windowRect = GetWindowShape();
            double width = windowRect.Width;
            double height = windowRect.Height;
            double left = windowRect.X;
            double top = windowRect.Y;

            // Add top left and bottom right marker so the final image will have the size of the screen.
            Rect[] rects = new Rect[] {
                new Rect(0, 0, 1, 1),
                new Rect(maxScreenWidth - 1, maxScreenHeight - 1, 1, 1),
                new Rect(left, top, 20, 60),
                new Rect(left + width - 20, top, 20, 60),
                new Rect(left, top + height - 60, 20, 60),
                new Rect(left + width - 20, top + height - 60, 20, 60),
                new Rect(left + 20, top, Math.Max(0, width - 40), height),
                new Rect(left, top + 60, width, Math.Max(0, height - 120)),
            };

            var drawings = new DrawingGroup();
            for (var i = 0; i < rects.Length; i++)
            {
                ImageDrawing drawing = new ImageDrawing();
                drawing.Rect = rects[i];
                drawing.ImageSource = windowImages[i];
                drawings.Children.Add(drawing);
            }
            
            var windowImage = new DrawingImage(drawings);
            // Improve performance by freezing.
            windowImage.Freeze();
            var maskDrawing = new ImageDrawing(windowImage, new Rect(-(maxScreenWidth - width) / 2, -(maxScreenHeight - height) / 2, maxScreenWidth, maxScreenHeight));
            maskDrawing.Freeze();
            windowMaskBrush = new DrawingBrush(maskDrawing);
            windowMaskBrush.Freeze();
        }

        /// <summary>
        /// Renders the ClawWindow.
        /// </summary>
        /// <param name="drawingContext">Drawing context.</param>
        internal unsafe void Render(DrawingContext drawingContext)
        {
            Rect windowRect = GetWindowRect();
            double width = windowRect.Width;
            double height = windowRect.Height;
            double left = windowRect.X;
            double top = windowRect.Y;

            WindowState state = window.WindowState;

            if (state != WindowState.Maximized)
            {
                drawingContext.PushOpacityMask(windowMaskBrush);
            }

            RenderBackground(drawingContext);

            if (state != WindowState.Maximized)
            {
                drawingContext.Pop();
            }

            Point[] points = null;

            if (state != WindowState.Maximized)
            {
                points = new Point[] {
                    new Point(left + 20, top + 1),
                    new Point(left + width - 20, top + 1),
                    new Point(left + width - 1, top + 60),
                    new Point(left + width - 1, top + height - 60),
                    new Point(left + width - 20, top + height - 1),
                    new Point(left + 20, top + height - 1),
                    new Point(left + 1, top + height - 60),
                    new Point(left + 1, top + 60),
                };
            }
            else
            {
                points = new Point[] {
                    new Point(left + 1, top + 1),
                    new Point(left + width - 1, top + 1),
                    new Point(left + width - 1, top + height - 1),
                    new Point(left + 1, top + height - 1),
                };
            }

            for (var i = 0; i < points.Length - 1; i++)
            {
                drawingContext.DrawLine(borderPen, points[i], points[i + 1]);
            }
            drawingContext.DrawLine(borderPen, points[points.Length - 1], points[0]);
        }

        /// <summary>
        /// Renders the window background.
        /// </summary>
        /// <param name="drawingContext">Drawing context.</param>
        private unsafe void RenderBackground(DrawingContext drawingContext)
        {
            Rect winRect = GetWindowRect();
            double width = winRect.Width;
            double height = winRect.Height;

            Rect screenRect = new Rect(-(maxScreenWidth - width) / 2, -(maxScreenHeight - height) / 2, maxScreenWidth, maxScreenHeight);

            drawingContext.DrawRectangle(backColorBrush, null, winRect);
            drawingContext.DrawImage(processedTileImage, screenRect);
            drawingContext.DrawRectangle(fadeOutBrush, null, winRect);

            // Draw the highlight effect, only on the grid lines!
            drawingContext.PushOpacityMask(processedTileBrush);
            DrawHighlight(screenRect, drawingContext);
            drawingContext.Pop();
        }

        /// <summary>
        /// Draws the highlight.
        /// </summary>
        /// <param name="screenRect">Rectangle of the screen with the window centered.</param>
        /// <param name="drawingContext">Drawing context.</param>
        private unsafe void DrawHighlight(Rect screenRect, DrawingContext drawingContext)
        {
            Rect windowRect = GetWindowRect();
            double width = windowRect.Width;

            double screenStart = ((maxScreenWidth - width) / 2 + window.HighlightPosition);
            double screenMid = screenStart + window.HighlightWidth / 2;
            double screenEnd = screenMid + window.HighlightWidth / 2;

            var brush = new LinearGradientBrush(new GradientStopCollection(new GradientStop[] {
                new GradientStop(transparentForeColor, 0.0),
                new GradientStop(transparentForeColor, screenStart / maxScreenWidth),
                new GradientStop(LookAndFeel.Instance.ForeColor, screenMid / maxScreenWidth),
                new GradientStop(transparentForeColor, screenEnd / maxScreenWidth),
                new GradientStop(transparentForeColor, 1.0),
            }), 0);

            drawingContext.DrawRectangle(brush, null, screenRect);
        }
    }
}
