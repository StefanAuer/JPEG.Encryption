

#ifndef PRNG_H_
#define PRNG_H_



void initDcPrng();
unsigned int dc_GetRandomNumber(unsigned char n_bits);


/*
 * Get a random bit
 * returns: a single random bit
 * Author: Andreas Unterweger
 */
unsigned char GetRandomBit(void);

/*
 * Get a random number
 * E.g. GetRandomNumber(8) will return values from 0 to 255
 * in n_bits: number of bits for a random number.
 * returns: a random number
 * Author: Andreas Unterweger
 */
unsigned int GetRandomNumber(unsigned char n_bits);

/*
 * Get a random byte value
 * Author: Andreas Unterweger
 */
unsigned char GetRandomByte();


/*
 * initialize the AES pseudo random number generator
 * using an internal initial vector and the arguments keyArray and keyArraySize
 */
void initAesPrng(unsigned char* keyArray, int keyArraySize);

#endif /* PRNG_H_ */
