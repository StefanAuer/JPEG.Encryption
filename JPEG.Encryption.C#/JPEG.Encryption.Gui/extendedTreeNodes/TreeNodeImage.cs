using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using JPEG.Encryption.BL;
using System.Drawing;


namespace JPEG.Encryption
{
    /// <summary>
    /// This class is a special node for a TreeView control.
    /// It contains additional properties in order to store e.g. the password
    /// </summary>
    public class TreeNodeImage : TreeNode
    {
        public int imageIndex { get; private set;}              // image index of ImageList 
        public IList<RoI> RoIs { get; set; }                    // the "region of interest" (e.g. a face)
        public Image Image { get; set; }                        // the image itself
        public string Password { get; set; }                    // the password 

        /// <summary>
        /// Constructor
        /// </summary>
        public TreeNodeImage()
        {
            this.imageIndex = 0;
            this.ImageIndex = this.imageIndex;
            this.SelectedImageIndex = this.imageIndex;
            this.RoIs = new List<RoI>();
            this.Password = "privacy";
        }
    }
}
