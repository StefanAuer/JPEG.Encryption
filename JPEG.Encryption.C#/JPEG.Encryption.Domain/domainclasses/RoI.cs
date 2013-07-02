/// RoI.cs
/// 
/// Domain class containing all information for a "Region of Interest"
/// 
/// Authors:
/// Stefan Auer, Alexander Bliem
/// 
/// Date: 01.03.2013


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JPEG.Encryption.BL
{
    /// <summary>
    /// represent information concerning Region of Interest (RoI)
    /// </summary>
    [Serializable]
    public class RoI
    {
        // properties
        public Guid Guid { get; set; }
        public Guid TableImageGuid { get; set; }

        private int x { get; set; }
        public int X { get { return x; } set { if (value > 0) x = value; } }

        private int y { get; set; }
        public int Y { get { return y; } set { if (value > 0) y = value; } }

        private int width;
        public int Width { get { return width; } set { if (value > 0) width = value; } }

        private int height;
        public int Height { get { return height; } set { if (value > 0) height = value; } }

        /// <summary>
        /// constructor
        /// </summary>
        public RoI()
        {
            this.Guid = Guid.Empty;           // empty guid
            this.TableImageGuid = Guid.Empty; // empty guid
            this.width = 0;                   // initial height
            this.height = 0;                  // initial height
            this.x = 0;                       // initial x
            this.y = 0;                       // initial y
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="x">x coordinate of the RoI</param>
        /// <param name="y">y coordinate of the RoI</param>
        /// <param name="width">width of the RoI</param>
        /// <param name="height">height of the RoI</param>
        public RoI(int x, int y, int width, int height, Guid guid, Guid tableImageGuid)
        {
            this.Guid = guid;
            this.TableImageGuid = tableImageGuid;
            this.width = width;
            this.height = height;
            this.x = x;
            this.y = y;
        }
    }
}
