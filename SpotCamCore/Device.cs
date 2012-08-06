using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpotCam
{
    public class Device
    {
        internal Interop.SPOT_DEVICE_STRUCT baseData;
        
        internal Device(Interop.SPOT_DEVICE_STRUCT device)
        {
            baseData = device;
        }

        public Diagnostics.DeviceClass DeviceClass { get { return baseData.DeviceClass;} }

        public Diagnostics.DeviceIoBus DeviceIoBus { get { return baseData.DeviceIoBus;} }

        public Diagnostics.CameraAttributes Attributes { get { return baseData.Attributes;} }

        public string Description { get { return baseData.Description; } }

        /// <summary>
        /// Unique for every camera on a single computer system.
        /// Not globally unique.
        /// </summary>
        public UInt64 DeviceUID { get { return baseData.DeviceUID; } }

        public bool IsPoweredOn { get { return baseData.DeviceUID != 0; } }
    }
}
