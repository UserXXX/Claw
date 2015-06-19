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
    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        /// <summary>
        /// The x-coordinate of the point.
        /// </summary>
        public int x;
        /// <summary>
        /// The y-coordinate of the point.
        /// </summary>
        public int y;

        /// <summary>
        /// Creates a new point.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        public POINT(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    /// <summary>
    /// Quote from MSDN:
    /// Contains information about a window's maximized size and position and its minimum and maximum tracking size.
    /// 
    /// For more details see https://msdn.microsoft.com/en-us/library/windows/desktop/ms632605%28v=vs.85%29.aspx.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MINMAXINFO
    {
        /// <summary>
        /// Reserved; do not use.
        /// </summary>
        public POINT ptReserved;
        /// <summary>
        /// The maximized width (x member) and the maximized height (y member) of the window. For top-level windows, this value is based on the width of the primary monitor.
        /// </summary>
        public POINT ptMaxSize;
        /// <summary>
        /// The position of the left side of the maximized window (x member) and the position of the top of the maximized window (y member). For top-level windows, this value is based on the position of the primary monitor.
        /// </summary>
        public POINT ptMaxPosition;
        /// <summary>
        /// The minimum tracking width (x member) and the minimum tracking height (y member) of the window. This value can be obtained programmatically from the system metrics SM_CXMINTRACK and SM_CYMINTRACK (see the GetSystemMetrics function).
        /// </summary>
        public POINT ptMinTrackSize;
        /// <summary>
        /// The maximum tracking width (x member) and the maximum tracking height (y member) of the window. This value is based on the size of the virtual screen and can be obtained programmatically from the system metrics SM_CXMAXTRACK and SM_CYMAXTRACK (see the GetSystemMetrics function).
        /// </summary>
        public POINT ptMaxTrackSize;
    };

    /// <summary>
    /// Quote from MSDN:
    /// The MONITORINFO structure contains information about a display monitor.
    /// The GetMonitorInfo function stores information in a MONITORINFO structure or a MONITORINFOEX structure.
    /// The MONITORINFO structure is a subset of the MONITORINFOEX structure. The MONITORINFOEX structure adds a string member to contain a name for the display monitor.
    /// 
    /// For more details see: https://msdn.microsoft.com/en-us/library/dd145065%28v=vs.85%29.aspx.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class MONITORINFO
    {
        /// <summary>
        /// The size of the structure, in bytes.
        /// Set this member to sizeof ( MONITORINFO ) before calling the GetMonitorInfo function. Doing so lets the function determine the type of structure you are passing to it.
        /// </summary>
        public int cbSize = Marshal.SizeOf(typeof(MONITORINFO));

        /// <summary>
        /// A RECT structure that specifies the display monitor rectangle, expressed in virtual-screen coordinates. Note that if the monitor is not the primary display monitor, some of the rectangle's coordinates may be negative values.
        /// </summary>            
        public RECT rcMonitor = new RECT();

        /// <summary>
        /// A RECT structure that specifies the work area rectangle of the display monitor, expressed in virtual-screen coordinates. Note that if the monitor is not the primary display monitor, some of the rectangle's coordinates may be negative values.
        /// </summary>            
        public RECT rcWork = new RECT();

        /// <summary>
        /// A set of flags that represent attributes of the display monitor.
        /// </summary>            
        public int dwFlags = 0;
    }

    /// <summary>
    /// Quote from MSDN:
    /// The RECT structure defines the coordinates of the upper-left and lower-right corners of a rectangle.
    /// 
    /// For more details see https://msdn.microsoft.com/en-us/library/dd162897%28v=vs.85%29.aspx.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 0)]
    public struct RECT
    {
        /// <summary>
        /// The x-coordinate of the upper-left corner of the rectangle.
        /// </summary>
        public int left;

        /// <summary>
        /// The y-coordinate of the upper-left corner of the rectangle.
        /// </summary>
        public int top;

        /// <summary>
        /// The x-coordinate of the lower-right corner of the rectangle.
        /// </summary>
        public int right;

        /// <summary>
        /// The y-coordinate of the lower-right corner of the rectangle.
        /// </summary>
        public int bottom;
    }
}
