using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpotCam
{
    public enum DisconnectionReason
    {
        Unknown,
        Requested,
        DeviceRemoved,
        ForcedByService
    }

    public class DisconnectionEventArgs : EventArgs
    {
        public DisconnectionEventArgs(DisconnectionReason reason)
        {
            Reason = reason;
        }

        public DisconnectionReason Reason { get; private set; }
    }
}
