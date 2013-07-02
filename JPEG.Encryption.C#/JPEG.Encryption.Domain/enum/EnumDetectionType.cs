/// EnumDetectionType.cs
/// 
/// This enumeration represents the available face detection algorithms.
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
    /// This enumeration represents the available face detection algorithms.
    /// </summary>
    public enum EnumDetectionType
    {
        DEFAULT = 0,
        DO_CANNY_PRUNING = 1,
        SCALE_IMAGE = 2,
        FIND_BIGGEST_OBJECT = 3,
        DO_ROUGH_SEARCH = 4,
        NONE = 5
    }
}
