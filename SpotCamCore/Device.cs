using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpotCam
{
    public class Device
    {
        Interop.SPOT_DEVICE_STRUCT baseData;
        
        internal Device(Interop.SPOT_DEVICE_STRUCT device, int deviceIndex)
        {
            baseData = device;
            DeviceListIndex = deviceIndex;
            IsPoweredOn = baseData.DeviceUID != 0;
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

        public bool IsPoweredOn { get; internal set; }

        internal int DeviceListIndex { get; private set; }
    }
}
