using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Claw.UI.Controls
{
    /// <summary>
    /// Base class of all windows used by Claw.
    /// </summary>
    public class ClawWindow : Window
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, UInt32 msg, IntPtr wParam, IntPtr lParam);

        private HwndSource hwndSource;

        private ClawWindowRenderer renderer;
        private ClawWindowAnimator animator;

        private const int RESIZE_BORDER = 4;

        private CursorLocation cursorLocation = CursorLocation.Default;
        private enum CursorLocation
        {
            Top = 3,
            Left = 1,
            Right = 2,
            Bottom = 6,
            TopLeft = 4,
            TopRight = 5,
            BottomLeft = 7,
            BottomRight = 8,
            Default = 9,
        }

        /// <summary>
        /// The renderer used for rendering this window.
        /// </summary>
        public ClawWindowRenderer Renderer
        {
            get { return renderer; }
            set
            {
                renderer = value;
                InvalidateVisual();
            }
        }

        private static PropertyMetadata highlightPositionMetadata = new PropertyMetadata(OnHighlightPositionChanged);
        /// <summary>
        /// Property for the position of the highlight.
        /// </summary>
        public static readonly DependencyProperty HighlightPositionProperty =
            DependencyProperty.Register("HighlightPosition", typeof(Double), typeof(ClawWindow), highlightPositionMetadata);
        /// <summary>
        /// Horizontal start position of the highlight effect.
        /// </summary>
        public Double HighlightPosition
        {
            get { return (Double)GetValue(HighlightPositionProperty); }
            set
            {
                SetValue(HighlightPositionProperty, value);
                InvalidateVisual();
            }
        }

        private static PropertyMetadata highlightWidthMetadata = new PropertyMetadata(100.0, OnHighlightWidthChanged);
        /// <summary>
        /// Property for the width of the highlight.
        /// </summary>
        public static readonly DependencyProperty HighlightWidthProperty =
            DependencyProperty.Register("HighlightWidth", typeof(Double), typeof(ClawWindow), highlightWidthMetadata);
        /// <summary>
        /// Width of the highlight effect.
        /// </summary>
        public Double HighlightWidth
        {
            get { return (Double)GetValue(HighlightWidthProperty); }
            set
            {
                SetValue(HighlightWidthProperty, value);
                InvalidateVisual();
            }
        }

        /// <summary>
        /// Creates a new ClawWindow.
        /// </summary>
        public ClawWindow()
        {
            // Set width, heigth, left and top, so the renderer will deliver a valid mask.
            Width = 300;
            Height = 300;
            Left = 100;
            Top = 100;
            renderer = new ClawWindowRenderer(this);
            animator = new ClawWindowAnimator(this);

            LookAndFeel.Instance.Changed += LookChanged;
        }

        protected override void OnInitialized(EventArgs e)
        {
            SourceInitialized += OnSourceInitialized;
            base.OnInitialized(e);
        }

        /// <summary>
        /// Called when Win32 interaction is initialized.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void OnSourceInitialized(object sender, EventArgs e)
        {
            hwndSource = (HwndSource)PresentationSource.FromVisual(this);
        }

        #region Property Callbacks

        /// <summary>
        /// Called when the highlights position changed.
        /// </summary>
        /// <param name="d">The dependency object. This is the ClawWindow.</param>
        /// <param name="e">Event arguments.</param>
        private static void OnHighlightPositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ClawWindow window = (ClawWindow)d;
            window.InvalidateVisual();
        }

        /// <summary>
        /// Called when the highlights width changed.
        /// </summary>
        /// <param name="d">The dependency object. This is the ClawWindow.</param>
        /// <param name="e">Event arguments.</param>
        private static void OnHighlightWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ClawWindow window = (ClawWindow)d;
            window.InvalidateVisual();
        }

        #endregion

        /// <summary>
        /// Called when the look and feel changes.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="evt">The event arguments.</param>
        private void LookChanged(object sender, EventArgs evt)
        {
            InvalidateVisual();
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && cursorLocation == CursorLocation.Default)
            {
                DragMove();
            }

            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            const double EDGE_WIDTH = 20;
            const double EDGE_HEIGHT = 60;

            if (WindowState != WindowState.Maximized)
            {
                Point position = e.GetPosition(this);
                if (e.LeftButton == MouseButtonState.Pressed && cursorLocation != CursorLocation.Default)
                {
                    ResizeWindow();
                }
                else
                {
                    if (position.X > EDGE_WIDTH && position.X < Width - EDGE_HEIGHT)
                    {
                        if (position.Y < RESIZE_BORDER)
                        {
                            cursorLocation = CursorLocation.Top;
                            Cursor = Cursors.SizeNS;
                            return;
                        }
                        if (position.Y > Height - (RESIZE_BORDER + 1))
                        {
                            cursorLocation = CursorLocation.Bottom;
                            Cursor = Cursors.SizeNS;
                            return;
                        }
                    }
                    if (position.Y > EDGE_HEIGHT && position.Y < Height - EDGE_HEIGHT)
                    {
                        if (position.X < RESIZE_BORDER)
                        {
                            cursorLocation = CursorLocation.Left;
                            Cursor = Cursors.SizeWE;
                            return;
                        }
                        if (position.X > Width - (RESIZE_BORDER + 1))
                        {
                            cursorLocation = CursorLocation.Right;
                            Cursor = Cursors.SizeWE;
                            return;
                        }
                    }
                    if (position.X < EDGE_WIDTH && position.Y < EDGE_HEIGHT && position.Y >= EDGE_HEIGHT - (EDGE_HEIGHT / EDGE_WIDTH) * position.X && position.Y <= EDGE_HEIGHT - (EDGE_HEIGHT / EDGE_WIDTH) * position.X + RESIZE_BORDER)
                    {
                        cursorLocation = CursorLocation.TopLeft;
                        Cursor = Cursors.SizeNWSE;
                        return;
                    }
                    if (position.X > Width - EDGE_WIDTH && position.Y < EDGE_HEIGHT && position.Y >= (EDGE_HEIGHT / EDGE_WIDTH) * position.X + EDGE_HEIGHT - (EDGE_HEIGHT / EDGE_WIDTH) * Width && position.Y <= (EDGE_HEIGHT / EDGE_WIDTH) * position.X + EDGE_HEIGHT - (EDGE_HEIGHT / EDGE_WIDTH) * Width + RESIZE_BORDER)
                    {
                        cursorLocation = CursorLocation.TopRight;
                        Cursor = Cursors.SizeNESW;
                        return;
                    }
                    if (position.X < EDGE_WIDTH && position.Y > Height - EDGE_HEIGHT && position.Y <= Height - (EDGE_HEIGHT - (EDGE_HEIGHT / EDGE_WIDTH) * position.X) && position.Y >= Height - (EDGE_HEIGHT - (EDGE_HEIGHT / EDGE_WIDTH) * position.X) - RESIZE_BORDER)
                    {
                        cursorLocation = CursorLocation.BottomLeft;
                        Cursor = Cursors.SizeNESW;
                        return;
                    }
                    if (position.X > Width - EDGE_WIDTH && position.Y > Height - EDGE_HEIGHT && position.Y <= Height - ((EDGE_HEIGHT / EDGE_WIDTH) * position.X + EDGE_HEIGHT - (EDGE_HEIGHT / EDGE_WIDTH) * Width) && position.Y >= Height - ((EDGE_HEIGHT / EDGE_WIDTH) * position.X + EDGE_HEIGHT - (EDGE_HEIGHT / EDGE_WIDTH) * Width) - RESIZE_BORDER)
                    {
                        cursorLocation = CursorLocation.BottomRight;
                        Cursor = Cursors.SizeNWSE;
                        return;
                    }

                    cursorLocation = CursorLocation.Default;
                    Cursor = Cursors.Arrow;
                }
            }
            base.OnMouseMove(e);
        }

        /// <summary>
        /// Handles resizing.
        /// </summary>
        private void ResizeWindow()
        {
            SendMessage(hwndSource.Handle, 0x112, (IntPtr)(61440 + cursorLocation), IntPtr.Zero);
        }

        protected override sealed void OnRender(DrawingContext drawingContext)
        {
            renderer.Render(drawingContext);
        }
    }
}
