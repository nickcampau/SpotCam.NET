using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpotCam.Diagnostics
{
    public enum ImageSensorType : uint
    {
        /// <summary>
        /// Unknown image sensor type
        /// </summary>
        Unknown = 0,
        
        /// <summary>
        /// Conventional interline CCD
        /// </summary>
        CcdInterline = 0x0011,

        /// <summary>
        /// Conventional full-frame CCD
        /// </summary>
        CcdFullFrame = 0x0021,

        /// <summary>
        /// Conventional frame-transfer CCD
        /// </summary>
        CcdFrameTransfer = 0x0031,

        /// <summary>
        /// Electron-Multiplication interline CCD
        /// </summary>
        CcdInterlineEM = 0x0111,

        /// <summary>
        /// Electron-Multiplication frame-transfer CCD
        /// </summary>
        CcdFrameTransferEM = 0x0131,

        /// <summary>
        /// CMOS rolling shutter
        /// </summary>
        CmosRollingShutter = 0x0042,

        /// <summary>
        /// CMOS global shutter
        /// </summary>
        CmosGlobalShutter = 0x0012
    }
}
