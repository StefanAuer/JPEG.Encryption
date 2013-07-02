/// CameraException.cs
/// 
/// This exception object will be thrown if there is a problem with the USB camera.
/// 
/// Authors:
/// Stefan Auer, Alexander Bliem
/// 
/// Date: 01.03.2013

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JPEG.Encryption.BL.exception
{
    /// <summary>
    /// This exception is thrown if there is a problem with the camera capture
    /// </summary>
    public class CameraException : Exception
    {

        /// <summary>
        /// the exception text
        /// </summary>
        public string Text { get; private set; }

        public CameraException(string text)
        {
            this.Text = text;
        }
    }
}
