/* For windows message constants see winuser.h */

namespace Winforms.Components.ApplicationIdleData
{
    /// <summary>
    /// Windows messages that will cause the component to consider the application not idle.
    /// </summary>
    public enum ActivityMessages : int
    {
        /// <summary>
        /// Cursor moved while within the nonclient area.
        /// </summary>
        WM_NCMOUSEMOVE = 0x00A0,
        /// <summary>
        /// Mouse left button pressed while the cursor was within the nonclient area.
        /// </summary>
        WM_NCLBUTTONDOWN = 0x00A1,
        /// <summary>
        /// Mouse left button released while the cursor was within the nonclient area.
        /// </summary>
        WM_NCLBUTTONUP = 0x00A2,
        /// <summary>
        /// Mouse left button double-clicked while the cursor was within the nonclient area.
        /// </summary>
        WM_NCLBUTTONDBLCLK = 0x00A3,
        /// <summary>
        /// Mouse right button pressed while the cursor was within the nonclient area.
        /// </summary>
        WM_NCRBUTTONDOWN = 0x00A4,
        /// <summary>
        /// Mouse right button released while the cursor was within the nonclient area.
        /// </summary>
        WM_NCRBUTTONUP = 0x00A5,
        /// <summary>
        /// Mouse right button double-clicked while the cursor was within the nonclient area.
        /// </summary>
        WM_NCRBUTTONDBLCLK = 0x00A6,
        /// <summary>
        /// Mouse middle button pressed while the cursor was within the nonclient area.
        /// </summary>
        WM_NCMBUTTONDOWN = 0x00A7,
        /// <summary>
        /// Mouse middle button released while the cursor was within the nonclient area.
        /// </summary>
        WM_NCMBUTTONUP = 0x00A8,
        /// <summary>
        /// Mouse middle button double-clicked while the cursor was within the nonclient area.
        /// </summary>
        WM_NCMBUTTONDBLCLK = 0x00A9,
        /// <summary>
        /// Key pressed.
        /// </summary>
        WM_KEYDOWN = 0x0100,
        /// <summary>
        /// Key released.
        /// </summary>
        WM_KEYUP = 0x0101,
        /// <summary>
        /// F10 key, or held down the ALT key and then another key pressed.
        /// </summary>
        WM_SYSKEYDOWN = 0x0104,
        /// <summary>
        /// Key released that was pressed while the ALT key was held down.
        /// </summary>
        WM_SYSKEYUP = 0x0105,
        /// <summary>
        /// Cursor moved.
        /// </summary>
        WM_MOUSEMOVE = 0x0200,
        /// <summary>
        /// Mouse left button pressed.
        /// </summary>
        WM_LBUTTONDOWN = 0x0201,
        /// <summary>
        /// Mouse left button released.
        /// </summary>
        WM_LBUTTONUP = 0x0202,
        /// <summary>
        /// Mouse left button double-clicked.
        /// </summary>
        WM_LBUTTONDBLCLK = 0x0203,
        /// <summary>
        /// Mouse right button pressed.
        /// </summary>
        WM_RBUTTONDOWN = 0x0204,
        /// <summary>
        /// Mouse right button released.
        /// </summary>
        WM_RBUTTONUP = 0x0205,
        /// <summary>
        /// Mouse right button double-clicked.
        /// </summary>
        WM_RBUTTONDBLCLK = 0x0206,
        /// <summary>
        /// Mouse middle button pressed.
        /// </summary>
        WM_MBUTTONDOWN = 0x0207,
        /// <summary>
        /// Mouse middle button releaseed.
        /// </summary>
        WM_MBUTTONUP = 0x0208,
        /// <summary>
        /// Mouse middle button double-clicked.
        /// </summary>
        WM_MBUTTONDBLCLK = 0x0209,
        /// <summary>
        /// Mouse wheel rotated.
        /// </summary>
        WM_MOUSEWHEEL = 0x020A,
    }
}
