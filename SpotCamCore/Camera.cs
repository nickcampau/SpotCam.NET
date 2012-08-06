using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpotCam
{
    using SpotCam.Interop;
    using SpotCam.Diagnostics;

    public sealed class Camera
    {
        Device device;
        bool isConnected = false;

        internal Camera(Device device)
        {
            if (null == device)
                throw new ArgumentNullException();
            this.device = device;
            unsafe
            {
                ulong devUid = device.DeviceUID;
                var serviceException = SpotCamService.MakeException(SpotCamService.SpotSetValue(CoreParameter.DeviceUid, new IntPtr(&devUid)));
                if (serviceException != null)
                    throw new InvalidOperationException("Unable to set the device identifier", serviceException);
                SpotCamService.MakeException(SpotCamService.SpotInit()).ThrowIfNotNull();
                isConnected = true;
                SpotVersionDetails cameraDetails;
                SpotCamService.SpotGetVersionInfo2(out cameraDetails);
                this.Model = cameraDetails.CameraModelNum;
                this.SerialNumber = cameraDetails.CameraSerialNum;
                Version tempVersion = new Version();
                if (Version.TryParse(cameraDetails.CameraHadrwareRevNum, out tempVersion))
                    this.HardwareVersion = tempVersion;
                else
                    this.HardwareVersion = new Version();
                if (Version.TryParse(cameraDetails.CameraFirmwareRevNum, out tempVersion))
                    this.FirmwareVersion = tempVersion;
                else
                    this.FirmwareVersion = new Version();
                CameraAttributes attibutes;
                SpotCamService.SpotGetCameraAttributes(out attibutes);
                this.Attributes = attibutes;  
                ImageSensorType sensorType;
                SpotCamService.SpotGetValue(CoreParameter.ImageSensorType, new IntPtr(&sensorType));
                ImageSensorType = sensorType;
            }
        }

        public string Description { get { return device.Description; } }

        public ulong DeviceUid { get { return device.DeviceUID; } }

        public string Model { get; private set;}

        public string SerialNumber { get; private set;}

        public Version HardwareVersion { get; private set;}

        public Version FirmwareVersion { get; private set;}

        public Diagnostics.CameraAttributes Attributes { get; private set; }

        public Diagnostics.ImageSensorType ImageSensorType { get; private set; }

        public override string ToString()
        {
            return device.Description;
        }

        event EventHandler<EventArgs> OnConnected;

        event EventHandler<DisconnectionEventArgs> OnDisconnecting;

        event EventHandler<DisconnectionEventArgs> OnDisconnected;


        internal void HandleConnection()
        {
            if (!isConnected)
            {
                if (OnDisconnecting != null)
                    OnConnected(this, EventArgs.Empty);
                isConnected = true;
            }
        }

        internal void HandleDisconnection(DisconnectionReason reason)
        {
            if (isConnected)
            {
                var args = new DisconnectionEventArgs(reason);
                if (DisconnectionReason.DeviceRemoved != reason && OnDisconnecting != null)
                    OnDisconnecting(this, args);
                if (OnDisconnected != null)
                    OnDisconnected(this, args);
                isConnected = false;
            }
        }
    }
}
