using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowCustomization
{
    [Flags]
    public enum SystemButton
    {
            Minimize = 1,
            Maximize = 2,
            Close = 4
    }

    public delegate void HelpButtonClicked(object sender, object context);
}
