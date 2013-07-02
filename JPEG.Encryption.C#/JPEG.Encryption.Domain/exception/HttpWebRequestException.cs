/// HttpWebRequestException.cs
/// 
/// This exception object will be thrown if there is a problem with a web request
/// 
/// Authors:
/// Stefan Auer, Alexander Bliem
/// 
/// Date: 01.03.2013

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JPEG.Encryption.BL.exception
{
    /// <summary>
    /// This exception is thrown if there is a connection problem with 
    /// the Raspberry Pi
    /// </summary>
    public class HttpWebRequestException : Exception
    {
        /// <summary>
        /// the exception text
        /// </summary>
        public string Text { get; private set; }      
        
        /// <summary>
        /// the IP address as text
        /// </summary>
        public string IP   { get; private set; }      

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="text">The text of the exception</param>
        /// <param name="IP">The IP address</param>
        public HttpWebRequestException(string text, string IP)
        {
            this.Text = text;
            this.IP = IP;
        }
    }
}
