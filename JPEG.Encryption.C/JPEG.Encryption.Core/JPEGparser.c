/**
 * @file   JPEGparser.c
 * @author Alexander Bliem abliem.itsb-m2012@fh-salzburg.ac.at
 * @author Stefan Auer sauer.itsb-m2012@fh-salzburg.ac.at
 * @date   01.07.2013
 * @version 1.0
 * @brief This is a minimal decoder for baseline JPEG images and includes a JPEG en- and
 * decryption algorithm, based on an AES OFB_Mode pseudo number generator.
 * It accepts memory dumps of JPEG files as input and generates a memory dump of
 * the encrypted JPEG file. All YCbCr subsampling schemes with power-of-two ratios are
 * supported, as are restart intervals. Progressive or lossless JPEG is not
 * supported.
 * The JPEG decoder is based on the nanoJPEG open source project.
 *
 * NanoJPEG -- KeyJ's Tiny Baseline JPEG Decoder
 * version 1.3 (2012-03-05)
 * by Martin J. Fiedler <martin.fiedler@gmx.net>
 *
 * This software is published under the terms of KeyJ's Research License,
 * version 0.2. Usage of this software is subject to the following conditions:
 *  0. There's no warranty whatsoever. The author(s) of this software can not
 *     be held liable for any damages that occur when using this software.
 *  1. This software may be used freely for both non-commercial and commercial
 *     purposes.
 *  2. This software may be redistributed freely as long as no fees are charged
 *     for the distribution and this license information is included.
 *  3. This software may be modified freely except for this license information,
 *     which must not be changed in any way.
 *  4. If anything other than configuration, indentation or comments have been
 *     altered in the code, the original author(s) must receive a copy of the
 *     modified code.
 */


#include <stdlib.h>
#include <string.h>
#include "JPEGparser.h"
#include "prng.h"
#include <stdio.h>


// the following structures are used to save the data of one entire mcu
typedef struct _RLE_Element{	// contains one code-word-value-pair with code size and value size
	unsigned int codeValuePair;
  	unsigned char sizeCode;
   	unsigned char sizeValue;
}RLE_Element_t;

typedef struct _RLE{			// contains all entropy coded cwv-pairs of one block
  	RLE_Element_t rle[64];
   	int nUsedElements;
}RLE_t;

typedef struct _comp{			// contains all blocks of one component
   	RLE_t block[4];
   	int nUsedBlocks;
}comp_t;

typedef struct _mcu{			// contains all components of one mcu
   	comp_t comp[4];
   	int nUsedComp;
}mcu_t;



// contains component reference data
typedef struct _nj_cmp {
    int cid;				//component id
    int ssx, ssy;			//subsampling
    int width, height;
    int stride;
    int qtsel;				// quantizer tabel id for this comp
    int actabsel, dctabsel;	// ac and dc tabel id for this comp
    //int dcpred;				// contains the dc-value of the last comp
} nj_component_t;



// dc and ac huffman tables (refers to notings.org)
#define FAST_BITS   9  // larger handles more cases; smaller stomps less cache

typedef struct{
	unsigned char  fast[1 << FAST_BITS];
	unsigned short code[256];
	unsigned char  values[256];
	unsigned char  size[257];
  	unsigned int maxcode[18];
	int    delta[17];   // old 'firstsymbol' - old 'firstcode'
} huffman;



typedef struct _nj_ctx {
    nj_result_t error;			// error code

    const unsigned char *pos;	// points to a position in the JPEG-in-buffer
    int size;					// size of the remaining JPEG-in-buffer
    int initialSize;			// size of the JPEG-in-buffer

    unsigned char *posOut;		// points to a position in the JPEG-out-buffer
    unsigned char *posIn;		// points always to the first byte of the JPEG-in-buffer
    int sizeOut;				// size of the JPEG-out-buffer (varies from the size of the JPEG-in-buffer) (0xFF-Problem)

    int buf, bufbits;			// buf and bufbits is used to read single bits out of the JPEG-in-buffer
    int length;					// contains the data length of the last found JPEG-header
    int width, height;			// image width respectively height
    int mbwidth, mbheight;		// how many mcu per width respectively height
    int mbsizex, mbsizey;		// size of one mcu (blocks x blocks)
    int ncomp;					// number of components e.g. ncomp = 3 (Y Cb Cr)
    nj_component_t comp[4];		// component data
    //int qtused, qtavail;		// not used
    //unsigned char qtab[4][64];// not used

    //int block[64];			// not used
    huffman huff_dc[4];			// contains the dc huffman tables (refers to nothings.org)
    huffman huff_ac[4];			// contains the ac huffman tables (refers to nothings.org)
    mcu_t mcu;					// contains the data from entire mcu for en/decrypting

    int bitIndex;				// contains the bit index for the current JPEG-out-buffer-byte

    unsigned char inRoiFlag;	// indicates if the current mcu is inside the roi and thus must be en/decrypted
    int* roiArray;				// defines the position of the rois (x1,y1,w1,h1,x2,y2,w2,h2,...)
    int roiArraySize;			// defines the number of rois in roiArray (4...one roi, x,y,w,h)
    int cryptoMode;				// 0...NO_CRYPTO, 1...ENCRYPTION, 2...DECRYPTION

    // the following 4 variables define the en/decryption detail 0...no swap
    int swapBlock;				// 1...blocks inside mcu are swapped
    int swapCWV_block;			// 1...the ac-code-word-value pairs inside one block is swapped
    int swapValues;				// 1...scramble the value bits of the ac-cwv-pairs
    int swapDC;					// 1...scramble the value bits of the dc-cmw-pairs

    int rstinterval;			// needed for JPEGs with restart interval
} nj_context_t;


// global variable for JPEG-handling
static nj_context_t nj;

// is mask for n rightmost bits (1<<n)-1
static unsigned int bmask[17]={0,1,3,7,15,31,63,127,255,511,1023,2047,4095,8191,16383,32767,65535};

// error reporting
#define njThrow(e) do { nj.error = e; return; } while (0)
#define njCheckError() do { if (nj.error) return; } while (0)

// for dc correction
#define NUM_OF_DC_VAL_TO_CORR	16		// number of max dc values which have to be corrected
#define DC_MAX_VALUE 			2047	// max allowed DC-value for correction (JPEG Spec Table F.1.)

