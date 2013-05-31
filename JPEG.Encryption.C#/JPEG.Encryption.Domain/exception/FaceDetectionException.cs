/// FaceDetectionException.cs
/// 
/// This exception object will be thrown if there is a problem with the face detection.
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
    /// This exception is thrown if there is a problem with the face detection
    /// </summary>
    public class FaceDetectionException : Exception
    {
        /// <summary>
        /// the exception text
        /// </summary>
        public string Text { get; private set; } 

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="text">the exception text</param>
        public FaceDetectionException(string text)
        {
            this.Text = text;
        }
    }
}
