using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace SpotCam
{
    using SpotCam.Interop;

    public static class CameraFactory
    {
        private static Camera currentCamera = null;

        private static object deviceLock = new Object();

        /// <summary>
        /// Initializes the factory. Must be called before any operations are performed.
        /// </summary>
        public static void Initialize()
        {
            var returnCode = SpotCamService.SpotStartUp(IntPtr.Zero);
            if (!(SpotCamReturnCode.Success == returnCode || SpotCamReturnCode.ErrorStartupAlreadyDone == returnCode))
                throw SpotCamService.MakeException(returnCode);
        }

        /// <summary>
        /// Shuts down the factory. Initialize must be called again to restart the factory.
        /// </summary>
        public static void Shutdown()
        {
            System.Diagnostics.Debug.Assert(SpotCamReturnCode.Success == SpotCamService.SpotShutDown());
            currentCamera = null;
        }

        /// <summary>
        /// Returns a collection of currently connected devices.
        /// </summary>
        public static ICollection<Device> DeviceList
        {
            get
            {
                IntPtr buffer = IntPtr.Zero;
                SPOT_DEVICE_STRUCT[] rawArray;
                try
                {
                    buffer = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(SPOT_DEVICE_STRUCT)) * SpotCamService.MaxDevices);
                    int numberOfDevices = SpotCamService.MaxDevices;
                    SpotCamService.SpotFindDevices(buffer, ref numberOfDevices);
                    rawArray = SpotCamService.MarshalArray<SPOT_DEVICE_STRUCT>(buffer, numberOfDevices);
                }
                finally
                {
                    Marshal.FreeHGlobal(buffer);
                }
                return rawArray.Select(x => new Device(x)).ToArray();
            }
        }

        /// <summary>
        /// Creates a new Camera from a Device.
        /// </summary>
        /// <param name="device">The device </param>
        /// <returns>The newly created Camera</returns>
        /// <exception cref="ArgumentNullException">The Device supplied is null</exception>
        /// <exception cref="DeviceNotFound"></exception>
        public static Camera Create(this Device device)
        {
            if (null == device)
                throw new ArgumentNullException();
            if (!device.IsPoweredOn)
                throw new ArgumentException("The device is not powered on");
            lock (deviceLock)
            {
                // The SpotCam service only allows one camera at a time to be controlled.
                if (currentCamera != null)
                    currentCamera.HandleDisconnection(DisconnectionReason.ForcedByService);
                currentCamera = null;
                currentCamera = new Camera(device);
                return currentCamera;
            }
        }

        public static void Reconnect(Camera camera)
        {
            if (null == camera)
                throw new ArgumentNullException();
            lock (deviceLock)
            {
                ulong devUid = camera.DeviceUid;
                unsafe
                {
                    var serviceException = SpotCamService.MakeException(SpotCamService.SpotSetValue(CoreParameter.DeviceUid, new IntPtr(&devUid)));
                    if (serviceException != null)
                        throw new InvalidOperationException("Unable to set the device identifier", serviceException);
                }
                SpotCamService.MakeException(SpotCamService.SpotInit()).ThrowIfNotNull();
                currentCamera = camera;                
            }
            camera.HandleConnection();
        }

        public static void Disconnect(this Camera camera)
        {
            if (null == camera)
                throw new ArgumentNullException();
            lock (deviceLock)
            {
                camera.HandleDisconnection(DisconnectionReason.Requested);
                Interop.SpotCamService.SpotExit();
            }
        }
        
        internal static void ThrowIfNotNull(this Exception ex)
        {
            if (null == ex)
                return;
            throw ex;
        }

    }
}