//#define DC_MAX_VALUE_CRYPTO 	2047	// max allowed DC-value during en/decryption
#define DC_MAX_VALUE_CRYPTO 	1023	// max allowed DC-value during en/decryption
//#define DC_MAX_VALUE_CRYPTO 	255		// max allowed DC-value during en/decryption

#define DC_MAX_VALUE_LEN	7			// max length of DC magnitude (JPEG Spec. Table F.1.)
										// 7 bits => -127...-64, 64...127

/**
* 	@fn static int njShowBits(int x)
*	@brief returns the next x bits of the JPEG-in-buffer and handle markers
* 	@param[in] x number of bits to show
* 	@return requestedBits
*/
static int njShowBits(int bits) {
    unsigned char newbyte;
    if (!bits) return 0;
    while (nj.bufbits < bits) {
        if (nj.size <= 0) {
            nj.buf = (nj.buf << 8) | 0xFF;
            nj.bufbits += 8;
            continue;
        }
        newbyte = *nj.pos++;
        nj.size--;
        nj.bufbits += 8;
        nj.buf = (nj.buf << 8) | newbyte;
        if (newbyte == 0xFF) {
            if (nj.size) {
                unsigned char marker = *nj.pos++;
                nj.size--;
                switch (marker) {
                	case 0x00:
                    case 0xFF: 	break;

                    case 0xD9: 	break;

                    default:
                        if ((marker & 0xF8) != 0xD0)
                            nj.error = NJ_SYNTAX_ERROR;
                        else {
                            nj.buf = (nj.buf << 8) | marker;
                            nj.bufbits += 8;
                        }
                        break;
                }
            }
            else
                nj.error = NJ_SYNTAX_ERROR;
        }
    }
    return (nj.buf >> (nj.bufbits - bits)) & ((1 << bits) - 1);
}


/**
* 	@fn static inline void njSkipBits(int x)
*	@brief skip x bits of the bufferd bits
* 	@param[in] void
* 	@return res x bits are returned
*/
static inline void njSkipBits(int bits) {
    if (nj.bufbits < bits)
        (void) njShowBits(bits);
    nj.bufbits -= bits;

}

/**
* 	@fn static inline int njGetBits(int x)
*	@brief returns the next x bits of the JPEG-in-buffer, and will skip the amount of returned bits
* 	@param[in] int x number of requested bits
* 	@return res x bits are returned
*/
static inline int njGetBits(int bits) {
    int res = njShowBits(bits);
    njSkipBits(bits);
    return res;
}

/**
* 	@fn static inline void njByteAlign(void)
*	@brief align byte for restart interval
* 	@param[in] void
* 	@return void
*/
static inline void njByteAlign(void) {
    nj.bufbits &= 0xF8;
}

/**
* 	@fn static void njSkip(int count)
*	@brief skip the bits from JPEG-in-buffer and decreases the JPEG-in-buffer size
* 	@param[in] count number of bits to skip
* 	@return void
*/
static void njSkip(int count) {
    nj.pos += count;
    nj.size -= count;
    nj.length -= count;
    if (nj.size < 0) nj.error = NJ_SYNTAX_ERROR;
}

/**
* 	@fn static inline void copyJpegInToOutSOS()
*	@brief copy the JPEG-header-data from the JPEG-in-buffer to the JPEG-out-buffer
* 	@param[in] void
* 	@return void
*/
static inline void copyJpegInToOutSOS(){
	nj.sizeOut = nj.initialSize-nj.size;
	memcpy(nj.posOut, nj.posIn, nj.sizeOut);
	nj.posOut += (nj.sizeOut);
}


/**
* 	@fn static inline unsigned short njDecode16(const unsigned char *pos)
*	@brief copy the JPEG-header-data from the JPEG-in-buffer to the JPEG-out-buffer
* 	@param[in] const unsigned char *pos position in the JPEG-in-buffer
* 	@return unsigned short returns 2 byte from JPEG-in-buffer
*/
static inline unsigned short njDecode16(const unsigned char *pos) {
    return (pos[0] << 8) | pos[1];
}

/**
* 	@fn static void njDecodeLength(void)
*	@brief decodes the length of a JPEG-header after a marker is detected
* 	@param[in] void
* 	@return void
*/
static void njDecodeLength(void) {
    if (nj.size < 2) njThrow(NJ_SYNTAX_ERROR);
    nj.length = njDecode16(nj.pos);
    if (nj.length > nj.size) njThrow(NJ_SYNTAX_ERROR);
    njSkip(2);
}


/**
* 	@fn static inline void njSkipMarker(void)
*	@brief skip the size of a JPEG marker
* 	@param[in] void
* 	@return void
*/
static inline void njSkipMarker(void) {
    njDecodeLength();
    njSkip(nj.length);
}

/**
* 	@fn static inline void scaleROIs(void)
*	@brief after the SOF-Marker is read, the roi position in Pixel is converted to full mcu areas
* 	@param[in] void
* 	@return void
*/
static inline void scaleROIs(void){
	int i;
	if(nj.roiArraySize % 4 != 0){
		nj.roiArraySize = 0;	//if invalid or no ROI, the size = 0 => the whole pic is encrypted
	}
	else{
		for(i=0 ; i < nj.roiArraySize ; i=i+4){

			nj.roiArray[0+i] = (nj.roiArray[0+i] / (8*nj.comp[0].ssx))-1;	//-1 mcu to make sure, that the encrypted roi is larger than the committed pixel roi
			if(nj.roiArray[0+i] < 0)
				nj.roiArray[0+i] = 0;

			nj.roiArray[1+i] = (nj.roiArray[1+i] / (8*nj.comp[0].ssy))-1;	//-1 mcu to make sure, that the encrypted roi is larger than the committed pixel roi
			if(nj.roiArray[1+i] < 0)
				nj.roiArray[1+i] = 0;

			nj.roiArray[2+i] = nj.roiArray[0+i] + (nj.roiArray[2+i] / (8*nj.comp[0].ssx)) + 2;	//+2: +1 to compensate -1, +1 to make sure that encrypted roi is larger than committed pixel roi
			if(nj.roiArray[2+i] > nj.mbwidth)
				nj.roiArray[2+i] = nj.mbwidth;

			nj.roiArray[3+i] = nj.roiArray[1+i] + (nj.roiArray[3+i] / (8*nj.comp[0].ssy)) + 2;	//+2: +1 to compensate -1, +1 to make sure that encrypted roi is larger than committed pixel roi
			if(nj.roiArray[3+i] > nj.mbheight)
				nj.roiArray[3+i] = nj.height;
		}
	}
}

