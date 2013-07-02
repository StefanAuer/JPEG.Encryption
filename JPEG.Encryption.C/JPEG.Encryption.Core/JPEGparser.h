/**
 * @file   JPEGparser.h
 * @author Alexander Bliem abliem.itsb-m2012@fh-salzburg.ac.at
 * @author Stefan Auer sauer.itsb-m2012@fh-salzburg.ac.at
 * @date   01.07.2013
 * @version 1.0
 * @brief JPEGparser.h is the header for JPEGparser.c
 * The JPEG decoder is based on the nanoJPEG open source project.
 *
 * NanoJPEG -- KeyJ's Tiny Baseline JPEG Decoder
 * version 1.3 (2012-03-05)
 * by Martin J. Fiedler <martin.fiedler@gmx.net>
 */

#ifndef JPEGPARSER_H_
#define JPEGPARSER_H_


#define DC	0
#define AC	1

#define NO_CRYPTO	0
#define ENCRYPTION	1
#define DECRYPTION	2


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


// njGetWidth: Return the width (in pixels) of the most recently decoded image.
int njGetWidth(void);


// njGetHeight: Return the height (in pixels) of the most recently decoded image.
int njGetHeight(void);


// njGetImageSize: Returns the size (in bytes) of the image data returned by njGetImage().
int njGetImageSize(void);


// njGetOriginalImageBufferSize: Returns the size (in bytes) of the input image data
int njGetOriginalImageBufferSize(void);


// njGetCryptoImageBufferSize: Returns the size (in bytes) of the encrypted image data
int njGetCryptoImageBufferSize(void);


#endif /* JPEGPARSER_H_ */
