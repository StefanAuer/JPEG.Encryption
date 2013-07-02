/// IRaspberryPi.cs
/// 
/// This interface determines all required methods for the connecting to a 
/// previously configured Raspberry Pi.
/// 
/// Authors:
/// Stefan Auer, Alexander Bliem
/// 
/// Date: 01.03.2013


using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JPEG.Encryption.BL.Interfaces
{
    /// <summary>
    /// Business logic interface used for communicating with a Raspberry Pi
    /// In this scenario the Pi has a running httpd (Apache) 
    /// </summary>
    public interface IRaspberryPi
    {
        Image GetImageFromURLSync(string url, string IP);
        Task<Image> GetImageFromUrlASync(string url);
    }
}