/**
* 	@fn static inline void njDecodeSOF(void)
*	@brief decode the SOF-Marker, This header specifies the source image characteristics, the components in the frame,
*	and the sampling factors for each component, and specifies the quantized tables destinations
* 	@param[in] void
* 	@return void
*/
static inline void njDecodeSOF(void) {
    int i, ssxmax = 0, ssymax = 0;
    nj_component_t* c;
    njDecodeLength();
    if (nj.length < 9) njThrow(NJ_SYNTAX_ERROR);
    if (nj.pos[0] != 8) njThrow(NJ_UNSUPPORTED);
    nj.height = njDecode16(nj.pos+1);
    nj.width = njDecode16(nj.pos+3);
    nj.ncomp = nj.pos[5];
    njSkip(6);
    switch (nj.ncomp) {
        case 1:
        case 3:
            break;
        default:
            njThrow(NJ_UNSUPPORTED);
            break;
    }
    if (nj.length < (nj.ncomp * 3)) njThrow(NJ_SYNTAX_ERROR);
    for (i = 0, c = nj.comp;  i < nj.ncomp;  ++i, ++c) {
        c->cid = nj.pos[0];
        if (!(c->ssx = nj.pos[1] >> 4)) njThrow(NJ_SYNTAX_ERROR);
        if (c->ssx & (c->ssx - 1)) njThrow(NJ_UNSUPPORTED);  // non-power of two
        if (!(c->ssy = nj.pos[1] & 15)) njThrow(NJ_SYNTAX_ERROR);
        if (c->ssy & (c->ssy - 1)) njThrow(NJ_UNSUPPORTED);  // non-power of two
        if ((c->qtsel = nj.pos[2]) & 0xFC) njThrow(NJ_SYNTAX_ERROR);
        njSkip(3);
        //nj.qtused |= 1 << c->qtsel;
        if (c->ssx > ssxmax) ssxmax = c->ssx;
        if (c->ssy > ssymax) ssymax = c->ssy;
    }
    if (nj.ncomp == 1) {
        c = nj.comp;
        c->ssx = c->ssy = ssxmax = ssymax = 1;
    }
    nj.mbsizex = ssxmax << 3;
    nj.mbsizey = ssymax << 3;
    nj.mbwidth = (nj.width + nj.mbsizex - 1) / nj.mbsizex;
    nj.mbheight = (nj.height + nj.mbsizey - 1) / nj.mbsizey;
    for (i = 0, c = nj.comp;  i < nj.ncomp;  ++i, ++c) {
        c->width = (nj.width * c->ssx + ssxmax - 1) / ssxmax;
        c->stride = (c->width + 7) & 0x7FFFFFF8;
        c->height = (nj.height * c->ssy + ssymax - 1) / ssymax;
        c->stride = nj.mbwidth * nj.mbsizex * c->ssx / ssxmax;
        if (((c->width < 3) && (c->ssx != ssxmax)) || ((c->height < 3) && (c->ssy != ssymax))) njThrow(NJ_UNSUPPORTED);
    }
    njSkip(nj.length);

    //---validate the ROIs
    scaleROIs();
}

/**
* 	@fn static inline void build_huffman(huffman *h, unsigned char *count)
*	@brief builds the ac and dc huffman tables (refers to nothings.org)
* 	@param[in] huffman *h, unsigned char *count
* 	@return void
*/
static inline void build_huffman(huffman *h, unsigned char *count)
{
   int i,j,k=0,code;
   // build size list for each symbol (from JPEG spec)
   for (i=0; i < 16; ++i)
      for (j=0; j < count[i]; ++j)
         h->size[k++] = (unsigned char) (i+1);
   h->size[k] = 0;

   // compute actual symbols (from jpeg spec)
   code = 0;
   k = 0;
   for(j=1; j <= 16; ++j) {
      // compute delta to add to code to compute symbol id
      h->delta[j] = k - code;
      if (h->size[k] == j) {
         while (h->size[k] == j)
            h->code[k++] = (unsigned short) (code++);
         //if (code-1 >= (1 << j)) return e("bad code lengths","Corrupt JPEG");
         if (code-1 >= (1 << j)) njThrow(NJ_UNSUPPORTED);
      }
      // compute largest code + 1 for this size, preshifted as needed later
      h->maxcode[j] = code << (16-j);
      code <<= 1;
   }
   h->maxcode[j] = 0xfffffff;

   // build non-spec acceleration table; 255 is flag for not-accelerated
   memset(h->fast, 255, 1 << FAST_BITS);
   for (i=0; i < k; ++i) {
      int s = h->size[i];
      if (s <= FAST_BITS) {
         int c = h->code[i] << (FAST_BITS-s);
         int m = 1 << (FAST_BITS-s);
         for (j=0; j < m; ++j) {
            h->fast[c+j] = (unsigned char) i;
         }
      }
   }
}


/**
* 	@fn static inline void DecodeDHT(void)
*	@brief decode the DHT-Marker, and build the ac and dc huffman tables
* 	@param[in] void
* 	@return void
*/
static inline void DecodeDHT(void){
    int valueCnt;
    unsigned char i, huffTabClass, huffTabDestId, * huffTabPointer;
    static unsigned char dhtCodesCountPerBitLen[16];
    njDecodeLength();
    while(nj.length>0){	//Parse through whole tables
    	valueCnt=0;
    	huffTabDestId = nj.pos[0];
        if (huffTabDestId & 0xEC) njThrow(NJ_SYNTAX_ERROR);
        if (huffTabDestId & 0x02) njThrow(NJ_UNSUPPORTED);

        //Table Class 0..DC 1..AC
        huffTabClass = huffTabDestId >> 4;

        //Table ID defines destination (four possible destinations)
        huffTabDestId = huffTabDestId & 0x0F;

        //read the number of Huffman codes for each of the 16 possible codeword lengths
        for (i = 1;  i <= 16;  ++i){
        	dhtCodesCountPerBitLen[i-1] = nj.pos[i];
        	valueCnt += dhtCodesCountPerBitLen[i-1];
        }
        njSkip(17);

        // build Huffman table
        if(huffTabClass == 0){
        	build_huffman(nj.huff_dc+huffTabDestId, dhtCodesCountPerBitLen);
        	huffTabPointer = nj.huff_dc[huffTabDestId].values;
        }
        else{
        	build_huffman(nj.huff_ac+huffTabDestId, dhtCodesCountPerBitLen);
        	huffTabPointer = nj.huff_ac[huffTabDestId].values;
        }

        // fill Huffman table
        for (i = 0 ; i < valueCnt ; i++){
        	huffTabPointer[i] = nj.pos[i];
        }
        njSkip(valueCnt);
    }
    if (nj.length) njThrow(NJ_SYNTAX_ERROR);
}


