using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using SpotCam;
using SpotCam.Diagnostics;
using SpotCam.Interop;

namespace CaptureAndSaveImageFile
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                CameraFactory.Initialize();
                foreach (var device in CameraFactory.DeviceList)
                    Console.WriteLine("Found device: {0}", device.Description);
                SpotCamServiceDetails versionInfo;
                SpotCamService.SpotGetVersionInfo2(out versionInfo);
                Console.WriteLine("Connected to driver:\n{0} {1}.{2}.{3} - {4}", versionInfo.ProductName, versionInfo.VerMajor, versionInfo.VerMinor, versionInfo.VerUpdate, versionInfo.BuildDetails);
                Console.WriteLine(versionInfo.Copyright);
                var camera = CameraFactory.DeviceList.Last().Create();
                Console.WriteLine("Camera Details:");
                Console.WriteLine("Model:{0}, SN:{1}, Firmware Rev: {2}, Hardware Rev: {3}", camera.Model, camera.SerialNumber, camera.FirmwareVersion, camera.HardwareVersion);
                Console.WriteLine("Capturing test TIFF image");
                CaptureAndSaveTestImage(camera, "SampleImage.tif");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.BackgroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Fatal Error:\n{0}", ex.Message);
                Console.ResetColor();
            }
            finally
            {
                CameraFactory.Shutdown();
            }
        }

        public static void CaptureAndSaveTestImage(Camera camera, string fileName)
        {
            var monochromeBitDepths = SpotCamService.BitDepths.Where(x => x < 24).ToArray();
            if (!monochromeBitDepths.Any())
                throw new NotSupportedException("The camera does not support acquiring monochrome images");
            short maxMonoBitDepth = monochromeBitDepths.Max();
            SpotCamService.BitDepth = maxMonoBitDepth;
            if (camera.Attributes.HasFlag(CameraAttributes.Mosaic))
            {
                // Mosaic cameras can return monochrome images in one of two ways.
                // As a raw mosaic image (bayer pixel pattern)
                // With a binning value greater than 1 (if supported by the camera)
                var binningLevelsAboveOne = from binSize in SpotCamService.BinningSizes
                                            where binSize > 1
                                            orderby binSize ascending
                                            select binSize;
                if (binningLevelsAboveOne.Any())
                    SpotCamService.BinSize = binningLevelsAboveOne.First();
                else
                    SpotCamService.ReturnRawMosaicData = true;
            }
            var imageSize = GetImageSize();
            //Create a BitmapData and Lock all pixels to be written
            var bmp = new Bitmap(imageSize.Width, imageSize.Height, maxMonoBitDepth <= 8 ? PixelFormat.Format8bppIndexed : PixelFormat.Format16bppGrayScale);
            var bmpData = bmp.LockBits(new Rectangle(Point.Empty, bmp.Size), ImageLockMode.WriteOnly, bmp.PixelFormat);
            var getImageReturnCode = SpotCamService.SpotGetImage(0, 0, 0, bmpData.Scan0, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
            bmp.UnlockBits(bmpData);
            if (SpotCamReturnCode.Success != getImageReturnCode)
                throw new InvalidOperationException(String.Format("Error capturing image - {0}.", getImageReturnCode));
            // WinForms draws the image buffer to screen in descending Y order (bottom to top)
            // To correct for this just flip the Y axis
            bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
            SaveToTiffFile(bmp, fileName);
        }

        private static Size GetImageSize()
        {
            SPOT_SIZE imageSize = SpotCamService.AcquiredImageSize;
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
                for (int ix = 0; ix < nonCreatableCachedPalette.Entries.Length; ++ix)
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
    }
}