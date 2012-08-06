using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpotCam
{
    public enum ReadoutChannelMode : uint
    {
        Unknown = 0,
        SingleChannel = 1,
        DualChannel = 0x10002
    }
}