/**
* 	@fn static inline void njDecodeDRI(void)
*	@brief decode the DRI-Marker, and set the restart interval to nj.rstinterval
* 	@param[in] void
* 	@return void
*/

static inline void njDecodeDRI(void) {
    njDecodeLength();
    if (nj.length < 2) njThrow(NJ_SYNTAX_ERROR);
    nj.rstinterval = njDecode16(nj.pos);
    njSkip(nj.length);
}



/**
* 	@fn static int getHuffCWVPair(huffman* huffTbl, unsigned char* c, RLE_Element_t* pElement)
*	@brief This function decodes the entropy coded JPEG-data and saves the code-word-value pair (CWVP) and the code size and value size.
*	The implementation is based on nanoJPEG and www.nothings.org
*	CWVP = the run-length coded symbols together with their corresponding coefficient values
*	Therefore the Huffman table huffTbl is used and the data are saved into the run length coded element pElement.
*	If a dc- coefficient is decoded the pointer c must be NULL.
*	If an ac- coefficient is decoded the pointer is valid and the code from the cwv-pair is assigned to it.
* 	@param[in] huffman* huffTbl, unsigned char* c, RLE_Element_t* pElement
* 	@return int value 0..an error occurs, 1..ok
*/
static int getHuffCWVPair(huffman* huffTbl, unsigned char* c, RLE_Element_t* pElement) {
    int size, k, code, cwvPair=0;
    int value = njShowBits(16);

    // look at the top FAST_BITS and determine what symbol ID it is,
    // if the code is <= FAST_BITS
    code = (value >> (16 - FAST_BITS)) & ((1 << FAST_BITS)-1);
    k = huffTbl->fast[code];
    if (k < 255) {
       size = huffTbl->size[k];
       if (!size) { nj.error = NJ_SYNTAX_ERROR; return 0; }
       njSkipBits(size);
       cwvPair = (value >> (16-size)) & bmask[size];
       pElement->sizeCode=size;
       value = huffTbl->values[k];

    }
    else{
    	for (k=FAST_BITS+1 ; ; ++k){
    		if (value < huffTbl->maxcode[k])
    			break;	//k found
    	}
    	if (k == 17) { // error! code not found
    		njSkipBits(16);
    		return -1;
    	}
    	if (k > value)
    		return -1;
    	    // convert the huffman code to the symbol id
    	code = ((value >> (16 - k)) & bmask[k]) + huffTbl->delta[k];
    	njSkipBits(k);
    	cwvPair = (value >> (16-k)) & bmask[k];
    	pElement->sizeCode=k;
    	value = huffTbl->values[code];
    }

    if (c) *c = (unsigned char)value;	//if DC-coefficient is decoded than c is NULL

    size = value & 15;
    if(!size) {
    	pElement->codeValuePair = cwvPair;
    	pElement->sizeValue = 0;
    	return 1;
    }

    value = njGetBits(size);

    pElement->sizeValue = size;
    cwvPair = (cwvPair << size) | value;
    pElement->codeValuePair = cwvPair;
    return 1;
}



/**
* 	@fn static inline void decodeBlock(nj_component_t* c, RLE_t* pBlock)
*	@brief This function decodes an block using the huffmantable for the component c and
*	saves the data for one entire block into the run length coded element pBlock.
*	The code from the decoded code-word-value-pair is used to control the while loop.
* 	@param[in] nj_component_t* c, RLE_t* pBlock
* 	@return void
*/
static inline void decodeBlock(nj_component_t* c, RLE_t* pBlock) {
    unsigned char code = 0;
    //int value;
    int coef = 0;
    pBlock->nUsedElements = 0;


    getHuffCWVPair(&nj.huff_dc[c->dctabsel], NULL, &pBlock->rle[0]);
    pBlock->nUsedElements = 1;

    do {
    	getHuffCWVPair(&nj.huff_ac[c->actabsel], &code, &pBlock->rle[pBlock->nUsedElements]);
    	pBlock->nUsedElements++;
    	if (!code) break;  // EOB
        if (!(code & 0x0F) && (code != 0xF0)) njThrow(NJ_SYNTAX_ERROR);
        coef += (code >> 4) + 1;
        if (coef > 63) njThrow(NJ_SYNTAX_ERROR);
    } while (coef < 63);
}



/**
* 	@fn static inline void encodeMcu(void)
*	@brief This function encodes the de- or encrypted run length coded data into a valid JPEG.
*	The JPEG is encoded into the JPEG-out-buffer. This function causes the difference of the JPEG-in-buffer-size and the JPEG-out-buffer-size.
*	If an 0xFF occurs an 0x00 must be attached to it.
* 	@param[in] void
* 	@return void
*/
static inline void encodeMcu(void){
	RLE_Element_t* pElement;
	int comp, block, element;
	int bitInNextByte=0;

	for(comp=0 ; comp <= nj.mcu.nUsedComp-1; comp++){
		for(block=0 ; block <= nj.mcu.comp[comp].nUsedBlocks-1 ; block++){
			for(element=0 ; element <= nj.mcu.comp[comp].block[block].nUsedElements-1 ; element++){

				pElement = &nj.mcu.comp[comp].block[block].rle[element];

				bitInNextByte = nj.bitIndex + pElement->sizeCode + pElement->sizeValue - 8;

				while (bitInNextByte > 0){
					if((*nj.posOut++ |= (pElement->codeValuePair >> bitInNextByte)) == 0xFF){
						*nj.posOut++ = 0x00;		//if previous data byte is 0xff, following byte must be 0x00
						nj.sizeOut++;
					}
					bitInNextByte -= 8;
					nj.sizeOut++;
				}
				if((*nj.posOut |= (pElement->codeValuePair) << (-bitInNextByte)) == 0xFF){	//check if 0xff
					nj.posOut++;
					nj.sizeOut++;
					*nj.posOut = 0x00;
				}
				if(bitInNextByte == 0){
					nj.bitIndex=0;
					nj.posOut++;
					nj.sizeOut++;
				}
				else
					nj.bitIndex = 8+ bitInNextByte;		//bitInNextByte is always negative
			}
		}
	}
}


