using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpotCam.Diagnostics
{
    [Flags]
    public enum FirmwareUpdateResults
    {
        PowerOffDevice = 1,
        PowerOffComputer = 2,
        RebootComputer  = 4
    }

    [Flags]
    public enum FirmwareUpdateOptions
    {
        AllowDowngradeOnCamera = 1,
        AllowDowngradeOnInterfaceCard = 2
    }
}
