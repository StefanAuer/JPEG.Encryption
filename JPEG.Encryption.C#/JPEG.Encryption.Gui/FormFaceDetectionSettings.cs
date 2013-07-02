/// FormFaceDetectionSettings.cs
/// 
/// With this form the user can modify advanced properties of the
/// face detection algorithms.
/// 
/// Authors:
/// Stefan Auer, Alexander Bliem
/// 
/// Date: 01.03.2013


using JPEG.Encryption.BL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace JPEG.Encryption.Gui
{
    public partial class FormFaceDetectionSettings : Form
    {
        #region private members
        
        private FaceDetectionSettings faceDetectionSettings;

        #endregion

        #region constructor

        public FormFaceDetectionSettings(EnumMode mode = EnumMode.Online)
        {
            InitializeComponent();
            this.faceDetectionSettings = new FaceDetectionSettings(mode);
            propertyGridFacedetectionSettings.SelectedObject = this.faceDetectionSettings;
        }

        #endregion

        #region public methods

        public FaceDetectionSettings GetFaceDetectionSettings()
        {
            return this.faceDetectionSettings;
        }

        public DialogResult ShowAsDialog(FaceDetectionSettings faceDetectionSettings)
        {
            this.faceDetectionSettings = faceDetectionSettings;
            this.propertyGridFacedetectionSettings.SelectedObject = faceDetectionSettings;

            return this.ShowDialog();
        }

        private void buttonSettingsOK_Click(object sender, EventArgs e)
        {
            FaceDetectionSettings settings = (FaceDetectionSettings)propertyGridFacedetectionSettings.SelectedObject;
            this.faceDetectionSettings = settings;

            // min. scale has to be > 1
            if (this.faceDetectionSettings.ImageScale <= 1)
                this.faceDetectionSettings.ImageScale = 1.1;

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        #endregion

        private void buttonSettingsCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }


    }
}
