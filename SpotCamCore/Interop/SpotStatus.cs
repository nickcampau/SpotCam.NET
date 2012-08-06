using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpotCam.Interop
{
    internal enum DeviceEvent : int
    {
        Unknown = 0,
        DeviceAdded = 1,
        DeviceRemoved = 2
    }
    
    internal enum SpotStatus : int
    {
        Unknown = -1,

        ///<summary>
        /// 
        ///</summary>
        Idle = 0,

        ///<summary>
        /// 
        ///</summary>
        DriverNotInitialized = 1,

        ///<summary>
        /// 
        ///</summary>
        ExposingRed = 2,

        ///<summary>
        /// 
        ///</summary>
        ExposingGreen = 3,

        ///<summary>
        /// 
        ///</summary>
        ExposingBlue = 4,

        ///<summary>
        /// 
        ///</summary>
        ImageReadRed = 5,

        ///<summary>
        /// 
        ///</summary>
        ImageReadGreen = 6,

        ///<summary>
        /// 
        ///</summary>
        ImageReadBlue = 7,

        ///<summary>
        /// 
        ///</summary>
        ComputingExposure = 8,

        ///<summary>
        /// 
        ///</summary>
        ComputingWhiteBalance = 9,

        ///<summary>
        /// 
        ///</summary>
        GettingImage = 10,

        ///<summary>
        /// 
        ///</summary>
        LiveImageReady = 11,

        ///<summary>
        /// 
        ///</summary>
        ExposingClear = 12,

        ///<summary>
        /// 
        ///</summary>
        Exposing = 13,

        ///<summary>
        /// 
        ///</summary>
        ImageReadClear = 14,

        ///<summary>
        /// 
        ///</summary>
        ImageRead = 15,

        ///<summary>
        /// 
        ///</summary>
        SequentialImageWaiting = 16,

        ///<summary>
        /// 
        ///</summary>
        SequentialImageReady = 17,

        ///<summary>
        /// 
        ///</summary>
        ImageProcessing = 18,

        ///<summary>
        /// 
        ///</summary>
        WaitingForTrigger = 19,

        ///<summary>
        /// 
        ///</summary>
        WaitingForBlockedLight = 20,

        ///<summary>
        /// 
        ///</summary>
        WaitingForMoveToBackground = 21,

        ///<summary>
        /// 
        ///</summary>
        TtlOutputDelay = 22,

        ///<summary>
        /// 
        ///</summary>
        ExternalTriggerDelay = 23,

        ///<summary>
        /// 
        ///</summary>
        WaitingForColorFilter = 24,

        ///<summary>
        /// 
        ///</summary>
        GetLiveImages = 25,

        ///<summary>
        /// 
        ///</summary>
        FanDelay = 26,

        ///<summary>
        /// 
        ///</summary>
        FirmwareUpdating = 27,

        ///<summary>
        /// 
        ///</summary>
        Aborted = 100,

        ///<summary>
        /// 
        ///</summary>
        Error = 101,

        ///<summary>
        /// 
        ///</summary>
        Running = 500,


    }
}
