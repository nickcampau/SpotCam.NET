using System;
using System.Runtime.InteropServices;

namespace SpotCam.Interop
{
    /// <summary>
    /// Exposure durations are always expressed in nanoseconds
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct SPOT_EXPOSURE_STRUCT2
    {
        public UInt64 RedExposureDuration;
        public UInt64 GreenExposureDuration;
        public UInt64 BlueExposureDuration;
        public UInt64 ExposureDuration;
        public Int16 Gain;
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
        public Int16 Reserved; // Must be set to zero
        [MarshalAs(UnmanagedType.R4)]
        public float RedVal;
        [MarshalAs(UnmanagedType.R4)]
        public float GreenVal;
        [MarshalAs(UnmanagedType.R4)]
        public float BlueVal;
    }

    public enum SpotMessageType : int
    {
        Information = 1,
        Warning = 2,
        Error = 3
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class SPOT_MESSAGE_STRUCT
    {
        /// <summary>
        ///  The message classification
        /// </summary>
        [MarshalAs(UnmanagedType.U4)]
        public SpotMessageType MessageType;

        private Int32 Reserved;
        /// <summary>
        /// The contents of the message
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string Message;

        public Int32 Value;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
        private byte[] Reserved2;
    }


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SPOT_DEVICE_STRUCT
    {
        public Diagnostics.DeviceClass DeviceClass;

        public Diagnostics.DeviceIoBus DeviceIoBus;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 28)]
        private byte[] Reserved;

        public Diagnostics.CameraAttributes Attributes;
        
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 54)]
        public string Description;

        /// <summary>
        /// Unique for every camera on a single computer system.
        /// Not globally unique.
        /// </summary>
        public UInt64 DeviceUID;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SPOT_TIMESTAMP_STRUCT
    {
        public UInt16 Year;   // Calendar year (eg. 2004)
        public UInt16 Month;  // Zero-based month (0=Jan, 11=Dec)
        public UInt16 Day;    // Day of month (1-31)
        public UInt16 Hour;
        public UInt16 Minute;
        public UInt16 Second;
        public UInt32 Microsecond;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SPOT_LIVE_HISTOGRAM_STRUCT
    {
        [MarshalAs(UnmanagedType.LPArray, SizeConst=0xFFFF)]
        public Int32[] RedPixelCounts;
        [MarshalAs(UnmanagedType.LPArray, SizeConst = 0xFFFF)]
        public Int32[] GreenPixelCounts;
        [MarshalAs(UnmanagedType.LPArray, SizeConst = 0xFFFF)]
        public Int32[] BluePixelCounts;

        /// <summary>
        /// Area used for filling histogram
        /// </summary>
        public SPOT_RECT SamplingArea;
        
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=96)]
        byte[] Reserved;
    }


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SPOT_LIVE_IMAGE_SCALING_STRUCT
    {
        [MarshalAs(UnmanagedType.Bool)]
        public bool  AutoScale;

        [MarshalAs(UnmanagedType.R4)]
        public float BlackOverflowPercent;

        [MarshalAs(UnmanagedType.R4)]
        public float WhiteOverflowPercent;  // Percent of pixels at black and at white

        public SPOT_RECT SamplingArea;  // Area used for filling histogram when auto-scaling
        public Int32   BlackPoint;      // May be negative
        public Int32   WhitePoint;
        public Int32   Scale;           // Scale must be <= nWhitePoint
        
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=96)]
        byte[]  Reserved;
    }


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SPOT_LAB_COLOR
    {
        [MarshalAs(UnmanagedType.R8)]
        double L;
        [MarshalAs(UnmanagedType.R8)]
        double a;
        [MarshalAs(UnmanagedType.R8)]
        double b;
    }


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SPOT_LAB_PIXEL_ADJUSTMENTS_STRUCT
    {
        /// <summary>
        /// Size of the structure in bytes
        /// </summary>
        [MarshalAs(UnmanagedType.SysInt)]
        long Size;

        /// <summary>
        /// Luminance gamma adjustment.
        /// Range {> 0.0} A value of 1.0 equals no effect
        /// </summary>
        [MarshalAs(UnmanagedType.R4)]
        float  LuminanceGamma;

        /// <summary>
        /// Luminance contrast adjustment. 
        /// Range { >= 0.0} A value of 0.0 equals no effect
        /// </summary>
        [MarshalAs(UnmanagedType.R4)]
        float  LuminanceContrast;

        /// <summary>
        /// Source illuminate color temperature in degrees Kelvin.
        /// Range {2400-11000} A value of 0 will use a default temperature
        /// </summary>
        [MarshalAs(UnmanagedType.R4)]
        float  SourceColorTemperature;

        /// <summary>
        /// Color saturation adjustment.
        /// Range { >= 0.0} A value of 1.0 equals no effect 
        /// </summary>
        [MarshalAs(UnmanagedType.R4)]
        float  Saturation;
    }


}