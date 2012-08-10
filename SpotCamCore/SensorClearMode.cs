using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpotCam
{
    public enum SensorClearMode : uint
    {
        /// <summary>
        /// Unknown sensor clearing mode.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Continuously clear the sensor's readout registers while idle.
        /// </summary>
        Continuous = 1,

        /// <summary>
        /// Continuously clear the sensor's readout registers while idle
        /// but allow an exposure to preempt the currently in process clear cycle.
        /// </summary>
        PreEmptable = 2,

        /// <summary>
        /// Prohibit the camera from clearing the sensor's readout registers.
        /// </summary>
        /// <remarks>
        /// Usage of this mode should be considered carefully.
        /// Disabling the clearing of the sensor's readout registers will result in an accumulation 
        /// of electrical charge on the image sensor.
        /// This extra charge will then be added to the photonic signal on the completion of the exposure.
        /// Enable this mode only to prevent a partial clearing to occur when acquiring short exposure
        /// images in succession. (Live, Sequential Imaging) 
        /// </remarks> 
        NeverClear = 4
    }
}
