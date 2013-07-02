namespace JPEG.Encryption.Gui
{
    partial class FormFaceDetectionSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormFaceDetectionSettings));
            this.propertyGridFacedetectionSettings = new System.Windows.Forms.PropertyGrid();
            this.buttonSettingsOK = new System.Windows.Forms.Button();
            this.buttonSettingsCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // propertyGridFacedetectionSettings
            // 
            this.propertyGridFacedetectionSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGridFacedetectionSettings.Location = new System.Drawing.Point(12, 12);
            this.propertyGridFacedetectionSettings.Name = "propertyGridFacedetectionSettings";
            this.propertyGridFacedetectionSettings.Size = new System.Drawing.Size(260, 209);
            this.propertyGridFacedetectionSettings.TabIndex = 0;
            // 
            // buttonSettingsOK
            // 
            this.buttonSettingsOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSettingsOK.Location = new System.Drawing.Point(150, 227);
            this.buttonSettingsOK.Name = "buttonSettingsOK";
            this.buttonSettingsOK.Size = new System.Drawing.Size(125, 29);
            this.buttonSettingsOK.TabIndex = 1;
            this.buttonSettingsOK.Text = "OK";
            this.buttonSettingsOK.UseVisualStyleBackColor = true;
            this.buttonSettingsOK.Click += new System.EventHandler(this.buttonSettingsOK_Click);
            // 
            // buttonSettingsCancel
            // 
            this.buttonSettingsCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSettingsCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonSettingsCancel.Location = new System.Drawing.Point(12, 227);
            this.buttonSettingsCancel.Name = "buttonSettingsCancel";
            this.buttonSettingsCancel.Size = new System.Drawing.Size(132, 29);
            this.buttonSettingsCancel.TabIndex = 2;
            this.buttonSettingsCancel.Text = "Cancel";
            this.buttonSettingsCancel.UseVisualStyleBackColor = true;
            this.buttonSettingsCancel.Click += new System.EventHandler(this.buttonSettingsCancel_Click);
            // 
            // FormFaceDetectionSettings
            // 
            this.AcceptButton = this.buttonSettingsOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonSettingsCancel;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.buttonSettingsCancel);
            this.Controls.Add(this.buttonSettingsOK);
            this.Controls.Add(this.propertyGridFacedetectionSettings);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(300, 300);
            this.MinimumSize = new System.Drawing.Size(300, 300);
            this.Name = "FormFaceDetectionSettings";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Face detection values";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid propertyGridFacedetectionSettings;
        private System.Windows.Forms.Button buttonSettingsOK;
        private System.Windows.Forms.Button buttonSettingsCancel;
    }
}