/// Bridge.cs
/// 
/// This static class invokes the C-DLL containing the encryption algorithms.
/// It uses PInvoke.
/// 
/// Authors:
/// Stefan Auer, Alexander Bliem
/// 
/// Date: 01.03.2013


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using JPEG.Encryption.BL;
using System.Security.Cryptography;
using JPEG.Encryption.BL.Interfaces;
using JPEG.Encryption.BL.exception;

namespace JPEG.Encryption.BL
{
    public class Bridge 
    {
        /// <summary>
        /// signature of the C-Dll for encrypting an image
        /// </summary>
        /// <param name="in_jpegToEncrypt">the image that will be encrypted</param>
        /// <param name="in_jpegSize">the size of the byte array</param>
        /// <param name="in_roiArray">this array contains the information concerning the Region of Interests</param>
        /// <param name="in_roiArraySize">the size of the RoI-array</param>
        /// <param name="in_encryptionKey">the key used for encryption</param>
        /// <param name="in_encryptionKeySize">size of the key</param>
        /// <param name="in_cryptoFlags">determines which encryption mode will be applied</param>
        /// <param name="out_jpegEncrypted">out: the encrypted image</param>
        /// <param name="out_jpegEncryptedSize">out: the size of the encrypted image</param>
        /// <param name="out_errorCode">out: the error code. 0 if success.</param>
        [DllImport(@"bridge.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void encJpeg(
            [MarshalAs(UnmanagedType.LPArray)]     byte[]    in_jpegToEncrypt,
            Int32                                            in_jpegSize,
            Int32[]                                          in_roiArray,
            Int32                                            in_roiArraySize,
            [MarshalAs(UnmanagedType.LPArray)]     byte[]    in_encryptionKey,
            Int32                                            in_encryptionKeySize,
            byte                                             in_cryptoFlags,
            [MarshalAs(UnmanagedType.LPArray)]     byte[]    out_jpegEncrypted,
            ref IntPtr                                       out_jpegEncryptedSize,
            ref IntPtr                                       out_errorCode
         );

        /// <summary>
        /// signature of the C-Dll for decrypting an image
        /// </summary>
        /// <param name="in_jpegToDecrypt">the image that will be encrypted</param>
        /// <param name="in_jpegSize">the size of the byte array</param>
        /// <param name="in_roiArray">this array contains the information concerning the Region of Interests</param>
        /// <param name="in_roiArraySize">the size of the RoI-array</param>
        /// <param name="in_decryptionKey">the key used for decryption</param>
        /// <param name="in_decryptionKeySize">the size of the key</param>
        /// <param name="in_cryptoFlags">determines which decryption mode will be applied</param>
        /// <param name="out_jpegDecrypted">out: the decrypted image</param>
        /// <param name="out_jpegDecryptedSize">out: the size of the decrypted image</param>
        /// <param name="out_errorCode">out: the error code. 0 if success.</param>
        [DllImport(@"bridge.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void decJpeg(
            [MarshalAs(UnmanagedType.LPArray)]     byte[]   in_jpegToDecrypt,
            Int32                                           in_jpegSize,
            Int32[]                                         in_roiArray,
            Int32                                           in_roiArraySize,
            [MarshalAs(UnmanagedType.LPArray)]     byte[]   in_decryptionKey,
            Int32                                           in_decryptionKeySize,
            byte                                            in_cryptoFlags,
            [MarshalAs(UnmanagedType.LPArray)]     byte[]   out_jpegDecrypted,
            ref IntPtr                                      out_jpegDecryptedSize,
            ref IntPtr                                      out_errorCode
         );

        /// <summary>
        /// encrypts an image
        /// </summary>
        /// <param name="image">the image that will be encrypted</param>
        /// <param name="roIs">the Region of Interests</param>
        /// <param name="password">the password used for encryption</param>
        /// <param name="cryptoFlags">determines which encryption mode will be applied</param>
        /// <param name="errorCode">if an error occurs this value will be set is not 0</param>
        /// <returns>returns an encrypted image</returns>
        public static Image EncryptJPG(Image image, IList<RoI> roIs, string password, CryptoFlags cryptoFlags, out EnumErrorCode errorCode)
        {
            // create JpegOperation from business logic
            IBusinessLogic businessLogic = new BusinessLogic();
            IJpegOperation jpegOperation = businessLogic.CreateJpegOperationBL();

            // convert Image to byte array
            byte[] in_jpegToEncrypt = jpegOperation.ConvertImageToByteArray(image);
            Int32 in_size = in_jpegToEncrypt.Length;

            // convert RoIs to the correct format
            int counter = 0;
            Int32[] in_roiArray = new Int32[roIs.Count() * 4];
            foreach (RoI roi in roIs)
            {
                in_roiArray[(counter * 4) + 0] = roIs[counter].X;
                in_roiArray[(counter * 4) + 1] = roIs[counter].Y;
                in_roiArray[(counter * 4) + 2] = roIs[counter].Width;
                in_roiArray[(counter * 4) + 3] = roIs[counter].Height;
                counter++;
            }
            Int32 in_roiArraySize = roIs.Count() * 4;

            // create MD5 hash of the password
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] textToHash = Encoding.Default.GetBytes(password);
            byte[] in_encryptionKey = md5.ComputeHash(textToHash);
            Int32 in_encryptionKeySize = in_encryptionKey.Length;

            // convert crypto flags 
            byte in_cryptoFlags = cryptoFlags.ToByte();

            // get memory for the encrypted image
            byte[] out_jpegEncrypted = new byte[in_size*2];
            byte[] out_statistics   =  new byte[in_size];
            IntPtr out_jpegEncryptedSize = new IntPtr();
            int[] ia2 = new int[1];
            GCHandle gch1 = GCHandle.Alloc(ia2);
            out_jpegEncryptedSize = Marshal.UnsafeAddrOfPinnedArrayElement(ia2, 0);

            // create a pointer for the error code (will be set from C-code)
            IntPtr out_errorCode = new IntPtr();
            int[] ia = new int[1];
            GCHandle gch = GCHandle.Alloc(ia);
            out_errorCode = Marshal.UnsafeAddrOfPinnedArrayElement(ia, 0);

            // now try to call the encryption algorithm
            try
            {
                encJpeg(
                    in_jpegToEncrypt,
                    in_size,
                    in_roiArray,
                    in_roiArraySize,
                    in_encryptionKey,
                    in_encryptionKeySize,
                    in_cryptoFlags,
                    out_jpegEncrypted,
                    ref out_jpegEncryptedSize,
                    ref out_errorCode
                );

                // get the error code
                errorCode = ConvertIntToErrorCode(out_errorCode.ToInt32());

                // truncate byte array
                out_jpegEncrypted = out_jpegEncrypted.Take(out_jpegEncryptedSize.ToInt32()).ToArray();
            }
            catch 
            {
                throw new EncryptionException("Error while encrypting!");
            }

            // convert byte array to Image and return
            return jpegOperation.ConvertByteArrayToImage(out_jpegEncrypted);
        }

        /// <summary>
        /// decrypts an image
        /// </summary>
        /// <param name="image">the image that will be dencrypted</param>
        /// <param name="roIs">the Region of Interests</param>
        /// <param name="password">the password used for decryption</param>
        /// <param name="cryptoFlags">determines which encryption mode will be applied</param>
        /// <param name="errorCode">if an error occurs this value will be set is not 0</param>
        /// <returns>returns an decrypted image</returns>
        public static Image DecryptJPG(Image image, IList<RoI> roIs, string password, CryptoFlags cryptoFlags, out EnumErrorCode errorCode)
        {
            // create JpegOperation from business logic
            IBusinessLogic businessLogic = BLFactory.CreateBusinessLogic();
            IJpegOperation jpegOperation = businessLogic.CreateJpegOperationBL();

            // convert Image to byte array
            byte[] in_jpegToDecrypt = jpegOperation.ConvertImageToByteArray(image);
            Int32 in_size = in_jpegToDecrypt.Length;

            // convert RoIs to the correct format
            int counter = 0;
            Int32[] in_roiArray = new Int32[roIs.Count() * 4];
            foreach (RoI roi in roIs)
            {
                in_roiArray[(counter * 4) + 0] = roIs[counter].X;
                in_roiArray[(counter * 4) + 1] = roIs[counter].Y;
                in_roiArray[(counter * 4) + 2] = roIs[counter].Width;
                in_roiArray[(counter * 4) + 3] = roIs[counter].Height;
                counter++;
            }
            Int32 in_roiArraySize = roIs.Count() * 4;

            // create MD5 hash of the password
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] textToHash = Encoding.Default.GetBytes(password);
            byte[] in_decryptionKey = md5.ComputeHash(textToHash);
            Int32 in_decryptionKeySize = in_decryptionKey.Length;

            // convert crypto flags 
            byte in_cryptoFlags = cryptoFlags.ToByte();

            // get memory for the decrypted image
            byte[] out_jpegDecrypted = new byte[in_size * 2];
            byte[] out_statistics = new byte[in_size];
            IntPtr out_jpegDecryptedSize = new IntPtr();
            int[] ia2 = new int[1];
            GCHandle gch1 = GCHandle.Alloc(ia2);
            out_jpegDecryptedSize = Marshal.UnsafeAddrOfPinnedArrayElement(ia2, 0);

            // create a pointer for the error code (will be set from C-code)
            int[] ia = new int[1];
            IntPtr out_errorCode = new IntPtr();
            GCHandle gch = GCHandle.Alloc(ia);
            out_errorCode = Marshal.UnsafeAddrOfPinnedArrayElement(ia, 0);

            // try to decrypt the image
            try
            {
                decJpeg(
                    in_jpegToDecrypt,
                    in_size,
                    in_roiArray,
                    in_roiArraySize,
                    in_decryptionKey,
                    in_decryptionKeySize,
                    in_cryptoFlags,
                    out_jpegDecrypted,
                    ref out_jpegDecryptedSize,
                    ref out_errorCode
                );

                // get the error code
                errorCode = ConvertIntToErrorCode(out_errorCode.ToInt32());
            
                // truncate byte array
                out_jpegDecrypted = out_jpegDecrypted.Take(out_jpegDecryptedSize.ToInt32()).ToArray();
            }
            catch 
            {
                throw new EncryptionException("Error while decrypting!");
            }

            return jpegOperation.ConvertByteArrayToImage(out_jpegDecrypted);
         
        }

        /// <summary>
        /// convert errorcide to enum
        /// </summary>
        /// <param name="errorCode">errorcode as integer</param>
        /// <returns>EnumErrorCode of errorCode</returns>
        private static EnumErrorCode ConvertIntToErrorCode(int errorCode)
        {
            switch (errorCode)
            {
                case 0:
                    return EnumErrorCode.SUCCESS;
                case 1:
                    return EnumErrorCode.NO_JPEG;
                case 2:
                    return EnumErrorCode.UNSUPPORTED;
                case 3:
                    return EnumErrorCode.OUT_OF_MEM;
                case 4:
                    return EnumErrorCode.INTERNAL_ERR;
                case 5:
                    return EnumErrorCode.SYNTAX_ERROR;
                default:
                    return EnumErrorCode.UNKNOWN_ERROR;
            }
        }

    }
}
