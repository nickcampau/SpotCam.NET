using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpotCam.Diagnostics
{
    [Flags]
    public enum GainPortAttributes : uint
    {
        None = 0,
        Computable   = 0x01,
        SameAsPort0  = 0x02,
        ElectronMultipling = 0x04,
        Gating       = 0x08
    }
}
