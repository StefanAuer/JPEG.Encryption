/// IJpegOperation.cs
/// 
/// This interface determines all required methods for converting and determination of JPEG
/// 
/// Authors:
/// Stefan Auer, Alexander Bliem
/// 
/// Date: 01.03.2013


using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace JPEG.Encryption.BL.Interfaces
{

    public interface IJpegOperation
    {
        /// <summary>
        /// converts an image into an byte array
        /// </summary>
        /// <param name="image">the image that will be converted</param>
        /// <returns>the byte array or NULL if an error occurs</returns>
        byte[] ConvertImageToByteArray(Image image);

        /// <summary>
        /// converts an byte array to an image
        /// </summary>
        /// <param name="array">the byte array that will be converted</param>
        /// <returns>the image or NULL if an error occurs</returns>
        Image ConvertByteArrayToImage(byte[] array);

        /// <summary>
        /// Method to determine the given file is a JPG
        /// </summary>
        /// <param name="image">the image to be checked</param>
        /// <returns>true if JPEG</returns>
        bool IsJpegImage(Image image);
    }

}
