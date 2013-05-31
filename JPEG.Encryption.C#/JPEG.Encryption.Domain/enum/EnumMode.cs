/// EnumErrorCode.cs
/// 
/// This enumeration represents the mode for online (USB-Camera) or offline (static image)
/// 
/// Authors:
/// Stefan Auer, Alexander Bliem
/// 
/// Date: 04.05.2013


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JPEG.Encryption.BL
{
    /// <summary>
    /// This enumeration is used for changing the preset settings in FormFaceDetectionSettings
    /// </summary>
    public enum EnumMode
    {
        Offline = 0,
        Online = 1
    };
}
