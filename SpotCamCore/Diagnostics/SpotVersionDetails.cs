using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace SpotCam.Diagnostics
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct SpotVersionDetails
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 255)]
        public string ProductName;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 101)]
        public string Copyright;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string CameraFirmwareRevNum;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string CardHardwareRevNum;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string CardFirmwareRevNum;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 46)]
        public string BuildDetails;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 73)]
        public Byte[] Reserved;

        // Driver version
        public UInt16 VerBugFix;
        public UInt16 VerMajor;
        public UInt16 VerMinor;
        public UInt16 VerUpdate;

        private Byte Reserved2;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string CameraSerialNum;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string CameraModelNum;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string CameraHadrwareRevNum;
    }

}
