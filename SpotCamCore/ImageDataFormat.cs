using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpotCam
{
    public enum ImageDataFormat : short
    {
        Unknown = 0,

        /// <summary>
        /// Blue, Green, Red
        /// Rows are padded to be divisible by 4
        /// </summary>
        Bgr = 1,

        /// <summary>
        /// Red, Green, Blue
        /// Rows are padded to be divisible by 4
        /// </summary>
        Rgb = 2,

        /// <summary>
        /// Alpha, Red, Green, Blue
        /// </summary>
        Argb = 3,

        /// <summary>
        /// Alpha, Blue, Green, Red
        /// </summary>
        Abgr = 4,

        /// <summary>
        /// Red, Green, Blue, Alpha
        /// </summary>
        Rgba = 5,

        /// <summary>
        /// Blue, Green, Red, Alpha
        /// </summary>
        Bgra = 6
    }
}