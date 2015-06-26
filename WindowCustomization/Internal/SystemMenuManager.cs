using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Interop;

namespace WindowCustomization.Internal
{
    internal class SystemMenuManager
    {
        [DllImport("user32.dll")]
        private static extern long GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern long SetWindowLong(IntPtr hWnd, int nIndex, long dwNewLong);

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll")]
        private static extern bool EnableMenuItem(IntPtr hMenu, uint uIDEnableItem, uint uEnable);

        [DllImport("user32.dll")]
        private static extern IntPtr DestroyMenu(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        static extern bool SetWindowPos(IntPtr hwnd, IntPtr hwndInsertAfter, int x, int y, int width, int height, uint flags);

        [DllImport("user32.dll")]
        public static extern bool InsertMenuItem(IntPtr hMenu, uint uItem, bool fByPosition, [In] ref MENUITEMINFO lpmii);

        [DllImport("uxtheme.dll")]
        public static extern int SetWindowThemeAttribute(IntPtr hWnd, WindowThemeAttributeType wtype, ref WTA_OPTIONS attributes, uint size);

        public enum WindowThemeAttributeType : uint
        {
            /// <summary>Non-client area window attributes will be set.</summary>
            WTA_NONCLIENT = 1,
        }

        public struct WTA_OPTIONS
        {
            public uint Flags;
            public uint Mask;
        }

        private static uint WTNCA_NODRAWCAPTION = 0x00000001;
        private static uint WTNCA_NODRAWICON = 0x00000002;
        private static uint WTNCA_NOSYSMENU = 0x00000004;
        private static uint WTNCA_NOMIRRORHELP = 0x00000008;



        private const int GWL_STYLE = -16;
        private const int GWL_EXSTYLE = -20;

        private const long WS_MAXIMIZEBOX = 0x10000;
        private const long WS_MINIMIZEBOX = 0x20000;
        private const long WS_SYSMENU = 0x80000;
        private const long WS_POPUP = 2147483648;
        private const long WS_VISIBLE = 268435456;
        private const long WS_CLIPSIBLINGS = 67108864;
        private const long WS_CLIPCHILDREN = 33554432;


        private const long WS_EX_LEFT = 0;
        private const long WS_EX_LTRREADING = 0;
        private const long WS_EX_RIGHTSCROLLBAR = 0;
        private const long WS_EX_TOOLWINDOW = 128;
        private const long WS_EX_LAYERED = 524288;
        private const long WS_EX_DLGMODALFRAME = 0x0001;
        private const long WS_EX_CONTEXTHELP = 0x0400;

        private const int SWP_NOSIZE = 0x0001;
        private const int SWP_NOMOVE = 0x0002;
        private const int SWP_NOZORDER = 0x0004;
        private const int SWP_FRAMECHANGED = 0x0020;


        private const uint MF_BYCOMMAND = 0x00000000;
        private const uint MF_GRAYED = 0x00000001;
        private const uint MF_ENABLED = 0x00000000;

        private const uint SC_CLOSE = 0xF060;
        public const uint SC_CONTEXTHELP = 0xF180;

        private const uint WM_SETICON = 0x0080;
        public const int WM_SYSCOMMAND = 0x0112;




        internal static void DisableSystemButtons(Window window, SystemButton buttons)
        {
            var hwnd = GetWindowHwnd(window);
            var value = GetWindowLong(hwnd, GWL_STYLE);


            if (buttons.IsSet<SystemButton>(SystemButton.Minimize))
            {
                value = value & ~WS_MINIMIZEBOX;
            }

            if (buttons.IsSet<SystemButton>(SystemButton.Maximize))
            {
                value = value & ~WS_MAXIMIZEBOX;
            }

            if (buttons.IsSet<SystemButton>(SystemButton.Close))
            {
                DisableCloseButton(window);
            }


            SetWindowLong(hwnd, GWL_STYLE, value);
        }


        internal static void EnableSystemButtons(Window window, SystemButton buttons)
        {
            var hwnd = GetWindowHwnd(window);
            var value = GetWindowLong(hwnd, GWL_STYLE);

            if (buttons.IsSet<SystemButton>(SystemButton.Minimize))
            {
                value |= WS_MINIMIZEBOX;
            }

            if (buttons.IsSet<SystemButton>(SystemButton.Maximize))
            {
                value |= WS_MAXIMIZEBOX;
            }

            if (buttons.IsSet<SystemButton>(SystemButton.Close))
            {
                EnableCloseButton(window);
            }

            SetWindowLong(hwnd, GWL_STYLE, value);

        }

        #region Close enable / disable



        private static void EnableCloseButton(Window window)
        {
            var hwnd = GetWindowHwnd(window);
            var menuHwnd = GetSystemMenu(hwnd, false);

            EnableMenuItem(menuHwnd, SC_CLOSE, MF_ENABLED);
            DestroyMenu(menuHwnd);
        }

        private static void DisableCloseButton(Window window)
        {
            var hwnd = GetWindowHwnd(window);
            var menuHwnd = GetSystemMenu(hwnd, false);

            EnableMenuItem(menuHwnd, SC_CLOSE, MF_BYCOMMAND | MF_GRAYED);
        }


        #endregion


        internal static void EnableSystemMenu(Window window)
        {
            var hwnd = GetWindowHwnd(window);
            var value = GetWindowLong(hwnd, GWL_STYLE);

            value = value | WS_SYSMENU;
            SetWindowLong(hwnd, GWL_STYLE, value);
        }

        internal static void DisableSystemMenu(Window window)
        {
            var hwnd = GetWindowHwnd(window);
            var value = GetWindowLong(hwnd, GWL_STYLE);

            value = value & ~WS_SYSMENU;
            SetWindowLong(hwnd, GWL_STYLE, value);
        }


        internal static void ShowHelpButton(Window window, HelpButtonClicked callbackDelegate)
        {
            var hwnd = GetWindowHwnd(window);
            var value = GetWindowLong(hwnd, GWL_EXSTYLE);

            DisableSystemButtons(window, SystemButton.Minimize | SystemButton.Maximize);
            value |= WS_EX_CONTEXTHELP;

            SetWindowLong(hwnd, GWL_EXSTYLE, value);


            Internal.WindowMessageHelper.RegisterWindowsMessages(window, callbackDelegate);
        }

        internal static void HideHelpButton(Window window, bool showMinimize, bool showMaximize)
        {
            Internal.WindowMessageHelper.UnregisterWindowsMessages(window);

            var hwnd = GetWindowHwnd(window);
            var value = GetWindowLong(hwnd, GWL_EXSTYLE);

            value = value & ~WS_EX_CONTEXTHELP;

            SetWindowLong(hwnd, GWL_EXSTYLE, value);


            if (showMinimize)
                EnableSystemButtons(window, SystemButton.Minimize);

            if (showMaximize)
                EnableSystemButtons(window, SystemButton.Maximize);
        }


        // DEBUG
        private static void EnableWindowIcon(Window window)
        {
            var hwnd = GetWindowHwnd(window);
            var value = GetWindowLong(hwnd, GWL_EXSTYLE);
            value = value &~ 1;
            SetWindowLong(hwnd, GWL_EXSTYLE, value);

            SendMessage(hwnd, 128, IntPtr.Zero, IntPtr.Zero);
            SendMessage(hwnd, 128, new IntPtr(1), IntPtr.Zero);
        }


        internal static void HideIcon(Window window)
        {
            HideChromeElementCore(window, true, false);
        }

        internal static void HideCaption(Window window)
        {
            HideChromeElementCore(window, false, true);
        }

        internal static void HideIconAndCaption(Window window)
        {
            // Works but looks ugly somehow
            //HideChromeElementCore(window, true, true);



        }


        internal static void SetFlags(Window window)
        {
            var hwnd = GetWindowHwnd(window);
            var style = GetWindowLong(hwnd, GWL_STYLE);
            var exStyle = GetWindowLong(hwnd, GWL_EXSTYLE);

            style = style | WS_POPUP | WS_VISIBLE | WS_CLIPSIBLINGS | WS_CLIPCHILDREN;
            exStyle = exStyle | WS_EX_LEFT | WS_EX_LTRREADING | WS_EX_RIGHTSCROLLBAR | WS_EX_TOOLWINDOW | WS_EX_LAYERED;

            SetWindowLong(hwnd, GWL_STYLE, style);
            SetWindowLong(hwnd, GWL_EXSTYLE, exStyle);


        }



        // Helper

        private static IntPtr GetWindowHwnd(Window window)
        {
            var tmp = new WindowInteropHelper(window);
            tmp.EnsureHandle();
            return new WindowInteropHelper(window).Handle;

        }


        private static void HideChromeElementCore(Window window, bool hideIcon, bool hideCaption)
        {
            var wta = new WTA_OPTIONS();

            if (hideCaption)
            {
                wta.Flags |= WTNCA_NODRAWCAPTION;
                wta.Mask |= WTNCA_NODRAWCAPTION;
            }

            if (hideIcon)
            {
                wta.Flags |= WTNCA_NODRAWICON;
                wta.Mask |= WTNCA_NODRAWICON;
            }

            var hwnd = GetWindowHwnd(window);
            SetWindowThemeAttribute(hwnd, WindowThemeAttributeType.WTA_NONCLIENT, ref wta,
                (uint)Marshal.SizeOf(typeof(WTA_OPTIONS)));
        }
    }

}
