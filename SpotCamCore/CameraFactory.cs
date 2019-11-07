using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace SpotCam
{
    using SpotCam.Interop;

    public static class CameraFactory
    {
        private volatile static Camera currentCamera = null;
        private volatile static DeviceList deviceList;
        private static object deviceLock = new Object();
        private volatile static SpotDeviceNotifyCallback deviceNotifyCallback;
        private volatile static SpotCallback statusCallback;

        /// <summary>
        /// Initializes the factory. Must be called before any operations are performed.
        /// </summary>
        public static void Initialize()
        {
            SpotCamService.SpotStartUp(IntPtr.Zero).CheckSuccessIgnoreWarning();
            deviceList = new DeviceList();
            deviceNotifyCallback = new SpotDeviceNotifyCallback(OnDeviceNotification);
            SpotCamService.SpotSetDeviceNotificationCallback(deviceNotifyCallback, UIntPtr.Zero);
        }

        /// <summary>
        /// Shut down the camera factory.
        /// </summary>
        /// <remarks>Initialize must be called again to restart the factory.</remarks>
        public static void Shutdown()
        {
            System.Diagnostics.Debug.Assert(SpotCamReturnCode.Success == SpotCamService.SpotShutDown());
            currentCamera = null;
            deviceNotifyCallback = null;
            statusCallback = null;
            deviceList = null;
#if !SPOTCAM_PREVENT_GC_COLLECTIONS
            GC.Collect();
#endif
        }


        /// <summary>
        /// Returns a collection of currently connected devices.
        /// </summary>
        public static DeviceList DeviceList
        {
            get
            {
                return deviceList;
            }
        }

        /// <summary>
        /// A associatedDevice is now available to the host
        /// </summary>
        public static event EventHandler<EventArgs> DeviceAdded;

        /// <summary>
        /// A associatedDevice has been removed from the host
        /// </summary>
        public static event EventHandler<EventArgs> DeviceRemoved;
        
        private static void OnDeviceNotification(DeviceEvent eventType, int extraInfo, IntPtr deviceUid, UIntPtr userData)
        {
            ulong uid = (ulong)Marshal.ReadInt64(deviceUid);
            switch (eventType)
            {
                case DeviceEvent.DeviceAdded:
                    var existingDev = DeviceList.Where(x => x.DeviceUID == uid).FirstOrDefault();
                    if (existingDev != null)
                        existingDev.IsPoweredOn = true;
                    else
                        DeviceList.Refresh();
                    if (DeviceAdded != null)
                        DeviceAdded(new Object(), EventArgs.Empty); 
                    break;
                case DeviceEvent.DeviceRemoved:
                    lock (deviceLock)
                    {
                        if (currentCamera != null && currentCamera.DeviceUid == uid)
                        {
                            currentCamera.OnDisconnection(DisconnectionReason.DeviceRemoved);
                            currentCamera = null;
                        }
                    }
                    DeviceList.Remove(DeviceList.Where(x => x.DeviceUID == uid).FirstOrDefault());
                    if (DeviceRemoved != null)
                        DeviceRemoved(new Object(), EventArgs.Empty); 
                    break;
                case DeviceEvent.Unknown:
                default:
                    break;
            }
        }

        /// <summary>
        /// Creates a new Camera from a Device.
        /// </summary>
        /// <param name="associatedDevice">The associatedDevice </param>
        /// <returns>The newly created Camera</returns>
        /// <exception cref="ArgumentNullException">The Device supplied is null</exception>
        /// <exception cref="CameraException">
        /// This actual exception may be an one of the exceptions derived from CameraException.
        /// </exception>
        /// <exception cref="InvalidColorProfileException">A default ICC profile for the camera cannot be found</exception>
        /// <exception cref="SpotCamServiceException"></exception>
        /// <exception cref="OutOfMemoryException">Not enough memory to create the camera</exception>
        public static Camera Create(this Device device)
        {
            if (null == device)
                throw new ArgumentNullException();
            lock (deviceLock)
            {
                // The SpotCam service only allows one camera at a time to be controlled.
                if (currentCamera != null)
                    currentCamera.OnDisconnection(DisconnectionReason.ForcedByService);
                currentCamera = null;
                currentCamera = new Camera(device);
                return currentCamera;
            }
        }

        /// <summary>
        /// Reestablishes a connection to an existing camera.
        /// </summary>
        /// <param name="camera"></param>
        public static void Reconnect(Camera camera)
        {
            if (null == camera)
                throw new ArgumentNullException();
            lock (deviceLock)
            {
                SpotCamService.DeviceUid = camera.DeviceUid;
                SpotCamService.SpotInit().CheckSuccess();
                currentCamera = camera;                
            }
            camera.OnConnection();
        }

        /// <summary>
        /// Disconnects a 
        /// </summary>
        /// <param name="camera"></param>
        public static void Disconnect(this Camera camera)
        {
            if (null == camera)
                throw new ArgumentNullException();
            lock (deviceLock)
            {
                camera.OnDisconnection(DisconnectionReason.Requested);
                SpotCamService.SpotExit();
            }
        }
        
        /// <summary>
        /// Checks to see if the exception is non-null and throws
        /// </summary>
        /// <param name="ex">The exception to throw. If null nothing happens</param>
        private static void ThrowIf(this Exception ex)
        {
            if (null != ex)
                throw ex;
        }

    }
}
