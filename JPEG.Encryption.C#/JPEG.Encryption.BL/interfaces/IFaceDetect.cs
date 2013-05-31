/// IFaceDetect.cs
/// 
/// This interface determines all required methods for face detection.
/// 
/// Authors:
/// Stefan Auer, Alexander Bliem
/// 
/// Date: 01.03.2013


using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using JPEG.Encryption.BL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace JPEG.Encryption.BL.Interfaces
{
    public interface IFaceDetect
    {
        /// <summary>
        /// Get a IList of RoIs from an image
        /// </summary>
        /// <param name="image">the image that will be scanned for faces</param>
        /// <param name="detectionType">the face detection algorithm that will be used</param>
        /// <param name="scale">determines the how the image will be scaled before the face detection algorithm starts</param>
        /// <param name="minNeighbors">determine the minimum amout of required neighbors</param>
        /// <param name="minSize">minimum size of objects. Every object smaller than this value will be ignored</param>
        /// <returns>IList of found RoIs</returns>
        IList<RoI> DetectFaces(Image image, EnumDetectionType detectionType, double scale, int minNeighbors, Size minSize);
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Array GetFaceDetectionAlgorithms();
    }
}
