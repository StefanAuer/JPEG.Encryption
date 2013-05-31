

#include <stdio.h>
#include <stdlib.h>
#include <string.h>		//memcpy
#include <stdbool.h>

#include "prng.h"
#include "rijndael.h"

// variables for AES
static unsigned char iv[16];	// 128 bit initial vector for output feedback mode
static int bits_consumed = 128;	// number of already used bits out of one random block
RIJNDAEL_context ctx;


// variables for DC-Pseudo Random Number Generator
static unsigned int dc_iv;				// 128 bit initial vector for dc-prng
static unsigned char randValues[16];	// dc random block
static int dc_bits_consumed = 128;		// number of already used bits out of one random block
//static unsigned char ringBuffer[16];	// dc ringbuffer
//static unsigned char* riBuWrite=ringBuffer;
//static unsigned char* riBuRead=ringBuffer;

// Functions for the DC- Pseudo Random Number Generator

/*
 * Creates a new number block for DC-Values
 * Authors: Andreas Unterweger
 * 			Alexander Bliem
 */
void dc_NewRandomNumberBlock() {

	int i,j=0;

	// init array
	for (i = 0 ; i < 16 ; i++)
		randValues[i] = 0;

	// save rand values in array
	for(i=0 ; i < 16 ; i++){
		if(i%2 == 0){
			dc_iv = rand() % 65536;	 //returns a random number out of the space 2^16
			j=8;
		}
		randValues[i] = dc_iv >> (j);
		j=0;
	}
}

/*
 * Get a random bit for DC-Values
 * returns: a single random bit
 * Author: Andreas Unterweger
 */
unsigned char dc_GetRandomBit() {

	// do we need new values?
	if (dc_bits_consumed == 128) {
		dc_NewRandomNumberBlock();
		dc_bits_consumed = 0;
	}

	// get random bit
	unsigned char retval = (randValues[dc_bits_consumed / 8] & (1 << (dc_bits_consumed % 8)))
			>> (dc_bits_consumed % 8);

	dc_bits_consumed++;
	return retval;
}

/*
 * Get a random number for DC-Values
 * E.g. dc_GetRandomNumber(8) will return values from 0 to 255
 * in n_bits: number of bits for a random number.
 * returns: a random number
 * Author: Andreas Unterweger
 */
inline unsigned int dc_GetRandomNumber(unsigned char n_bits) {
	unsigned long long retval = 0;
	int i;

	// get random bits and "build" random value
	for (i = 0; i < n_bits; i++) {
		retval <<= 1;
		retval |= dc_GetRandomBit();
	}
	return retval;
}

/*
 * Get a random byte value for DC-Values
 * Author: Andreas Unterweger
 */
/*
 unsigned char dc_GetRandomByte() {
	return dc_GetRandomNumber(8);
}
*/

void initDcPrng(){

	// set static key
	dc_iv = 112358;

	//set consumed bits to 128 => new random block has to be generated
	dc_bits_consumed = 128;

	// init srand using dc initial vector
	srand(dc_iv);
}


// Functions for the AES- Pseudo Random Number Generator

/*
 * Creates a new number block
 * Author: Andreas Unterweger
 */
 void NewRandomNumberBlock() {

	int i;
	unsigned char tmp[16];

	// init array
	for (i = 0; i < 16; i++)
		tmp[i] = 0;

	// encrypt and save value in tmp again
	block_encrypt(&ctx, tmp, sizeof(tmp), tmp, iv);
	//rijndael_encrypt(&ctx, iv, tmp);

	//Copy value back to IV for reuse (simulate OFB mode)
	memcpy(iv, tmp, 128 / 8);

}

/*
 * Get a random bit
 * returns: a single random bit
 * Author: Andreas Unterweger
 */
inline unsigned char GetRandomBit() {

	// do we need new values?
	if (bits_consumed == 128) {
		NewRandomNumberBlock();
		bits_consumed = 0;
	}

	// get random bit
	unsigned char retval = (iv[bits_consumed / 8] & (1 << (bits_consumed % 8)))
			>> (bits_consumed % 8);

	bits_consumed++;
	return retval;
}

/*
 * Get a random number
 * E.g. GetRandomNumber(8) will return values from 0 to 255
 * in n_bits: number of bits for a random number.
 * returns: a random number
 * Author: Andreas Unterweger
 */
inline unsigned int GetRandomNumber(unsigned char n_bits) {
	unsigned long long retval = 0;
	int i;

	// get random bits and "build" random value
	for (i = 0; i < n_bits; i++) {
		retval <<= 1;
		retval |= GetRandomBit();
	}
	return retval;
}

/*
 * Get a random byte value
 * Author: Andreas Unterweger
 */
inline unsigned char GetRandomByte() {
	return GetRandomNumber(8);
}


void initAesPrng(unsigned char* keyArray, int keyArraySize){

	unsigned char tempKey[16];
	int i;
	//set initial vector
	unsigned char tempIV[] = {0xFF, 0xDC, 0xBA, 0x98, 0x76, 0x54, 0x32, 0x10, 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF};
	memcpy(iv, tempIV, 128/8);	// 128 bit / 8 = 16 byte

	//set consumed bits to 128 => new random block has to be generated
	bits_consumed = 128;

	// init array
	for (i = 0; i < 16; i++)
		tempKey[i] = 0;

	//check key
	if(keyArraySize <= 16)
		memcpy(tempKey, keyArray, keyArraySize);
	else
		memcpy(tempKey, keyArray, 16);

	//check for max allowed key size
	if (keyArraySize != 16)
		keyArraySize = 16;

	// set mode to OFB
	ctx.mode = MODE_OFB;

	// set key as string
	//char * keyString = "KeyStringKeyString";  // 16 byte

	//copy string to key
	//memcpy(key, keyString, keysize);

	// init rijndael (=AES)
	rijndael_setup(&ctx, keyArraySize, tempKey);
}

