using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Interop;

namespace WindowCustomization.Internal
{
    internal class WindowMessageHelper
    {

        private static HelpButtonClicked _callback = null;
        private static Window _window;

        internal static void RegisterWindowsMessages(Window window, HelpButtonClicked callback)
        {
            ((HwndSource) PresentationSource.FromVisual(window)).AddHook(new HwndSourceHook(WindowMessage));
            _window = window;
            _callback = callback;
        }

        internal static void UnregisterWindowsMessages(Window window)
        {
            ((HwndSource) PresentationSource.FromVisual(window)).RemoveHook(WindowMessage);
        }

        internal static IntPtr WindowMessage(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if(msg == SystemMenuManager.WM_SYSCOMMAND && ((int)wParam & 0xFFF0) == SystemMenuManager.SC_CONTEXTHELP)
            {
                if (_callback != null)
                    _callback(_window, null);

                handled = true;
            }

            return IntPtr.Zero;
        }
    }
}
