using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace SpotCam.Interop
{
    using SpotBool = Int32;

    internal delegate void SpotCallback(SpotStatus status, UIntPtr extraInfo, UIntPtr userData);
    internal delegate void SpotDeviceNotifyCallback(DeviceEvent eventType, int extraInfo, IntPtr deviceUid, UIntPtr userData);

    public static class SpotCamService
    {
        public const int MaxDevices = 25;
        public const int IntervalShortAsPossible = -1;
        public const int InfiniteImages = Int32.MaxValue;

        [DllImport("SpotCamProxy.dll", ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        internal static extern SpotCamReturnCode SpotStartUp(IntPtr value);

        [DllImport("SpotCamProxy.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        internal static extern SpotCamReturnCode SpotShutDown();

        [DllImport("SpotCamProxy.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        internal static extern SpotCamReturnCode SpotInit();

        [DllImport("SpotCamProxy.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        internal static extern SpotCamReturnCode SpotExit();

        [DllImport("SpotCamProxy.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern void SpotGetVersionInfo2(
            out Diagnostics.SpotCamServiceDetails versionInfo);

        [DllImport("SpotCamProxy.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        internal static extern SpotCamReturnCode SpotGetCameraAttributes(out Diagnostics.CameraAttributes attributes);

        [DllImport("SpotCamProxy.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern SpotCamReturnCode SpotSetValue(
            CoreParameter param,
            IntPtr value);

        [DllImport("SpotCamProxy.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern SpotCamReturnCode SpotGetValue(
            CoreParameter param,
            IntPtr value);

        [DllImport("SpotCamProxy.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int SpotGetValueSize(CoreParameter param);

        [DllImport("SpotCamProxy.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern SpotCamReturnCode SpotGetImage(
            UInt16 reserved,
            UInt32 reserved1,
            UInt16 reserved2,
            IntPtr imageBuffer,
            IntPtr redPixelCnts,
            IntPtr greenPixelCnts,
            IntPtr bluePixelCnts);

        [DllImport("SpotCamProxy.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern SpotCamReturnCode SpotGetLiveImages(
            [MarshalAs(UnmanagedType.Bool)] bool computeExposure,
            SensorOpticalFilter filterColor,
            Rotation rotateDirection,
            [MarshalAs(UnmanagedType.Bool)] bool flipHoriz,
            [MarshalAs(UnmanagedType.Bool)] bool flipVert,
            IntPtr imageBuffer);

        [DllImport("SpotCamProxy.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern SpotCamReturnCode SpotGetSequentialImages(
            int numberOfImages,
            int intervalMSec,
            [MarshalAs(UnmanagedType.Bool)] bool autoExposeOnEach,
            [MarshalAs(UnmanagedType.Bool)] bool useTriggerOrSetTTLOuputOnEach,
            [MarshalAs(UnmanagedType.Bool)] bool deferProcessing,
            IntPtr imageBuffers);

        [DllImport("SpotCamProxy.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern SpotCamReturnCode SpotRetrieveSequentialImage(IntPtr imageBuffer);


        [DllImport("SpotCamProxy.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        internal static extern SpotCamReturnCode SpotFindDevices(IntPtr deviceListArray, ref int arrayLength);

        [DllImport("SpotCamProxy.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        internal static extern void SpotSetAbortFlag(IntPtr abortToken);

        [DllImport("SpotCamProxy.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int SpotSetTTLOutputState([MarshalAs(UnmanagedType.Bool)] bool setActive);

        [DllImport("SpotCamProxy.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        internal static extern void SpotSetCallback(
            [MarshalAs(UnmanagedType.FunctionPtr)] SpotCallback callback,
            UIntPtr userData);

        [DllImport("SpotCamProxy.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        internal static extern void SpotSetDeviceNotificationCallback(
            [MarshalAs(UnmanagedType.FunctionPtr)] SpotDeviceNotifyCallback callback,
            UIntPtr userData);

        //[DllImport("SpotCamProxy.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        //public static extern int SpotSetColorTransformation(
        //    int precision,
        //    float sourceColorTemperature,
        //    [MarshalAs(UnmanagedType.FunctionPtr)]SpotColorTransformCallback callback,
        //    [MarshalAs(UnmanagedType.Bool)] bool threadSafe,
        //    UIntPtr userData);

        [DllImport("SpotCamProxy.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int SpotDumpCameraMemory([MarshalAs(UnmanagedType.LPStr)] string fileName);

        [DllImport("SpotCamProxy.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        internal static extern int SpotGetCameraErrorCode();

        internal static Tuple<T, T> MarshalTuple<T>(IntPtr buffer) where T : struct
        {
            var first = (T)Marshal.PtrToStructure(buffer, typeof(T));
            var second = (T)Marshal.PtrToStructure(IntPtr.Add(buffer, Marshal.SizeOf(typeof(T))), typeof(T));
            return Tuple.Create(first, second);
        }

        internal static T[] MarshalArray<T>(IntPtr buffer, int arrayLength)
        {
            var newArray = new T[arrayLength];
            for (int ix = 0; ix < arrayLength; ++ix)
            {
                newArray[ix] = (T)Marshal.PtrToStructure(buffer, typeof(T));
                buffer = IntPtr.Add(buffer, Marshal.SizeOf(typeof(T)));
            }
            return newArray;
        }

        internal static T[] MarshalLengthPrefixArray<T>(IntPtr buffer) where T : struct, IConvertible
        {
            int arrayLength = Convert.ToInt32((T)(Marshal.PtrToStructure(buffer, typeof(T))));
            var newArray = new T[arrayLength];
            for (int ix = 0; ix < arrayLength; ++ix)
            {
                buffer = IntPtr.Add(buffer, Marshal.SizeOf(typeof(T)));
                newArray[ix] = (T)Marshal.PtrToStructure(buffer, typeof(T));
            }
            return newArray;
        }

        /// <summary>
        /// Constructs an  appropriate Exception class that represents the SpotCamReturn code.
        /// If the return code is not an error the function will return null.
        /// </summary>
        /// <param name="returnCode">The return code to look up the exception class for</param>
        /// <param name="treatWarningAsError">Enables warning codes to be considered exceptions</param>
        /// <returns>The new Exception or null if the code is not an exception</returns>
        internal static Exception MakeDefaultException(SpotCamReturnCode returnCode, bool treatWarningAsError = false)
        {
            if (SpotCamReturnCode.Success == returnCode) 
                return null; // Success is never an exception!
            Exception exception = null;
            switch (returnCode)
            {
                case SpotCamReturnCode.WarnServiceConfigurationUnknown:
                    string serviceConfigurationUnknownMessage = "The SpotCam service configuration contains settings that are unknown to this version of the SpotCam service.";
                    if (treatWarningAsError)
                        exception = new ServiceConfigurationException(serviceConfigurationUnknownMessage); 
                    else
                        System.Diagnostics.Debug.WriteLine(serviceConfigurationUnknownMessage);
                    break;
                case SpotCamReturnCode.WarnColorLibNotLoaded:
                    exception = new SpotCamServiceException(returnCode, "The SpotCam service is not installed properly.\nThe color library failed to load.");
                    break;
                case SpotCamReturnCode.WarnInvalidOutputIcc:
                    exception = new InvalidColorProfileException("The specified color profile is not valid for an output profile");
                    break;
                case SpotCamReturnCode.WarnInvalidInputIcc:
                    exception = new InvalidColorProfileException("The specified color profile is not valid for an input profile");
                    break;
                case SpotCamReturnCode.WarnUnsupportedCameraFeatures:
                    string unsupportedCameraFeaturesMessage = "The camera has features that are not supported fully by this version of the SpotCam service";
                    if (treatWarningAsError)
                        exception = new CameraNotSupportedException(unsupportedCameraFeaturesMessage);
                    else
                        System.Diagnostics.Debug.WriteLine(unsupportedCameraFeaturesMessage);
                    break;
                case SpotCamReturnCode.Abort:
                    exception = new OperationCanceledException();
                    break;
                case SpotCamReturnCode.ErrorOutOfMemory:
                    exception = new OutOfMemoryException();
                    break;
                case SpotCamReturnCode.ErrorExposureTooShort:
                    exception = new EnvironmentalException(EnvironmentalIssue.MoreLuminanceNeeded);
                    break;
                case SpotCamReturnCode.ErrorExposureTooLong:
                    exception = new EnvironmentalException(EnvironmentalIssue.LessLuminanceNeeded);
                    break;
                case SpotCamReturnCode.ErrorNoCameraResponse:
                    exception = new FatalCameraException(
                        "The camera has stopped responding.\n" +
                        "Check that the camera's data cable is still connected and is powered on.\n" +
                        "If that doesn't correct the problem try initializing and/or power cycling the camera.");
                    break;
                case SpotCamReturnCode.ErrorValueOutOfRange:
                    exception = new ArgumentOutOfRangeException("The value passed to the ");
                    break;
                case SpotCamReturnCode.ErrorInvalidParam:
                    exception = new ArgumentException("The SpotCam parameter is not valid");
                    break;
                case SpotCamReturnCode.ErrorDriverNotInitialized:
                    exception = new NotSupportedException("Unable to continue because the connection to the camera has not been established yet");
                    break;
                case SpotCamReturnCode.ErrorCameraError:
                    exception = new CameraException(
                        String.Format("Camera reported an error code of {0}", SpotCamService.SpotGetCameraErrorCode()));
                    break;
                case SpotCamReturnCode.ErrorReadingCameraInfo:
                    exception = new FatalCameraException(
                        "There was an error initializing the camera.\n" +
                        "Details:\n"+
                        "The camera configuration was invalid.\n" +
                        "Try power cycling the device and check that the device is properly connected.");
                    break;
                case SpotCamReturnCode.ErrorNotCapable:
                    exception = new NotSupportedException("The camera or the SpotCam service is does not support the functionality");
                    break;
                case SpotCamReturnCode.ErrorColorFilterNotIn:
                    exception = new EnvironmentalException(EnvironmentalIssue.SliderNotInColorFilterPosition);
                    break;
                case SpotCamReturnCode.ErrorColorFilterNotOut:
                    exception = new EnvironmentalException(EnvironmentalIssue.SliderNotInClearPosition);
                    break;
                case SpotCamReturnCode.ErrorCameraBusy:
                    exception = new CameraBusyException();
                    break;
                case SpotCamReturnCode.ErrorCameraNotSupported:
                    exception = new CameraNotSupportedException();
                    break;
                case SpotCamReturnCode.ErrorNoImageAvailable:
                    exception = new SpotCamServiceException(returnCode, "The image queue is empty");
                    break;
                case SpotCamReturnCode.ErrorFileOpen:
                    exception = new System.IO.FileLoadException();
                    break;
                case SpotCamReturnCode.ErrorFlatfieldIncompatible:
                    exception = new ArgumentException("The flatfield correction file is incompatible with the camera or the current camera settings");
                    break;
                case SpotCamReturnCode.ErrorNoDevicesFound:
                    exception = new FatalCameraException("The camera is no longer connected to the computer");
                    break;
                case SpotCamReturnCode.ErrorBrightnessChanged:
                    exception = new EnvironmentalException(
                        EnvironmentalIssue.UnstableLuminance, 
                        "The illumination intensity changed during exposure computation");
                    break;
                case SpotCamReturnCode.ErrorCameraAndCardIncompatible:
                    exception = new FatalCameraException("The interface card does not support the camera connected to it");
                    break;
                case SpotCamReturnCode.ErrorBiasFrameIncompatible:
                    exception = new ArgumentException("The bias correction file is incompatible with the camera or the current camera settings");
                    break;
                case SpotCamReturnCode.ErrorBackgroundImageIncompatible:
                    exception = new ArgumentException("The background correction file is incompatible with the camera or the current camera settings");
                    break;
                case SpotCamReturnCode.ErrorBackgroundTooBright:
                    exception = new EnvironmentalException(
                        EnvironmentalIssue.ZeroLuminanceRequired,
                        "All light must be blocked to acquire a background image");
                    break;
                case SpotCamReturnCode.ErrorInvalidFile:
                    exception = new ArgumentException("The supplied file is not a valid format");
                    break;
                case SpotCamReturnCode.ErrorImageTooBright:
                    exception = new EnvironmentalException(EnvironmentalIssue.LessLuminanceNeeded);
                    break;
                case SpotCamReturnCode.ErrorNoCameraPower:
                    exception = new FatalCameraException("The camera is not powered on");
                    break;
                case SpotCamReturnCode.ErrorInsuf1394IsocBandwidth:
                    exception = new FatalCameraException("There is insufficient isochronous bandwidth available on the IEEE-1394 bus");
                    break;
                case SpotCamReturnCode.ErrorInsuf1394IsocResources:
                    exception = new FatalCameraException("There is insufficient IEEE-1394 isochronous resources available");
                    break;
                case SpotCamReturnCode.ErrorNo1394IsocChannel:
                    exception = new FatalCameraException("There is no isochronous channel available on the IEEE-1394 bus");
                    break;
                case SpotCamReturnCode.ErrorUsbVersionLowerThan2:
                    exception = new FatalCameraException("The camera must be connected to a USB 2.0 (Hi-Speed) or compatible port");
                    break;
                case SpotCamReturnCode.ErrorStartupNotDone:
                    exception = new NotSupportedException("Unable to continue because the Camera Factory has not been successfully initialized");
                    break;
                case SpotCamReturnCode.ErrorSpotcamServiceNotFound:
                    exception = new SpotCamServiceException(returnCode, "Unable to locate any installation of the SpotCam service");
                    break;
                case SpotCamReturnCode.ErrorWrongSpotcamServiceVersion:
                    exception = new SpotCamServiceException(returnCode, "Unable to locate an installation of a compatible version of the SpotCam service");
                    break;
                case SpotCamReturnCode.ErrorOperationNotSupported:
                    exception = new NotSupportedException("The SpotCam service rejected the operation");
                    break;
                case SpotCamReturnCode.ErrorServiceConfigurationFileError:
                    // System configuration is faulty. Nothing can be done about this, the file format is undocumented.
                    // Keep as a warning only and just log the error.
                    System.Diagnostics.Debug.WriteLine("Error processing the global service-configuration data file");
                    break;
                case SpotCamReturnCode.ErrorServiceConfigurationSyntaxError:
                    exception = new ServiceConfigurationException("The service-configuration is not formed correctly");
                    break;
                case SpotCamReturnCode.ErrorServiceConfigurationInvalid:
                    exception = new ServiceConfigurationException("The service-configuration specifies invalid settings");
                    break;
                case SpotCamReturnCode.ErrorStartupAlreadyDone:
                case SpotCamReturnCode.ErrorDriverAlreadyInit:
                case SpotCamReturnCode.ErrorNothingToDo:
                    exception = null; // these are not exceptions
                    break;
                case SpotCamReturnCode.ErrorRegistryQuery:
                case SpotCamReturnCode.ErrorRegistrySet:
                case SpotCamReturnCode.ErrorDeviveDriverLoad:
                case SpotCamReturnCode.ErrorMisc:
                case SpotCamReturnCode.ErrorDmaSetup:
                default:
                    if( returnCode > 0 || (treatWarningAsError && returnCode < 0) )
                        exception = new SpotCamServiceException(returnCode);
                    else
                       System.Diagnostics.Debug.WriteLine("The SpotCam service reported an unknown warning of {0}", returnCode);
                    break;
            }
            return exception;
        }
        
        public static void CheckSuccessIgnoreWarning(this SpotCamReturnCode code)
        {
            Exception ex = SpotCamService.MakeDefaultException(code, false);
            if (null != ex)
                throw ex;
        }

        public static void CheckSuccess(this SpotCamReturnCode code)
        {
            Exception ex = SpotCamService.MakeDefaultException(code, true);
            if (null != ex)
                throw ex;
        }


        static internal bool AutoExposure
        {
            get
            {
                SpotBool val;
                unsafe {
                CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.AutoExpose, new IntPtr(&val)));
                }
                return val != 0;
            }
            set
            {
                SpotBool val = value ? 1 : 0;
                unsafe
                {
                    CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.AutoExpose, new IntPtr(&val)));
                }
            }
        }

        static public bool ReturnRawMosaicData
        {
            get
            {
                SpotBool val;
                unsafe
                {
                    CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.ReturnRawMosaicData, new IntPtr(&val)));
                }
                return val != 0;
            }
            set
            {
                SpotBool val = value ? 1 : 0;
                unsafe
                {
                    CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.ReturnRawMosaicData, new IntPtr(&val)));
                }
            }
        }

        static internal float BrightnessAdjustment
        {
            get
            {
                float val;
                unsafe
                {
                    CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.BrightnessAdjustment, new IntPtr(&val)));
                }
                return val;
            }
            set
            {
                unsafe
                {
                    CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.BrightnessAdjustment, new IntPtr(&value)));
                }
            }
        }

        static internal short AutoGainLimit
        {
            get
            {
                short val;
                unsafe
                {
                    CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.AutoGainLimit, new IntPtr(&val)));
                }
                return val;
            }
            set
            {
                unsafe
                {
                    CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.AutoGainLimit, new IntPtr(&value)));
                }
            }
        }

        static public short BinSize
        {
            get
            {
                short val;
                unsafe
                {
                    CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.BinSize, new IntPtr(&val)));
                }
                return val;
            }
            set
            {
                unsafe
                {
                    CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.BinSize, new IntPtr(&value)));
                }
            }
        }

        static public short BitDepth
        {
            get
            {
                short val;
                unsafe
                {
                    CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.BitDepth, new IntPtr(&val)));
                }
                return val;
            }
            set
            {
                unsafe
                {
                    CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.BitDepth, new IntPtr(&value)));
                }
            }
        }

        static internal ulong DeviceUid
        {
            get
            {
                ulong val;
                unsafe
                {
                    CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.DeviceUid, new IntPtr(&val)));
                }
                return val;
            }
            set
            {
                unsafe
                {
                    CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.DeviceUid, new IntPtr(&value)));
                }
            }
        }

        static internal short DriverDeviceNumber
        {
            get
            {
                short val;
                unsafe
                {
                    CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.DriverDeviceNumber, new IntPtr(&val)));
                }
                return val;
            }
            set
            {
                unsafe
                {
                    CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.DriverDeviceNumber, new IntPtr(&value)));
                }
            }
        }

        static public SPOT_SIZE AcquiredImageSize
        {
            get
            {
                SPOT_SIZE val;
                unsafe
                {
                    CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.AcquiredImageSize, new IntPtr(&val)));
                }
                return val;
            }
            set
            {
                unsafe
                {
                    CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.AcquiredImageSize, new IntPtr(&value)));
                }
            }
        }

        static public SPOT_SIZE AcquiredLiveImageSize
        {
            get
            {
                SPOT_SIZE val;
                unsafe
                {
                    CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.AcquiredLiveImageSize, new IntPtr(&val)));
                }
                return val;
            }
            set
            {
                unsafe
                {
                    CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.AcquiredLiveImageSize, new IntPtr(&value)));
                }
            }
        }

        static public short[] BitDepths
        {
            get
            {
                var buffer = Marshal.AllocHGlobal(SpotCamService.SpotGetValueSize(CoreParameter.BitDepths));
                CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.BitDepths, buffer));
                var availableBitDepths = SpotCamService.MarshalLengthPrefixArray<short>(buffer);
                Marshal.FreeHGlobal(buffer);
                return availableBitDepths;
            }
        }

        static public short[] BinningSizes
        {
            get
            {
                var buffer = Marshal.AllocHGlobal(SpotCamService.SpotGetValueSize(CoreParameter.BinSizes));
                CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.BinSizes, buffer));
                var availableBitDepths = SpotCamService.MarshalLengthPrefixArray<short>(buffer);
                Marshal.FreeHGlobal(buffer);
                return availableBitDepths;
            }
        }

