//----------------------------------------------------//
// bridge.c                                121012     //
// AUER STEFAN, ALEXANDER BLIEM         R&D-project   //
//                                                    //
// this code file contains the functions for          //
// encrypting JPEG.                                   //
// This functions can be called from e.g. C# using    //
// PInvoke/DLLImport                                  //
//----------------------------------------------------//


#include <stdio.h>
#include <string.h>
#include "bridge.h"

#include "../JPEG.Encryption.Core/prng.h"
#include "../JPEG.Encryption.Core/rijndael.h"
#include "../JPEG.Encryption.Core/JPEGparser.h"
#include "../JPEG.Encryption.Core/JPEG.Encryption.Core.h"


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
){
	// The encryption is currently not JPEG file length preserving.
	// Worst case: the required memory is as twice as big as the original array
	// The caller can truncate the array by using out_jpegEncryptedSize
	*out_jpegEncryptedSize = in_jpegSize * 2;

	//initialize the AES pseudo random number generator using the key in_encryptionKey
    initAesPrng(in_encryptionKey, in_encryptionKeySize);

    // encode!
    int result = jpegCrypto(in_jpegToEncrypt, out_jpegEncrypted, in_jpegSize, out_jpegEncryptedSize, in_roiArray, &in_roiArraySize, in_cryptoFlags, ENCRYPTION);

    // in case of an error, submit the error code (0...SUCCESS);
    *out_errorCode = result;
}

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
){
	// The decryption is currently not JPEG file length preserving.
	// Worst case: the required memory is as twice as big as the original array
	// The caller can truncate the array by using out_jpegEncryptedSize
	*out_jpegDecryptedSize = in_jpegSize * 2;

	//initialize the AES pseudo random number generator using the key in_encryptionKey
    initAesPrng(in_decryptionKey, in_decryptionKeySize);

    // decode!
    int result = jpegCrypto(in_jpegToDecrypt, out_jpegDecrypted, in_jpegSize, out_jpegDecryptedSize, in_roiArray, &in_roiArraySize, in_cryptoFlags, DECRYPTION);

    // in case of an error, submit the error code
    // 0...SUCCESS
    *out_errorCode = result;
}


