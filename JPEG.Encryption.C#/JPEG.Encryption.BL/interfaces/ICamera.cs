/// ICamera.cs
/// 
/// This interface determines all required methods for handling a USB camera.
/// 
/// Authors:
/// Stefan Auer, Alexander Bliem
/// 
/// Date: 01.03.2013


using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace JPEG.Encryption.BL.Interfaces
{
    /// <summary>
    /// Business logic interface for handling a USB camera
    /// </summary>
    public interface ICamera
    {
        double FrameWidth  { get; }
        double FrameHeight { get; }
        bool IsCameraAvailable { get; }

        /// <summary>
        /// Get a image from connected camera
        /// </summary>
        /// <returns></returns>
        Image QueryFrame();

        /// <summary>
        /// Sets the resolution of the camera
        /// </summary>
        /// <param name="width">the new width</param>
        /// <param name="height">the new height</param>
        void SetCameraResolution(double width, double height);
    }
}
