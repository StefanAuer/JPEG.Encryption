using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JPEG.Encryption.BL;
using System.Windows.Forms;
using System.Drawing;

namespace JPEG.Encryption
{
    /// <summary>
    /// This class is a special node for a TreeView control.
    /// It contains additional properties in order to store e.g. the RoIs
    /// </summary>
    public class TreeNodeEncryptedImage : TreeNode
    {
        public int imageIndex { get; private set; }             // image index of ImageList 
        public string Password { get; set; }                    // the password 
        public IList<RoI> RoIs { get; set; }                    // the "region of interest" (e.g. a face)
        public Image Image { get; set; }                        // the encrypted image

        /// <summary>
        /// Constructor
        /// </summary>
        public TreeNodeEncryptedImage()
        {
            this.imageIndex = 1;
            this.ImageIndex = imageIndex;
            this.SelectedImageIndex = imageIndex;
            this.RoIs = new List<RoI>();
        }
    }
}
