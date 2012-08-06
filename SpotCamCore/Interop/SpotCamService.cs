using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace SpotCam.Interop
{

    internal delegate void SpotCallback(SpotStatus status, UIntPtr extraInfo, UIntPtr userData);
    internal delegate void SpotDeviceNotifyCallback(DeviceEvent eventType, int extraInfo, IntPtr deviceUid, UIntPtr userData);

    public static class SpotCamService
    {
        public const int MaxDevices = 25;
        public const int IntervalShortAsPossible = -1;
        public const int InfiniteImages = Int32.MaxValue;

        [DllImport("SpotCamProxy.dll", ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern SpotCamReturnCode SpotStartUp(IntPtr value);

        [DllImport("SpotCamProxy.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern SpotCamReturnCode SpotShutDown();

        [DllImport("SpotCamProxy.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern SpotCamReturnCode SpotInit();

        [DllImport("SpotCamProxy.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern SpotCamReturnCode SpotExit();

        [DllImport("SpotCamProxy.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern void SpotGetVersionInfo2(
            out Diagnostics.SpotVersionDetails versionInfo);

        [DllImport("SpotCamProxy.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        internal static extern SpotCamReturnCode SpotGetCameraAttributes(out Diagnostics.CameraAttributes attributes);

        [DllImport("SpotCamProxy.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern SpotCamReturnCode SpotSetValue(
            CoreParameter param,
            IntPtr value);

        [DllImport("SpotCamProxy.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern SpotCamReturnCode SpotGetValue(
            CoreParameter param,
            IntPtr value);

        [DllImport("SpotCamProxy.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int SpotGetValueSize(CoreParameter param);

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
            short filterColor,
            short rotateDirection,
            [MarshalAs(UnmanagedType.Bool)] bool flipHoriz,
            [MarshalAs(UnmanagedType.Bool)] bool flipVert,
            IntPtr imageBuffer);

        [DllImport("SpotCamProxy.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern SpotCamReturnCode SpotGetSequentialImages(
            int numImages,
            int intervalMSec,
            [MarshalAs(UnmanagedType.Bool)] bool autoExposeOnEach,
            [MarshalAs(UnmanagedType.Bool)] bool useTriggerOrSetTTLOuputOnEach,
            [MarshalAs(UnmanagedType.Bool)] bool deferProcessing,
            IntPtr imageBuffers);

        [DllImport("SpotCamProxy.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern SpotCamReturnCode SpotRetrieveSequentialImage(IntPtr imageBuffer);


        [DllImport("SpotCamProxy.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern SpotCamReturnCode SpotFindDevices(IntPtr deviceListArray, ref int arrayLength);

        [DllImport("SpotCamProxy.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern void SpotSetAbortFlag(IntPtr abortToken);

        [DllImport("SpotCamProxy.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int SpotSetTTLOutputState([MarshalAs(UnmanagedType.Bool)] bool setActive);

        [DllImport("SpotCamProxy.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        internal static extern void SpotSetCallback(SpotCallback callback, UIntPtr userData);

        [DllImport("SpotCamProxy.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        internal static extern void SpotSetDeviceNotificationCallback(SpotDeviceNotifyCallback callback, UIntPtr userData);

        //[DllImport("SpotCamProxy.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        //public static extern int SpotSetColorTransformation(
        //    int precision,
        //    float sourceColorTemperature,
        //    SPOTCOLORTRANSFORMCALLBACK pfnCallback,
        //    [MarshalAs(UnmanagedType.Bool)] bool bThreadSafe,
        //    UIntPtr userData);

        [DllImport("SpotCamProxy.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int SpotDumpCameraMemory([MarshalAs(UnmanagedType.LPStr)] string fileName);

        public static Tuple<T, T> MarshalTuple<T>(IntPtr buffer) where T : struct
        {
            var first = (T)Marshal.PtrToStructure(buffer, typeof(T));
            var second = (T)Marshal.PtrToStructure(IntPtr.Add(buffer, Marshal.SizeOf(typeof(T))), typeof(T));
            return Tuple.Create(first, second);
        }

        public static T[] MarshalArray<T>(IntPtr buffer, int arrayLength)
        {
            var newArray = new T[arrayLength];
            for (int ix = 0; ix < arrayLength; ++ix)
            {
                newArray[ix] = (T)Marshal.PtrToStructure(buffer, typeof(T));
                buffer = IntPtr.Add(buffer, Marshal.SizeOf(typeof(T)));
            }
            return newArray;
        }

        public static T[] MarshalLengthPrefixArray<T>(IntPtr buffer) where T : struct, IConvertible
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

        internal static Exception MakeException(SpotCamReturnCode returnCode, bool treatWarningAsError = false)
        {
            if (SpotCamReturnCode.Success == returnCode || (!treatWarningAsError && returnCode < 0))
                return null;
            Exception exception = null;
            switch (returnCode)
            {
                case SpotCamReturnCode.WarnServiceConfigurationUnknown:
                    break;
                case SpotCamReturnCode.WarnColorLibNotLoaded:
                    break;
                case SpotCamReturnCode.WarnInvalidOutputIcc:
                    break;
                case SpotCamReturnCode.WarnInvalidInputIcc:
                    break;
                case SpotCamReturnCode.WarnUnsupportedCameraFeatures:
                    break;
                case SpotCamReturnCode.Abort:
                    exception = new OperationCanceledException();
                    break;
                case SpotCamReturnCode.ErrorOutOfMemory:
                    exception = new OutOfMemoryException();
                    break;
                case SpotCamReturnCode.ErrorExposureTooShort:
                    break;
                case SpotCamReturnCode.ErrorExposureTooLong:
                    break;
                case SpotCamReturnCode.ErrorNoCameraResponse:
                    break;
                case SpotCamReturnCode.ErrorValueOutOfRange:
                    break;
                case SpotCamReturnCode.ErrorInvalidParam:
                    exception = new ArgumentException("The SpotCam parameter is not valid");
                    break;
                case SpotCamReturnCode.ErrorDriverNotInitialized:
                    break;
                case SpotCamReturnCode.ErrorRegistryQuery:
                    break;
                case SpotCamReturnCode.ErrorRegistrySet:
                    break;
                case SpotCamReturnCode.ErrorDeviveDriverLoad:
                    break;
                case SpotCamReturnCode.ErrorCameraError:
                    break;
                case SpotCamReturnCode.ErrorDriverAlreadyInit:
                    break;
                case SpotCamReturnCode.ErrorDmaSetup:
                    break;
                case SpotCamReturnCode.ErrorReadingCameraInfo:
                    break;
                case SpotCamReturnCode.ErrorNotCapable:
                    break;
                case SpotCamReturnCode.ErrorColorFilterNotIn:
                    break;
                case SpotCamReturnCode.ErrorColorFilterNotOut:
                    break;
                case SpotCamReturnCode.ErrorCameraBusy:
                    break;
                case SpotCamReturnCode.ErrorCameraNotSupported:
                    break;
                case SpotCamReturnCode.ErrorNoImageAvailable:
                    break;
                case SpotCamReturnCode.ErrorFileOpen:
                    break;
                case SpotCamReturnCode.ErrorFlatfieldIncompatible:
                    break;
                case SpotCamReturnCode.ErrorNoDevicesFound:
                    break;
                case SpotCamReturnCode.ErrorBrightnessChanged:
                    break;
                case SpotCamReturnCode.ErrorCameraAndCardIncompatible:
                    break;
                case SpotCamReturnCode.ErrorBiasFrameIncompatible:
                    break;
                case SpotCamReturnCode.ErrorBackgroundImageIncompatible:
                    break;
                case SpotCamReturnCode.ErrorBackgroundTooBright:
                    break;
                case SpotCamReturnCode.ErrorInvalidFile:
                    break;
                case SpotCamReturnCode.ErrorMisc:
                    break;
                case SpotCamReturnCode.ErrorImageTooBright:
                    break;
                case SpotCamReturnCode.ErrorNothingToDo:
                    break;
                case SpotCamReturnCode.ErrorNoCameraPower:
                    break;
                case SpotCamReturnCode.ErrorInsuf1394IsocBandwidth:
                    break;
                case SpotCamReturnCode.ErrorInsuf1394IsocResources:
                    break;
                case SpotCamReturnCode.ErrorNo1394IsocChannel:
                    break;
                case SpotCamReturnCode.ErrorUsbVersionLowerThan2:
                    break;
                case SpotCamReturnCode.ErrorStartupAlreadyDone:
                    break;
                case SpotCamReturnCode.ErrorStartupNotDone:
                    break;
                case SpotCamReturnCode.ErrorSpotcamServiceNotFound:
                    break;
                case SpotCamReturnCode.ErrorWrongSpotcamServiceVersion:
                    break;
                case SpotCamReturnCode.ErrorOperationNotSupported:
                    break;
                case SpotCamReturnCode.ErrorServiceConfigurationFileError:
                    break;
                case SpotCamReturnCode.ErrorServiceConfigurationSyntaxError:
                    break;
                case SpotCamReturnCode.ErrorServiceConfigurationInvalid:
                    break;
                default:
                    break;
            }
            if (null == exception)
                exception = new Exception(String.Format("SpotCam error {0}", returnCode));
            return exception;
        }

    }
}
