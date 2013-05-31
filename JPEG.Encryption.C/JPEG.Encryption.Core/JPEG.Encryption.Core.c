/**
 * @file   JPEG.Encryption.Core.c
 *
 * @author Alexander Bliem abliem.itsb-m2012@fh-salzburg.ac.at
 * @author Stefan Auer sauer.itsb-m2012@fh-salzburg.ac.at
 *
 * @date   31.10.2012
 * @version 1.0
 *
 * @brief This module contains the main() function, and calls the needed function for \n
 * JPEG encryption and decryption.
 *
 * detailed description of file.
 */

#include "JPEG.Encryption.Core.h"
#include "JPEGparser.h"
#include "prng.h"

#include <stdio.h>
#include <stdlib.h>
#include <stdbool.h>

#include <string.h>		//strcat
#include <dirent.h>		//directory file handling


#define AES_KEY_SIZE				16		// AES key has to be 128 bit = 16 byte
#define NUM_SUPP_ROI				16*4	// 16 supported RoIs
#define NO_CRYPT					0
#define ENCRYPT						1
#define DECRYPT						2

#define SWAP_BLOCKS					1
#define SWAP_CWV_PAIRS				2
#define SWAP_VALUES					4
#define SWAP_DC						8

#define PRINT_INFO_CONSOLE 			0


/**
* 	@fn int main(int argc, char* argv[])
*	@brief
*   @param[out]
* 	@param[in] argc, argv[]
* 	@return 0
*/
int main (int argc, char* argv[]){

	FILE *fpImageIn=NULL;		// JPEG read => in
	FILE *fpImageOut=NULL;		// JPEG write => out
    long sizeIn, sizeOut;
    unsigned char *bufferIn=NULL, *bufferOut=NULL;

    unsigned char key[AES_KEY_SIZE+1];
    for(int i=0 ; i<AES_KEY_SIZE ; i++)
    	key[i]='\0';
    int keyLen=0;

    //int cryptoDetail = SWAP_BLOCKS + SWAP_CWV_PAIRS + SWAP_VALUES + SWAP_DC;	//do all four encryption steps
    int cryptoDetail = 0;
    int cryptoMode = NO_CRYPT;

    int roiArraySize = 0 ;	// 0..there is no roi, the whole pic is encrypted
    int roiArray[NUM_SUPP_ROI];		// each roi contains x, y, width and height (in pixels) stored in int array

	setbuf(stdout, NULL);


	//START process argument counter and vector
	if (argc < 6)	{
		printf("missing arguments, see help!\n");
		myHelp();
		return 2;
	}

	if (argc >= 6)	{

		// JPEG in
		fpImageIn = fopen(argv[1],"rb");
		if (fpImageIn == NULL){
			printf("\nError reading JPEG-in-file %s!\n",argv[1]);
			exit(1);
		}

		// JPEG out
		fpImageOut = fopen(argv[2],"wb");
		if (fpImageOut == NULL)	{
			printf("\nCould not create JPEG-out-file!\n");
			exit(1);
		}

		// password
		keyLen=0;
		while(argv[3][keyLen] != '\0'){
			key[keyLen] = argv[3][keyLen];
			keyLen++;
			if (keyLen>AES_KEY_SIZE)
				break;
		}

		// en- or decrypt
		if (argv[4][0] == 'e')
			cryptoMode = ENCRYPT;
		else if (argv[4][0] == 'd')
			cryptoMode = DECRYPT;
		else{
			printf("\nWrong crypto mode parameter");
			myHelp();
			exit(1);
		}

		// crypto detail
		if (atoi(argv[5]) <= 15)
			cryptoDetail = atoi(argv[5]);
		else{
			printf("\nWrong crypto detail parameter");
			myHelp();
			exit(1);
		}

		//check for roi
		if (argc > 6){
			int roiValues = argc-6-1;
			if(roiValues % 4 == 0){
				roiArraySize = atoi(argv[6]);
				if (roiArraySize > NUM_SUPP_ROI){
					printf("\nToo much ROIs. Max %i ROIs supported.",NUM_SUPP_ROI/4);
					exit(1);
				}
				for (int i=0 ; i < roiArraySize ; i++){
					roiArray[i] = atoi(argv[7+i]);
				}
			}
			else{
				printf("\nWrong number of ROI values");
				myHelp();
				exit(1);
			}
		}
	}
	//END process argument counter and vector


	// read in file to bufferIn and close in file
	fseek(fpImageIn, 0, SEEK_END);
	sizeIn = (int) ftell(fpImageIn);
	bufferIn = malloc(sizeIn);
	fseek(fpImageIn, 0, SEEK_SET);
	fread(bufferIn, 1, sizeIn, fpImageIn);
	fclose(fpImageIn);

	// allocate buffer for output jpg file
	// encrypted output buffer can be more than input buffer => *2
	sizeOut = sizeIn * 2;
	bufferOut = calloc(sizeOut, sizeof(unsigned char));	//Allocates a block of memory and initializes all its bits to zero


	// start crypto
	initAesPrng(key, AES_KEY_SIZE);
	if (jpegCrypto(bufferIn, bufferOut, sizeIn, &sizeOut, roiArray, &roiArraySize, cryptoDetail, cryptoMode)) {
		printf("\nError encrypting the input file");
		return 1;
	}

	#if (PRINT_INFO_CONSOLE)
	printf("\nJPEG parsed, size (width x height) %i x %i",njGetWidth(), njGetHeight());
	printf("\nJPEG in Buffer size:\t%i Bytes", (int)njGetOriginalImageBufferSize());
	printf("\nJPEG out Buffer size:\t%i Bytes\n", (int)njGetCryptoImageBufferSize());
	#endif


	// write to bufferout to output JPEG file
	fwrite(bufferOut,sizeof(char),sizeOut,fpImageOut);
	fclose(fpImageOut);

	// free ...
	free(bufferIn);
	free(bufferOut);

	return 0;
}



/**
* 	@fn void myHelp(void)
*	@brief Prints a little help, how to call the encryption core and all the possible input parameters
*   @param[out]
* 	@param[in] argc, argv[]
* 	@return void
*/
void myHelp(void)
{
	printf("\nJPEG.Encryption.Core.exe FileIn.jpg FileOut.jpg password CryptoMode CryptoDetail [sizeOfROIArray x1 y1 w1 h1 x2 y2 w2 h2 ...] \n");

}