/**
* 	@fn static inline void addRSTm(int m)
*	@brief This function aligns the last byte and add the restart marker number m to the image
* 	@param[in] int m number of restart marker to add
* 	@return void
*/
static inline void addRSTm(int m){
	if(nj.bitIndex>0){
		*nj.posOut |= (0xff >> nj.bitIndex);
		nj.posOut++;
		nj.sizeOut++;
		nj.bitIndex=0;
	}
	*nj.posOut++ = 0xFF;
	nj.sizeOut++;
	*nj.posOut++ = (0xD0 + m);
	nj.sizeOut++;
}



/**
* 	@fn static inline void addEOI(void)
*	@brief This function aligns the last byte and add the end of image marker EOI = 0xFFD9
* 	@param[in] void
* 	@return void
*/
static inline void addEOI(void){
	if(nj.bitIndex>0){
		*nj.posOut |= (0xff >> nj.bitIndex);
		nj.posOut++;
		nj.sizeOut++;
		nj.bitIndex=0;
	}
	*nj.posOut++ = 0xFF;
	nj.sizeOut++;
	*nj.posOut++ = 0xD9;
	nj.sizeOut++;
}


/**
* 	@fn static inline int getValue(RLE_Element_t* pElement)
*	@brief This function returns the DC-value stored in the structure RLE_Element_t
* 	@param[in] RLE_Element_t* pElement one Run-Length-Encoded Element (codeword-value-pair, size of code and size of value) is stored in this structure
* 	@return int newDC DC-Value stored in structure RLE_Element_t
*/
static inline int getValue(RLE_Element_t* pElement){
	int newDC;
	newDC = (pElement->codeValuePair & bmask[pElement->sizeValue]);
	if (newDC < (1<<(pElement->sizeValue-1))){
		newDC += ((-1) << pElement->sizeValue) + 1;
	}
	return newDC;
}



/**
* 	@fn static inline void encryptMCU(void)
*	@brief This function is responsible for the JPEG encryption.
*	The block-, cwv-pair- and value- encryption is performed according to the flags: nj.swapBlock, nj.swapCWV_block, nj.swapValues and nj.swapDC.
* 	@param[in] void
* 	@return void
*/
static inline void encryptMCU(int* dcCorr){

	int comp, block, element;
	RLE_Element_t* pElement;

	//vars for dc-correction
	int dcValue, dcValueCrypto, i;
	//unsigned int tempCWVPair;

	unsigned int pos;

	// swap blocks inside each component, do not swap cb and cr blocks with each other
	if(nj.swapBlock){
		RLE_t* pBlockPos;
		RLE_t* pBlock;
		RLE_t tempBlock;

		for(comp=0 ; comp <= nj.mcu.nUsedComp-1 ; comp++){
			if(nj.mcu.comp[comp].nUsedBlocks > 1){//only swap if possible (more than 1 block)
				for(block=0 ; block <= nj.mcu.comp[comp].nUsedBlocks-1 ; block++){
					pos = GetRandomByte() % nj.mcu.comp[comp].nUsedBlocks;
					pBlock = &nj.mcu.comp[comp].block[block];
					pBlockPos = &nj.mcu.comp[comp].block[pos];
					tempBlock = *pBlockPos;
					*pBlockPos = *pBlock;
					*pBlock = tempBlock;
				}
			}
		}
	}

	// swap code-word-value pairs in each block
	if(nj.swapCWV_block){
		RLE_Element_t* pElementPos;
		RLE_Element_t temp;

		for(comp=0 ; comp <= nj.mcu.nUsedComp-1; comp++){
			for(block=0 ; block <= nj.mcu.comp[comp].nUsedBlocks-1 ; block++){
				if(nj.mcu.comp[comp].block[block].nUsedElements > 3){	// only swap if possible (2 and more ac-values)
					for(element=1 ; element <= nj.mcu.comp[comp].block[block].nUsedElements-2 ; element++){//do not swap 0 Element (DC-coeff) and do not swap last element (EOB)
						//modulo will limit the random number to the size of array, -2 regards DC and EOB Value,  1 (for loop start value) is used to ensure that DC value is not changed
						pos = (GetRandomByte() % (nj.mcu.comp[comp].block[block].nUsedElements-2))+1;
						pElement = &nj.mcu.comp[comp].block[block].rle[element];
						pElementPos = &nj.mcu.comp[comp].block[block].rle[pos];

						temp = *pElementPos;
						*pElementPos = *pElement;
						*pElement = temp;
					}
				}
			}
		}
	}


	//Toggle values from code-word-value-pairs
	i=0;
	if(nj.swapValues || nj.swapDC){
		for(comp=0 ; comp <= nj.mcu.nUsedComp-1; comp++){
			for(block=0 ; block <= nj.mcu.comp[comp].nUsedBlocks-1 ; block++){
				for(element=0 ; element <= nj.mcu.comp[comp].block[block].nUsedElements-1 ; element++){
					pElement = &nj.mcu.comp[comp].block[block].rle[element];
					if(element == 0){
						//encrypt DC-coefficient
						if(nj.swapDC){

							// if limit is exceeded the encryption is stopped, additionally the max dc diff between dc and newDc is limited using DC_MAX_VALUE_LEN
							if(dcCorr[i] < DC_MAX_VALUE_CRYPTO && dcCorr[i] > -DC_MAX_VALUE_CRYPTO){
								dcValue = getValue(pElement);

								if(pElement->sizeValue <= DC_MAX_VALUE_LEN)
									pElement->codeValuePair ^= (unsigned int)dc_GetRandomNumber((unsigned char)pElement->sizeValue);
								else
									pElement->codeValuePair ^= (unsigned int)dc_GetRandomNumber(DC_MAX_VALUE_LEN);

								dcValueCrypto = getValue(pElement);
								dcCorr[i] += dcValue - dcValueCrypto;
							}
							i++;
						}
					}
					else{
						//encrypt AC-coefficient
						if(nj.swapValues)
							pElement->codeValuePair ^= (unsigned int)GetRandomNumber((unsigned char)pElement->sizeValue);
					}
				}
			}
		}
	}
}//end encryptMCU

