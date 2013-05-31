/// FormMain.cs
/// 
/// This is the main form of the application.
/// 
/// Authors:
/// Stefan Auer, Alexander Bliem
/// 
/// Date: 04.05.2013


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using JPEG.Encryption.BL;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.Net;
using JPEG.Encryption.BL.Interfaces;
using JPEG.Encryption.BL.exception;
using System.Runtime.Serialization.Formatters.Binary;
using System.Configuration;


namespace JPEG.Encryption.Gui
{
    public partial class FormMain : Form
    {
        #region Private members

        // business logic objects
        private IJpegOperation jpegOperationBL;                // methods dealing with JPG
        private ICamera cameraBL;                              // methods for handling the camera
        private IFaceDetect faceDetectBL;                      // methods for face detection
        private IRaspberryPi raspberryPiBL;                    // methods for dealing with Raspberry Pi

        // threads
        private Thread loadImageFromRasperryPiThread = null;   // thread for loading images from the Pi
        private Thread faceDetectionThread;                    // thread for detecting faces
        private bool sendRequestsToPi = true;                  // bool variable to control while loop

        // face detection settings
        FaceDetectionSettings faceDetectionSettings = new FaceDetectionSettings(EnumMode.Offline);                       // face detection parameters
        FormFaceDetectionSettings formFaceDetectionSettingsOffline = new FormFaceDetectionSettings(EnumMode.Offline);    // WinForm to modify the parameters
        FormFaceDetectionSettings formFaceDetectionSettingsOnline = new FormFaceDetectionSettings(EnumMode.Online);      // WinForm to modify the parameters

        // for manual drawing of the RoIs
        private Point mouseDownPoint;               	       // where the mouse was put down

        // calculating frames per second (fps)
        private int currentSecond = -1;
        private int currentFrameRate = 0;
        private int numberOfFrames;

        // Raspberry Pi IP
        string IP;

        #endregion

