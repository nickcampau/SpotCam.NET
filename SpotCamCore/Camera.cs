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
        internal Camera(Device device)
        {
            if (null == device)
                throw new ArgumentNullException();
            Description = device.Description;
            DeviceUid = device.DeviceUID;
            try
            {
                SpotCamService.DeviceUid = device.DeviceUID;
                // If the Device UID is zero then the camera is not connected or is powered off.
                // Set the interface number instead and retrieve the UID after a successful initialization.
                if (0 == device.DeviceUID)
                    SpotCamService.DriverDeviceNumber = Convert.ToInt16(device.DeviceListIndex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Unable to set the target device", ex);                    
            }
            Initialize();
        }

        private void Initialize()
        {
            var initCode = SpotCamService.SpotInit();
            if (initCode == SpotCamReturnCode.ErrorNoCameraPower)
            {
                State = CameraState.PoweredOff;
                return;
            }
            try
            {
                initCode.CheckSuccess();
            }
            catch (Exception)
            {
                State = CameraState.Unknown;
                throw;
            }
            State = CameraState.Active;
            if (0 == DeviceUid)
                DeviceUid = SpotCamService.DeviceUid;
            unsafe
            {
                SpotCamServiceDetails cameraDetails;
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
                if (SpotCamReturnCode.Success == SpotCamService.SpotGetCameraAttributes(out attibutes))
                    Attributes = attibutes;
                ImageSensorType sensorType;
                if (SpotCamReturnCode.Success == SpotCamService.SpotGetValue(CoreParameter.ImageSensorType, new IntPtr(&sensorType)))
                    ImageSensorType = sensorType;
            }
        }

        public string Description { get; private set; }

        public ulong DeviceUid { get; private set; }

        public string Model { get; private set;}

        public string SerialNumber { get; private set;}

        public Version HardwareVersion { get; private set;}

        public Version FirmwareVersion { get; private set;}

        public Diagnostics.CameraAttributes Attributes { get; private set; }

        public Diagnostics.ImageSensorType ImageSensorType { get; private set; }

        private CameraState _state;
        public CameraState State
        {
            get { return _state; }
            private set
            {
                bool changed = _state != value;
                _state = value;
                if (StateChanged != null && changed)
                    StateChanged(this, EventArgs.Empty);
            }
        }

        public override string ToString()
        {
            return String.Format("{0} ({1})", Description, State);
        }

        event EventHandler<EventArgs> Connected;

        event EventHandler<DisconnectionEventArgs> Disconnecting;

        event EventHandler<DisconnectionEventArgs> Disconnected;

        event EventHandler<EventArgs> StateChanged;


        internal void OnConnection()
        {
            State = CameraState.Active;
            if (Connected != null)
                Connected(this, EventArgs.Empty);
        }

        internal void OnDisconnection(DisconnectionReason reason)
        {
            var args = new DisconnectionEventArgs(reason);
            if (DisconnectionReason.DeviceRemoved != reason && Disconnecting != null)
                Disconnecting(this, args);
            switch (reason)
            {
                case DisconnectionReason.DevicePoweredOff:
                    State = CameraState.PoweredOff;
                    break;
                case DisconnectionReason.DeviceRemoved:
                    State = CameraState.Missing;
                    break;
                case DisconnectionReason.Requested:
                case DisconnectionReason.ForcedByService:
                    State = CameraState.Inactive;
                    break;
                case DisconnectionReason.Unknown:
                default:
                    State = CameraState.Inactive;
                    break;
            }
            if (Disconnected != null)
                Disconnected(this, args);
        }
    }
}
