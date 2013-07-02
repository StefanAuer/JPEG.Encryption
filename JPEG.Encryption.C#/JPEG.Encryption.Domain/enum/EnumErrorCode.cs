/// EnumErrorCode.cs
/// 
/// This enumeration represents the errors that may occur during the encryption.
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
    /// This enumeration represents the errors that may occur during the encryption
    /// </summary>
    public enum EnumErrorCode 
    {
        SUCCESS = 0,   // success
        NO_JPEG,       // not a JPEG file
        UNSUPPORTED,   // unsupported JPEG format
        OUT_OF_MEM,    // out of memory
        INTERNAL_ERR,  // internal error
        SYNTAX_ERROR,  // syntax error
        UNKNOWN_ERROR  // unknown error
    };
}