/**
* 	@fn static inline void decryptMCU(void)
*	@brief This function is responsible for the JPEG decryption.
*	The block-, cwv-pair- and value- decryption is performed according to the flags: j.swapBlock, nj.swapCWV_block, nj.swapValues and nj.swapDC.
*	Befor the decryption is performed the needed keys for the later decryption steps are generated. This ensures, that the order of the encryption steps is reverted.
* 	@param[in] void
* 	@return void
*/
static inline void decryptMCU(int* dcCorr){

	int keyBlockBuffer[4*4];
	int keyCWVBuffer[4*4*64];
	int keyBlockPos=0;
	int keyCWVPos=0;

	int comp, block, element;
	RLE_Element_t* pElement;
	unsigned int pos;

	//vars for dc-correction
	int dcValue, dcValueCrypto, i;

	//get Keys for later decryption steps
	if(nj.swapBlock){
		for(comp=0 ; comp <= nj.mcu.nUsedComp-1; comp++){
			if(nj.mcu.comp[comp].nUsedBlocks > 1){//only swap if possible (more than 1 block)
				for(block=0 ; block <= nj.mcu.comp[comp].nUsedBlocks-1 ; block++){
					//modulo will limit the random number to the size of array,
					keyBlockBuffer[keyBlockPos++] = GetRandomByte() % nj.mcu.comp[comp].nUsedBlocks;
				}
			}
		}
	}
	if(nj.swapCWV_block){
		for(comp=0 ; comp <= nj.mcu.nUsedComp-1; comp++){
			for(block=0 ; block <= nj.mcu.comp[comp].nUsedBlocks-1 ; block++){
				if(nj.mcu.comp[comp].block[block].nUsedElements > 3){	// only swap if possible (2 and more ac-values)
					for(element=1 ; element <= nj.mcu.comp[comp].block[block].nUsedElements-2 ; element++){	//do not swap 0 Element (DC-coeff) and do not swap last element (EOB)
						//modulo will limit the random number to the size of array, -2 regards DC and EOB Value, +1 is used to ensure that DC value is not changed
						keyCWVBuffer[keyCWVPos++] = (GetRandomByte() % (nj.mcu.comp[comp].block[block].nUsedElements-2))+1;
					}
				}
			}
		}
	}


	//Toggle values from code-word-value-pairs
	i=0;
	if(nj.swapValues || nj.swapDC){
		for(comp=0 ; comp <= nj.mcu.nUsedComp-1; comp++){
			for(block=0 ; block <= nj.mcu.comp[comp].nUsedBlocks-1 ; block++){
				for(element=0 ; element <= nj.mcu.comp[comp].block[block].nUsedElements-1 ; element++){
					pElement = &nj.mcu.comp[comp].block[block].rle[element];
					if(element == 0){
						//encrypt DC-coefficient
						if(nj.swapDC){

							// if limit is exceeded the encryption is stopped, additionally the max dc diff between dc and newDc is limited using DC_MAX_VALUE_LEN
							if(dcCorr[i] < DC_MAX_VALUE_CRYPTO && dcCorr[i] > -DC_MAX_VALUE_CRYPTO){
								dcValue = getValue(pElement);
								if(pElement->sizeValue <= DC_MAX_VALUE_LEN)
									pElement->codeValuePair ^= (unsigned int)dc_GetRandomNumber((unsigned char)pElement->sizeValue);
								else
									pElement->codeValuePair ^= (unsigned int)dc_GetRandomNumber(DC_MAX_VALUE_LEN);

								dcValueCrypto = getValue(pElement);
								dcCorr[i] += dcValue - dcValueCrypto;
							}
							i++;
						}
					}
					else{
						//encrypt AC-coefficient
						if(nj.swapValues)
							pElement->codeValuePair ^= (unsigned int)GetRandomNumber((unsigned char)pElement->sizeValue);
					}
				}
			}
		}
	}

	// swap code-word-value pairs in each block
	RLE_Element_t* pElementPos;
	RLE_Element_t temp;
	if(nj.swapCWV_block){
		for(comp=nj.mcu.nUsedComp-1 ; comp >= 0 ; comp--){
			for(block=nj.mcu.comp[comp].nUsedBlocks-1 ; block >= 0 ; block--){
				if(nj.mcu.comp[comp].block[block].nUsedElements > 3){	// only swap if possible (2 and more ac-values)
					for(element=nj.mcu.comp[comp].block[block].nUsedElements-2 ; element >= 1 ; element--){//do not swap 0 Element (DC-coeff) and do not swap last element (EOB)
						pos = keyCWVBuffer[--keyCWVPos];
						pElement = &nj.mcu.comp[comp].block[block].rle[element];
						pElementPos = &nj.mcu.comp[comp].block[block].rle[pos];
						temp = *pElementPos;
						*pElementPos = *pElement;
						*pElement = temp;
					}
				}
			}
		}
	}


	// swap blocks inside each component, do not swap cb and cr blocks with each other
	RLE_t* pBlockPos;
	RLE_t* pBlock;
	RLE_t tempBlock;
	if(nj.swapBlock){
		for(comp=nj.mcu.nUsedComp-1 ; comp >= 0 ; comp--){
			if(nj.mcu.comp[comp].nUsedBlocks > 1){	//only swap if possible (more than 1 block)
				for(block=nj.mcu.comp[comp].nUsedBlocks-1 ; block >= 0 ; block--){
					pos = keyBlockBuffer[--keyBlockPos];
					pBlock = &nj.mcu.comp[comp].block[block];
					pBlockPos = &nj.mcu.comp[comp].block[pos];
					tempBlock = *pBlockPos;
					*pBlockPos = *pBlock;
					*pBlock = tempBlock;
				}
			}
		}
	}
}//end decryptMCU