        #region Constructor
        /// <summary>
        /// constructor
        /// </summary>
        public FormMain()
        {
            // WinForms requirement
            InitializeComponent();

            // init business logic 
            InitializeBusinessLogic();

            // check if there is a camera
            if (cameraBL.IsCameraAvailable == false)
            {
                MessageBox.Show("Unable to capture pictures from camera! Is there a camera?", "Sorry", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.tabControljpeg.TabPages.Remove(this.tabPageCamera);
            }
            
            // load the available face detection algorithms
            LoadFaceDetectionAlgorithms();

            // thread for getting images from Raspberry Pi
            loadImageFromRasperryPiThread = new Thread(new ThreadStart(GetImageFromURL));
            loadImageFromRasperryPiThread.IsBackground = true;
            loadImageFromRasperryPiThread.Priority = ThreadPriority.Highest;

            string width = ConfigurationManager.AppSettings["CameraCaptureWidth"]   == "" ? "640" : ConfigurationManager.AppSettings["CameraCaptureWidth"];
            string height = ConfigurationManager.AppSettings["CameraCaptureHeight"] == "" ? "480" : ConfigurationManager.AppSettings["CameraCaptureHeight"];
            this.comboBoxCameraResolutions.Text = width + "x" + height;

            // get default IP address
            string IP = ConfigurationManager.AppSettings["RaspberryPiIP"];
            if (IP != "" && IP.Split('.').Count() == 4)
            {
                string[] splitArray = IP.Split('.');
                maskedTextBoxIP1.Text = splitArray[0];
                maskedTextBoxIP2.Text = splitArray[1];
                maskedTextBoxIP3.Text = splitArray[2];
                maskedTextBoxIP4.Text = splitArray[3];
            }

        }

        #endregion

        #region Button events

        /// <summary>
        /// event handler for buttonLoad
        /// </summary>
        private void buttonLoadJpg_Click(object sender, EventArgs e)
        {
            // init
            string path = string.Empty;
            int pictureBoxWidth = pictureBoxPreview.Width;
            int pictureBoxHeight = pictureBoxPreview.Height;

            // open file dialog
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "jpeg files (*.jpeg)|*.jpg;*.jpeg|All files (*.*)|*.*";

            // OK was clicked
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // enable multiple files
                    foreach (string fileName in openFileDialog.FileNames)
                    {
                        // check if file is valid
                        if (fileName != "" &&                                      // file name is not empty 
                             File.Exists(fileName) &&                              // file exists
                             (Path.GetExtension(fileName).ToLower() == ".jpg" ||   // .jpg
                              Path.GetExtension(fileName).ToLower() == ".jpeg"))   // .jpeg
                        {
                            // get image and properties
                            Image loadedImage = Bitmap.FromFile(fileName);

                            if (jpegOperationBL.IsJpegImage(loadedImage) == true)  // check JPEGformat
                                // add image to list view
                                AddImageToTreeView(loadedImage, fileName, treeViewImages);
                            else
                                MessageBox.Show("This file is not a JPEG!", "Sorry!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            
                        }
                        else
                            MessageBox.Show("Invalid file!", "Sorry", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Error while loading image file!", "Sorry", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// event handler for buttonEncrypt
        /// </summary>
        private void buttonEncrypt_Click(object sender, EventArgs e)
        {
            // get all nodes (including leaves)
            IList<TreeNode> allNodes = GetAllNodesOfTreeView(treeViewImages);

            // step through the nodes 
            foreach (var node in allNodes)
            {
                // only nodes of type "TreeNodeImage" will be processed 
                if (node is TreeNodeImage)
                {
                    EnumErrorCode errorCode = EnumErrorCode.SUCCESS;

                    TreeNodeImage treeNodeImage = (TreeNodeImage)node;

                    // only selected nodes will be processed 
                    if (treeNodeImage.Checked == true)
                    {
                        // default: use all crypto flags
                        Image encryptedImage = Bridge.EncryptJPG(treeNodeImage.Image, treeNodeImage.RoIs, treeNodeImage.Password, new CryptoFlags(), out errorCode);
                        if (encryptedImage != null && errorCode == EnumErrorCode.SUCCESS)
                        {
                            AddEncryptedImageToTreeView(encryptedImage, treeNodeImage);
                            treeNodeImage.Expand();
                        }

                        // as this node was processed: uncheck
                        treeNodeImage.Checked = false;
                    }

                    if (errorCode != EnumErrorCode.SUCCESS)
                    {
                        switch (errorCode)
                        {

                            case EnumErrorCode.NO_JPEG:
                                MessageBox.Show("This file is not a JPEG file:\n" + node.Text, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); ;
                                break;
                            case EnumErrorCode.INTERNAL_ERR:
                                MessageBox.Show("An internal error occurred while encoding!\n" + node.Text, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); ;
                                break;
                            case EnumErrorCode.OUT_OF_MEM:
                                MessageBox.Show("Running out of memory!\n" + node.Text, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); ;
                                break;
                            case EnumErrorCode.SYNTAX_ERROR:
                                MessageBox.Show("Syntax error occurred\n" + node.Text, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); ;
                                break;
                            case EnumErrorCode.UNKNOWN_ERROR:
                                MessageBox.Show("Unknown error occurred!\n" + node.Text, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); ;
                                break;
                            case EnumErrorCode.UNSUPPORTED:
                                MessageBox.Show("This JPEG format is not supported\n" + node.Text, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); ;
                                break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// event handler for buttonDecrypt
        /// </summary>
        private void buttonDecrypt_Click(object sender, EventArgs e)
        {
            // get all nodes (including leaves)
            IList<TreeNode> allNodes = GetAllNodesOfTreeView(treeViewImages);

            // step through nodes
            foreach (var node in allNodes)
            {
                EnumErrorCode errorCode = EnumErrorCode.SUCCESS;

                // get checked nodes 
                if (node.Checked == true)
                {
                    Image imageToDecrypt = null;
                    IList<RoI> rois = new List<RoI>();
                    string password = "";
                    TreeNode newNode = null;

                    // cast
                    if (node is TreeNodeEncryptedImage)
                    {
                        TreeNodeEncryptedImage encryptedNode = (TreeNodeEncryptedImage)node;
                        imageToDecrypt = encryptedNode.Image;
                        rois = encryptedNode.RoIs;
                        password = encryptedNode.Password;
                        newNode = encryptedNode;
                    }
                    else if (node is TreeNodeImage)
                    {
                        TreeNodeImage unencryptedNode = (TreeNodeImage)node;
                        imageToDecrypt = unencryptedNode.Image;
                        rois = unencryptedNode.RoIs;
                        password = unencryptedNode.Password;
                        newNode = unencryptedNode;
                    }

                    // default: use all crypto flags
                    Image decryptedImage = Bridge.DecryptJPG(imageToDecrypt, rois, password, new CryptoFlags(), out errorCode);
                    AddDecryptedImageToTreeView(decryptedImage, newNode);
                    newNode.Expand();

                    // as this node was processed: uncheck
                    node.Checked = false;
                }

                if (errorCode != EnumErrorCode.SUCCESS)
                    MessageBox.Show("Error while encrypting image!");
            }
        }

        /// <summary>
        /// event handler for buttonDetectFaces
        /// </summary>
        private void buttonDetectFaces_Click(object sender, EventArgs e)
        {
            // start thread!
            StartFaceDetectionThread();
        }

        /// <summary>
        /// event handler for buttonAdvancedOpenCVSettings
        /// </summary>
        private void buttonAdvancedOpenCVSettings_Click(object sender, EventArgs e)
        {
            ShowAdvancedSettingsOffline();
        }

        /// <summary>
        /// event handler for buttonAdvancedSettingsOpenCVCamera
        /// </summary>
        private void buttonAdvancedSettingsOpenCVCamera_Click(object sender, EventArgs e)
        {
            ShowAdvancedSettingsOnline();
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            ConnectToRaspberryPi();
        }

        #endregion

        #region TreeView events and methods

        /// <summary>
        ///  event handler for TreeView (node was selected)
        /// </summary>
        private void treeViewImages_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNodeImage selectedTreeNodeImage;

            // is a node selected?
            if (treeViewImages.SelectedImageIndex >= 0)
            {
                // selected node is a TreeNodeImage
                selectedTreeNodeImage = treeViewImages.SelectedNode as TreeNodeImage;
                if (selectedTreeNodeImage != null)
                {
                    pictureBoxPreview.Image = selectedTreeNodeImage.Image;
                    textBoxPassword.Text = selectedTreeNodeImage.Password;
                    treeViewImages.Invalidate();
                }

                // selected node is a TreeNodeEncryptedImage
                TreeNodeEncryptedImage selectedTreeNodeEncryptedImage = treeViewImages.SelectedNode as TreeNodeEncryptedImage;
                if (selectedTreeNodeEncryptedImage != null)
                {
                    pictureBoxPreview.Image = selectedTreeNodeEncryptedImage.Image;
                    textBoxPassword.Text = selectedTreeNodeEncryptedImage.Password;
                }

                // set visibility of controls according node type
                SetRegionOfInterestVisibility(treeViewImages.SelectedNode is TreeNodeImage);

                pictureBoxPreview.Invalidate();

                // draw RoIs
                DrawRoIs();
            }
        }

        /// <summary>
        /// adds an image to a TreeView
        /// </summary>
        /// <param name="image">the image that will be added</param>
        /// <param name="fileName">the file name of the image</param>
        /// <param name="treeView">the TreeView where you want to add the image</param>
        private void AddImageToTreeView(Image image, string fileName, TreeView treeView)
        {
            TreeNodeImage treeNode = new TreeNodeImage();
            treeNode.Image = image;
            treeNode.Text = fileName;
            treeViewImages.Nodes.Add(treeNode);
        }

        /// <summary>
        /// adds an encrypted image to a TreeView
        /// </summary>
        /// <param name="image">the image that will be added</param>
        /// <param name="treeNodeImage">the TreeView where you want to add the image</param>
        private void AddEncryptedImageToTreeView(Image image, TreeNodeImage treeNodeImage)
        {
            TreeNodeEncryptedImage treeNodeEncryptedImage = new TreeNodeEncryptedImage();
            treeNodeEncryptedImage.Image = image;
            treeNodeEncryptedImage.Password = treeNodeImage.Password;
            treeNodeEncryptedImage.RoIs = DeepCopy(treeNodeImage.RoIs);
            treeNodeEncryptedImage.Text = "[encrypted image]";
            treeNodeImage.Nodes.Add(treeNodeEncryptedImage);
        }

        /// <summary>
        /// adds an decrypted image a TreeView
        /// </summary>
        /// <param name="image">the image that will be added</param>
        /// <param name="treeNodeEncryptedImage">the TreeView where you want to add the image</param>
        private void AddDecryptedImageToTreeView(Image image, TreeNode node)
        {
            if (node is TreeNodeEncryptedImage)
            {
                TreeNodeImage treeNodeImage = new TreeNodeImage();
                treeNodeImage.Image = image;
                treeNodeImage.Text = "[decrypted image]";
                treeNodeImage.RoIs = DeepCopy(((TreeNodeEncryptedImage)node).RoIs);
                treeNodeImage.Password = ((TreeNodeEncryptedImage)node).Password;
                ((TreeNodeEncryptedImage)node).Nodes.Add(treeNodeImage);
            }
            else if (node is TreeNodeImage)
            {
                TreeNodeImage treeNodeImage = new TreeNodeImage();
                treeNodeImage.Image = image;
                treeNodeImage.Text = "[decrypted image]";
                treeNodeImage.RoIs = DeepCopy(((TreeNodeImage)node).RoIs);
                treeNodeImage.Password = ((TreeNodeImage)node).Password;
                ((TreeNodeImage)node).Nodes.Add(treeNodeImage);
            }
        
        }

        /// <summary>
        /// get all nodes of a TreeView
        /// </summary>
        /// <param name="treeView">the TreeView where you want to retrieve all nodes</param>
        /// <returns>An IList of all nodes</returns>
        private IList<TreeNode> GetAllNodesOfTreeView(TreeView treeView)
        {
            // init result
            List<TreeNode> result = new List<TreeNode>();

            // add nodes recursively
            foreach (TreeNode node in treeView.Nodes)
                result = AddRecursive(node, result);

            return result;
        }

        /// <summary>
        /// Method for getting nodes of a TreeView recursively
        /// </summary>
        /// <param name="treeNode">A treenode</param>
        /// <param name="result">the container for the result (will be passed through recursive calls)</param>
        /// <returns></returns>
        private List<TreeNode> AddRecursive(TreeNode treeNode, List<TreeNode> result)
        {
            // if node has childs: go to next level
            if (treeNode.Nodes.Count > 0)
                foreach (TreeNode node in treeNode.Nodes)
                    AddRecursive(node, result);

            // add current node
            result.Add(treeNode);

            return result;
        }

        #endregion

        #region CheckBox events

        /// <summary>
        ///  event handler for checkBoxSelectAll
        /// </summary>
        private void checkBoxSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            bool isChecked = checkBoxSelectAll.Checked;

            foreach (TreeNode node in GetAllNodesOfTreeView(treeViewImages))
            {
                // set according user input
                node.Checked = isChecked;
                
                // show all nodes (avoids that a checkbox is set in a collapsed node)
                node.ExpandAll();
            }
        }
        #endregion

        #region RadioButton events

        /// <summary>
        /// event handler for radioButtonGetRoIFromMouse
        /// </summary>
        private void radioButtonGetRoIFromMouse_CheckedChanged(object sender, EventArgs e)
        {
            // enable OpenCV options according user selection
            SetOpenCVControlEnabled(radioButtonGetRoIfromOpenCV.Checked);

            // clear RoIs
            if (treeViewImages.SelectedNode is TreeNodeImage)
                ((TreeNodeImage)treeViewImages.SelectedNode).RoIs.Clear();
            else if (treeViewImages.SelectedNode is TreeNodeEncryptedImage)
                ((TreeNodeEncryptedImage)treeViewImages.SelectedNode).RoIs.Clear();
        }

        /// <summary>
        /// event handler for radioButtonGetRoIfromOpenCV
        /// </summary>
        private void radioButtonGetRoIfromOpenCV_CheckedChanged(object sender, EventArgs e)
        {
            // enable OpenCV options according user selection
            SetOpenCVControlEnabled(radioButtonGetRoIfromOpenCV.Checked);
        }

        /// <summary>
        /// event handler for radioButtonShowEncryptedFrame
        /// </summary>
        private void radioButtonShowEncryptedFrame_CheckedChanged(object sender, EventArgs e)
        {
            checkBoxEncryptDcCoefficients.Enabled = radioButtonShowEncryptedFrame.Checked;
            checkBoxEncryptSwapBlocks.Enabled = radioButtonShowEncryptedFrame.Checked;
            checkBoxEncryptSwapCWVPair.Enabled = radioButtonShowEncryptedFrame.Checked;
            checkBoxEncryptSwapValues.Enabled = radioButtonShowEncryptedFrame.Checked;
        }

        #endregion

        #region MaskedTextBox events

        /// <summary>
        /// event handler for maskedTextBoxIP1 
        /// </summary>
        private void maskedTextBoxIP1_TextChanged(object sender, EventArgs e)
        {
            buttonRefresh.Enabled = true;
        }

        /// <summary>
        /// event handler for maskedTextBoxIP2 
        /// </summary>
        private void maskedTextBoxIP2_TextChanged(object sender, EventArgs e)
        {
            buttonRefresh.Enabled = true;
        }

        /// <summary>
        /// event handler for maskedTextBoxIP3 
        /// </summary>
        private void maskedTextBoxIP3_TextChanged(object sender, EventArgs e)
        {
            buttonRefresh.Enabled = true;
        }

        /// <summary>
        /// event handler for maskedTextBoxIP4
        /// </summary>
        private void maskedTextBoxIP4_TextChanged(object sender, EventArgs e)
        {
            buttonRefresh.Enabled = true;
        }

        /// <summary>
        /// event handler for maskedTextBoxIP1
        /// </summary>
        private void maskedTextBoxIP1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.OemPeriod)
                maskedTextBoxIP2.Focus();
        }

        /// <summary>
        /// event handler for maskedTextBoxIP2
        /// </summary>
        private void maskedTextBoxIP2_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.OemPeriod)
                maskedTextBoxIP3.Focus();
        }

        /// <summary>
        /// event handler for maskedTextBoxIP3
        /// </summary>
        private void maskedTextBoxIP3_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.OemPeriod)
                maskedTextBoxIP4.Focus();
        }

        #endregion

        #region Textbox events

        /// <summary>
        /// event handler for textBoxPassword 
        /// </summary>
        private void textBoxPassword_TextChanged(object sender, EventArgs e)
        {
            // get selected node 
            TreeNode selectedNode = treeViewImages.SelectedNode;

            if (treeViewImages.SelectedImageIndex >= 0)
            {
                // set password 
                TreeNodeImage selectedTreeNodeImage = treeViewImages.SelectedNode as TreeNodeImage;
                if (selectedTreeNodeImage != null)
                    selectedTreeNodeImage.Password = textBoxPassword.Text;

                TreeNodeEncryptedImage selectedTreeNodeEncryptedImage = treeViewImages.SelectedNode as TreeNodeEncryptedImage;
                if (selectedTreeNodeEncryptedImage != null)
                    selectedTreeNodeEncryptedImage.Password = textBoxPassword.Text;
            }
        }
        
        #endregion

        #region Mouse events and DrawRoIs
        /// <summary>
        /// mouse down on the picture was clicked
        /// </summary>
        private void pictureBoxPreview_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left &&                    // left mouse button
                pictureBoxPreview.Image != null &&                  // image in picture box is not null
                treeViewImages.SelectedNode is TreeNodeImage &&     // drawing is only allowed on an unencrypted/decrypted image
                radioButtonGetRoIFromMouse.Checked == true)         // checkbox indicates user wants to draw a RoI
            {
                // remember mouse position
                mouseDownPoint = e.Location;
            }
            // right mouse button was clicked etc.
            else if (e.Button == MouseButtons.Right)
            {
                contextMenuStripJpegFile.Show();
            }
        }