//////////////////////////////////////////////////////////////

///<summary>
/// 
///</summary>
internal static SPOT_RECT ImageRect
{
  get
  {
    SPOT_RECT val;
    unsafe
    {
      CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.ImageRect, new IntPtr(&val)));
    }
    return val;
  }
  set
  {
    unsafe
    {
      CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.ImageRect, new IntPtr(&value)));
    }
  }
}

///<summary>
/// 
///</summary>
internal static SPOT_COLOR_ENABLE_STRUCT2 ColorEnable
{
  get
  {
    SPOT_COLOR_ENABLE_STRUCT2 val;
    unsafe
    {
      CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.ColorEnable, new IntPtr(&val)));
    }
    return val;
  }
  set
  {
    unsafe
    {
      CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.ColorEnable, new IntPtr(&value)));
    }
  }
}

//internal static X ColorOrder
//{
//    get
//    {
//        Int32 val;
//        unsafe
//        {
//            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.ColorOrder, new IntPtr(&val)));
//        }
//        return Convert.ToX(val);
//    }
//    set
//    {
//        Int32 undefined = Convert.ToInt32(value)
//        unsafe
//        {
//            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.ColorOrder, new IntPtr(&undefined)));
//        }
//    }
//}

///<summary>
/// 
///</summary>
internal static SPOT_WHITE_BAL_STRUCT WhiteBalance
{
    get
    {
        SPOT_WHITE_BAL_STRUCT val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.WhiteBalance, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.WhiteBalance, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static PhotographicLighting PhotographicLighting
{
    get
    {
        PhotographicLighting val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.PhotographicLighting, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.PhotographicLighting, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static bool CorrectChipDefects
{
    get
    {
        SpotBool val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.CorrectChipDefects, new IntPtr(&val)));
        }
        return val != 0;
    }
    set
    {
        SpotBool val = value ? 1 : 0;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.CorrectChipDefects, new IntPtr(&val)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static bool MessageEnable
{
    get
    {
        SpotBool val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.MessageEnable, new IntPtr(&val)));
        }
        return val != 0;
    }
    set
    {
        SpotBool val = value ? 1 : 0;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.MessageEnable, new IntPtr(&val)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static float LiveBrightnessAdjustment
{
    get
    {
        float val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.LiveBrightnessAdj, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.LiveBrightnessAdj, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static Tuple<float, float> BrightnessAdjustmentLimits
{
    get
    {
        var ptr = Marshal.AllocHGlobal(SpotGetValueSize(CoreParameter.BrightnessAdjustmentLimits));
        try
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.BrightnessAdjustmentLimits, ptr));
            return MarshalTuple<float>(ptr);
        }
        finally
        {
            Marshal.FreeHGlobal(ptr);
        }
    }
}

///<summary>
/// 
///</summary>
internal static SPOT_RECT MaxImageRectSize
{
    get
    {
        SPOT_RECT val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.MaxImageRectSize, new IntPtr(&val)));
        }
        return val;
    }
}

