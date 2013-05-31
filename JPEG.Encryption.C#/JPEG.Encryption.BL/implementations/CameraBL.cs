/// CameraBL.cs
/// 
/// The camera business logic is responsable for handling a 
/// connected USB camera.
/// For further details see ICamera.cs as well.
/// 
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

namespace JPEG.Encryption.BL.Implementations
{
    /// <summary>
    /// class implementing IFaceDetect
    /// wrapper class
    /// </summary>
    public class CameraBL : ICamera
    {
        public double FrameWidth  { get; private set; }
        public double FrameHeight { get; private set; }
        public bool IsCameraAvailable
        {
            get
            {
                return grabber != null;
            }
            private set {}
        }

        private Capture grabber;

        public CameraBL()
        {
            try
            {
                // check if all libraries were loaded
                if (DependencyCheck.Execute())
                {
                    // init grabber
                    this.grabber = new Capture();

                    // get current height and width
                    this.FrameWidth = grabber.GetCaptureProperty(CAP_PROP.CV_CAP_PROP_FRAME_WIDTH);
                    this.FrameHeight = grabber.GetCaptureProperty(CAP_PROP.CV_CAP_PROP_FRAME_HEIGHT);
                }
            }
            catch (NullReferenceException)
            {
                this.IsCameraAvailable = false;
            }
        }

        public Image QueryFrame()
        {
            try
            {
                return grabber.QueryFrame().ToBitmap();
            }
            catch
            {
                return null; 
            }
        }

        public void SetCameraResolution(double width, double height)
        {
            if (width > 0 && height > 0 && this.IsCameraAvailable == true)
            {
                try
                {
                    // set width and height
                    grabber.SetCaptureProperty(CAP_PROP.CV_CAP_PROP_FRAME_WIDTH, width);
                    grabber.SetCaptureProperty(CAP_PROP.CV_CAP_PROP_FRAME_HEIGHT, height);

                    // the desired values and the real values may differ (if camera does not support the requested resolution)
                    this.FrameWidth = (int)grabber.GetCaptureProperty(CAP_PROP.CV_CAP_PROP_FRAME_WIDTH); ;
                    this.FrameHeight = (int)grabber.GetCaptureProperty(CAP_PROP.CV_CAP_PROP_FRAME_HEIGHT); ;
                }
                catch 
                {
                    throw new CameraException("Unable to reset camera resolution!");
                }
            }
        }
    }
}


