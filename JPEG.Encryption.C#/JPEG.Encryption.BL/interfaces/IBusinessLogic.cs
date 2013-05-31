/// IBusinessLogic.cs
/// 
/// This interface determines all required methods for the business logic objects.
/// 
/// Authors:
/// Stefan Auer, Alexander Bliem
/// 
/// Date: 01.03.2013

using JPEG.Encryption.BL.Interfaces;
using System;
using System.Collections.Generic;

namespace JPEG.Encryption.BL
{
    /// <summary>
    /// Interface for the business logic.
    /// The business logic is devided in different sections.
    /// The sections are interfaces as well.
    /// </summary>
    public interface IBusinessLogic
    {
        /// <summary>
        /// Creates a camera business logic for handling the video device
        /// </summary>
        /// <returns>returns a camera business logic</returns>
        ICamera CreateCameraBL();

        /// <summary>
        /// Creates a RaspberryPiBL business logic
        /// </summary>
        /// <returns>returns a RaspberryPiBL business logic</returns>
        IFaceDetect CreateFaceDetectBL();

        /// <summary>
        /// Creates a JpegOperation business logic for converting byte arrays and images
        /// </summary>
        /// <returns>returns a JpegOperation business logic</returns>
        IJpegOperation CreateJpegOperationBL();

        /// <summary>
        /// Creates a RaspberryPiBL business logic for handling the Raspberry Pi
        /// </summary>
        /// <returns>returns a RaspberryPi business logic</returns>
        IRaspberryPi CreateRaspberryPiBL();
    }
}
