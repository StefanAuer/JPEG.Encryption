// NanoJPEG -- KeyJ's Tiny Baseline JPEG Decoder
// version 1.3 (2012-03-05)
// by Martin J. Fiedler <martin.fiedler@gmx.net>
//
// This software is published under the terms of KeyJ's Research License,
// version 0.2. Usage of this software is subject to the following conditions:
// 0. There's no warranty whatsoever. The author(s) of this software can not
//    be held liable for any damages that occur when using this software.
// 1. This software may be used freely for both non-commercial and commercial
//    purposes.
// 2. This software may be redistributed freely as long as no fees are charged
//    for the distribution and this license information is included.
// 3. This software may be modified freely except for this license information,
//    which must not be changed in any way.
// 4. If anything other than configuration, indentation or comments have been
//    altered in the code, the original author(s) must receive a copy of the
//    modified code.


#ifndef JPEGPARSER_H_
#define JPEGPARSER_H_

//---start
#define DC	0
#define AC	1

#define NO_CRYPTO	0
#define ENCRYPTION	1
#define DECRYPTION	2
//---end


// nj_result_t: Result codes for njDecode().
typedef enum _nj_result {
    NJ_OK = 0,        // no error, decoding successful
    NJ_NO_JPEG,       // not a JPEG file
    NJ_UNSUPPORTED,   // unsupported format
    //NJ_OUT_OF_MEM,    // out of memory
    //NJ_INTERNAL_ERR,  // internal error
    NJ_SYNTAX_ERROR,  // syntax error
    __NJ_FINISHED,    // used internally, will never be reported
} nj_result_t;


// njInit: Initialize NanoJPEG.
// For safety reasons, this should be called at least one time before using
// using any of the other NanoJPEG functions.
void njInit(void);

// jpegCrypto: Decode a JPEG image.
// Decodes a memory dump of a JPEG file into internal buffers.
// Parameters:
//   jpegIn = The pointer to the memory dump.
//	 jpegOut = The pointer to the encrypted memory dump
//   sizeIn = The size of the JPEG input file.
//	 sizeOut = the size of the JPEG output file.
// Return value: The error code in case of failure, or NJ_OK (zero) on success.
nj_result_t jpegCrypto(const void* jpegIn, const void* jpegOut, const long sizeIn, long *sizeOut, int* in_roiArray, int* in_roiArraySize, int cryptoDetail, int cryptoMode);

// njGetWidth: Return the width (in pixels) of the most recently decoded
// image. If njDecode() failed, the result of njGetWidth() is undefined.
int njGetWidth(void);

// njGetHeight: Return the height (in pixels) of the most recently decoded
// image. If njDecode() failed, the result of njGetHeight() is undefined.
int njGetHeight(void);


// njGetImageSize: Returns the size (in bytes) of the image data returned
// by njGetImage(). If njDecode() failed, the result of njGetImageSize() is
// undefined.
int njGetImageSize(void);




int njGetOriginalImageBufferSize(void);
int njGetCryptoImageBufferSize(void);




#endif /* JPEGPARSER_H_ */
