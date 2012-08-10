using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace SpotCam
{
    using Interop;

    public class DeviceList : ObservableCollection<Device>
    {
        internal DeviceList()
            : base()
        {
            foreach (var item in GetServiceList())
            {
                Add(item);
            }
        }

        /// <summary>
        /// Updates the associatedDevice list with an explicit query of the 
        /// </summary>
        public void Refresh()
        {
            var newDeviceList = GetServiceList();
            using (BlockReentrancy())
            {
                ClearItems();
                foreach (var item in newDeviceList)
                {
                    Add(item);
                }
            }
        }

        private Device[] GetServiceList()
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
            return rawArray.Select((item, index) => new Device(item, index)).ToArray();
        }
    }
}
