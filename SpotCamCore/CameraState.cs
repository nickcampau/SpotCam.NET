using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpotCam
{
    public enum CameraState
    {
        /// <summary>
        /// The state of the camera is unknown
        /// </summary>
        Unknown,

        /// <summary>
        /// The camera is either powered off or in a low power sleep state.
        /// </summary>
        PoweredOff,

        /// <summary>
        /// The camera is no longer 
        /// </summary>
        Missing,

        /// <summary>
        /// The camera is powered on and available 
        /// to be connected.
        /// </summary>
        Inactive,
        
        /// <summary>
        /// The camera is currently connected and functioning 
        /// </summary>
        Active
    }
}