/**
* 	@fn static inline void setNewDCData(compCnt, blockCnt, newDCSize, newDC)
*	@brief This function encodes and set a new DC-Value, DC-Code and DC-Size to the current en/-decrypted MCU
* 	@param[in] 	compCnt		current component id
* 				blockCnt	current block id
* 				newDCSize	size of new DC-Value (needed bits)
* 				newDC		new DC-Value
*
* 	@return void
*/
static inline void setNewDCData(int compCnt, int blockCnt, int newDC){

	int absDC;
	int newDCSize=0;

	//if < 0 than absDC=-newDC and newDC is decreased,
	absDC = (newDC < 0) ? -newDC-- : newDC;
	while(absDC != 0){
		absDC >>= 1;
		newDCSize++;
	}
	newDC &= bmask[newDCSize];

	nj.mcu.comp[compCnt].block[blockCnt].rle[0].sizeValue = newDCSize;
	nj.mcu.comp[compCnt].block[blockCnt].rle[0].sizeCode = nj.huff_dc[nj.comp[compCnt].dctabsel].size[newDCSize];
	nj.mcu.comp[compCnt].block[blockCnt].rle[0].codeValuePair = ((nj.huff_dc[nj.comp[compCnt].dctabsel].code[newDCSize] << newDCSize) | newDC);
}



/**
* 	@fn static inline void DecodeScan(void)
*	@brief This function decodes the SOS-Marker (Start of Scan). After the SOS-header is read, the whole "JPEG-header" is copied from the in- to the out-buffer.
*	After that, all mcus are decoded and en-/decrypted if they are located inside the roi. Finally every single mcu is copied from the in- to the out-buffer.
*	This function also ensures the correction of the dc-error caused by the DC-encryption.
* 	@param[in] void
* 	@return void
*/
static inline void DecodeScan(void){
	int i, mbx, mby, sbx, sby, roiCnt;
	int rstcount = nj.rstinterval, nextrst = 0;
	int blockCnt=0, compCnt=0;
	nj_component_t* c;	//nj.comp is referenced to c, for easier coding
    RLE_t* pBlock;

	//variables for DC correction
	int corrNeeded=0;
	int DCcorrection[NUM_OF_DC_VAL_TO_CORR];
	int dcValue, newDCValue;

    for(i=0;i<NUM_OF_DC_VAL_TO_CORR;i++){
    	DCcorrection[i]=0;
    }

    // scan SOS header
    njDecodeLength();
    if (nj.length < (4 + 2 * nj.ncomp)) njThrow(NJ_SYNTAX_ERROR);
    if (nj.pos[0] != nj.ncomp) njThrow(NJ_UNSUPPORTED);
    njSkip(1);
    for (i = 0, c = nj.comp;  i < nj.ncomp;  ++i, ++c) {	//check the component specification parameter
        if (nj.pos[0] != c->cid) njThrow(NJ_SYNTAX_ERROR);
        if (nj.pos[1] & 0xEE) njThrow(NJ_SYNTAX_ERROR);
        c->dctabsel = nj.pos[1] >> 4;
        c->actabsel = nj.pos[1] & 0x0F;
        njSkip(2);
    }
    if (nj.pos[0] || (nj.pos[1] != 63) || nj.pos[2]) njThrow(NJ_UNSUPPORTED);	// check the spectral selection (start=0, end=63) and high(0) and low(0) successive approximation bit position
    njSkip(nj.length);

    // copy the "headers" of the JPEG-in-buffer to the JPEG-out-buffer
    copyJpegInToOutSOS();

    // handles every single mcu
    for (mbx = mby = 0;;) {

    	nj.inRoiFlag = 0;
    	if(nj.roiArraySize == 0)
    		nj.inRoiFlag = 1;
    	else{
    		for(roiCnt=0 ; roiCnt < nj.roiArraySize ; roiCnt = roiCnt + 4){
    			if((mbx > nj.roiArray[0+roiCnt]) && (mbx < nj.roiArray[2+roiCnt])){
    				if((mby > nj.roiArray[1+roiCnt]) && (mby < nj.roiArray[3+roiCnt])){
    					nj.inRoiFlag = 1;
    					break;	//enough if mcu is inside one roi
    				}
    			}
    		}
    	}
        compCnt=0;
    	for (i = 0, c = nj.comp;  i < nj.ncomp;  ++i, ++c){	//--- handle components in mcu
    		blockCnt=0;
    		for (sby = 0;  sby < c->ssy;  ++sby){			//--- handle y block
    			for (sbx = 0;  sbx < c->ssx;  ++sbx) {		//--- handle x block
    				pBlock = &nj.mcu.comp[compCnt].block[blockCnt];
    				blockCnt++;
    				decodeBlock(c, pBlock);
                	njCheckError();
                }
            }
    		nj.mcu.comp[compCnt].nUsedBlocks = blockCnt;
    		compCnt++;
        }
    	nj.mcu.nUsedComp = compCnt;

    	// if in ROI, than the mcu must be en-/decrypted
    	if(nj.inRoiFlag==1){
    		corrNeeded=1;
    		if(nj.cryptoMode == ENCRYPTION)
    			encryptMCU(DCcorrection);
    		if (nj.cryptoMode == DECRYPTION)
    			decryptMCU(DCcorrection);
       	}//end inRoi
    	else{
    		if(!nj.swapDC)	//dc-correction not needed
    			corrNeeded = 0;

    		if(corrNeeded){
    			corrNeeded = 0;
        		i=0;
    			for(compCnt=0 ; compCnt<nj.mcu.nUsedComp ; compCnt++){
        			for(blockCnt=0 ; blockCnt < nj.mcu.comp[compCnt].nUsedBlocks ; blockCnt++){

        				//printf("\n%i\t%i\t",i, DCcorrection[i]);
        				dcValue = getValue(&nj.mcu.comp[compCnt].block[blockCnt].rle[0]);
        				newDCValue = dcValue + DCcorrection[i];

        				if(nj.cryptoMode == ENCRYPTION){
        					if(newDCValue > DC_MAX_VALUE){
        						newDCValue = DC_MAX_VALUE-dcValue;
        						if (newDCValue > DC_MAX_VALUE){
        							newDCValue = DC_MAX_VALUE-1+dcValue;
        						}
        						DCcorrection[i] = DCcorrection[i] + dcValue - newDCValue;
        						corrNeeded = 1;
        					}
        					else if(newDCValue < -DC_MAX_VALUE){
        						newDCValue = -DC_MAX_VALUE-dcValue;
        						if(newDCValue < -DC_MAX_VALUE){
        							newDCValue = -DC_MAX_VALUE+1+dcValue;
        						}
        						DCcorrection[i] = DCcorrection[i] + dcValue - newDCValue;
        						corrNeeded = 1;
        					}
        					else{
        						DCcorrection[i] = 0;
        					}
        				}


        				if(nj.cryptoMode == DECRYPTION){
        					if(DCcorrection[i] > DC_MAX_VALUE){
        						newDCValue = -DC_MAX_VALUE-dcValue;
        						DCcorrection[i] = DCcorrection[i] + dcValue - newDCValue;
        						corrNeeded = 1;
        					}
        					else if(DCcorrection[i] < -DC_MAX_VALUE){
        						newDCValue = DC_MAX_VALUE-dcValue;
        						DCcorrection[i] = DCcorrection[i] + dcValue - newDCValue;
        						corrNeeded = 1;
        					}
        					else{
        						DCcorrection[i] = 0;
        					}

        				}
        				//printf("%i\t%i\t%i\t%i", dcValue, newDCValue, DCcorrection[i],corrNeeded);
        				i++;

        				if(i>NUM_OF_DC_VAL_TO_CORR)
        					njThrow(NJ_UNSUPPORTED);

        				setNewDCData(compCnt, blockCnt, newDCValue);
        			}
        		}
        	}//end if correction is needed
    		else{
				for(i=0 ; i<NUM_OF_DC_VAL_TO_CORR ; i++){
    		   		DCcorrection[i] = 0;
    			}
    		}
    	}//end is not in RoI

    	// with the following function the mcu data are written to the JPEG-out-buffer
    	encodeMcu();

        // mcu finished, now check for next mcu
        if (++mbx >= nj.mbwidth) {
            mbx = 0;
            if (++mby >= nj.mbheight) break;
        }
        if (nj.rstinterval && !(--rstcount)) {
            njByteAlign();
            i = njGetBits(16);
            if (((i & 0xFFF8) != 0xFFD0) || ((i & 7) != nextrst)) njThrow(NJ_SYNTAX_ERROR);
            addRSTm(nextrst);
            nextrst = (nextrst + 1) & 7;
            rstcount = nj.rstinterval;

        }
    }//end of handle all MCUs

    addEOI();
    nj.error = __NJ_FINISHED;
}


