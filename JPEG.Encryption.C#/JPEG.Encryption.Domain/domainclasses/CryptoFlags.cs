/// CryptoFlags.cs
/// 
/// This class contains the information which stage of the 
/// encryption algorithm will be used.
/// 
/// Authors:
/// Stefan Auer, Alexander Bliem
/// 
/// Date: 01.03.2013


using System;

namespace JPEG.Encryption.BL
{
    /// <summary>
    /// This class is used to determine which level of encryption is used
    /// </summary>
    public class CryptoFlags
    {
        public bool SwapBlock     { get; set; }     // 0  (equals 0 or 1)
        public bool SwapCWVPair   { get; set; }     // 1  (equals 0 or 2)
        public bool SwapValues    { get; set; }     // 2  (equals 0 or 4)
        public bool DcCoeffCrypto { get; set; }     // 3  (equals 0 or 8)
        
        /// <summary>
        /// Constructor
        /// </summary>
        public CryptoFlags()
        {
            // enable all levels by default
            this.DcCoeffCrypto = true;
            this.SwapBlock = true;
            this.SwapCWVPair = true;
            this.SwapValues = true;
        }

        /// <summary>
        /// convert to byte
        /// </summary>
        /// <returns></returns>
        public byte ToByte()
        {
            byte result = 0;
    
            // set the flags
            if (this.SwapBlock == true)
                result += 1;

            if (this.SwapCWVPair == true)
                result += 2;

            if (this.SwapValues == true)
                result += 4;

            if (this.DcCoeffCrypto == true)
                result += 8;

            return result;
        }
    }
}