///<summary>
/// 
///</summary>
internal static short[] Port0GainValues8
{
    get
    {
        var ptr = Marshal.AllocHGlobal(SpotGetValueSize(CoreParameter.Port0GainValues8));
        try
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.Port0GainValues8, ptr));
            return MarshalLengthPrefixArray<short>(ptr);
        }
        finally
        {
            Marshal.FreeHGlobal(ptr);
        }
    }
}

///<summary>
/// 
///</summary>
internal static short[] Port0LiveGainValues
{
    get
    {
        var ptr = Marshal.AllocHGlobal(SpotGetValueSize(CoreParameter.Port0LiveGainValues));
        try
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.Port0LiveGainValues, ptr));
            return MarshalLengthPrefixArray<short>(ptr);
        }
        finally
        {
            Marshal.FreeHGlobal(ptr);
        }
    }
}

///<summary>
/// 
///</summary>
internal static short[] Port0GainValues16
{
    get
    {
        var ptr = Marshal.AllocHGlobal(SpotGetValueSize(CoreParameter.Port0GainValues16));
        try
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.Port0GainValues16, ptr));
            return MarshalLengthPrefixArray<short>(ptr);
        }
        finally
        {
            Marshal.FreeHGlobal(ptr);
        }
    }
}

///<summary>
/// 
///</summary>
internal static short MaxLiveAccelerationLevel
{
    get
    {
        short val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.MaxLiveAccelerationLevel, new IntPtr(&val)));
        }
        return val;
    }
}

