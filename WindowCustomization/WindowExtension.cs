using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace WindowCustomization
{
    public static class WindowExtension
    {

        /// <summary>
        /// Allows to specify buttons on a window that shall be disabled.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="buttons">The button to be activated - multiple buttons combined via | are possible.</param>
        public static void DisableButtons(this Window target, SystemButton buttons)
        {
            Internal.SystemMenuManager.DisableSystemButtons(target, buttons);
        }

        /// <summary>
        /// Allows to specify which buttons (minimize, maximize and close) shall be displayed on a given window.
        /// To be called after a call to DisableButtons to re-enable buttons.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="buttons">The button to be activated - multiple buttons combined via | are possible.</param>
        public static void EnableSystemButtons(this Window target, SystemButton buttons)
        {
            Internal.SystemMenuManager.EnableSystemButtons(target, buttons);
        }


        /// <summary>
        /// Enables the system menu, thus restoring the window's icon, the system menu itself and all the buttons (minimize, maximize, close).
        /// To be called after the system menu was disabled via DisableSystemMenu.
        /// </summary>
        /// <param name="target"></param>
        public static void EnableSystemMenu(this Window target)
        {
            Internal.SystemMenuManager.EnableSystemMenu(target);
        }


        /// <summary>
        /// Disables the system menu. That means no minimize, maximize and close buttons as well as no icon and no menu
        /// on right mouseclick over the window caption bar. The window can still be resized, though.
        /// </summary>
        /// <param name="target"></param>
        public static void DisableSystemMenu(this Window target)
        {
            Internal.SystemMenuManager.DisableSystemMenu(target);
        }

        /// <summary>
        /// Hides the minimize and maximize buttons and displays a help button (?) instead of them.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="callbackDelegate">The function to be called when the ?-button is clicked.</param>
        public static void ShowHelpButton(this Window target, HelpButtonClicked callbackDelegate)
        {
            Internal.SystemMenuManager.ShowHelpButton(target, callbackDelegate);
        }

        /// <summary>
        /// Hides the previously displayed ?-Button. Furthermore two bools allow for individual control over the minimize
        /// and maximize buttons.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="showMinimizeButton">Determines whether the minimized button shall be enabled. By default this flag is true.</param>
        /// <param name="showMaximizeButton">Determines whether the minimized button shall be enabled. By default this flag is true.</param>
        public static void HideHelpButton(this Window target, bool showMinimizeButton = true, bool showMaximizeButton = true)
        {
            Internal.SystemMenuManager.HideHelpButton(target, showMinimizeButton, showMaximizeButton);
        }

        /// <summary>
        /// Hides the caption of the window on which it is called.
        /// The caption is still visible in the taskbar, the Taskmanager and Alt+[Tab]
        /// </summary>
        /// <param name="target"></param>
        public static void HideWindowCaption(this Window target)
        {
            Internal.SystemMenuManager.HideCaption(target);
        }

        /// <summary>
        /// Hides the icon of a given window. The icon won't be visibly present anymore, but it's dimensions will still be clickable
        /// and will cause the system menu to open.
        /// The icon will still be displayed in the taskbar and (under Win7) in the taskbar popup thumbnail.
        /// 
        /// ATTENTION:
        /// In order to function as expected, this function needs to be called inside the target window's SourceInitialized event handler.
        /// </summary>
        /// <param name="target"></param>
        public static void HideWindowIcon(this Window target)
        {
            Internal.SystemMenuManager.HideIcon(target);
        }

        public static void Test(this Window target)
        {
            Internal.SystemMenuManager.SetFlags(target);
        }

    }
}
