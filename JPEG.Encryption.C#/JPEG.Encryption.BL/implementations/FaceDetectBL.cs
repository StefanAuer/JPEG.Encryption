/// FaceDetectBL.cs
/// 
/// This class is responsable for the face detection.
/// For further details see IFaceDectect.cs as well.
/// 
/// Authors:
/// Stefan Auer, Alexander Bliem
/// 
/// Date: 01.03.2013


using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using JPEG.Encryption.BL.Interfaces;
using JPEG.Encryption.BL;
using JPEG.Encryption.BL.exception;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Configuration;

namespace JPEG.Encryption.BL.Implementations
{
    /// <summary>
    /// class implementing IFaceDetect
    /// </summary>
    public class FaceDetectBL : IFaceDetect
    {
        private HaarCascade face;
        private string fileNameHaarCascade = ConfigurationManager.AppSettings["FaceDetectionXML"];

        public FaceDetectBL()
        {
            // check if all libraries were loaded
            if (DependencyCheck.Execute())
            {
                // does the file exist?
                if (File.Exists(fileNameHaarCascade))
                    this.face = new HaarCascade(fileNameHaarCascade);
                else
                {
                    // the user has entered an invalid file name; let's try the default file name
                    string defaultFileName = @".\haarcascade_frontalface_default.xml";
                    if (File.Exists(defaultFileName))
                        this.face = new HaarCascade(defaultFileName);
                    else
                        throw new FaceDetectionException("Error while loading file: " + fileNameHaarCascade);
                }
            }
        }

        public IList<RoI> DetectFaces(Image image, EnumDetectionType detectionType, double scale, int minNeighbors, Size minSize)
        {
            IList<RoI> rois = new List<RoI>();

            // convert to openCV format
            HAAR_DETECTION_TYPE detectionTypeOpenCV = EnumHelper.StringToEnum<HAAR_DETECTION_TYPE>(detectionType.ToString());

            try
            {
                Image<Gray, byte> gray = new Image<Gray, byte>(new Bitmap(image));

                //Face Detector
                MCvAvgComp[][] facesDetected = gray.DetectHaarCascade(
                    face,
                    scale,
                    minNeighbors,
                    detectionTypeOpenCV,
                    minSize
                    );


                // create a RoI object for every detected face
                foreach (MCvAvgComp f in facesDetected[0])
                {
                    RoI roi = new RoI();
                    roi.X = f.rect.X;
                    roi.Y = f.rect.Y;
                    roi.Width = f.rect.Width;
                    roi.Height = f.rect.Height;
                    rois.Add(roi);
                }
            }
            catch 
            {
                throw new FaceDetectionException("Error while detecting faces!");
            }
            return rois;
        }
        public Array GetFaceDetectionAlgorithms()
        {
            return Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.GetValues(typeof(Emgu.CV.CvEnum.HAAR_DETECTION_TYPE));
        }
    }
}