        /// <summary>
        /// mouse is moving over the picture box
        /// </summary>
        private void pictureBoxPreview_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left &&                    // left mouse button
                pictureBoxPreview.Image != null &&                  // image in picture box is not null
                treeViewImages.SelectedNode is TreeNodeImage &&     // drawing is only allowed on an unencrypted/decrypted image
                radioButtonGetRoIFromMouse.Checked == true)         // checkbox indicates user wants to draw a RoI
            {
                // draw rectangle according user input
                int x = Math.Min(e.X, mouseDownPoint.X);
                int y = Math.Min(e.Y, mouseDownPoint.Y);
                int width = Math.Abs(e.X - mouseDownPoint.X);
                int height = Math.Abs(e.Y - mouseDownPoint.Y); ;

                pictureBoxPreview.Refresh();
                Graphics graphics = pictureBoxPreview.CreateGraphics();
                graphics.DrawRectangle(Pens.Yellow, x, y, width, height);
            }
        }

        /// <summary>
        /// mouse button is released
        /// </summary>
        private void pictureBoxPreview_MouseUp(object sender, MouseEventArgs e)
        {
            // init values
            RoI roi = new RoI();
            TreeNodeImage treeNodeImage = treeViewImages.SelectedNode as TreeNodeImage;

            if (e.Button == MouseButtons.Left &&                    // left mouse button
                pictureBoxPreview.Image != null &&                  // image in picture box is not null
                treeViewImages.SelectedNode is TreeNodeImage &&     // drawing is only allowed on an unencrypted/decrypted image
                radioButtonGetRoIFromMouse.Checked == true)         // checkbox indicates user wants to draw a RoI
            {
                // the image in the picture box has not the originial size
                // so we have to calculate the correct values
                int imageScreenHeight = 0;
                int imageScreenWidth = 0;

                double widthCorrection = 0;
                double heightCorrection = 0;

                int correctedPictureBoxPreviewWidth = pictureBoxPreview.Width - 4;     // -4 because the picture is a litte bit smaller than the picturebox 
                int correctedPictureBoxPreviewHeight = pictureBoxPreview.Height - 4;   // -4 because the picture is a litte bit smaller than the picturebox 

                double imageHeightToPictureBoxHeightRatio = (double)pictureBoxPreview.Image.Height / correctedPictureBoxPreviewHeight;
                double imageWidthToPictureBoxWidthRatio = (double)pictureBoxPreview.Image.Width / correctedPictureBoxPreviewWidth;

                double pictureRatio = ((double)pictureBoxPreview.Image.Width)/ (pictureBoxPreview.Image.Height);

                // portrait image
                if (pictureBoxPreview.Image.Height > pictureBoxPreview.Image.Width)
                {
                    // calculate the size of the image on the screen
                    imageScreenHeight = correctedPictureBoxPreviewHeight;
                    //imageScreenWidth = (int)((double)pictureBoxPreview.Width * pictureRatio);
                    imageScreenWidth = (int)(imageScreenHeight * pictureRatio);

                    // because the image is centered in the pictureBox
                    widthCorrection = (correctedPictureBoxPreviewWidth - imageScreenWidth) / 2f;
                }

                // landscape image
                else
                {
                    // calculate the size of the image on the screen
                    imageScreenWidth = correctedPictureBoxPreviewWidth;
                    imageScreenHeight = (int)(imageScreenWidth / pictureRatio);
                    

                    // because the image is centered in the pictureBox
                    heightCorrection = (correctedPictureBoxPreviewHeight - imageScreenHeight) / 2f;
                }

                // get mouse values 
                int mouseX = Math.Min(e.X, mouseDownPoint.X);
                int mouseY = Math.Min(e.Y, mouseDownPoint.Y);
                int mouseWidth = Math.Abs(e.X - mouseDownPoint.X);
                int mouseHeight = Math.Abs(e.Y - mouseDownPoint.Y);

                // calculate RoI values (for the original image!)
                int x = (int)(((double)(mouseX - widthCorrection) / imageScreenWidth) * pictureBoxPreview.Image.Width);
                int y = (int)(((double)(mouseY - heightCorrection) / imageScreenHeight) * pictureBoxPreview.Image.Height);
                int width = (int)(((double)mouseWidth / imageScreenWidth) * pictureBoxPreview.Image.Width);
                int height = (int)(((double)mouseHeight / imageScreenHeight) * pictureBoxPreview.Image.Height);

                roi.X = x;
                roi.Y = y;
                roi.Width = width;
                roi.Height = height;
            }
                
            // add area to list (if there is something to add)
            if (roi.Width > 0 && roi.Height > 0 && treeNodeImage != null)
            {
                treeNodeImage.RoIs.Add(roi);               
            }
            DrawRoIs();
        }

        /// <summary>
        /// draw current RoIs
        /// </summary>
        private void DrawRoIs()
        {
            if (InvokeRequired)
                this.Invoke(new Action(DrawRoIs));
            else
            {
                // refresh image (without RoIs)
                if (treeViewImages.SelectedNode is TreeNodeImage)
                    pictureBoxPreview.Image = ((TreeNodeImage)treeViewImages.SelectedNode).Image;
                else if (treeViewImages.SelectedNode is TreeNodeEncryptedImage)
                    pictureBoxPreview.Image = ((TreeNodeEncryptedImage)treeViewImages.SelectedNode).Image;
                
                pictureBoxPreview.Refresh();

                // get graphics
                Graphics graphics = pictureBoxPreview.CreateGraphics();
                

                IList<RoI> roiList = new List<RoI>();

                // get currently selected treeNode
                TreeNode treeNode = treeViewImages.SelectedNode;
                TreeNodeImage treeNodeImage = treeNode as TreeNodeImage;
                TreeNodeEncryptedImage treeNodeEncryptedImage = treeNode as TreeNodeEncryptedImage;

                if (treeNodeImage != null || treeNodeEncryptedImage != null)
                {
                    roiList = treeNodeImage != null ? treeNodeImage.RoIs : treeNodeEncryptedImage.RoIs;
                }

                foreach (var singleRoI in roiList)
                {
                    int imageScreenHeight = 0;
                    int imageScreenWidth = 0;

                    int correctedPictureBoxPreviewWidth = pictureBoxPreview.Width - 2;     // -4 because the picture is a litte bit smaller than the picturebox 
                    int correctedPictureBoxPreviewHeight = pictureBoxPreview.Height - 2;   // -4 because the picture is a litte bit smaller than the picturebox 
                    double widthCorrection = 0;
                    double heightCorrection = 0;

                    double imageHeightToScreenHeightRatio = (double)pictureBoxPreview.Image.Height / correctedPictureBoxPreviewHeight;
                    double imageWidthToScreenWidthRatio = (double)pictureBoxPreview.Image.Width / correctedPictureBoxPreviewWidth;

                    double screenRatio = 0;

                    double pictureRatio = (double)pictureBoxPreview.Image.Width / pictureBoxPreview.Image.Height;

                    if (pictureBoxPreview.Image.Height > pictureBoxPreview.Image.Width)
                    {
                        // set screen ratio
                        screenRatio = imageHeightToScreenHeightRatio;

                        // calculate the size of the image on the screen
                        imageScreenHeight = correctedPictureBoxPreviewHeight;
                        //imageScreenWidth = (int)((double)pictureBoxPreview.Width * pictureRatio);
                        imageScreenWidth = (int)(imageScreenHeight * pictureRatio);

                        // because the image is centered in the pictureBox
                        widthCorrection = (correctedPictureBoxPreviewWidth - imageScreenWidth) / 2f;
                    }
                    else
                    {
                        // set screen ratio
                        screenRatio = imageWidthToScreenWidthRatio;

                        // calculate the size of the image on the screen
                        imageScreenWidth = correctedPictureBoxPreviewWidth;
                        imageScreenHeight = (int)(imageScreenWidth / pictureRatio);

                        // because the image is centered in the pictureBox
                        heightCorrection = (correctedPictureBoxPreviewHeight - imageScreenHeight) / 2f;
                    }

                    int x = (int)(((singleRoI.X) / screenRatio) + (widthCorrection));
                    int y = (int)(((singleRoI.Y) / screenRatio) + (heightCorrection));
                    int width = (int)(singleRoI.Width / screenRatio);
                    int height = (int)(singleRoI.Height / screenRatio);

                    graphics.DrawRectangle(Pens.Yellow, x, y, width, height);
                    
                }
                graphics.Dispose();
            }
        }


        #endregion

        #region TabControlEvents

        /// <summary>
        /// event handler for tabControljpg (selected tab changed)
        /// </summary>
        private void tabControljpeg_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            if (tabControljpeg.SelectedTab == tabPageCamera)
            {
                // get pictures from camera when user has tabControljpeg active
                Application.Idle += TestFrameWork;

                // different settings for "online" (= picture from camera) and "offline" (= picture from file)
                this.faceDetectionSettings = this.formFaceDetectionSettingsOnline.GetFaceDetectionSettings(); ;
            }
            else
            {
                Application.Idle -= TestFrameWork;
                this.faceDetectionSettings = this.formFaceDetectionSettingsOffline.GetFaceDetectionSettings(); ;
            }
        }

        #endregion

        #region ComboBox events

        /// <summary>
        /// event handler for comboBoxCameraResolution
        /// </summary>
        private void comboBoxCameraResolutions_SelectedIndexChanged(object sender, EventArgs e)
        {
            // get selected values
            string[] resoltion = comboBoxCameraResolutions.Text.Split('x');
            double width = double.Parse(resoltion[0]);
            double height = double.Parse(resoltion[1]);

            // reset camera and get the resolution from the camera
            cameraBL.SetCameraResolution(width, height);
        }
        
        #endregion

        #region Thread safe control handling

        /// <summary>
        /// thread safe change of Label text
        /// </summary>
        /// <param name="label">the Label where the text will be changed</param>
        /// <param name="text">the new text for the Label</param>
        /// <param name="color">the new fore color</param>
        private void UpdateLabelText(Label label, string text, Color color)
        {
            if (InvokeRequired)
                this.Invoke(new Action<Label, string, Color>(UpdateLabelText), label, text, color);
            else
            {
                label.Text = text;
                label.ForeColor = color;
            }
        }

        /// <summary>
        /// thread safe change of a Label visibility
        /// </summary>
        /// <param name="control">the Control where the visibility will be changed</param>
        /// <param name="visible">the new visibility</param>
        private void UpdateControlVisibility(Control control, bool visible)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action<Control, bool>(UpdateControlVisibility), control, visible);
            else
                control.Visible = visible;
        }

        /// <summary>
        /// thread safe change of a Control visibility
        /// </summary>
        /// <param name="control">the control where the you want to determine if it is enabled or not</param>
        /// <param name="enabled">the new enabled status</param>
        private void UpdateControlEnabled(Control control, bool enabled)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action<Control, bool>(UpdateControlEnabled), control, enabled);
            else
                control.Enabled = enabled;
        }


        /// <summary>
        /// get the selected TreeNode of a TreeView (thread safe)
        /// </summary>
        /// <param name="treeView">the TreeView with TreeNodes</param>
        /// <returns>the selected TreeNode</returns>
        private TreeNode GetSelectedNode(TreeView treeView)
        {
            if (this.InvokeRequired)
                return (TreeNode)Invoke((Func<TreeNode>)delegate { return GetSelectedNode(treeView); });
            else
                return treeView.SelectedNode;
        }
        #endregion

        #region FaceDetection

        /// <summary>
        /// Loads the available face detection algorithms and puts them into the ComboBoxes
        /// </summary>
        private void LoadFaceDetectionAlgorithms()
        {
            // for each algorithm
            foreach (var algorithm in faceDetectBL.GetFaceDetectionAlgorithms())
            {
                // fill the two combo boxes
                comboBoxFaceDetectionAlgorithm.Items.Add(algorithm.ToString());
                comboBoxOpenCV.Items.Add(algorithm.ToString());
            }

            // add "[none]" as well
            if (comboBoxFaceDetectionAlgorithm.Items.Count > 0)
            {
                comboBoxFaceDetectionAlgorithm.Items.Insert(0, "NONE");
                comboBoxFaceDetectionAlgorithm.SelectedIndex = 1;
            }

            // preselect item
            if (comboBoxOpenCV.Items.Count > 0)
                comboBoxOpenCV.SelectedIndex = 0;
        }

        /// <summary>
        /// Start the face detection in a seperate thread
        /// </summary>
        private void StartFaceDetectionThread()
        {
            // start thread if not already running. The must be only one instance for face detection (due to OpenCV)
            if (faceDetectionThread == null || faceDetectionThread.IsAlive == false)
            {
                faceDetectionThread = new Thread(new ParameterizedThreadStart(DetectFacesOnPreview));
                faceDetectionThread.IsBackground = true;
                faceDetectionThread.Priority = ThreadPriority.Highest;
                faceDetectionThread.Start(comboBoxOpenCV.SelectedItem.ToString());
            }
        }

        /// <summary>
        /// Method for detecting faces accoring the given algorithm
        /// </summary>
        /// <param name="algorithmName">the name of the desired algorithm</param>
        private void DetectFacesOnPreview(object algorithmName)
        {
            if (radioButtonGetRoIfromOpenCV.Checked == true)
            {

                try
                {
                    // get user selection
                    TreeNode selectedNode = GetSelectedNode(treeViewImages);
                    EnumDetectionType selectedAlgorithm = EnumHelper.StringToEnum<EnumDetectionType>(algorithmName.ToString());

                    Image currentImage = pictureBoxPreview.Image;
                    if (selectedNode is TreeNodeImage)
                        currentImage = ((TreeNodeImage)selectedNode).Image;
                    else if (selectedNode is TreeNodeEncryptedImage)
                        currentImage = ((TreeNodeEncryptedImage)selectedNode).Image;

                    if (currentImage != null)
                    {
                        // show "[detecting faces]"
                        UpdateControlVisibility(labelStatus, true);

                        // get settings according user requirements
                        double scale = this.faceDetectionSettings.ImageScale;
                        int minNeighbors = this.faceDetectionSettings.MinNeighbors;
                        Size minSize = this.faceDetectionSettings.MinSize;

                        // detect faces
                        IList<RoI> rois = faceDetectBL.DetectFaces(currentImage, selectedAlgorithm, scale, minNeighbors, minSize);

                        // save the RoI information directly at the node
                        if (selectedNode is TreeNodeImage)
                            ((TreeNodeImage)selectedNode).RoIs = rois;
                        else if (selectedNode is TreeNodeEncryptedImage)
                            ((TreeNodeEncryptedImage)selectedNode).RoIs = rois;

                        // draw RoIs on image
                        DrawRoIs();

                        // hide "[detecting faces]"
                        UpdateControlVisibility(labelStatus, false);
                    }
                }
                catch
                {
                    MessageBox.Show("Error while detecting faces!", "Sorry", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Draw the RoIs on an image
        /// </summary>
        /// <param name="rois">IList of RoIs which will be drawn on the image</param>
        /// <param name="image">The image where the different RoIs will be drawn</param>
        private void DrawRoiOnImage(IList<RoI> rois, Image image)
        {
            foreach (RoI roi in rois)
            {
                //draw a rectangle around the detected face
                Graphics graphics = Graphics.FromImage(image);
                graphics.DrawRectangle(new Pen(Color.Red, 2), roi.X, roi.Y, roi.Width, roi.Height);
            }
        }

        /// <summary>
        /// Enables or disables OpenCV relevant controls
        /// </summary>
        /// <param name="enabled">if true, controls are enabled</param>
        private void SetOpenCVControlEnabled(bool enabled)
        {
            labelOpenCValgorithm.Enabled = enabled;
            comboBoxOpenCV.Enabled = enabled;
            buttonDetectFaces.Enabled = enabled;
            labelOpenCvAdvancedSettings.Enabled = enabled;
            buttonAdvancedOpenCVSettings.Enabled = enabled;
        }

        /// <summary>
        /// Sets the visibility of RoI relevant controls
        /// </summary>
        /// <param name="visible">if true, controls are set to visible </param>
        private void SetRegionOfInterestVisibility(bool visible)
        {
            radioButtonGetRoIFromMouse.Visible = visible;
            radioButtonGetRoIfromOpenCV.Visible = visible;
            comboBoxOpenCV.Visible = visible;
            labelOpenCValgorithm.Visible = visible;
            buttonDetectFaces.Visible = visible;
            buttonAdvancedOpenCVSettings.Visible = visible;
            labelOpenCvAdvancedSettings.Visible = visible;
        }
        #endregion

        #region Encryption

        /// <summary>
        /// Determines the user settings and creates the corresponding CryptoFlags
        /// </summary>
        /// <returns>returns the corresponding CryptoFlags</returns>
        private CryptoFlags GetCryptoFlags()
        {
            // init result
            CryptoFlags cryptoFlags = new CryptoFlags();

            // get the flags
            cryptoFlags.DcCoeffCrypto = checkBoxEncryptDcCoefficients.Checked;
            cryptoFlags.SwapBlock = checkBoxEncryptSwapBlocks.Checked;
            cryptoFlags.SwapCWVPair = checkBoxEncryptSwapCWVPair.Checked;
            cryptoFlags.SwapValues = checkBoxEncryptSwapValues.Checked;

            // return result
            return cryptoFlags;
        }

        /// <summary>
        /// Main method to test the encryption/decyrption framework
        /// </summary>
        void TestFrameWork(object sender, EventArgs e)
        {
            Image curFrame = cameraBL.QueryFrame(); ;       // the original camera caputure
            
            // if there is no camera: curFrame is null
            // NOTE: if camera is disabled during the application is running
            // curFrame contains the last captured frame
            if (curFrame != null)
            {
                Image curFrameClone = (Image)curFrame.Clone();  // the image where the rectangles will be drawn 

                var width = cameraBL.FrameWidth;                // frame width
                var height = cameraBL.FrameHeight;              // frame height

                EnumErrorCode errorCode;                        // error code if something goes wrong
                CryptoFlags cryptoFlags = GetCryptoFlags();     // the different levels of encryption put into an object

                IList<RoI> rois = new List<RoI>();              // list of "Region of Interests"

                // get current frame rate
                int currentFrameRate = GetCurrentFrameRate();
                labelCameraFrameRate.Text = "Current frame rate: " + currentFrameRate + "fps";

                // get camera resolution
                labelCameraResolution.Text = "Camera resolution: " + (int)width + "x" + (int)height;
                labelScreenResution.Text = "Screen resolution: " + pictureBoxVideo.Width + "x" + pictureBoxVideo.Height;

                // [none] selected
                if (comboBoxFaceDetectionAlgorithm.SelectedIndex == 0 && radioButtonShowEncryptedFrame.Checked == true)
                {
                    Image encryptedImage = Bridge.EncryptJPG(curFrame, new List<RoI>(), textBoxPasswordVideo.Text, cryptoFlags, out errorCode);
                    pictureBoxVideo.Image = encryptedImage;
                }

                // the face detection DLL must only be used once at the same time. 
                // Maybe there is currently a face detection running at the "JPEG Browser"
                else if (faceDetectionThread == null || faceDetectionThread.IsAlive == false)
                {
                    // check if a camera is available
                    if (cameraBL.IsCameraAvailable == true)
                    {
                        try
                        {
                            // determine detection type
                            EnumDetectionType detectionType = (EnumDetectionType)Enum.Parse(typeof(EnumDetectionType), comboBoxFaceDetectionAlgorithm.Text);

                            // a detection algorithm was selected
                            if (detectionType != EnumDetectionType.NONE)
                            {
                                // get settings according user requirements
                                double scale = this.faceDetectionSettings.ImageScale;
                                int minNeighbors = this.faceDetectionSettings.MinNeighbors;
                                Size minSize = this.faceDetectionSettings.MinSize;

                                // get region of interests
                                rois = faceDetectBL.DetectFaces(curFrame, detectionType, scale, minNeighbors, minSize);

                                // if no RoI was found: add a fake RoI to force no encryption on an "empty" image
                                if (rois.Count == 0)
                                    rois = new List<RoI>() { new RoI() { X = 1000, Y = 1000, Height = 0, Width = 0 } };
                            }

                            // user wants to encrypt the image
                            if (radioButtonShowEncryptedFrame.Checked == true)
                            {
                                // encrpyt
                                Image encryptedImage = Bridge.EncryptJPG(curFrame, rois, textBoxPasswordVideo.Text, cryptoFlags, out errorCode);

                                //Action for each element detected
                                if (checkBoxShowRoI.Checked == true)
                                {
                                    foreach (RoI roi in rois)
                                    {
                                        //draw a rectangle around the detected face
                                        Graphics graphics = Graphics.FromImage(encryptedImage);
                                        graphics.DrawRectangle(new Pen(Color.Red, 2), roi.X, roi.Y, roi.Width, roi.Height);
                                    }
                                }
                                pictureBoxVideo.Image = encryptedImage;
                            }
                            // user wants to see the decrypted image
                            else if (radioButtonShowDecryptedFrame.Checked == true)
                            {
                                // encrypt and decrypt an image
                                Image encryptedImage = Bridge.EncryptJPG(curFrame, rois, textBoxPasswordVideo.Text, new CryptoFlags(), out errorCode);
                                Image decryptedImage = Bridge.DecryptJPG(encryptedImage, rois, textBoxPasswordVideo.Text, new CryptoFlags(), out errorCode);

                                // draw RoIs on image
                                if (checkBoxShowRoI.Checked == true)
                                    DrawRoiOnImage(rois, decryptedImage);

                                // assign image to picture box
                                pictureBoxVideo.Image = decryptedImage;
                            }

                            // user wants to see the original frame
                            else if (radioButtonShowOriginalFrame.Checked == true)
                            {
                                // draw RoIs on image
                                if (checkBoxShowRoI.Checked == true && detectionType != EnumDetectionType.NONE)
                                    DrawRoiOnImage(rois, curFrameClone);

                                // assign image to picture box
                                pictureBoxVideo.Image = curFrameClone;
                            }
                            pictureBoxVideo.Invalidate();
                        }
                        catch (FaceDetectionException faceDetectionException)
                        {
                            Console.WriteLine(faceDetectionException.Text);
                        }
                        catch (CameraException cameraCaptureException)
                        {
                            Console.WriteLine(cameraCaptureException.Text);
                        }
                    }
                }
            }
        }
        
        #endregion

        #region ContextMenu


        /// <summary>
        /// event handler for resetRoIsToolStripMenuItem
        /// </summary>
        private void resetRoIsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // an unencrypted image was selected -> clear RoIs
            TreeNodeImage treeNodeImage = treeViewImages.SelectedNode as TreeNodeImage;
            if (treeNodeImage != null)
                treeNodeImage.RoIs.Clear();

            contextMenuStripJpegFile.Hide();
            DrawRoIs();
        }

        /// <summary>
        /// event handler for saveImageAsToolStripMenuItem
        /// </summary>
        private void saveImageAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Image imageToSave = null;

            // refresh image (without RoIs)
            if (treeViewImages.SelectedNode is TreeNodeImage)
                imageToSave = ((TreeNodeImage)treeViewImages.SelectedNode).Image;
            else if (treeViewImages.SelectedNode is TreeNodeEncryptedImage)
                imageToSave = ((TreeNodeEncryptedImage)treeViewImages.SelectedNode).Image;

            // save image
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = "image.jpg";
            saveFileDialog.Filter = "JPEG Files|*.jpg;*.jpeg|All Files|*.*";
            if (imageToSave != null && 
                saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    imageToSave.Save(saveFileDialog.FileName);
        }

        /// <summary>
        /// event handler for contextMenuStripJpegFile
        /// </summary>
        private void contextMenuStripJpegFile_Opening(object sender, CancelEventArgs e)
        {
            // it is not allowed to reset RoIs of an encrypted image (we would lose important information)
            if (treeViewImages.SelectedNode is TreeNodeEncryptedImage || pictureBoxPreview.Image == null)
                resetRoIsToolStripMenuItem.Enabled = false;
            else
                resetRoIsToolStripMenuItem.Enabled = true;

            if (pictureBoxPreview.Image == null)
                saveImageAsToolStripMenuItem.Enabled = false;
            else
                saveImageAsToolStripMenuItem.Enabled = true;

            
        }

        #endregion

        #region Raspberry Pi

        private void ConnectToRaspberryPi()
        {
            this.sendRequestsToPi = false;
            if (tabControljpeg.SelectedTab == tabPageRaspberry)
            {
                buttonRefresh.Enabled = false;

                // get values from text box. Remove leading zeros. Set emtpy inputs to zero.
                int temp;
                int IP1 = int.TryParse(maskedTextBoxIP1.Text, out temp) == true ? temp : 0;
                int IP2 = int.TryParse(maskedTextBoxIP2.Text, out temp) == true ? temp : 0;
                int IP3 = int.TryParse(maskedTextBoxIP3.Text, out temp) == true ? temp : 0;
                int IP4 = int.TryParse(maskedTextBoxIP4.Text, out temp) == true ? temp : 0;

                // get IP address
                this.IP = IP1 + "." + IP2 + "." + IP3 + "." + IP4;
                UpdateLabelText(labelPiStatus, "Connecting to Raspberry Pi at " + IP, Color.Black);

                // init thread
                if (loadImageFromRasperryPiThread != null)
                {
                    pictureBoxRaspberry.Image = null;
                    this.sendRequestsToPi = false;
                    loadImageFromRasperryPiThread.Abort();
                    loadImageFromRasperryPiThread = null;
                    loadImageFromRasperryPiThread = new Thread(new ThreadStart(GetImageFromURL));
                    loadImageFromRasperryPiThread.IsBackground = true;
                    loadImageFromRasperryPiThread.Priority = ThreadPriority.Highest;

                }
                this.sendRequestsToPi = true;
                loadImageFromRasperryPiThread.Start();
            }
        }

        /// <summary>
        /// Gets the image from URL.
        /// </summary>
        /// <returns></returns>
        private void GetImageFromURL()
        {
            IBusinessLogic businessLogic = BLFactory.CreateBusinessLogic();
            IRaspberryPi raspberryPiBL = businessLogic.CreateRaspberryPiBL();

            while (this.sendRequestsToPi == true)
            {
                Image result;
                string url = "http://" + IP + "/camera.jpg?" + new Random().Next();

                try
                {
                    // sync call 
                    result = raspberryPiBL.GetImageFromURLSync(url, IP);

                    // async call
                    // result = raspberryPiBL.GetImageFromUrlASync(url).Result;

                    pictureBoxRaspberry.Invalidate();
                    if (result != null)
                    {
                        UpdateLabelText(labelPiStatus, "Connected!", Color.Black);
                        pictureBoxRaspberry.Image = result;
                    }
                }
                catch (HttpWebRequestException ex)
                {
                    if (ex.IP != this.IP)
                        UpdateLabelText(labelPiStatus, "Error while connecting to Rasperry Pi at " + this.IP, Color.Red);
                }
                catch (Exception)
                {
                    UpdateLabelText(labelPiStatus, "Error while connecting to Rasperry Pi at " + this.IP, Color.Red);
                }

                UpdateControlEnabled(buttonRefresh, true);
            }
        }

        #endregion

        #region Miscellaneous
        /// <summary>
        /// load business logic objects
        /// </summary>
        private void InitializeBusinessLogic()
        {
            jpegOperationBL = BLFactory.CreateBusinessLogic().CreateJpegOperationBL();
            cameraBL = BLFactory.CreateBusinessLogic().CreateCameraBL();
            faceDetectBL = BLFactory.CreateBusinessLogic().CreateFaceDetectBL();
            raspberryPiBL = BLFactory.CreateBusinessLogic().CreateRaspberryPiBL();
        }

        private int GetCurrentFrameRate()
        {
            numberOfFrames++;
            if (currentSecond != DateTime.Now.Second)
            {
                currentSecond = DateTime.Now.Second;
                currentFrameRate = numberOfFrames;
                numberOfFrames = 0;
            }
            return currentFrameRate;
        }

        private void ShowAdvancedSettingsOnline()
        {
            // call by reference may cause problems. So clone the settings first
            FaceDetectionSettings priviousSettings = (FaceDetectionSettings)this.faceDetectionSettings.Clone();

            // if user clicks "OK" than assign new values. Otherwise take the old ones
            if (this.formFaceDetectionSettingsOnline.ShowAsDialog(this.faceDetectionSettings) == System.Windows.Forms.DialogResult.OK)
                this.faceDetectionSettings = formFaceDetectionSettingsOnline.GetFaceDetectionSettings();
            else
                this.faceDetectionSettings = priviousSettings;
        }

        private void ShowAdvancedSettingsOffline()
        {
            // call by reference may cause problems. So clone the settings first
            FaceDetectionSettings priviousSettings = (FaceDetectionSettings)this.faceDetectionSettings.Clone();

            // if user clicks "OK" than assign new values. Otherwise take the old ones
            if (this.formFaceDetectionSettingsOffline.ShowAsDialog(this.faceDetectionSettings) == System.Windows.Forms.DialogResult.OK)
                this.faceDetectionSettings = formFaceDetectionSettingsOffline.GetFaceDetectionSettings();
            else
                this.faceDetectionSettings = priviousSettings;
        }


        /// <summary>
        /// generic method for deep copy of objects
        /// </summary>
        /// <typeparam name="T">data type of object to clone</typeparam>
        /// <param name="obj">the object that you want to deep copy</param>
        /// <returns>the deep copied object</returns>
        public static T DeepCopy<T>(T obj)
        {
            // use a memory stream for deep copy ;-)
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }

        #endregion
   }    
}
