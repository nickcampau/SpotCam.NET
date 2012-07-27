using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace SpotCam.Interop
{

    public static class BaseService
    {
        public const int SPOT_MAX_DEVICES = 25;

        [DllImport("SpotCamProxy.dll", ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern SpotCamReturnCode SpotStartUp(IntPtr value);

        [DllImport("SpotCamProxy.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern SpotCamReturnCode SpotShutDown();

        [DllImport("SpotCamProxy.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern SpotCamReturnCode SpotInit();

        [DllImport("SpotCamProxy.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern void SpotGetVersionInfo2(
            ref SPOT_VERSION_STRUCT2 versionInfo);

        [DllImport("SpotCamProxy.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern SpotCamReturnCode SpotSetValue(
            SpotCamParameter param,
            IntPtr value);

        [DllImport("SpotCamProxy.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern SpotCamReturnCode SpotGetValue(
            SpotCamParameter param,
            IntPtr value);

        [DllImport("SpotCamProxy.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int SpotGetValueSize(SpotCamParameter param);

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
        public static extern int SpotRetrieveSequentialImage(IntPtr imageBuffer);

        public static Tuple<T, T> MarshalTuple<T>(IntPtr buffer) where T : struct
        {
            var first = (T)Marshal.PtrToStructure(buffer, typeof(T));
            var second = (T)Marshal.PtrToStructure(IntPtr.Add(buffer, Marshal.SizeOf(typeof(T))), typeof(T));
            return Tuple.Create(first, second);
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
    }
}
