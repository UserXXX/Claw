using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Claw.Native
{
    /// <summary>
    /// Wrapper class for p/invoke calls to the user32.dll.
    /// </summary>
    public static class User32
    {
        /// <summary>
        /// Quote from MSDN:
        /// Sends the specified message to a window or windows. The SendMessage function calls the window procedure for the specified window and does not return until the window procedure has processed the message.
        /// 
        /// For more details see https://msdn.microsoft.com/en-us/library/windows/desktop/ms644950%28v=vs.85%29.aspx.
        /// </summary>
        /// <param name="hWnd">A handle to the window whose window procedure will receive the message. If this parameter is HWND_BROADCAST ((HWND)0xffff), the message is sent to all top-level windows in the system, including disabled or invisible unowned windows, overlapped windows, and pop-up windows; but the message is not sent to child windows.
        /// Message sending is subject to UIPI. The thread of a process can send messages only to message queues of threads in processes of lesser or equal integrity level.</param>
        /// <param name="msg">The message to be sent.</param>
        /// <param name="wParam">Additional message-specific information.</param>
        /// <param name="lParam">Additional message-specific information.</param>
        /// <returns>The return value specifies the result of the message processing; it depends on the message sent.</returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 msg, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// Quote from MSDN:
        /// The MonitorFromWindow function retrieves a handle to the display monitor that has the largest area of intersection with the bounding rectangle of a specified window.
        /// 
        /// For more details see https://msdn.microsoft.com/en-us/library/dd145064%28v=vs.85%29.aspx.
        /// </summary>
        /// <param name="hwnd">A handle to the window of interest.</param>
        /// <param name="dwFlags">Determines the function's return value if the window does not intersect any display monitor.</param>
        /// <returns>If the window intersects one or more display monitor rectangles, the return value is an HMONITOR handle to the display monitor that has the largest area of intersection with the window.
        /// If the window does not intersect a display monitor, the return value depends on the value of dwFlags.</returns>
        [DllImport("user32.dll")]
        public static extern IntPtr MonitorFromWindow(IntPtr hwnd, int dwFlags);

        /// <summary>
        /// Quote from MSDN:
        /// The GetMonitorInfo function retrieves information about a display monitor.
        /// 
        /// For more details see https://msdn.microsoft.com/en-us/library/dd144901%28v=vs.85%29.aspx.
        /// </summary>
        /// <param name="hMonitor">A handle to the display monitor of interest.</param>
        /// <param name="lpmi">A pointer to a MONITORINFO or MONITORINFOEX structure that receives information about the specified display monitor.
        /// You must set the cbSize member of the structure to sizeof(MONITORINFO) or sizeof(MONITORINFOEX) before calling the GetMonitorInfo function. Doing so lets the function determine the type of structure you are passing to it.
        /// The MONITORINFOEX structure is a superset of the MONITORINFO structure. It has one additional member: a string that contains a name for the display monitor. Most applications have no use for a display monitor name, and so can save some bytes by using a MONITORINFO structure.</param>
        /// <returns>If the function succeeds, the return value is nonzero (true).
        /// If the function fails, the return value is zero (false).</returns>
        [DllImport("user32.dll")]
        public static extern bool GetMonitorInfo(IntPtr hMonitor, MONITORINFO lpmi);
    }
}
