using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.Drawing;
using System.Diagnostics;
using SpotCam.Interop;

namespace CaptureAndSaveImageFile
{
    class Program
    {
        static void Main(string[] args)
        {
            SpotCamReturnCode retCode = BaseService.SpotStartUp(IntPtr.Zero);
            Console.WriteLine("SpotStartUp returned: {0}", retCode);
            if (retCode > 0)
                return;
            /// Device selection override
            //unsafe
            //{
            //    short deviceNum = 1;
            //    BaseService.SpotSetValue(SpotCamParameter.DriverDeviceNumber, new IntPtr(&deviceNum));
            //}
            retCode = BaseService.SpotInit();
            Console.WriteLine("SpotInit returned: {0}", retCode);
            SPOT_VERSION_STRUCT2 versionInfo = new SPOT_VERSION_STRUCT2();
            BaseService.SpotGetVersionInfo2(ref versionInfo);
            Console.WriteLine("Connected to driver:\n{0} {1}.{2}.{3} - {4}", versionInfo.ProductName, versionInfo.VerMajor, versionInfo.VerMinor, versionInfo.VerUpdate, versionInfo.BuildDetails);
            Console.WriteLine(versionInfo.Copyright);
            if (retCode == SpotCamReturnCode.Success)
            {
                Console.WriteLine("Camera Details:");
                Console.WriteLine("Model:{0}, SN:{1}, Firmware Rev: {2}, Hardware Rev: {3}", versionInfo.CameraModelNum, versionInfo.CameraSerialNum, versionInfo.CameraFirmwareRevNum, versionInfo.CameraHadrwareRevNum);
                Console.WriteLine("Capturing test TIFF image");
                CaptureAndSaveTestImage("SampleImage.tif");
            }
            Console.WriteLine("SpotShutDown returned: {0}", BaseService.SpotShutDown());
        }

        public unsafe static void CaptureAndSaveTestImage(string fileName)
        {
            //Create a BitmapData and Lock all pixels to be written
            var imageSize = GetImageSize();
            var monochromeBitDepths = GetSupportedBitDepths().Where(x => x < 24).ToArray();
            if (!monochromeBitDepths.Any())
                throw new NotSupportedException("The camera does not support acquiring monochrome images");
            short maxMonoBitDepth = monochromeBitDepths.Max();
            unsafe
            {
                Debug.Assert(SpotCamReturnCode.Success == BaseService.SpotSetValue(SpotCamParameter.BitDepth, new IntPtr(&maxMonoBitDepth)));
            }
            var bmp = new Bitmap(imageSize.Width, imageSize.Height, maxMonoBitDepth <= 8 ? PixelFormat.Format8bppIndexed : PixelFormat.Format16bppGrayScale);
            var bmpData = bmp.LockBits(new Rectangle(Point.Empty, bmp.Size), ImageLockMode.WriteOnly, bmp.PixelFormat);
            var getImageReturnCode = BaseService.SpotGetImage(0, 0, 0, bmpData.Scan0, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
            bmp.UnlockBits(bmpData);
            if (SpotCamReturnCode.Success != getImageReturnCode)
                throw new InvalidOperationException(String.Format("Error capturing image - {0}.", getImageReturnCode));
            // WinForms draws the image buffer to screen in descending Y order (bottom to top)
            // To correct for this just flip the Y axis
            bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
            SaveToTiffFile(bmp, fileName);
        }

        private unsafe static Size GetImageSize()
        {
            SPOT_SIZE imageSize;
            unsafe
            {
                var returnCode = BaseService.SpotGetValue(SpotCamParameter.AcquiredImageSize, new IntPtr(&imageSize));
                Debug.Assert(SpotCamReturnCode.Success == returnCode,
                    "Unable to query device for the final image resolution",
                    "SpotCamReturnCode: {0}", returnCode);
            }
            return new Size((int)imageSize.Width, (int)imageSize.Height);
        }

        private static void SaveToTiffFile(Bitmap bitmap, string fileName)
        {
            // The WPF class System.Windows.Media.Imaging.TiffBitmapEncoder
            // is needed to save monochrome images greater than 8 bits.
            // WinForms uses GDI+ to handle saving images which has no support for these large 
            // bit depth images. 
            if (bitmap.PixelFormat == PixelFormat.Format8bppIndexed)
            {
                // Bitmap palettes are not constructible they can only be
                // created from within the source bitmap. In addition
                // calls to the Bitmap.Palette parameter return a copy of the palette
                // not a reference to it.
                // Therefore it must be captured, modified and then replaced
                var nonCreatableCachedPalette = bitmap.Palette;
                for (int ix = 0; ix < bitmap.Palette.Entries.Length; ++ix)
                    nonCreatableCachedPalette.Entries[ix] = Color.FromArgb(ix, ix, ix);
                bitmap.Palette = nonCreatableCachedPalette;
                bitmap.Save(fileName, ImageFormat.Tiff);
            }
            else
            {
                using (var fileStream = new System.IO.FileStream(fileName, System.IO.FileMode.Create))
                {
                    BitmapData bmpData = bitmap.LockBits(new Rectangle(Point.Empty, bitmap.Size), ImageLockMode.ReadOnly, bitmap.PixelFormat);
                    try
                    {
                        var wpfImage = System.Windows.Media.Imaging.BitmapFrame.Create(
                            bmpData.Width, bmpData.Height, 96.0, 96.0,
                            System.Windows.Media.PixelFormats.Gray16,
                            System.Windows.Media.Imaging.BitmapPalettes.WebPalette,
                            bmpData.Scan0, bmpData.Stride * bmpData.Height, bmpData.Stride);
                        var encoder = new System.Windows.Media.Imaging.TiffBitmapEncoder();
                        encoder.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(wpfImage));
                        encoder.Compression = System.Windows.Media.Imaging.TiffCompressOption.None;
                        encoder.Save(fileStream);
                    }
                    finally
                    {
                        bitmap.UnlockBits(bmpData);
                    }
                }
            }
        }

        static short[] GetSupportedBitDepths()
        {
            var buffer = Marshal.AllocHGlobal(BaseService.SpotGetValueSize(SpotCamParameter.BitDepths));
            var returnCode = BaseService.SpotGetValue(SpotCamParameter.BitDepths, buffer);
            Debug.Assert(SpotCamReturnCode.Success == returnCode,
                "Unable to query device for available bit depths",
                "SpotCamReturnCode: {0}", returnCode);
            var availableBitDepths = BaseService.MarshalLengthPrefixArray<short>(buffer);
            Marshal.FreeHGlobal(buffer);
            return availableBitDepths;
        }

    }
}
