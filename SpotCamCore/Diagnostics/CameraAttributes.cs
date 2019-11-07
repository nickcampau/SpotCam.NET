using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpotCam.Diagnostics
{
    [Flags]
    public enum CameraAttributes : uint
    {
        /// <summary>
        /// No camera attributes specified
        /// </summary>
        None = 0,

        /// <summary>
        /// camera can return color images
        /// </summary>
        Color                   = 0x00000001,

        /// <summary>
        /// camera has a color filter slider
        /// </summary>
        Slider                  = 0x00000002,

        /// <summary>
        /// camera can run live mode
        /// </summary>
        LiveMode                = 0x00000004,

        /// <summary>
        /// camera has a Bayer pattern color mosaic CCD chip
        /// </summary>
        Mosaic                  = 0x00000008,

        /// <summary>
        /// camera has an edge-type external trigger
        /// </summary>
        EdgeTrigger             = 0x00000010,

        /// <summary>
        /// camera has a bulb-type external trigger
        /// </summary>
        BulbTrigger             = 0x00000020,

        /// <summary>
        /// camera has a color filter with a clear state
        /// </summary>
        ClearFilter             = 0x00000040,

        /// <summary>
        /// camera is an IEEE-1394/FireWire associatedDevice
        /// </summary>
        Ieee1394                = 0x00000080,

        /// <summary>
        /// camera has a color filter (color images require multiple shots)
        /// </summary>
        ColorFilter             = 0x00000100,

        /// <summary>
        /// camera can read the sensor temperature
        /// </summary>
        TemperatureReadout      = 0x00000200,

        /// <summary>
        /// camera can regulate the sensor temperature
        /// </summary>
        TemperatureRegulation   = 0x00000400,

        /// <summary>
        /// camera can set trigger input active state
        /// </summary>
        TriggerActiveState      = 0x00000800,

        /// <summary>
        /// camera has separate amplifier circuits for live mode and capture
        /// </summary>
        DualAmplifier           = 0x00001000,

        /// <summary>
        /// camera can accurately time TTL output and trigger delays (to microseconds)
        /// </summary>
        AccurateTtlDelayTiming  = 0x00002000,

        /// <summary>
        /// camera can detect the color filter slider position
        /// </summary>
        SliderPositionDetection = 0x00004000,

        /// <summary>
        /// camera can compute exposure
        /// </summary>
        AutoExposure            = 0x00008000,

        /// <summary>
        /// camera has a TTL output
        /// </summary>
        TtlOutput               = 0x00010000,

        /// <summary>
        /// camera can shift the position of the image sensor for higher resolution
        /// </summary>
        SensorShifting          = 0x00020000,

        /// <summary>
        /// camera has a mechanical filter wheel
        /// </summary>
        FilterWheel             = 0x00040000,

        /// <summary>
        /// camera can do black-level subtraction
        /// </summary>
        BlackLevelSubtract      = 0x00080000,

        /// <summary>
        /// camera can do chip defect correction
        /// </summary>
        ChipDefectCorrection    = 0x00100000,

        /// <summary>
        /// camera has an internal mechanical shutter
        /// </summary>
        InternalShutter         = 0x00200000,

        /// <summary>
        /// camera's internal shutter can be used for exposure
        /// </summary>
        ExposureShutter         = 0x00400000,

        /// <summary>
        /// camera can activate TTL output during exposure
        /// </summary>
        TtlOutputDuringExposure = 0x00800000,

        /// <summary>
        /// camera can use a single readout channel in live mode
        /// </summary>
        SingleChannelLiveMode   = 0x01000000,

        /// <summary>
        /// camera can use multiple parallel readout channels in live mode
        /// </summary>
        MultiChannelLiveMode    = 0x02000000,

        /// <summary>
        /// camera's firmware can be updated
        /// </summary>
        FirmwareUpdate          = 0x04000000,

        /// <summary>
        /// camera can provide histogram information and do stretching in live mode
        /// </summary>
        LiveHistogram           = 0x08000000,

        /// <summary>
        /// camera can provide native 8-bit image data readout for still image capture
        /// </summary>
        OctetReadout            = 0x10000000,

        /// <summary>
        /// camera can change the fan speed automatically before exposure
        /// </summary>
        FanExposureSpeedSetting = 0x20000000 
    }
}
