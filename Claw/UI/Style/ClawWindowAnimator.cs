using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.Animation;

namespace Claw.UI.Style
{
    /// <summary>
    /// Animates one ClawWindow. There is always one instance of this class associated with one ClawWindow instance.
    /// </summary>
    public class ClawWindowAnimator
    {
        private ClawWindow window;

        private Storyboard highlightStoryboard;

        /// <summary>
        /// Static initializer.
        /// </summary>
        static ClawWindowAnimator()
        {
            Timeline.DesiredFrameRateProperty.OverrideMetadata(typeof(Timeline),
                new FrameworkPropertyMetadata { DefaultValue = 30 });
        }

        /// <summary>
        /// Creates a new ClawWindowAnimator for the given window.
        /// </summary>
        /// <param name="window">The ClawWindow to animate.</param>
        public ClawWindowAnimator(ClawWindow window) 
        {
            if (window == null)
            {
                throw new ArgumentNullException("window");
            }

            this.window = window;
            window.IsVisibleChanged += OnIsVisibleChanged;
            window.SizeChanged += OnSizeChanged;
        }

        /// <summary>
        /// Called when the size of the window changed.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.WidthChanged)
            {
                if (highlightStoryboard.GetCurrentState(window) == ClockState.Active)
                {
                    highlightStoryboard.Stop(window);
                    highlightStoryboard.Remove(window);
                }

                highlightStoryboard = CreateHighlightAnimation(window.HighlightPosition);
                highlightStoryboard.Completed += RestartHighlightAnimation;
                highlightStoryboard.Freeze();
                highlightStoryboard.Begin(window, true);
            }
        }

        /// <summary>
        /// Called when the visible state of the window changed.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (window.IsVisible)
            {
                OnWindowShown();
            }
        }

        /// <summary>
        /// Called when the window gets shown.
        /// </summary>
        private void OnWindowShown()
        {
            Storyboard storyboard = CreateOpenWindowAnimation();
            storyboard.Completed += OnOpenCompleted;
            storyboard.Freeze();
            window.Opening = true;
            storyboard.Begin(window, true);

            // Start the highlight animation.
            RestartHighlightAnimation(null, null);
        }

        /// <summary>
        /// Called when the window completed it's opening.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void OnOpenCompleted(object sender, EventArgs e)
        {
            window.Opening = false;
            window.InvalidateVisual();
        }

        /// <summary>
        /// Callback for animation finsh. Restarts the highlight animation.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Event arguments.</param>
        private void RestartHighlightAnimation(object sender, EventArgs e)
        {
            highlightStoryboard = CreateHighlightAnimation(-window.HighlightWidth);
            highlightStoryboard.Freeze();
            highlightStoryboard.Begin(window, true);
        }

        /// <summary>
        /// Creates a new Storyboard for the open animation of the ClawWindow.
        /// </summary>
        /// <returns>The created Storyboard.</returns>
        public virtual Storyboard CreateOpenWindowAnimation()
        {
            const double BLEND_IN_DURATION = 0.25;
            const double FINISH_DURATION = 0.1;

            double width = window.Width;

            double[] froms = { 0, width - 1, window.Left + width / 2, window.Left - 1 };
            double[] tos = { width - 1, width, window.Left - 1, window.Left };
            Duration[] durations = {
                new Duration(TimeSpan.FromSeconds(BLEND_IN_DURATION)),
                new Duration(TimeSpan.FromSeconds(FINISH_DURATION)),
                new Duration(TimeSpan.FromSeconds(BLEND_IN_DURATION)),
                new Duration(TimeSpan.FromSeconds(FINISH_DURATION)),
            };
            TimeSpan[] beginTimes = {
                TimeSpan.Zero,
                TimeSpan.FromSeconds(BLEND_IN_DURATION),
                TimeSpan.Zero,
                TimeSpan.FromSeconds(BLEND_IN_DURATION),
            };
            PropertyPath[] paths = {
                new PropertyPath(Window.WidthProperty),
                new PropertyPath(Window.WidthProperty),
                new PropertyPath(Window.LeftProperty),
                new PropertyPath(Window.LeftProperty),
            };

            Storyboard storyboard = new Storyboard();
            for (int i = 0; i < froms.Length; i++)
            {
                DoubleAnimation animation = new DoubleAnimation();
                animation.From = froms[i];
                animation.To = tos[i];
                animation.BeginTime = beginTimes[i];
                animation.Duration = durations[i];
                Storyboard.SetTarget(animation, window);
                Storyboard.SetTargetProperty(animation, paths[i]);
                storyboard.Children.Add(animation);
            }

            return storyboard;
        }

        /// <summary>
        /// Creates the animation for the highlight.
        /// </summary>
        /// <param name="from">The value to start at in window space.</param>
        /// <returns>The created storyboard.</returns>
        public virtual Storyboard CreateHighlightAnimation(double from)
        {
            Storyboard storyboard = new Storyboard();

            double width = window.Opening ? window.Width : window.ActualWidth;

            DoubleAnimation animation = new DoubleAnimation();
            animation.From = from;
            animation.To = width;
            animation.Duration = new Duration(TimeSpan.FromSeconds((width - from) / 60));
            animation.RepeatBehavior = RepeatBehavior.Forever;
            Storyboard.SetTarget(animation, window);
            Storyboard.SetTargetProperty(animation, new PropertyPath(ClawWindow.HighlightPositionProperty));
            storyboard.Children.Add(animation);

            return storyboard;
        }
    }
}
