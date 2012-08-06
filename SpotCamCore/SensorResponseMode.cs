using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpotCam
{
    public enum SensorResponseMode : uint
    {
        Default = 0,
        EnhancedIr = 1,
        DynamicRange = 2,
        Sensitivity =  4,
        AntiBlooming = 8,
        GlowSuppression = 16
    }
}
