﻿using Claw.Native;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Claw.UI.Style
{
    /// <summary>
    /// Base class of all windows used by Claw.
    /// </summary>
    public abstract class ClawWindow : Window
    {
        private HwndSource hwndSource;

        private ClawWindowRenderer renderer;
        private ClawWindowAnimator animator;

        private bool opening = false;

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

        private Button btClose;
        private Button btMaximize;
        private Button btNormalize;
        private Button btMinimize;

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
        /// Specifies whether the window is currently opening.
        /// </summary>
        public bool Opening
        {
            get { return opening; }
            set { opening = value; }
        }

        /// <summary>
        /// Creates a new ClawWindow.
        /// </summary>
        protected ClawWindow()
        {
            // Set width, heigth, left and top, so the renderer will deliver a valid mask.
            Width = 300;
            Height = 300;
            Left = 100;
            Top = 100;
            renderer = new ClawWindowRenderer(this);
            animator = new ClawWindowAnimator(this);

            LookAndFeel.Changed += LookChanged;

            Initialized += OnInitialized;
            StateChanged += OnStateChanged;
        }

        /// <summary>
        /// Called when the window has been initialized.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void OnInitialized(object sender, EventArgs e)
        {
            CreateControls();
        }

        /// <summary>
        /// Creates the controls used by this window.
        /// </summary>
        private void CreateControls()
        {
            Panel baseComponent = BaseComponent;

            btClose = CreateButton("CloseImage", "CloseText");
            btClose.Margin = new Thickness(10, 10, 25, 10);
            btClose.Click += OnCloseClick;
            baseComponent.Children.Add(btClose);

            btMaximize = CreateButton("MaximizeImage", "MaximizeText");
            btMaximize.Margin = new Thickness(10, 10, 50, 10);
            btMaximize.Click += OnMaximizeClick;
            btMaximize.Visibility = ResizeMode == ResizeMode.NoResize ? Visibility.Hidden : Visibility.Visible;
            baseComponent.Children.Add(btMaximize);

            btNormalize = CreateButton("NormalizeImage", "NormalizeText");
            btNormalize.Margin = new Thickness(10, 10, 35, 10);
            btNormalize.Visibility = Visibility.Hidden;
            btNormalize.Click += OnNormalizeClick;
            baseComponent.Children.Add(btNormalize);

            btMinimize = CreateButton("MinimizeImage", "MinimizeText");
            btMinimize.Margin = new Thickness(10, 10, 75, 10);
            btMinimize.Visibility = ResizeMode == ResizeMode.NoResize ? Visibility.Hidden : Visibility.Visible;
            btMinimize.Click += OnMinimizeClick;
            baseComponent.Children.Add(btMinimize);
        }

        /// <summary>
        /// Creates a button.
        /// </summary>
        /// <param name="imageSource">Image to use.</param>
        /// <param name="toolTipReference">Reference where to find the ToolTip resource.</param>
        /// <returns>The created button.</returns>
        private static Button CreateButton(string imageSource, string toolTipReference)
        {
            var button = new Button();
            button.HorizontalAlignment = HorizontalAlignment.Right;
            button.VerticalAlignment = VerticalAlignment.Top;
            button.Width = 20;
            button.Height = 20;
            
            var img = new Image();
            img.SetResourceReference(Image.SourceProperty, imageSource);
            button.Content = img;
            
            var toolTipText = new TextBlock();
            toolTipText.SetResourceReference(TextBlock.TextProperty, toolTipReference);
            toolTipText.SetResourceReference(TextBlock.ForegroundProperty, "BackBrush");
            var toolTip = new ToolTip();
            toolTip.Content = toolTipText;
            button.ToolTip = toolTip;

            return button;
        }

        /// <summary>
        /// Handles messages from the OS for this window.
        /// </summary>
        /// <param name="hwnd">Handle.</param>
        /// <param name="msg">Message to handle.</param>
        /// <param name="wParam">1st parameter.</param>
        /// <param name="lParam">2nd parameter.</param>
        /// <param name="handled">Whether the message was handled or not.</param>
        /// <returns></returns>
        private IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (ResizeMode == ResizeMode.NoResize)
            {
                return IntPtr.Zero;
            }

            switch (msg)
            {
                case 0x0024:/* WM_GETMINMAXINFO */
                    WmGetMinMaxInfo(hwnd, lParam);
                    handled = true;
                    break;
            }

            return IntPtr.Zero;
        }

        /// <summary>
        /// Handles the message that gets the windows min and max sizes.
        /// </summary>
        /// <param name="hwnd">Handle.</param>
        /// <param name="lParam">1st parameter.</param>
        private void WmGetMinMaxInfo(IntPtr hwnd, IntPtr lParam)
        {
            MINMAXINFO mmi = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));

            // Adjust the maximized size and position to fit the work area of the correct monitor.
            const int MONITOR_DEFAULTTONEAREST = 0x00000002;
            IntPtr monitor = NativeMethods.MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);

            if (monitor != System.IntPtr.Zero)
            {
                MONITORINFO monitorInfo = new MONITORINFO();
                NativeMethods.GetMonitorInfo(monitor, monitorInfo);
                RECT rcWorkArea = monitorInfo.RcWork;
                RECT rcMonitorArea = monitorInfo.RcMonitor;
                mmi.PtMaxPosition = new POINT(Math.Abs(rcWorkArea.Left - rcMonitorArea.Left), Math.Abs(rcWorkArea.Top - rcMonitorArea.Top));
                mmi.PtMaxSize = new POINT(Math.Abs(rcWorkArea.Right - rcWorkArea.Left), Math.Abs(rcWorkArea.Bottom - rcWorkArea.Top));
                // Need to set the minimum size to make resizing work properly.
                if (Opening)
                {
                    mmi.PtMinTrackSize = new POINT(0, 0);
                }
                else
                {
                    mmi.PtMinTrackSize = new POINT((int)MinWidth, (int)MinHeight);
                }
            }

            Marshal.StructureToPtr(mmi, lParam, true);
        }

        /// <summary>
        /// Called when the minimize button is clicked.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void OnMinimizeClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// Called when the normalize button is clicked.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void OnNormalizeClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Normal;
            InvalidateVisual();
        }

        /// <summary>
        /// Called when the maximize button is clicked.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void OnMaximizeClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Maximized;
            InvalidateVisual();
        }

        /// <summary>
        /// Called when the close button is clicked.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void OnCloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Handles the StateChanged event for this ClawWindow.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void OnStateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                btClose.Margin = new Thickness(10);
                btMaximize.Visibility = Visibility.Hidden;
                btNormalize.Visibility = Visibility.Visible;
                btMinimize.Margin = new Thickness(10, 10, 60, 10);
            }
            else
            {
                btClose.Margin = new Thickness(10, 10, 25, 10);
                btMaximize.Visibility = Visibility.Visible;
                btNormalize.Visibility = Visibility.Hidden;
                btMinimize.Margin = new Thickness(10, 10, 75, 10);
            }
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
            hwndSource.AddHook(new HwndSourceHook(WindowProc));
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
            if (e == null)
            {
                throw new ArgumentNullException("e");
            }

            if (e.LeftButton == MouseButtonState.Pressed && cursorLocation == CursorLocation.Default)
            {
                DragMove();
            }

            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("e");
            }

            if (ResizeMode == ResizeMode.NoResize)
            {
                Cursor = Cursors.Arrow;
                base.OnMouseMove(e);
                return;
            }

            if (WindowState != WindowState.Maximized)
            {
                Point position = e.GetPosition(this);
                if (e.LeftButton == MouseButtonState.Pressed && cursorLocation != CursorLocation.Default)
                {
                    ResizeWindow();
                }
                else
                {
                    ShowCursor(position);
                }
            }
            base.OnMouseMove(e);
        }

        /// <summary>
        /// Shows the cursor corresponding to the cursor position.
        /// </summary>
        /// <param name="position">Position of the cursor.</param>
        private void ShowCursor(Point position)
        {
            const double EDGE_WIDTH = 20;
            const double EDGE_HEIGHT = 60;

            double width = Opening ? Width : ActualWidth;
            if (position.X > EDGE_WIDTH && position.X < width - EDGE_HEIGHT && ShowCursorNS(position))
            {
                return;
            }
            if (position.Y > EDGE_HEIGHT && position.Y < Height - EDGE_HEIGHT && ShowCursorWE(position, width))
            {
                return;
            }
            if (position.X < EDGE_WIDTH && position.Y < EDGE_HEIGHT && position.Y >= EDGE_HEIGHT - (EDGE_HEIGHT / EDGE_WIDTH) * position.X && position.Y <= EDGE_HEIGHT - (EDGE_HEIGHT / EDGE_WIDTH) * position.X + RESIZE_BORDER)
            {
                cursorLocation = CursorLocation.TopLeft;
                Cursor = Cursors.SizeNWSE;
                return;
            }
            if (position.X > width - EDGE_WIDTH && position.Y < EDGE_HEIGHT && position.Y >= (EDGE_HEIGHT / EDGE_WIDTH) * position.X + EDGE_HEIGHT - (EDGE_HEIGHT / EDGE_WIDTH) * width && position.Y <= (EDGE_HEIGHT / EDGE_WIDTH) * position.X + EDGE_HEIGHT - (EDGE_HEIGHT / EDGE_WIDTH) * Width + RESIZE_BORDER)
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
            if (position.X > width - EDGE_WIDTH && position.Y > Height - EDGE_HEIGHT && position.Y <= Height - ((EDGE_HEIGHT / EDGE_WIDTH) * position.X + EDGE_HEIGHT - (EDGE_HEIGHT / EDGE_WIDTH) * width) && position.Y >= Height - ((EDGE_HEIGHT / EDGE_WIDTH) * position.X + EDGE_HEIGHT - (EDGE_HEIGHT / EDGE_WIDTH) * Width) - RESIZE_BORDER)
            {
                cursorLocation = CursorLocation.BottomRight;
                Cursor = Cursors.SizeNWSE;
                return;
            }

            cursorLocation = CursorLocation.Default;
            Cursor = Cursors.Arrow;
        }

        private bool ShowCursorWE(Point position, double width)
        {
            if (position.X < RESIZE_BORDER)
            {
                cursorLocation = CursorLocation.Left;
                Cursor = Cursors.SizeWE;
                return true;
            }
            if (position.X > width - (RESIZE_BORDER + 1))
            {
                cursorLocation = CursorLocation.Right;
                Cursor = Cursors.SizeWE;
                return true;
            }
            return false;
        }

        private bool ShowCursorNS(Point position)
        {
            if (position.Y < RESIZE_BORDER)
            {
                cursorLocation = CursorLocation.Top;
                Cursor = Cursors.SizeNS;
                return true;
            }
            if (position.Y > Height - (RESIZE_BORDER + 1))
            {
                cursorLocation = CursorLocation.Bottom;
                Cursor = Cursors.SizeNS;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Handles resizing.
        /// </summary>
        private void ResizeWindow()
        {
            NativeMethods.SendMessage(hwndSource.Handle, 0x112, (IntPtr)(61440 + cursorLocation), IntPtr.Zero);
        }

        protected override sealed void OnRender(DrawingContext drawingContext)
        {
            renderer.Render(drawingContext);
        }

        /// <summary>
        /// The base component of the window. Needed to insert controls into the windows control hierarchy.
        /// </summary>
        protected abstract Panel BaseComponent
        {
            get;
        }
    }
}
