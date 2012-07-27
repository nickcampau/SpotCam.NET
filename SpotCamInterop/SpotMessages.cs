using System;
using System.Runtime.InteropServices;

namespace SpotCam.Interop
{
    using size_t = UIntPtr;

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct SPOT_VERSION_STRUCT2
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
        private Byte[] Reserved;

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

    /// <summary>
    /// Exposure durations are always expressed in ns
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct SPOT_EXPOSURE_STRUCT64
    {
        public ulong RedExpDur;
        public ulong GreenExpDur;
        public ulong BlueExpDur;
        public ulong ExpDur;
        public short Gain;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SPOT_RECT
    {
        public Int32 Left;
        public Int32 Top;
        public Int32 Right;
        public Int32 Bottom;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SPOT_SIZE
    {
        public UInt16 Width;
        public UInt16 Height;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SPOT_COLOR_ENABLE_STRUCT2
    {
        [MarshalAs(UnmanagedType.Bool)]
        public bool EnableRed;

        [MarshalAs(UnmanagedType.Bool)]
        public bool EnableGreen;

        [MarshalAs(UnmanagedType.Bool)]
        public bool EnableBlue;

        [MarshalAs(UnmanagedType.Bool)]
        public bool EnableClear;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SPOT_WHITE_BAL_STRUCT
    {
        public short Reserved; // Must be set to zero
        public float RedVal;
        public float GreenVal;
        public float BlueVal;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class SPOT_MESSAGE_STRUCT
    {
        /// <summary>
        ///  One of the values defined for SPOT_MESSAGETYPE_XXX
        /// </summary>
        public int MessageType;
        private int Reserved;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string Message;

        public int Value;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
        private byte[] Reserved2;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SPOT_LAB_PIXEL_ADJUSTMENTS_STRUCT
    {
       size_t Size;                     // size of the structure in bytes
       float  LuminanceGamma;           // Luminance gamma adjustment.Range {> 0.0} A value of 1.0 equals no effect
       float  LuminanceContrast;        // Luminance contrast adjustment. Range { >= 0.0} A value of 0.0 equals no effect
       float  SourceColorTemperature;   // Source illuminate color temperature in degrees Kelvin. Range {2400-11000} A value of 0 will use a default temperature
       float  Saturation;               // Color saturation adjustment. Range { >= 0.0} A value of 1.0 equals no effect 
    }


}