using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpotCam
{
    public enum SensorOpticalFilter : short
    {
        /// <summary>
        /// Applies an optical filter that measures the Red, Green and Blue components separately
        /// </summary>
        RGB   =  0,

        /// <summary>
        /// Applies an optical Red filter
        /// </summary>
        Red   =  1,

        /// <summary>
        /// Applies an optical Green filter
        /// </summary>
        Green =  2,

        /// <summary>
        /// Applies an optical Blue filter
        /// </summary>
        Blue  =  3,

        /// <summary>
        /// Applies an optical filter that measures the Red Green components separately
        /// <remarks>Destination image buffer will still need to be a RGB(a) source</remarks>
        /// </summary>
        RedGreen = 21,

        /// <summary>
        /// Applies an optical filter that measures the Red Blue components separately
        /// <remarks>Destination image buffer will still need to be a RGB(a) source</remarks>
        /// </summary>
        RedBlue = 22,

        /// <summary>
        /// Applies an optical filter that measures the Green Blue components separately
        /// <remarks>Destination image buffer will still need to be a RGB(a) source</remarks>
        /// </summary>
        GreenBlue = 23,

        /// <summary>
        /// Applies an IR blocking filter only
        /// </summary>
        VisableOnly = 10,

        /// <summary>
        /// No optical filter applied
        /// </summary>
        None  = 99
    }

    public enum SensorMosaicPattern : short
    {
        /// <summary>
        /// Mosaic pattern unknown
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Bayer pattern - Even lines: GRG... Odd lines: BGB...  (zero-based index)
        /// </summary>
        BayerGreenRedGreen = 1,

        /// <summary>
        /// Bayer pattern - Even lines: RGR... Odd lines: GBG... (zero-based index)
        /// </summary>
        BayerRedGreenRed = 2,

        /// <summary>
        /// Bayer pattern - Even lines: BGB... Odd lines: RGR... (zero-based index)
        /// </summary>
        BayerBlueGreenBlue = 3,

        /// <summary>
        /// Bayer pattern - Even lines: GBG... Odd lines: RGR... (zero-based index)
        /// </summary>
        BayerGreenBlueGreen = 4
    }
}
