/// FaceDetectionSettings.cs
/// 
/// This class is passed to the PropertyGrid control in FormFaceDetectionSettings
/// 
/// Authors:
/// Stefan Auer, Alexander Bliem
/// 
/// Date: 04.05.2013

using JPEG.Encryption.BL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;

namespace JPEG.Encryption.Gui
{
    /// <summary>
    /// Class that will be displayed in the PropertyGrid (advanced options for OpenCV)
    /// </summary>
    [DefaultPropertyAttribute("Name")]
    public class FaceDetectionSettings : ICloneable
    {
        // private members
        private double scale;               
        private int minNeighbors;
        private Size minSize;

        /// <summary>
        /// Constructor
        /// </summary>
        public FaceDetectionSettings(EnumMode mode = EnumMode.Online)
        {
            if (mode == EnumMode.Online)
            {
                this.scale = 1.3;
                this.minNeighbors = 5;
                this.minSize = new Size(10, 10);
            }
            else
            {
                this.scale = 1.1;
                this.minNeighbors = 5;
                this.minSize = new Size(5, 5);
            }
        }

        /// <summary>
        /// Scale property with category attribute and description attribute added
        /// </summary>
        [CategoryAttribute("OpenCV Settings"), DescriptionAttribute("Determines the how the image will be scaled before the face detection algorithm starts")]
        public double ImageScale
        {
            get
            {
                return this.scale;
            }
            set
            {
                // check user input
                if (value <= 1)
                    this.scale = 1.1;
                else
                    this.scale = value;
            }
        }

        /// <summary>
        /// Minimum number of neighbors
        /// </summary>
        [CategoryAttribute("OpenCV Settings"), DescriptionAttribute("Determine the minimum amout of required neighbors")]
        public int MinNeighbors
        {
            get
            {
                return this.minNeighbors;
            }
            set
            {
                // check user input
                if (value <= 0)
                    this.minNeighbors = 1;
                else
                    this.minNeighbors = value;
            }
        }

        /// <summary>
        /// Minimum size of detected object
        /// </summary>
        [CategoryAttribute("OpenCV Settings"), DescriptionAttribute("Minimum size of objects. Every object smaller than this value will be ignored.")]
        public Size MinSize
        {
            get
            {
                return this.minSize;
            }
            set
            {
                // check user input
                if (value.Height <= 0)
                    this.minSize.Height = 1;
                else
                    this.minSize.Height = value.Height;

                if (value.Width <= 0)
                    this.minSize.Width = 1;
                else
                    this.minSize.Width = value.Width;
            }
        }

        /// <summary>
        /// This class implements IClonable
        /// </summary>
        /// <returns>A cloned FaceDetectionSettingsObject</returns>
        public object Clone()
        {
            FaceDetectionSettings settings = new FaceDetectionSettings();
            settings.ImageScale = this.ImageScale;
            settings.minNeighbors = this.minNeighbors;
            settings.MinSize = new Size(this.MinSize.Width, this.MinSize.Height);

            return settings;
        }
    } 
}
