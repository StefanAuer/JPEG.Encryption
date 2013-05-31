/// RaspberryPiBL.cs
/// 
/// This class is responsable for the connection to a 
/// previously configured Raspberry Pi.
/// For further details see IRaspberryPi.cs as well.
/// 
/// Authors:
/// Stefan Auer, Alexander Bliem
/// 
/// Date: 01.03.2013


using JPEG.Encryption.BL.Interfaces;
using JPEG.Encryption.BL.exception;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace JPEG.Encryption.BL.Implementations
{
    /// <summary>
    /// class implementing IJpegOperation
    /// </summary>
    public class RaspberryPiBL : IRaspberryPi
    {

        public Image GetImageFromURLSync(string url, string IP)
        {
            Image result = null;

            try
            {
                // give the Raspberry Pi some time (too many requests stresses it)
                Thread.Sleep(100);

                HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
                HttpWebResponse httpWebReponse = (HttpWebResponse)httpWebRequest.GetResponse();

                // get response
                Stream stream = httpWebReponse.GetResponseStream();
                
                result = Image.FromStream(stream);
            }
            catch (WebException)
            {
                result = null;
            }
            catch (HttpWebRequestException)
            {
                result = null;
            }
            catch (Exception)
            {
                throw new HttpWebRequestException("Unable to get image from URL: " + url, IP);
            }

            return result;
        }


        
        public Task<Image> GetImageFromUrlASync(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            Task<WebResponse> task = Task.Factory.FromAsync(
                request.BeginGetResponse,
                asyncResult => request.EndGetResponse(asyncResult),
                (object)null);

            return task.ContinueWith(t => ReadStreamFromResponse(t.Result));
        }

        
        private static Image ReadStreamFromResponse(WebResponse response)
        {
            Stream stream = response.GetResponseStream();
            return Image.FromStream(stream);
        }
    }
}


