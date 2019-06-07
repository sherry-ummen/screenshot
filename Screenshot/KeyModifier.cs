using System;

namespace Screenshot
{
    [Flags]
    public enum KeyModifier
    {
        Alt = 0x0001,
        Ctrl = 0x0002,
        Shift = 0x0004,
    }
}
