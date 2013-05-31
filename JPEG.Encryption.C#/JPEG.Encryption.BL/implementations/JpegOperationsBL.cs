/// JpegOperationBL.cs
/// 
/// This class provides different methods for handling JPEG images.
/// For further details see IJpegOperation.cs as well.
/// 
/// Authors:
/// Stefan Auer, Alexander Bliem
/// 
/// Date: 01.03.2013


using JPEG.Encryption.BL.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace JPEG.Encryption.BL.Implementations
{
    /// <summary>
    /// class implementing IJpegOperation
    /// </summary>
    public class JpegOperationsBL : IJpegOperation
    {
        public byte[] ConvertImageToByteArray(Image image)
        {
            byte[] array;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                try
                {
                    image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                    array = memoryStream.ToArray();
                }
                catch
                {
                    array = null;
                }
            }
            return array;
        }

        public Image ConvertByteArrayToImage(byte[] array)
        {
            Image returnImage;
            using (MemoryStream memoryStream = new MemoryStream(array))
            {
                try
                {
                    MemoryStream ms = new MemoryStream(array);
                    returnImage = Image.FromStream(ms);
                }
                catch
                {
                    returnImage = null;
                }
            }
            return returnImage;
        }

        public bool IsJpegImage(Image image)
        {
            try
            {
                // Two image formats can be compared using the Equals method
                // See http://msdn.microsoft.com/en-us/library/system.drawing.imaging.imageformat.aspx
                //
                return image.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (OutOfMemoryException)
            {
                // Image.FromFile throws an OutOfMemoryException 
                // if the file does not have a valid image format or
                // GDI+ does not support the pixel format of the file.
                return false;
            }
        }
    }
}


