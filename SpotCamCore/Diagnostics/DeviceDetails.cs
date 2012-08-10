using System;

namespace SpotCam.Diagnostics
{
    public enum DeviceClass : int
    {
        Unknown = 0,
        UniversalPciCard = 3,
        InsightPciCard = 4,
        RTSE18Card = 6,
        RT2Card = 7,
        FirewireCamera = 32,
        UsbCamera = 41
    }

    public enum DeviceIoBus : int
    {
        Pci = 2,
        Ieee1394 = 3,
        Usb = 4,
        Ipv4 = 5
    }


}
