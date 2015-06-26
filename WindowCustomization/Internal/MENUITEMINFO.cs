using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowCustomization.Internal
{
    internal class MENUITEMINFO
    {
            public uint cbSize;
            public uint fMask;
            public uint fType;
            public uint fState;
            public uint wID;
            public IntPtr hSubMenu;
            public IntPtr hbmpChecked;
            public IntPtr hbmpUnchecked;
            public UIntPtr dwItemData;
            public string dwTypeData;
            public uint cch;
            public IntPtr hbmpItem;
    }
}
