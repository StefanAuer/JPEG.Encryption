namespace JPEG.Encryption.Gui
{
    partial class FormMain
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.tabControljpeg = new System.Windows.Forms.TabControl();
            this.tabPageFile = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonLoadJpg = new System.Windows.Forms.Button();
            this.treeViewImages = new System.Windows.Forms.TreeView();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.pictureBoxPreview = new System.Windows.Forms.PictureBox();
            this.contextMenuStripJpegFile = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.saveImageAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetRoIsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBoxSetRegionOfInterest = new System.Windows.Forms.GroupBox();
            this.buttonAdvancedOpenCVSettings = new System.Windows.Forms.Button();
            this.labelOpenCvAdvancedSettings = new System.Windows.Forms.Label();
            this.buttonDetectFaces = new System.Windows.Forms.Button();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.labelStatus = new System.Windows.Forms.Label();
            this.labelOpenCValgorithm = new System.Windows.Forms.Label();
            this.comboBoxOpenCV = new System.Windows.Forms.ComboBox();
            this.radioButtonGetRoIfromOpenCV = new System.Windows.Forms.RadioButton();
            this.radioButtonGetRoIFromMouse = new System.Windows.Forms.RadioButton();
            this.buttonEncrypt = new System.Windows.Forms.Button();
            this.buttonDecrypt = new System.Windows.Forms.Button();
            this.checkBoxSelectAll = new System.Windows.Forms.CheckBox();
            this.tabPageCamera = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBoxVideo = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonAdvancedSettingsOpenCVCamera = new System.Windows.Forms.Button();
            this.labelAdvancedSettings = new System.Windows.Forms.Label();
            this.checkBoxEncryptSwapValues = new System.Windows.Forms.CheckBox();
            this.checkBoxEncryptSwapCWVPair = new System.Windows.Forms.CheckBox();
            this.checkBoxEncryptSwapBlocks = new System.Windows.Forms.CheckBox();
            this.checkBoxEncryptDcCoefficients = new System.Windows.Forms.CheckBox();
            this.comboBoxFaceDetectionAlgorithm = new System.Windows.Forms.ComboBox();
            this.labelSelectFaceDetection = new System.Windows.Forms.Label();
            this.textBoxPasswordVideo = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBoxShowRoI = new System.Windows.Forms.CheckBox();
            this.radioButtonShowDecryptedFrame = new System.Windows.Forms.RadioButton();
            this.radioButtonShowEncryptedFrame = new System.Windows.Forms.RadioButton();
            this.radioButtonShowOriginalFrame = new System.Windows.Forms.RadioButton();
            this.labelCameraFrameRate = new System.Windows.Forms.Label();
            this.labelCameraResolution = new System.Windows.Forms.Label();
            this.labelScreenResution = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBoxCameraResolutions = new System.Windows.Forms.ComboBox();
            this.tabPageRaspberry = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelRaspberry = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBoxRaspberry = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.maskedTextBoxIP1 = new System.Windows.Forms.MaskedTextBox();
            this.maskedTextBoxIP2 = new System.Windows.Forms.MaskedTextBox();
            this.maskedTextBoxIP3 = new System.Windows.Forms.MaskedTextBox();
            this.maskedTextBoxIP4 = new System.Windows.Forms.MaskedTextBox();
            this.labelPiStatus = new System.Windows.Forms.Label();
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.tabControljpeg.SuspendLayout();
            this.tabPageFile.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPreview)).BeginInit();
            this.contextMenuStripJpegFile.SuspendLayout();
            this.groupBoxSetRegionOfInterest.SuspendLayout();
            this.tabPageCamera.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxVideo)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.tabPageRaspberry.SuspendLayout();
            this.tableLayoutPanelRaspberry.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRaspberry)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControljpeg
            // 
            this.tabControljpeg.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControljpeg.Controls.Add(this.tabPageFile);
            this.tabControljpeg.Controls.Add(this.tabPageCamera);
            this.tabControljpeg.Controls.Add(this.tabPageRaspberry);
            this.tabControljpeg.Location = new System.Drawing.Point(18, 23);
            this.tabControljpeg.Margin = new System.Windows.Forms.Padding(4);
            this.tabControljpeg.Name = "tabControljpeg";
            this.tabControljpeg.SelectedIndex = 0;
            this.tabControljpeg.Size = new System.Drawing.Size(967, 634);
            this.tabControljpeg.TabIndex = 0;
            this.tabControljpeg.SelectedIndexChanged += new System.EventHandler(this.tabControljpeg_SelectedIndexChanged);
            // 
            // tabPageFile
            // 
            this.tabPageFile.Controls.Add(this.tableLayoutPanel1);
            this.tabPageFile.Location = new System.Drawing.Point(4, 27);
            this.tabPageFile.Margin = new System.Windows.Forms.Padding(4);
            this.tabPageFile.Name = "tabPageFile";
            this.tabPageFile.Padding = new System.Windows.Forms.Padding(4);
            this.tabPageFile.Size = new System.Drawing.Size(959, 603);
            this.tabPageFile.TabIndex = 0;
            this.tabPageFile.Text = "JPEG Browser";
            this.tabPageFile.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 44.21053F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55.78947F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 198F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 480F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Controls.Add(this.buttonLoadJpg, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.treeViewImages, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.pictureBoxPreview, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBoxSetRegionOfInterest, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.buttonEncrypt, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.buttonDecrypt, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.checkBoxSelectAll, 0, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(9, 7);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 75.79365F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 24.20635F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 53F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(937, 577);
            this.tableLayoutPanel1.TabIndex = 9;
            // 
            // buttonLoadJpg
            // 
            this.buttonLoadJpg.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonLoadJpg.Location = new System.Drawing.Point(4, 527);
            this.buttonLoadJpg.Margin = new System.Windows.Forms.Padding(4);
            this.buttonLoadJpg.Name = "buttonLoadJpg";
            this.buttonLoadJpg.Size = new System.Drawing.Size(106, 46);
            this.buttonLoadJpg.TabIndex = 25;
            this.buttonLoadJpg.Text = "load JPEG from file";
            this.buttonLoadJpg.UseVisualStyleBackColor = true;
            this.buttonLoadJpg.Click += new System.EventHandler(this.buttonLoadJpg_Click);
            // 
            // treeViewImages
            // 
            this.treeViewImages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewImages.CheckBoxes = true;
            this.tableLayoutPanel1.SetColumnSpan(this.treeViewImages, 3);
            this.treeViewImages.HideSelection = false;
            this.treeViewImages.ImageIndex = 0;
            this.treeViewImages.ImageList = this.imageList;
            this.treeViewImages.Location = new System.Drawing.Point(4, 4);
            this.treeViewImages.Margin = new System.Windows.Forms.Padding(4);
            this.treeViewImages.Name = "treeViewImages";
            this.tableLayoutPanel1.SetRowSpan(this.treeViewImages, 2);
            this.treeViewImages.SelectedImageIndex = 1;
            this.treeViewImages.Size = new System.Drawing.Size(448, 486);
            this.treeViewImages.TabIndex = 19;
            this.treeViewImages.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewImages_AfterSelect);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "picture.png");
            this.imageList.Images.SetKeyName(1, "pictureKey.png");
            // 
            // pictureBoxPreview
            // 
            this.pictureBoxPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxPreview.ContextMenuStrip = this.contextMenuStripJpegFile;
            this.pictureBoxPreview.Location = new System.Drawing.Point(501, 4);
            this.pictureBoxPreview.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBoxPreview.MaximumSize = new System.Drawing.Size(432, 370);
            this.pictureBoxPreview.MinimumSize = new System.Drawing.Size(432, 370);
            this.pictureBoxPreview.Name = "pictureBoxPreview";
            this.pictureBoxPreview.Size = new System.Drawing.Size(432, 370);
            this.pictureBoxPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxPreview.TabIndex = 16;
            this.pictureBoxPreview.TabStop = false;
            this.pictureBoxPreview.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBoxPreview_MouseDown);
            this.pictureBoxPreview.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBoxPreview_MouseMove);
            this.pictureBoxPreview.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBoxPreview_MouseUp);
            // 
            // contextMenuStripJpegFile
            // 
            this.contextMenuStripJpegFile.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveImageAsToolStripMenuItem,
            this.resetRoIsToolStripMenuItem});
            this.contextMenuStripJpegFile.Name = "contextMenuStripJpegFile";
            this.contextMenuStripJpegFile.Size = new System.Drawing.Size(163, 48);
            this.contextMenuStripJpegFile.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStripJpegFile_Opening);
            // 
            // saveImageAsToolStripMenuItem
            // 
            this.saveImageAsToolStripMenuItem.Image = global::JPEG.Encryption.Gui.Properties.Resources.Save_icon;
            this.saveImageAsToolStripMenuItem.Name = "saveImageAsToolStripMenuItem";
            this.saveImageAsToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.saveImageAsToolStripMenuItem.Text = "Save Image As ...";
            this.saveImageAsToolStripMenuItem.Click += new System.EventHandler(this.saveImageAsToolStripMenuItem_Click);
            // 
            // resetRoIsToolStripMenuItem
            // 
            this.resetRoIsToolStripMenuItem.Image = global::JPEG.Encryption.Gui.Properties.Resources.Refresh;
            this.resetRoIsToolStripMenuItem.Name = "resetRoIsToolStripMenuItem";
            this.resetRoIsToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.resetRoIsToolStripMenuItem.Text = "Reset RoI(s)";
            this.resetRoIsToolStripMenuItem.Click += new System.EventHandler(this.resetRoIsToolStripMenuItem1_Click);
            // 
            // groupBoxSetRegionOfInterest
            // 
            this.groupBoxSetRegionOfInterest.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxSetRegionOfInterest.Controls.Add(this.buttonAdvancedOpenCVSettings);
            this.groupBoxSetRegionOfInterest.Controls.Add(this.labelOpenCvAdvancedSettings);
            this.groupBoxSetRegionOfInterest.Controls.Add(this.buttonDetectFaces);
            this.groupBoxSetRegionOfInterest.Controls.Add(this.textBoxPassword);
            this.groupBoxSetRegionOfInterest.Controls.Add(this.label3);
            this.groupBoxSetRegionOfInterest.Controls.Add(this.labelStatus);
            this.groupBoxSetRegionOfInterest.Controls.Add(this.labelOpenCValgorithm);
            this.groupBoxSetRegionOfInterest.Controls.Add(this.comboBoxOpenCV);
            this.groupBoxSetRegionOfInterest.Controls.Add(this.radioButtonGetRoIfromOpenCV);
            this.groupBoxSetRegionOfInterest.Controls.Add(this.radioButtonGetRoIFromMouse);
            this.groupBoxSetRegionOfInterest.Location = new System.Drawing.Point(500, 378);
            this.groupBoxSetRegionOfInterest.Name = "groupBoxSetRegionOfInterest";
            this.tableLayoutPanel1.SetRowSpan(this.groupBoxSetRegionOfInterest, 3);
            this.groupBoxSetRegionOfInterest.Size = new System.Drawing.Size(434, 196);
            this.groupBoxSetRegionOfInterest.TabIndex = 20;
            this.groupBoxSetRegionOfInterest.TabStop = false;
            this.groupBoxSetRegionOfInterest.Text = "Options:";
            // 
            // buttonAdvancedOpenCVSettings
            // 
            this.buttonAdvancedOpenCVSettings.Enabled = false;
            this.buttonAdvancedOpenCVSettings.Location = new System.Drawing.Point(186, 155);
            this.buttonAdvancedOpenCVSettings.Name = "buttonAdvancedOpenCVSettings";
            this.buttonAdvancedOpenCVSettings.Size = new System.Drawing.Size(43, 23);
            this.buttonAdvancedOpenCVSettings.TabIndex = 10;
            this.buttonAdvancedOpenCVSettings.Text = "...";
            this.buttonAdvancedOpenCVSettings.UseVisualStyleBackColor = true;
            this.buttonAdvancedOpenCVSettings.Click += new System.EventHandler(this.buttonAdvancedOpenCVSettings_Click);
            // 
            // labelOpenCvAdvancedSettings
            // 
            this.labelOpenCvAdvancedSettings.AutoSize = true;
            this.labelOpenCvAdvancedSettings.Enabled = false;
            this.labelOpenCvAdvancedSettings.Location = new System.Drawing.Point(37, 156);
            this.labelOpenCvAdvancedSettings.Name = "labelOpenCvAdvancedSettings";
            this.labelOpenCvAdvancedSettings.Size = new System.Drawing.Size(150, 18);
            this.labelOpenCvAdvancedSettings.TabIndex = 9;
            this.labelOpenCvAdvancedSettings.Text = "Advanced settings:";
            // 
            // buttonDetectFaces
            // 
            this.buttonDetectFaces.Enabled = false;
            this.buttonDetectFaces.Location = new System.Drawing.Point(359, 124);
            this.buttonDetectFaces.Name = "buttonDetectFaces";
            this.buttonDetectFaces.Size = new System.Drawing.Size(52, 27);
            this.buttonDetectFaces.TabIndex = 8;
            this.buttonDetectFaces.Text = "go!";
            this.buttonDetectFaces.UseVisualStyleBackColor = true;
            this.buttonDetectFaces.Click += new System.EventHandler(this.buttonDetectFaces_Click);
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Location = new System.Drawing.Point(142, 28);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.Size = new System.Drawing.Size(258, 26);
            this.textBoxPassword.TabIndex = 7;
            this.textBoxPassword.Text = "privacy";
            this.textBoxPassword.TextChanged += new System.EventHandler(this.textBoxPassword_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 31);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(117, 18);
            this.label3.TabIndex = 6;
            this.label3.Text = "Set password:";
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Location = new System.Drawing.Point(121, 179);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(149, 18);
            this.labelStatus.TabIndex = 4;
            this.labelStatus.Text = "[detecting faces...]";
            this.labelStatus.Visible = false;
            // 
            // labelOpenCValgorithm
            // 
            this.labelOpenCValgorithm.AutoSize = true;
            this.labelOpenCValgorithm.Enabled = false;
            this.labelOpenCValgorithm.Location = new System.Drawing.Point(37, 129);
            this.labelOpenCValgorithm.Name = "labelOpenCValgorithm";
            this.labelOpenCValgorithm.Size = new System.Drawing.Size(147, 18);
            this.labelOpenCValgorithm.TabIndex = 3;
            this.labelOpenCValgorithm.Text = "Choose algorithm:";
            // 
            // comboBoxOpenCV
            // 
            this.comboBoxOpenCV.Enabled = false;
            this.comboBoxOpenCV.FormattingEnabled = true;
            this.comboBoxOpenCV.Location = new System.Drawing.Point(186, 125);
            this.comboBoxOpenCV.Name = "comboBoxOpenCV";
            this.comboBoxOpenCV.Size = new System.Drawing.Size(171, 26);
            this.comboBoxOpenCV.TabIndex = 2;
            // 
            // radioButtonGetRoIfromOpenCV
            // 
            this.radioButtonGetRoIfromOpenCV.AutoSize = true;
            this.radioButtonGetRoIfromOpenCV.Location = new System.Drawing.Point(19, 101);
            this.radioButtonGetRoIfromOpenCV.Name = "radioButtonGetRoIfromOpenCV";
            this.radioButtonGetRoIfromOpenCV.Size = new System.Drawing.Size(211, 22);
            this.radioButtonGetRoIfromOpenCV.TabIndex = 1;
            this.radioButtonGetRoIfromOpenCV.Text = "Get RoI(s) from OpenCV";
            this.radioButtonGetRoIfromOpenCV.UseVisualStyleBackColor = true;
            this.radioButtonGetRoIfromOpenCV.CheckedChanged += new System.EventHandler(this.radioButtonGetRoIfromOpenCV_CheckedChanged);
            // 
            // radioButtonGetRoIFromMouse
            // 
            this.radioButtonGetRoIFromMouse.AutoSize = true;
            this.radioButtonGetRoIFromMouse.Checked = true;
            this.radioButtonGetRoIFromMouse.Location = new System.Drawing.Point(19, 71);
            this.radioButtonGetRoIFromMouse.Name = "radioButtonGetRoIFromMouse";
            this.radioButtonGetRoIFromMouse.Size = new System.Drawing.Size(216, 22);
            this.radioButtonGetRoIFromMouse.TabIndex = 0;
            this.radioButtonGetRoIFromMouse.TabStop = true;
            this.radioButtonGetRoIFromMouse.Text = "Draw RoI(s) using mouse";
            this.radioButtonGetRoIFromMouse.UseVisualStyleBackColor = true;
            this.radioButtonGetRoIFromMouse.CheckedChanged += new System.EventHandler(this.radioButtonGetRoIFromMouse_CheckedChanged);
            // 
            // buttonEncrypt
            // 
            this.buttonEncrypt.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonEncrypt.ImageIndex = 1;
            this.buttonEncrypt.ImageList = this.imageList;
            this.buttonEncrypt.Location = new System.Drawing.Point(118, 527);
            this.buttonEncrypt.Margin = new System.Windows.Forms.Padding(4);
            this.buttonEncrypt.Name = "buttonEncrypt";
            this.buttonEncrypt.Size = new System.Drawing.Size(136, 46);
            this.buttonEncrypt.TabIndex = 24;
            this.buttonEncrypt.Text = "encrypt selected";
            this.buttonEncrypt.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonEncrypt.UseVisualStyleBackColor = true;
            this.buttonEncrypt.Click += new System.EventHandler(this.buttonEncrypt_Click);
            // 
            // buttonDecrypt
            // 
            this.buttonDecrypt.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDecrypt.ImageIndex = 0;
            this.buttonDecrypt.ImageList = this.imageList;
            this.buttonDecrypt.Location = new System.Drawing.Point(262, 527);
            this.buttonDecrypt.Margin = new System.Windows.Forms.Padding(4);
            this.buttonDecrypt.Name = "buttonDecrypt";
            this.buttonDecrypt.Size = new System.Drawing.Size(190, 46);
            this.buttonDecrypt.TabIndex = 22;
            this.buttonDecrypt.Text = "decrypt selected";
            this.buttonDecrypt.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonDecrypt.UseVisualStyleBackColor = true;
            this.buttonDecrypt.Click += new System.EventHandler(this.buttonDecrypt_Click);
            // 
            // checkBoxSelectAll
            // 
            this.checkBoxSelectAll.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.checkBoxSelectAll, 2);
            this.checkBoxSelectAll.Location = new System.Drawing.Point(3, 497);
            this.checkBoxSelectAll.Name = "checkBoxSelectAll";
            this.checkBoxSelectAll.Size = new System.Drawing.Size(90, 22);
            this.checkBoxSelectAll.TabIndex = 26;
            this.checkBoxSelectAll.Text = "select all";
            this.checkBoxSelectAll.UseVisualStyleBackColor = true;
            this.checkBoxSelectAll.CheckedChanged += new System.EventHandler(this.checkBoxSelectAll_CheckedChanged);
            // 
            // tabPageCamera
            // 
            this.tabPageCamera.Controls.Add(this.tableLayoutPanel2);
            this.tabPageCamera.Location = new System.Drawing.Point(4, 27);
            this.tabPageCamera.Margin = new System.Windows.Forms.Padding(4);
            this.tabPageCamera.Name = "tabPageCamera";
            this.tabPageCamera.Size = new System.Drawing.Size(959, 603);
            this.tabPageCamera.TabIndex = 2;
            this.tabPageCamera.Text = "JPEG from camera";
            this.tabPageCamera.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 29.07479F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.93387F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.34025F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.88382F));
            this.tableLayoutPanel2.Controls.Add(this.pictureBoxVideo, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.groupBox1, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.labelCameraFrameRate, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.labelCameraResolution, 2, 2);
            this.tableLayoutPanel2.Controls.Add(this.labelScreenResution, 2, 3);
            this.tableLayoutPanel2.Controls.Add(this.label4, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.comboBoxCameraResolutions, 1, 2);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 86.65481F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.914591F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4.626335F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4.626335F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(959, 603);
            this.tableLayoutPanel2.TabIndex = 6;
            // 
            // pictureBoxVideo
            // 
            this.pictureBoxVideo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.SetColumnSpan(this.pictureBoxVideo, 3);
            this.pictureBoxVideo.Location = new System.Drawing.Point(3, 3);
            this.pictureBoxVideo.Name = "pictureBoxVideo";
            this.pictureBoxVideo.Size = new System.Drawing.Size(637, 517);
            this.pictureBoxVideo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxVideo.TabIndex = 23;
            this.pictureBoxVideo.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.buttonAdvancedSettingsOpenCVCamera);
            this.groupBox1.Controls.Add(this.labelAdvancedSettings);
            this.groupBox1.Controls.Add(this.checkBoxEncryptSwapValues);
            this.groupBox1.Controls.Add(this.checkBoxEncryptSwapCWVPair);
            this.groupBox1.Controls.Add(this.checkBoxEncryptSwapBlocks);
            this.groupBox1.Controls.Add(this.checkBoxEncryptDcCoefficients);
            this.groupBox1.Controls.Add(this.comboBoxFaceDetectionAlgorithm);
            this.groupBox1.Controls.Add(this.labelSelectFaceDetection);
            this.groupBox1.Controls.Add(this.textBoxPasswordVideo);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.checkBoxShowRoI);
            this.groupBox1.Controls.Add(this.radioButtonShowDecryptedFrame);
            this.groupBox1.Controls.Add(this.radioButtonShowEncryptedFrame);
            this.groupBox1.Controls.Add(this.radioButtonShowOriginalFrame);
            this.groupBox1.Location = new System.Drawing.Point(651, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(305, 472);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Options:";
            // 
            // buttonAdvancedSettingsOpenCVCamera
            // 
            this.buttonAdvancedSettingsOpenCVCamera.Location = new System.Drawing.Point(177, 379);
            this.buttonAdvancedSettingsOpenCVCamera.Name = "buttonAdvancedSettingsOpenCVCamera";
            this.buttonAdvancedSettingsOpenCVCamera.Size = new System.Drawing.Size(43, 23);
            this.buttonAdvancedSettingsOpenCVCamera.TabIndex = 13;
            this.buttonAdvancedSettingsOpenCVCamera.Text = "...";
            this.buttonAdvancedSettingsOpenCVCamera.UseVisualStyleBackColor = true;
            this.buttonAdvancedSettingsOpenCVCamera.Click += new System.EventHandler(this.buttonAdvancedSettingsOpenCVCamera_Click);
            // 
            // labelAdvancedSettings
            // 
            this.labelAdvancedSettings.AutoSize = true;
            this.labelAdvancedSettings.Location = new System.Drawing.Point(26, 381);
            this.labelAdvancedSettings.Name = "labelAdvancedSettings";
            this.labelAdvancedSettings.Size = new System.Drawing.Size(150, 18);
            this.labelAdvancedSettings.TabIndex = 12;
            this.labelAdvancedSettings.Text = "Advanced settings:";
            // 
            // checkBoxEncryptSwapValues
            // 
            this.checkBoxEncryptSwapValues.AutoSize = true;
            this.checkBoxEncryptSwapValues.Checked = true;
            this.checkBoxEncryptSwapValues.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxEncryptSwapValues.Enabled = false;
            this.checkBoxEncryptSwapValues.Location = new System.Drawing.Point(52, 165);
            this.checkBoxEncryptSwapValues.Name = "checkBoxEncryptSwapValues";
            this.checkBoxEncryptSwapValues.Size = new System.Drawing.Size(120, 22);
            this.checkBoxEncryptSwapValues.TabIndex = 11;
            this.checkBoxEncryptSwapValues.Text = "Swap values";
            this.checkBoxEncryptSwapValues.UseVisualStyleBackColor = true;
            // 
            // checkBoxEncryptSwapCWVPair
            // 
            this.checkBoxEncryptSwapCWVPair.AutoSize = true;
            this.checkBoxEncryptSwapCWVPair.Checked = true;
            this.checkBoxEncryptSwapCWVPair.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxEncryptSwapCWVPair.Enabled = false;
            this.checkBoxEncryptSwapCWVPair.Location = new System.Drawing.Point(52, 137);
            this.checkBoxEncryptSwapCWVPair.Name = "checkBoxEncryptSwapCWVPair";
            this.checkBoxEncryptSwapCWVPair.Size = new System.Drawing.Size(141, 22);
            this.checkBoxEncryptSwapCWVPair.TabIndex = 10;
            this.checkBoxEncryptSwapCWVPair.Text = "Swap CWV pair";
            this.checkBoxEncryptSwapCWVPair.UseVisualStyleBackColor = true;
            // 
            // checkBoxEncryptSwapBlocks
            // 
            this.checkBoxEncryptSwapBlocks.AutoSize = true;
            this.checkBoxEncryptSwapBlocks.Checked = true;
            this.checkBoxEncryptSwapBlocks.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxEncryptSwapBlocks.Enabled = false;
            this.checkBoxEncryptSwapBlocks.Location = new System.Drawing.Point(52, 109);
            this.checkBoxEncryptSwapBlocks.Name = "checkBoxEncryptSwapBlocks";
            this.checkBoxEncryptSwapBlocks.Size = new System.Drawing.Size(120, 22);
            this.checkBoxEncryptSwapBlocks.TabIndex = 9;
            this.checkBoxEncryptSwapBlocks.Text = "Swap blocks";
            this.checkBoxEncryptSwapBlocks.UseVisualStyleBackColor = true;
            // 
            // checkBoxEncryptDcCoefficients
            // 
            this.checkBoxEncryptDcCoefficients.AutoSize = true;
            this.checkBoxEncryptDcCoefficients.Checked = true;
            this.checkBoxEncryptDcCoefficients.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxEncryptDcCoefficients.Enabled = false;
            this.checkBoxEncryptDcCoefficients.Location = new System.Drawing.Point(52, 81);
            this.checkBoxEncryptDcCoefficients.Name = "checkBoxEncryptDcCoefficients";
            this.checkBoxEncryptDcCoefficients.Size = new System.Drawing.Size(198, 22);
            this.checkBoxEncryptDcCoefficients.TabIndex = 8;
            this.checkBoxEncryptDcCoefficients.Text = "Encrypt DC coefficients";
            this.checkBoxEncryptDcCoefficients.UseVisualStyleBackColor = true;
            // 
            // comboBoxFaceDetectionAlgorithm
            // 
            this.comboBoxFaceDetectionAlgorithm.FormattingEnabled = true;
            this.comboBoxFaceDetectionAlgorithm.Location = new System.Drawing.Point(25, 335);
            this.comboBoxFaceDetectionAlgorithm.Name = "comboBoxFaceDetectionAlgorithm";
            this.comboBoxFaceDetectionAlgorithm.Size = new System.Drawing.Size(253, 26);
            this.comboBoxFaceDetectionAlgorithm.TabIndex = 7;
            // 
            // labelSelectFaceDetection
            // 
            this.labelSelectFaceDetection.AutoSize = true;
            this.labelSelectFaceDetection.Location = new System.Drawing.Point(21, 312);
            this.labelSelectFaceDetection.Name = "labelSelectFaceDetection";
            this.labelSelectFaceDetection.Size = new System.Drawing.Size(245, 18);
            this.labelSelectFaceDetection.TabIndex = 6;
            this.labelSelectFaceDetection.Text = "Select face detection algorithm:";
            // 
            // textBoxPasswordVideo
            // 
            this.textBoxPasswordVideo.Location = new System.Drawing.Point(24, 273);
            this.textBoxPasswordVideo.Name = "textBoxPasswordVideo";
            this.textBoxPasswordVideo.Size = new System.Drawing.Size(258, 26);
            this.textBoxPasswordVideo.TabIndex = 5;
            this.textBoxPasswordVideo.Text = "privacy";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 252);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(117, 18);
            this.label1.TabIndex = 4;
            this.label1.Text = "Set password:";
            // 
            // checkBoxShowRoI
            // 
            this.checkBoxShowRoI.AutoSize = true;
            this.checkBoxShowRoI.Checked = true;
            this.checkBoxShowRoI.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxShowRoI.Location = new System.Drawing.Point(25, 219);
            this.checkBoxShowRoI.Name = "checkBoxShowRoI";
            this.checkBoxShowRoI.Size = new System.Drawing.Size(121, 22);
            this.checkBoxShowRoI.TabIndex = 3;
            this.checkBoxShowRoI.Text = "Show RoI(s)";
            this.checkBoxShowRoI.UseVisualStyleBackColor = true;
            // 
            // radioButtonShowDecryptedFrame
            // 
            this.radioButtonShowDecryptedFrame.AutoSize = true;
            this.radioButtonShowDecryptedFrame.Location = new System.Drawing.Point(25, 191);
            this.radioButtonShowDecryptedFrame.Name = "radioButtonShowDecryptedFrame";
            this.radioButtonShowDecryptedFrame.Size = new System.Drawing.Size(196, 22);
            this.radioButtonShowDecryptedFrame.TabIndex = 2;
            this.radioButtonShowDecryptedFrame.Text = "Show decrypted frame";
            this.radioButtonShowDecryptedFrame.UseVisualStyleBackColor = true;
            // 
            // radioButtonShowEncryptedFrame
            // 
            this.radioButtonShowEncryptedFrame.AutoSize = true;
            this.radioButtonShowEncryptedFrame.Location = new System.Drawing.Point(24, 53);
            this.radioButtonShowEncryptedFrame.Name = "radioButtonShowEncryptedFrame";
            this.radioButtonShowEncryptedFrame.Size = new System.Drawing.Size(196, 22);
            this.radioButtonShowEncryptedFrame.TabIndex = 1;
            this.radioButtonShowEncryptedFrame.Text = "Show encrypted frame";
            this.radioButtonShowEncryptedFrame.UseVisualStyleBackColor = true;
            this.radioButtonShowEncryptedFrame.CheckedChanged += new System.EventHandler(this.radioButtonShowEncryptedFrame_CheckedChanged);
            // 
            // radioButtonShowOriginalFrame
            // 
            this.radioButtonShowOriginalFrame.AutoSize = true;
            this.radioButtonShowOriginalFrame.Checked = true;
            this.radioButtonShowOriginalFrame.Location = new System.Drawing.Point(24, 25);
            this.radioButtonShowOriginalFrame.Name = "radioButtonShowOriginalFrame";
            this.radioButtonShowOriginalFrame.Size = new System.Drawing.Size(174, 22);
            this.radioButtonShowOriginalFrame.TabIndex = 0;
            this.radioButtonShowOriginalFrame.TabStop = true;
            this.radioButtonShowOriginalFrame.Text = "Show original frame";
            this.radioButtonShowOriginalFrame.UseVisualStyleBackColor = true;
            // 
            // labelCameraFrameRate
            // 
            this.labelCameraFrameRate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelCameraFrameRate.AutoSize = true;
            this.tableLayoutPanel2.SetColumnSpan(this.labelCameraFrameRate, 2);
            this.labelCameraFrameRate.Location = new System.Drawing.Point(423, 528);
            this.labelCameraFrameRate.Name = "labelCameraFrameRate";
            this.labelCameraFrameRate.Size = new System.Drawing.Size(197, 18);
            this.labelCameraFrameRate.TabIndex = 7;
            this.labelCameraFrameRate.Text = "Current frame rate: 0 fps";
            // 
            // labelCameraResolution
            // 
            this.labelCameraResolution.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelCameraResolution.AutoSize = true;
            this.tableLayoutPanel2.SetColumnSpan(this.labelCameraResolution, 2);
            this.labelCameraResolution.Location = new System.Drawing.Point(423, 550);
            this.labelCameraResolution.Name = "labelCameraResolution";
            this.labelCameraResolution.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.labelCameraResolution.Size = new System.Drawing.Size(192, 23);
            this.labelCameraResolution.TabIndex = 24;
            this.labelCameraResolution.Text = "Camera resolution: WxH";
            // 
            // labelScreenResution
            // 
            this.labelScreenResution.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelScreenResution.AutoSize = true;
            this.tableLayoutPanel2.SetColumnSpan(this.labelScreenResution, 2);
            this.labelScreenResution.Location = new System.Drawing.Point(423, 585);
            this.labelScreenResution.Name = "labelScreenResution";
            this.labelScreenResution.Size = new System.Drawing.Size(184, 18);
            this.labelScreenResution.TabIndex = 25;
            this.labelScreenResution.Text = "Screen resolution: HxW";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(61, 550);
            this.label4.Name = "label4";
            this.label4.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.label4.Size = new System.Drawing.Size(214, 23);
            this.label4.TabIndex = 26;
            this.label4.Text = "Desired camera resolution: ";
            // 
            // comboBoxCameraResolutions
            // 
            this.comboBoxCameraResolutions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comboBoxCameraResolutions.FormattingEnabled = true;
            this.comboBoxCameraResolutions.Items.AddRange(new object[] {
            "320x240",
            "640x480",
            "800x600",
            "1024x768",
            "1280x720",
            "1920x1080"});
            this.comboBoxCameraResolutions.Location = new System.Drawing.Point(281, 549);
            this.comboBoxCameraResolutions.Name = "comboBoxCameraResolutions";
            this.comboBoxCameraResolutions.Size = new System.Drawing.Size(101, 26);
            this.comboBoxCameraResolutions.TabIndex = 27;
            this.comboBoxCameraResolutions.SelectedIndexChanged += new System.EventHandler(this.comboBoxCameraResolutions_SelectedIndexChanged);
            // 
            // tabPageRaspberry
            // 
            this.tabPageRaspberry.Controls.Add(this.tableLayoutPanelRaspberry);
            this.tabPageRaspberry.Location = new System.Drawing.Point(4, 27);
            this.tabPageRaspberry.Name = "tabPageRaspberry";
            this.tabPageRaspberry.Size = new System.Drawing.Size(959, 603);
            this.tabPageRaspberry.TabIndex = 3;
            this.tabPageRaspberry.Text = "JPEG from Raspberry Pi";
            this.tabPageRaspberry.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelRaspberry
            // 
            this.tableLayoutPanelRaspberry.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanelRaspberry.ColumnCount = 6;
            this.tableLayoutPanelRaspberry.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelRaspberry.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tableLayoutPanelRaspberry.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tableLayoutPanelRaspberry.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tableLayoutPanelRaspberry.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tableLayoutPanelRaspberry.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tableLayoutPanelRaspberry.Controls.Add(this.pictureBoxRaspberry, 0, 0);
            this.tableLayoutPanelRaspberry.Controls.Add(this.label5, 1, 0);
            this.tableLayoutPanelRaspberry.Controls.Add(this.maskedTextBoxIP1, 1, 1);
            this.tableLayoutPanelRaspberry.Controls.Add(this.maskedTextBoxIP2, 2, 1);
            this.tableLayoutPanelRaspberry.Controls.Add(this.maskedTextBoxIP3, 3, 1);
            this.tableLayoutPanelRaspberry.Controls.Add(this.maskedTextBoxIP4, 4, 1);
            this.tableLayoutPanelRaspberry.Controls.Add(this.labelPiStatus, 1, 2);
            this.tableLayoutPanelRaspberry.Controls.Add(this.buttonRefresh, 5, 1);
            this.tableLayoutPanelRaspberry.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanelRaspberry.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelRaspberry.Name = "tableLayoutPanelRaspberry";
            this.tableLayoutPanelRaspberry.RowCount = 4;
            this.tableLayoutPanelRaspberry.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 32.05128F));
            this.tableLayoutPanelRaspberry.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 67.94872F));
            this.tableLayoutPanelRaspberry.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 429F));
            this.tableLayoutPanelRaspberry.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelRaspberry.Size = new System.Drawing.Size(953, 597);
            this.tableLayoutPanelRaspberry.TabIndex = 1;
            // 
            // pictureBoxRaspberry
            // 
            this.pictureBoxRaspberry.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxRaspberry.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxRaspberry.Location = new System.Drawing.Point(3, 3);
            this.pictureBoxRaspberry.Name = "pictureBoxRaspberry";
            this.tableLayoutPanelRaspberry.SetRowSpan(this.pictureBoxRaspberry, 3);
            this.pictureBoxRaspberry.Size = new System.Drawing.Size(672, 570);
            this.pictureBoxRaspberry.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxRaspberry.TabIndex = 1;
            this.pictureBoxRaspberry.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.tableLayoutPanelRaspberry.SetColumnSpan(this.label5, 4);
            this.label5.Location = new System.Drawing.Point(681, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(207, 18);
            this.label5.TabIndex = 7;
            this.label5.Text = "Please enter IPv4 address:";
            // 
            // maskedTextBoxIP1
            // 
            this.maskedTextBoxIP1.AsciiOnly = true;
            this.maskedTextBoxIP1.Location = new System.Drawing.Point(681, 50);
            this.maskedTextBoxIP1.Mask = "000";
            this.maskedTextBoxIP1.Name = "maskedTextBoxIP1";
            this.maskedTextBoxIP1.Size = new System.Drawing.Size(49, 26);
            this.maskedTextBoxIP1.TabIndex = 8;
            this.maskedTextBoxIP1.Text = "192";
            this.maskedTextBoxIP1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.maskedTextBoxIP1.TextChanged += new System.EventHandler(this.maskedTextBoxIP1_TextChanged);
            this.maskedTextBoxIP1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.maskedTextBoxIP1_KeyUp);
            // 
            // maskedTextBoxIP2
            // 
            this.maskedTextBoxIP2.Location = new System.Drawing.Point(736, 50);
            this.maskedTextBoxIP2.Mask = "000";
            this.maskedTextBoxIP2.Name = "maskedTextBoxIP2";
            this.maskedTextBoxIP2.Size = new System.Drawing.Size(49, 26);
            this.maskedTextBoxIP2.TabIndex = 9;
            this.maskedTextBoxIP2.Text = "168";
            this.maskedTextBoxIP2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.maskedTextBoxIP2.TextChanged += new System.EventHandler(this.maskedTextBoxIP2_TextChanged);
            this.maskedTextBoxIP2.KeyUp += new System.Windows.Forms.KeyEventHandler(this.maskedTextBoxIP2_KeyUp);
            // 
            // maskedTextBoxIP3
            // 
            this.maskedTextBoxIP3.Location = new System.Drawing.Point(791, 50);
            this.maskedTextBoxIP3.Mask = "000";
            this.maskedTextBoxIP3.Name = "maskedTextBoxIP3";
            this.maskedTextBoxIP3.Size = new System.Drawing.Size(49, 26);
            this.maskedTextBoxIP3.TabIndex = 10;
            this.maskedTextBoxIP3.Text = "043";
            this.maskedTextBoxIP3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.maskedTextBoxIP3.TextChanged += new System.EventHandler(this.maskedTextBoxIP3_TextChanged);
            // 
            // maskedTextBoxIP4
            // 
            this.maskedTextBoxIP4.Location = new System.Drawing.Point(846, 50);
            this.maskedTextBoxIP4.Mask = "000";
            this.maskedTextBoxIP4.Name = "maskedTextBoxIP4";
            this.maskedTextBoxIP4.Size = new System.Drawing.Size(49, 26);
            this.maskedTextBoxIP4.TabIndex = 11;
            this.maskedTextBoxIP4.Text = "065";
            this.maskedTextBoxIP4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.maskedTextBoxIP4.TextChanged += new System.EventHandler(this.maskedTextBoxIP4_TextChanged);
            // 
            // labelPiStatus
            // 
            this.labelPiStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelPiStatus.AutoSize = true;
            this.tableLayoutPanelRaspberry.SetColumnSpan(this.labelPiStatus, 4);
            this.labelPiStatus.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPiStatus.Location = new System.Drawing.Point(895, 147);
            this.labelPiStatus.Name = "labelPiStatus";
            this.labelPiStatus.Size = new System.Drawing.Size(0, 16);
            this.labelPiStatus.TabIndex = 12;
            this.labelPiStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRefresh.Image = ((System.Drawing.Image)(resources.GetObject("buttonRefresh.Image")));
            this.buttonRefresh.Location = new System.Drawing.Point(901, 50);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(49, 26);
            this.buttonRefresh.TabIndex = 13;
            this.buttonRefresh.UseVisualStyleBackColor = true;
            this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1003, 673);
            this.Controls.Add(this.tabControljpeg);
            this.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(900, 600);
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "JPEG.Encryption";
            this.tabControljpeg.ResumeLayout(false);
            this.tabPageFile.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPreview)).EndInit();
            this.contextMenuStripJpegFile.ResumeLayout(false);
            this.groupBoxSetRegionOfInterest.ResumeLayout(false);
            this.groupBoxSetRegionOfInterest.PerformLayout();
            this.tabPageCamera.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxVideo)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPageRaspberry.ResumeLayout(false);
            this.tableLayoutPanelRaspberry.ResumeLayout(false);
            this.tableLayoutPanelRaspberry.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRaspberry)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControljpeg;
        private System.Windows.Forms.TabPage tabPageFile;
        private System.Windows.Forms.TabPage tabPageCamera;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.PictureBox pictureBoxPreview;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.TreeView treeViewImages;
        private System.Windows.Forms.GroupBox groupBoxSetRegionOfInterest;
        private System.Windows.Forms.Label labelOpenCValgorithm;
        private System.Windows.Forms.ComboBox comboBoxOpenCV;
        private System.Windows.Forms.RadioButton radioButtonGetRoIfromOpenCV;
        private System.Windows.Forms.RadioButton radioButtonGetRoIFromMouse;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label labelCameraFrameRate;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBoxShowRoI;
        private System.Windows.Forms.RadioButton radioButtonShowDecryptedFrame;
        private System.Windows.Forms.RadioButton radioButtonShowEncryptedFrame;
        private System.Windows.Forms.RadioButton radioButtonShowOriginalFrame;
        private System.Windows.Forms.TextBox textBoxPasswordVideo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxFaceDetectionAlgorithm;
        private System.Windows.Forms.Label labelSelectFaceDetection;
        private System.Windows.Forms.PictureBox pictureBoxVideo;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonDetectFaces;
        private System.Windows.Forms.CheckBox checkBoxEncryptSwapValues;
        private System.Windows.Forms.CheckBox checkBoxEncryptSwapCWVPair;
        private System.Windows.Forms.CheckBox checkBoxEncryptSwapBlocks;
        private System.Windows.Forms.CheckBox checkBoxEncryptDcCoefficients;
        private System.Windows.Forms.Label labelCameraResolution;
        private System.Windows.Forms.Label labelScreenResution;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBoxCameraResolutions;
        private System.Windows.Forms.Button buttonLoadJpg;
        private System.Windows.Forms.Button buttonEncrypt;
        private System.Windows.Forms.Button buttonDecrypt;
        private System.Windows.Forms.CheckBox checkBoxSelectAll;
        private System.Windows.Forms.TabPage tabPageRaspberry;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelRaspberry;
        private System.Windows.Forms.PictureBox pictureBoxRaspberry;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxIP1;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxIP2;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxIP3;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxIP4;
        private System.Windows.Forms.Label labelPiStatus;
        private System.Windows.Forms.Button buttonAdvancedOpenCVSettings;
        private System.Windows.Forms.Label labelOpenCvAdvancedSettings;
        private System.Windows.Forms.Button buttonAdvancedSettingsOpenCVCamera;
        private System.Windows.Forms.Label labelAdvancedSettings;
        private System.Windows.Forms.Button buttonRefresh;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripJpegFile;
        private System.Windows.Forms.ToolStripMenuItem saveImageAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetRoIsToolStripMenuItem;
    }
}