/**
* 	@fn void njInit(void)
*	@brief This function is needed for the memory initialization
* 	@param[in] void
* 	@return void
*/
void njInit(void) {
	memset(&nj, 0, sizeof(nj_context_t));
	initDcPrng();	// the dc encryption will use a different AES-PseudoNumberGenerator for safty reason
}


/**
* 	@fn nj_result_t jpegCrypto( const void* jpegIn, const void* jpegOut, const long sizeIn, long *sizeOut, int* in_roiArray, int* in_roiArraySize, int cryptoDetail, int cryptoMode)
*
*	@brief This function is called to decode and encrypt a JPEG dump.
*
* 	@param[in] 	const void* jpegIn		points to the JPEG input dump
* 				const void* jpegOut 	points to the JPEG output dump (return)
* 				const long sizeIn		size of the input picture
* 				long *sizeOut			size of the output picture
* 				int* in_roiArray 		RoI array; each input RoI contains x, y, width and height (in pixels) the values are returned to the caller in MCUs (instead of pixels)
* 				int* in_roiArraySize 	size of roiArray, 4 => one RoI
* 				int cryptoDetail		defines the en- and decryption level 	bit0 = 1 => swap block;
* 																				bit1 = 1 => swap code-word-value-pairs;
* 																				bit2 = 1 => swap AC value bits;
* 																				bit3 = 1 => swap DC value bits;
* 				int cryptoMode		0 ... no en- or decryption
* 									1 ... encryption
* 									2 ... decryption
*
* 	@return nj_result_t nj.error    0 ... encryption successful
* 									1 ... not a JPEG input file
* 									2 ... unsupported JPEG file
* 									3 ... syntax error in JPEG file
*/

nj_result_t jpegCrypto(const void* jpegIn, const void* jpegOut, const long sizeIn, long *sizeOut, int* in_roiArray, int* in_roiArraySize, int cryptoDetail, int cryptoMode) {
    njInit();
    nj.cryptoMode = cryptoMode;
    nj.pos = (const unsigned char*) jpegIn;
    nj.posIn = (unsigned char*) jpegIn;
    nj.posOut = (unsigned char*) jpegOut;
    nj.size = sizeIn & 0x7FFFFFFF;
    nj.initialSize = sizeIn & 0x7FFFFFFF;

    nj.swapBlock = cryptoDetail & 0x01;
    nj.swapCWV_block = (cryptoDetail>>1) & 0x01;
    nj.swapValues = (cryptoDetail>>2) & 0x01;
    nj.swapDC = (cryptoDetail>>3) & 0x01;

    nj.roiArray = in_roiArray;
    nj.roiArraySize = *in_roiArraySize;

    if (nj.size < 2) return NJ_NO_JPEG;
    if ((nj.pos[0] ^ 0xFF) | (nj.pos[1] ^ 0xD8)) return NJ_NO_JPEG;
    njSkip(2);
    while (!nj.error) {
        if ((nj.size < 2) || (nj.pos[0] != 0xFF)) return NJ_SYNTAX_ERROR;
        njSkip(2);
        switch (nj.pos[-1]) {
            case 0xC0: njDecodeSOF();  	break;	//---Start of frame marker Baseline DCT
            case 0xC1: njDecodeSOF();	break;	//---Start of frame marker Extended Sequential DCT
            case 0xC4: DecodeDHT();	   	break;	//---Define Huffman table(s)
            case 0xDA: DecodeScan(); 	break;	//---SOS Start of Scan
            case 0xDD: njDecodeDRI();   break;	//---Define restart interval
            case 0xFE: njSkipMarker(); 	break;	//---Comment
            default:
                //--- marker 0xDB added to the skipped markers
            	if ( ((nj.pos[-1] & 0xF0) == 0xE0) || (nj.pos[-1] == 0xDB) )
                    njSkipMarker();
                else
                    return NJ_UNSUPPORTED;
                break;
        }
    }
    if (nj.error != __NJ_FINISHED) return nj.error;
    *sizeOut = nj.sizeOut;
    nj.error = NJ_OK;
    return nj.error;
}


int njGetWidth(void)					{ return nj.width; }
int njGetHeight(void)  					{ return nj.height; }
int njGetImageSize(void)				{ return nj.width * nj.height * nj.ncomp; }
int njGetOriginalImageBufferSize(void)	{ return nj.initialSize; }
int njGetCryptoImageBufferSize(void)	{ return nj.sizeOut; }

