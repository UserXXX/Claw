using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Claw.Native
{
    /// <summary>
    /// This file contains structures defined by Windows.h, rewritten to use them for p/invoke.
    /// </summary>
    
    /// <summary>
    /// Quote from MSDN:
    /// The POINT structure defines the x- and y- coordinates of a point.
    /// 
    /// For details see https://msdn.microsoft.com/en-us/library/dd162805%28v=vs.85%29.aspx.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "POINT", Justification="The struct is named the same way as the native struct."), StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "x", Justification="The same name as in the original struct is used.")]
        private int x;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "y", Justification = "The same name as in the original struct is used.")]
        private int y;

        /// <summary>
        /// The x-coordinate of the point.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "X", Justification = "See variable x.")]
        public int X
        {
            get { return x; }
            set { x = value; }
        }

        /// <summary>
        /// The y-coordinate of the point.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Y", Justification = "See variable y.")]
        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        /// <summary>
        /// Creates a new point.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Justification = "The same names as in the original struct are used.")]
        public POINT(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is POINT))
            {
                return false;
            }

            POINT other = (POINT)obj;
            return other.x == x && other.y == y;
        }

        public override int GetHashCode()
        {
            return x + y - 5489;
        }

        public static bool operator ==(POINT point1, POINT point2)
        {
            return point1.Equals(point2);
        }

        public static bool operator !=(POINT point1, POINT point2)
        {
            return !point1.Equals(point2);
        }
    }

    /// <summary>
    /// Quote from MSDN:
    /// Contains information about a window's maximized size and position and its minimum and maximum tracking size.
    /// 
    /// For more details see https://msdn.microsoft.com/en-us/library/windows/desktop/ms632605%28v=vs.85%29.aspx.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "MINMAXINFO", Justification = "The struct is named the same way as the native struct."), StructLayout(LayoutKind.Sequential)]
    public struct MINMAXINFO
    {
        private POINT ptReserved;
        private POINT ptMaxSize;
        private POINT ptMaxPosition;
        private POINT ptMinTrackSize;
        private POINT ptMaxTrackSize;

        /// <summary>
        /// Reserved; do not use.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Pt", Justification = "The name is spelled correctly.")]
        public POINT PtReserved
        {
            get { return ptReserved; }
            set { ptReserved = value; }
        }

        /// <summary>
        /// The maximized width (x member) and the maximized height (y member) of the window. For top-level windows, this value is based on the width of the primary monitor.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Pt", Justification = "The name is spelled correctly.")]
        public POINT PtMaxSize
        {
            get { return ptMaxSize; }
            set { ptMaxSize = value; }
        }

        /// <summary>
        /// The position of the left side of the maximized window (x member) and the position of the top of the maximized window (y member). For top-level windows, this value is based on the position of the primary monitor.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Pt", Justification = "The name is spelled correctly.")]
        public POINT PtMaxPosition
        {
            get { return ptMaxPosition; }
            set { ptMaxPosition = value; }
        }

        /// <summary>
        /// The minimum tracking width (x member) and the minimum tracking height (y member) of the window. This value can be obtained programmatically from the system metrics SM_CXMINTRACK and SM_CYMINTRACK (see the GetSystemMetrics function).
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Pt", Justification = "The name is spelled correctly.")]
        public POINT PtMinTrackSize
        {
            get { return ptMinTrackSize; }
            set { ptMinTrackSize = value; }
        }

        /// <summary>
        /// The maximum tracking width (x member) and the maximum tracking height (y member) of the window. This value is based on the size of the virtual screen and can be obtained programmatically from the system metrics SM_CXMAXTRACK and SM_CYMAXTRACK (see the GetSystemMetrics function).
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Pt", Justification = "The name is spelled correctly.")]
        public POINT PtMaxTrackSize
        {
            get { return ptMaxTrackSize; }
            set { ptMaxTrackSize = value; }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is MINMAXINFO))
            {
                return false;
            }

            return this == (MINMAXINFO)obj;
        }

        public override int GetHashCode()
        {
            return 23145 + PtReserved.GetHashCode() + PtMaxSize.GetHashCode() + PtMaxPosition.GetHashCode() + PtMinTrackSize.GetHashCode() + PtMaxTrackSize.GetHashCode();
        }

        public static bool operator ==(MINMAXINFO info1, MINMAXINFO info2)
        {
            return info1.PtReserved == info2.PtReserved && info1.PtMaxSize == info2.PtMaxSize && info1.PtMaxPosition == info2.PtMaxPosition && info1.PtMinTrackSize == info2.PtMinTrackSize && info1.PtMaxTrackSize == info2.PtMaxTrackSize;
        }

        public static bool operator !=(MINMAXINFO info1, MINMAXINFO info2)
        {
            return !(info1 == info2);
        }
    };

    /// <summary>
    /// Quote from MSDN:
    /// The MONITORINFO structure contains information about a display monitor.
    /// The GetMonitorInfo function stores information in a MONITORINFO structure or a MONITORINFOEX structure.
    /// The MONITORINFO structure is a subset of the MONITORINFOEX structure. The MONITORINFOEX structure adds a string member to contain a name for the display monitor.
    /// 
    /// For more details see: https://msdn.microsoft.com/en-us/library/dd145065%28v=vs.85%29.aspx.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "MONITORINFO", Justification = "The struct is named the same way as the native struct."),
    StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class MONITORINFO
    {
        private int cbSize = Marshal.SizeOf(typeof(MONITORINFO));
        private RECT rcMonitor = new RECT();
        private RECT rcWork = new RECT();
        private int dwFlags = 0;

        /// <summary>
        /// The size of the structure, in bytes.
        /// Set this member to sizeof ( MONITORINFO ) before calling the GetMonitorInfo function. Doing so lets the function determine the type of structure you are passing to it.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Cb", Justification = "The name is spelled correctly.")]
        public int CbSize
        {
            get { return cbSize; }
            set { cbSize = value; }
        }

        /// <summary>
        /// A RECT structure that specifies the display monitor rectangle, expressed in virtual-screen coordinates. Note that if the monitor is not the primary display monitor, some of the rectangle's coordinates may be negative values.
        /// </summary> 
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Rc", Justification = "The name is spelled correctly.")]
        public RECT RcMonitor
        {
            get { return rcMonitor; }
            set { rcMonitor = value; }
        }

        /// <summary>
        /// A RECT structure that specifies the work area rectangle of the display monitor, expressed in virtual-screen coordinates. Note that if the monitor is not the primary display monitor, some of the rectangle's coordinates may be negative values.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Rc", Justification = "The name is spelled correctly.")]
        public RECT RcWork
        {
            get { return rcWork; }
            set { rcWork = value; }
        }

        /// <summary>
        /// A set of flags that represent attributes of the display monitor.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Flags", Justification="This is part of the native name."),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Dw", Justification = "The name is spelled correctly.")]
        public int DwFlags
        {
            get { return dwFlags; }
            set { dwFlags = value; }
        }
    }

    /// <summary>
    /// Quote from MSDN:
    /// The RECT structure defines the coordinates of the upper-left and lower-right corners of a rectangle.
    /// 
    /// For more details see https://msdn.microsoft.com/en-us/library/dd162897%28v=vs.85%29.aspx.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "RECT", Justification = "The struct is named the same way as the native struct."),
    StructLayout(LayoutKind.Sequential, Pack = 0)]
    public struct RECT
    {
        private int left;
        private int top;
        private int right;
        private int bottom;

        /// <summary>
        /// The x-coordinate of the upper-left corner of the rectangle.
        /// </summary>
        public int Left
        {
            get { return left; }
            set { left = value; }
        }

        /// <summary>
        /// The y-coordinate of the upper-left corner of the rectangle.
        /// </summary>
        public int Top
        {
            get { return top; }
            set { top = value; }
        }

        /// <summary>
        /// The x-coordinate of the lower-right corner of the rectangle.
        /// </summary>
        public int Right
        {
            get { return right; }
            set { right = value; }
        }

        /// <summary>
        /// The y-coordinate of the lower-right corner of the rectangle.
        /// </summary>
        public int Bottom
        {
            get { return bottom; }
            set { bottom = value; }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is RECT))
            {
                return false;
            }

            RECT other = (RECT)obj;
            return other.Left == Left && other.Top == Top && other.Right == Right && other.Bottom == Bottom;
        }

        public override int GetHashCode()
        {
            return 1923 - left + top + right + bottom;
        }

        public static bool operator ==(RECT rect1, RECT rect2)
        {
            return rect1.Equals(rect2);
        }

        public static bool operator !=(RECT rect1, RECT rect2)
        {
            return !rect1.Equals(rect2);
        }
    }
}