///<summary>
/// 
///</summary>
internal static float MaxWhiteBalanceRatio
{
    get
    {
        float val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.MaxWhiteBalanceRatio, new IntPtr(&val)));
        }
        return val;
    }
}


///<summary>
/// 
///</summary>
internal static float ExposureConversionFactor
{
    get
    {
        float val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.ExposureConversionFactor, new IntPtr(&val)));
        }
        return val;
    }
}

///<summary>
/// 
///</summary>
internal static SensorMosaicPattern MosaicPattern
{
    get
    {
        SensorMosaicPattern val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.MosaicPattern, new IntPtr(&val)));
        }
        return val;
    }
}

///<summary>
/// 
///</summary>
internal static Tuple<TimeSpan, TimeSpan> ExternalTriggerDelayLimits
{
    get
    {
        var ptr = Marshal.AllocHGlobal(SpotGetValueSize(CoreParameter.ExternalTriggerDelayLimits));
        try
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.ExternalTriggerDelayLimits, ptr));
            var rawTimes = MarshalTuple<Int32>(ptr);
            return Tuple.Create(TimeSpan.FromMilliseconds(rawTimes.Item1 / 1000.0),
                TimeSpan.FromMilliseconds(rawTimes.Item2 / 1000.0));
        }
        finally
        {
            Marshal.FreeHGlobal(ptr);
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_138 RegulatedTemperatureLimits
{
    get
    {
        X_138 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.RegulatedTemperatureLimits, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.RegulatedTemperatureLimits, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_139 HorizontalReadoutFrequencies
{
    get
    {
        X_139 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.HorizontalReadoutFrequencies, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.HorizontalReadoutFrequencies, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_140 TtlOutputDelayLimits
{
    get
    {
        X_140 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.TtlOutputDelayLimits, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.TtlOutputDelayLimits, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_141 ExposureResolution
{
    get
    {
        X_141 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.ExposureResolution, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.ExposureResolution, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_144 MaxGainPortNumber
{
    get
    {
        X_144 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.MaxGainPortNumber, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.MaxGainPortNumber, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_145 Port1GainValueLimits
{
    get
    {
        X_145 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.Port1GainValueLimits, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.Port1GainValueLimits, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_146 Port1LiveGainValueLimits
{
    get
    {
        X_146 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.Port1LiveGainValueLimits, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.Port1LiveGainValueLimits, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_147 Port2GainValueLimits
{
    get
    {
        X_147 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.Port2GainValueLimits, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.Port2GainValueLimits, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_148 Port2LiveGainValueLimits
{
    get
    {
        X_148 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.Port2LiveGainValueLimits, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.Port2LiveGainValueLimits, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_149 Port3GainValueLimits
{
    get
    {
        X_149 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.Port3GainValueLimits, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.Port3GainValueLimits, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_150 Port3LiveGainValueLimits
{
    get
    {
        X_150 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.Port3LiveGainValueLimits, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.Port3LiveGainValueLimits, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_151 BinSizes
{
    get
    {
        X_151 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.BinSizes, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.BinSizes, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_152 MaxPixelResolutionLevel
{
    get
    {
        X_152 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.MaxPixelResolutionLevel, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.MaxPixelResolutionLevel, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_155 MinImageSize
{
    get
    {
        X_155 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.MinImageSize, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.MinImageSize, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_156 ColorBinningSizes
{
    get
    {
        X_156 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.ColorBinningSizes, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.ColorBinningSizes, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_200 LiveAutoGainLimit
{
    get
    {
        X_200 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.LiveAutoGainLimit, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.LiveAutoGainLimit, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_201 SubtractBlackLevel
{
    get
    {
        X_201 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.SubtractBlackLevel, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.SubtractBlackLevel, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_202 MonitorFilterPosition
{
    get
    {
        X_202 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.MonitorFilterPosition, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.MonitorFilterPosition, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_203 ColorEnable2
{
    get
    {
        X_203 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.ColorEnable2, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.ColorEnable2, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_205 EnableTtlOutput
{
    get
    {
        X_205 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.EnableTtlOutput, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.EnableTtlOutput, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_206 TtlOutputDelayMs
{
    get
    {
        X_206 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.TtlOutputDelayMs, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.TtlOutputDelayMs, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_207 LiveGammaAdjustment
{
    get
    {
        X_207 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.LiveGammaAdjustment, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.LiveGammaAdjustment, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static TimeSpan LiveExposure64
{
    get
    {
        Int64 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.LiveExposure64, new IntPtr(&val)));
        }
        return TimeSpan.FromMilliseconds(val / 1000000.0);
    }
    set
    {
        Int64 val = Convert.ToInt64(value.TotalMilliseconds * 1000000.0);
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.LiveExposure64, new IntPtr(&val)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static TimeSpan MinimumExposureMsec
{
    get
    {
        short val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.MinimumExposureMsec, new IntPtr(&val)));
        }
        return TimeSpan.FromMilliseconds(val);
    }
    set
    {
        short val = Convert.ToInt16(value.TotalMilliseconds);
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.LiveExposure64, new IntPtr(&val)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static bool ReturnRawMosaicData
{
    get
    {
        SpotBool val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.ReturnRawMosaicData, new IntPtr(&val)));
        }
        return val != 0;
    }
    set
    {
        SpotBool val = value ? 1 : 0;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.ReturnRawMosaicData, new IntPtr(&val)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static short LiveAccelerationLevel
{
    get
    {
        short val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.LiveAccelerationLevel, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.LiveAccelerationLevel, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static bool EnhanceColors
{
    get
    {
        SpotBool val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.EnhanceColors, new IntPtr(&val)));
        }
        return val != 0;
    }
    set
    {
        SpotBool val = value ? 1 : 0;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.EnhanceColors, new IntPtr(&val)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static string FlatfieldCorrect
{
    get
    {
        X_215 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.FlatfieldCorrect, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        var ptr = String.IsNullOrEmpty(value) ? IntPtr.Zero : Marshal.StringToHGlobalAnsi(value);
        try
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.FlatfieldCorrect, ptr));
        }
        finally
        {
            if (ptr != IntPtr.Zero)
                Marshal.FreeHGlobal(ptr);
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_216 NoiseFilterThresholdPercent
{
    get
    {
        X_216 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.NoiseFilterThresholdPercent, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.NoiseFilterThresholdPercent, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_217 LiveAutoBrightness
{
    get
    {
        X_217 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.LiveAutoBrightness, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.LiveAutoBrightness, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_218 LiveAutoBrightnessAdjustment
{
    get
    {
        X_218 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.LiveAutoBrightnessAdjustment, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.LiveAutoBrightnessAdjustment, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_220 LiveMaxExposureMsec
{
    get
    {
        X_220 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.LiveMaxExposureMsec, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.LiveMaxExposureMsec, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_221 ExposureIncrement
{
    get
    {
        X_221 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.ExposureIncrement, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.ExposureIncrement, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_222 ExternalTriggerMode
{
    get
    {
        X_222 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.ExternalTriggerMode, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.ExternalTriggerMode, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_223 MaxExposureMsec
{
    get
    {
        X_223 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.MaxExposureMsec, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.MaxExposureMsec, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_224 WhiteBalanceComputationRect
{
    get
    {
        X_224 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.WhiteBalanceComputationRect, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.WhiteBalanceComputationRect, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_225 BiasFrameSubtract
{
    get
    {
        X_225 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.BiasFrameSubtract, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.BiasFrameSubtract, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_227 BusBandwidth
{
    get
    {
        X_227 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.BusBandwidth, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.BusBandwidth, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_228 ExposureComputationRect
{
    get
    {
        X_228 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.ExposureComputationRect, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.ExposureComputationRect, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_229 ExternalTriggerActiveState
{
    get
    {
        X_229 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.ExternalTriggerActiveState, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.ExternalTriggerActiveState, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_230 ExternalTriggerDelay
{
    get
    {
        X_230 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.ExternalTriggerDelay, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.ExternalTriggerDelay, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_231 RegulateTemperature
{
    get
    {
        X_231 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.RegulateTemperature, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.RegulateTemperature, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_232 RegulatedTemperature
{
    get
    {
        X_232 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.RegulatedTemperature, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.RegulatedTemperature, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_233 HorizontalReadoutFrequency
{
    get
    {
        X_233 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.HorizontalReadoutFrequency, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.HorizontalReadoutFrequency, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_234 TtlOutputActiveState
{
    get
    {
        X_234 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.TtlOutputActiveState, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.TtlOutputActiveState, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_235 TtlOutputDelay
{
    get
    {
        X_235 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.TtlOutputDelay, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.TtlOutputDelay, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_236 BackgroundImageSubtract
{
    get
    {
        X_236 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.BackgroundImageSubtract, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.BackgroundImageSubtract, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_237 GainPortNumber
{
    get
    {
        X_237 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.GainPortNumber, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.GainPortNumber, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_238 ColorRenderingIntent
{
    get
    {
        X_238 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.ColorRenderingIntent, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.ColorRenderingIntent, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_239 LiveEnhanceColors
{
    get
    {
        X_239 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.LiveEnhanceColors, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.LiveEnhanceColors, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_240 PixelResolutionLevel
{
    get
    {
        X_240 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.PixelResolutionLevel, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.PixelResolutionLevel, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_241 InputColorProfile
{
    get
    {
        X_241 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.InputColorProfile, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.InputColorProfile, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_242 OutputColorProfile
{
    get
    {
        X_242 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.OutputColorProfile, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.OutputColorProfile, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_243 ColorBinSize
{
    get
    {
        X_243 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.ColorBinSize, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.ColorBinSize, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_244 SensorClearMode
{
    get
    {
        X_244 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.SensorClearMode, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.SensorClearMode, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_245 CoolingLevel
{
    get
    {
        X_245 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.CoolingLevel, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.CoolingLevel, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_246 FanSpeed
{
    get
    {
        X_246 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.FanSpeed, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.FanSpeed, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_247 FanExposureSpeed
{
    get
    {
        X_247 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.FanExposureSpeed, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.FanExposureSpeed, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_248 FanExposureDelayMs
{
    get
    {
        X_248 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.FanExposureDelayMs, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.FanExposureDelayMs, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_249 PreAmpGainValue
{
    get
    {
        X_249 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.PreAmpGainValue, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.PreAmpGainValue, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_250 VerticalShiftPeriod
{
    get
    {
        X_250 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.VerticalShiftPeriod, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.VerticalShiftPeriod, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_251 VerticalClockVoltageBoost
{
    get
    {
        X_251 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.VerticalClockVoltageBoost, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.VerticalClockVoltageBoost, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_252 ReadoutCircuit
{
    get
    {
        X_252 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.ReadoutCircuit, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.ReadoutCircuit, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_253 NumberSkipLines
{
    get
    {
        X_253 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.NumberSkipLines, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.NumberSkipLines, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_254 ShutterMode
{
    get
    {
        X_254 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.ShutterMode, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.ShutterMode, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_260 EnablePowerStateControl
{
    get
    {
        X_260 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.EnablePowerStateControl, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.EnablePowerStateControl, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_261 ForceSingleChannelLiveMode
{
    get
    {
        X_261 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.ForceSingleChannelLiveMode, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.ForceSingleChannelLiveMode, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_262 LiveImageScaling
{
    get
    {
        X_262 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.LiveImageScaling, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.LiveImageScaling, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_263 LiveHistogram
{
    get
    {
        X_263 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.LiveHistogram, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.LiveHistogram, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_264 SensorResponseMode
{
    get
    {
        X_264 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.SensorResponseMode, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.SensorResponseMode, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_265 SequentialImageDiskCachePath
{
    get
    {
        X_265 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.SequentialImageDiskCachePath, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.SequentialImageDiskCachePath, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_267 LiveSubtractBlackLevel
{
    get
    {
        X_267 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.LiveSubtractBlackLevel, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.LiveSubtractBlackLevel, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_268 LivePixelResolutionLevel
{
    get
    {
        X_268 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.LivePixelResolutionLevel, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.LivePixelResolutionLevel, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_269 CoolerModeOnExit
{
    get
    {
        X_269 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.CoolerModeOnExit, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.CoolerModeOnExit, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_271 Readout8Bit
{
    get
    {
        X_271 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.Readout8Bit, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.Readout8Bit, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_272 ReadoutChannelMode
{
    get
    {
        X_272 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.ReadoutChannelMode, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.ReadoutChannelMode, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_273 Exposure64
{
    get
    {
        X_273 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.Exposure64, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.Exposure64, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_274 SequentialImageExposureDurations64
{
    get
    {
        X_274 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.SequentialImageExposureDurations64, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.SequentialImageExposureDurations64, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_280 AutoGainMinimum
{
    get
    {
        X_280 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.AutoGainMinimum, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.AutoGainMinimum, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_281 LiveAutoGainMinimum
{
    get
    {
        X_281 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.LiveAutoGainMinimum, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.LiveAutoGainMinimum, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_290 LabPixelAdjustments
{
    get
    {
        X_290 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.LabPixelAdjustments, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.LabPixelAdjustments, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_401 PixelSizeNm
{
    get
    {
        X_401 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.PixelSizeNm, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.PixelSizeNm, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_403 CoolingLevels
{
    get
    {
        X_403 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.CoolingLevels, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.CoolingLevels, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_404 FanSpeeds
{
    get
    {
        X_404 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.FanSpeeds, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.FanSpeeds, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_405 NumberReadoutCircuits
{
    get
    {
        X_405 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.NumberReadoutCircuits, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.NumberReadoutCircuits, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_406 PreAmpGainValues
{
    get
    {
        X_406 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.PreAmpGainValues, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.PreAmpGainValues, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_407 VerticalShiftPeriods
{
    get
    {
        X_407 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.VerticalShiftPeriods, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.VerticalShiftPeriods, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_408 MaxVerticalClockVoltageBoost
{
    get
    {
        X_408 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.MaxVerticalClockVoltageBoost, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.MaxVerticalClockVoltageBoost, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_411 Port0GainAttributes
{
    get
    {
        X_411 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.Port0GainAttributes, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.Port0GainAttributes, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_412 Port1GainAttributes
{
    get
    {
        X_412 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.Port1GainAttributes, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.Port1GainAttributes, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_413 Port2GainAttributes
{
    get
    {
        X_413 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.Port2GainAttributes, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.Port2GainAttributes, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_414 Port3GainAttributes
{
    get
    {
        X_414 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.Port3GainAttributes, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.Port3GainAttributes, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_415 ReadoutCircuitDescription
{
    get
    {
        X_415 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.ReadoutCircuitDescription, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.ReadoutCircuitDescription, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_416 ImageSensorModelDescription
{
    get
    {
        X_416 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.ImageSensorModelDescription, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.ImageSensorModelDescription, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_417 ImageSensorType
{
    get
    {
        X_417 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.ImageSensorType, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.ImageSensorType, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_419 MinFastSequentialImageExposureDuration64
{
    get
    {
        X_419 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.MinFastSequentialImageExposureDuration64, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.MinFastSequentialImageExposureDuration64, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_421 MaxNumberSkipLines
{
    get
    {
        X_421 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.MaxNumberSkipLines, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.MaxNumberSkipLines, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_422 SensorResponseModes
{
    get
    {
        X_422 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.SensorResponseModes, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.SensorResponseModes, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_423 SensorClearModes
{
    get
    {
        X_423 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.SensorClearModes, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.SensorClearModes, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_424 MinPixelResolutionLevel
{
    get
    {
        X_424 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.MinPixelResolutionLevel, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.MinPixelResolutionLevel, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_425 MaxNumberSequentialImageExposures
{
    get
    {
        X_425 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.MaxNumberSequentialImageExposures, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.MaxNumberSequentialImageExposures, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_426 PixelResolutionImageSizeFactors
{
    get
    {
        X_426 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.PixelResolutionImageSizeFactors, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.PixelResolutionImageSizeFactors, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_427 ReadoutChannelModes
{
    get
    {
        X_427 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.ReadoutChannelModes, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.ReadoutChannelModes, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_428 ExposureLimits64
{
    get
    {
        X_428 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.ExposureLimits64, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.ExposureLimits64, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_501 WaitForStatusChanges
{
    get
    {
        X_501 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.WaitForStatusChanges, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.WaitForStatusChanges, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_701 ImageOrientation
{
    get
    {
        X_701 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.ImageOrientation, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.ImageOrientation, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_702 ImageBufferFormat24Bpp
{
    get
    {
        X_702 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.ImageBufferFormat24Bpp, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.ImageBufferFormat24Bpp, new IntPtr(&value)));
        }
    }
}

///<summary>
/// 
///</summary>
internal static X_703 NumberBytesPerImageRow
{
    get
    {
        X_703 val;
        unsafe
        {
            CheckSuccess(SpotCamService.SpotGetValue(CoreParameter.NumberBytesPerImageRow, new IntPtr(&val)));
        }
        return val;
    }
    set
    {
        unsafe
        {
            CheckSuccess(SpotCamService.SpotSetValue(CoreParameter.NumberBytesPerImageRow, new IntPtr(&value)));
        }
    }
}

        
///////////////////////////////////////////////////////////////
    
    }
}
