//----------------------------------------------------//
// bridge.h                                121012     //
// AUER STEFAN, ALEXANDER BLIEM         R&D-project   //
//                                                    //
// this header file contains the function prototypes  //
// encrypting and decrypting JPEG.                    //
// This functions can be called from e.g. C# using    //
// PInvoke/DLLImport                                  //
// The caller has to care about memory management!    //
//----------------------------------------------------//

#ifndef BRIDGE_DLL_H_
#define BRIDGE_DLL_H_

#include "../JPEG.Encryption.Core/prng.h"
#include "../JPEG.Encryption.Core/rijndael.h"
#include "../JPEG.Encryption.Core/JPEGparser.h"
#include "../JPEG.Encryption.Core/JPEG.Encryption.Core.h"


// encrypts a jpeg byte array
// NOTE: caller is responsable for memory handling!
void encJpeg(
			unsigned char*     in_jpegToEncrypt,     // byte/char array with JPEG values
			long               in_jpegSize,          // 64 bit size
			int*               in_roiArray,          // one RoI contains x, y, width, height (in pixels)
			int                in_roiArraySize,      // one RoI contains 4 values; if 2xRoI => in_roiArraySize=8
			unsigned char*     in_encryptionKey,     // the key to used for encryption
			int                in_encryptionKeySize, // size of the key
			char               in_cryptoFlags,       // regulates which level of encryption is used
			unsigned char*     out_jpegEncrypted,    // the encrypted JPEG as byte array
			long *             out_jpegEncryptedSize,// the size of the encrypted array
			int*               out_errorCode         // determine errors (0=NO_ERROR)
);

// decrypts a jpeg byte array
// NOTE: caller is responsable for memory handling!
void decJpeg(
			unsigned char*     in_jpegToDecrypt,	 // byte/char array with JPEG values
			long 		       in_jpegSize,			 // 64 bit size
			int*               in_roiArray,			 // one RoI contains x, y, width, height (in pixels)
			int                in_roiArraySize,		 // one RoI contains 4 values; if 2xRoI => in_roiArraySize=8
			unsigned char*     in_decryptionKey, 	 // the key to used for decryption
			int                in_decryptionKeySize, // size of the key
			char               in_cryptoFlags,		 // regulates which level of decryption is used
			unsigned char*     out_jpegDecrypted,	 // the decrypted JPEG as char/byte array
			long *         	   out_jpegDecryptedSize,// the size of the decrypted array
			int*               out_errorCode		 // determine errors (0=NO_ERROR)
);

#endif
