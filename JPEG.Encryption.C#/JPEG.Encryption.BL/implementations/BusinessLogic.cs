/// BusinessLogic.cs
/// 
/// A class representing the business logic core.
/// It creates the other business logic objects.
/// For further details see IBusinessLogic.cs as well.
/// 
/// Authors:
/// Stefan Auer, Alexander Bliem
///
/// Date: 01.03.2013

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JPEG.Encryption.BL;
using JPEG.Encryption.BL.Implementations;
using JPEG.Encryption.BL.Interfaces;


namespace JPEG.Encryption.BL
{
    /// <summary>
    /// this class is responsible for creating the different business logic objects
    /// </summary>
    public class BusinessLogic : IBusinessLogic
    {
        public BusinessLogic() 
        {
            // check if necessary libs are available
            DependencyCheck.Execute();
        }
        

        // for details see IBusinessLogic
        public ICamera CreateCameraBL()
        {
            return new CameraBL();
        }

        // for details see IBusinessLogic
        public IFaceDetect CreateFaceDetectBL()
        {
            return new FaceDetectBL();
        }

        // for details see IBusinessLogic
        public IJpegOperation CreateJpegOperationBL()
        {
            return new JpegOperationsBL();
        }

        // for details see IBusinessLogic
        public IRaspberryPi CreateRaspberryPiBL()
        {
            return new RaspberryPiBL();
        }
    }
}
