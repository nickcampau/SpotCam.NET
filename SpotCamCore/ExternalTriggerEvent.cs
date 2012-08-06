using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpotCam
{
    public enum ExternalInputEvent : short
    {
        /// <summary>
        /// The external input will be ignored
        /// </summary>
        Ignore = 0,
        
        /// <summary>
        /// The transition from the inactive state to the active state will
        /// start the next pending exposure.
        /// See <seealso cref="GpioActiveState"/> for specifying the active state.
        /// </summary>
        StartExposureOnEdge = 1,
        
        /// <summary>
        /// The camera will expose for the entire duration that the
        /// input is held in the active state.
        /// See <seealso cref="GpioActiveState"/> for specifying the active state.
        /// </summary>
        BulbExposure = 2
    }

    public enum GpioActiveState : short
    {
        /// <summary>
        /// Defines the active state (true type) of the I/O 
        /// port to be the low voltage value.
        /// </summary>
        ActiveLow = 0,

        /// <summary>
        /// Defines the active state (true type) of the I/O 
        /// port to be the high voltage value.
        /// </summary>
        ActiveHigh = 1
    }

}
