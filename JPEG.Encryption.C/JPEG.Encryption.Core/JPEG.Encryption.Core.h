/**
 * @file   JPEG.Encryption.Core.h
 *
 * @author Alexander Bliem abliem.itsb-m2012@fh-salzburg.ac.at
 * @author Stefan Auer sauer.itsb-m2012@fh-salzburg.ac.at
 *
 * @date   31.10.2012
 * @version 1.0
 *
 * @brief Main.h is the header file for the module JPEG.Encryption.Core.c
 *
 * detailed description of file.
 */

#ifndef JPEG_ENCRYPTION_CORE_H_
#define JPEG_ENCRYPTION_CORE_H_





// JPEG Standard supports up to 255 image components, typical basline jpeg has 3 comp
#define MAX_NUMBER_OF_COMPENENTS_IN_FRAME		3

// JPEG Standard supports up to 4 huffman tables, basline jpeg has 2 huffman tables
#define NUMBER_OF_HUFFMANTABLES		2

#define M_START	0xFF		//each markers consists of two byte, the start of marker 0xFF and a second byte not equal to 0 or 0xFF
#define M_SOI	0xD8		//Start of Image
#define M_SOS	0xDA		//Start of Scan
#define M_DHT	0xC4		//Define Huffman table(s)
#define M_EOI	0xD9		//End of Image
#define M_SOF0	0xC0		//Start Of Frame (marker for basline DCT)

typedef struct{
	int FileLen;
	unsigned char* pFileInMemory;
} StructJPEGFileInMemory;

typedef struct{
	unsigned char 	componentID;
	unsigned char 	samplingFactorHhLv;		//higher 4 bits = horizontal sampling factor; lower 4 bits = vertical sampling factor
	unsigned char 	quantisationTableDest;

}structImageComponentsInFrame;

typedef struct{
	short unsigned int 				frameHeaderLength;
	unsigned char 					samplePrecision;
	short unsigned int 				numberOfLinesY;
	short unsigned int 				numberOfLinesX;
	unsigned char 					numberOfImageComponentsInFrame;
	structImageComponentsInFrame	ImageComponentsInFrame[MAX_NUMBER_OF_COMPENENTS_IN_FRAME];
} structFrameHeaderParameters;




#define NO_OF_HUFF_CODES_PER_LENGTH		16


typedef struct{
	unsigned char	numberOfHuffCodes[NO_OF_HUFF_CODES_PER_LENGTH];


}structSingleHuffmanTable;

typedef struct{
	short unsigned int	huffTableDefLen;
	structSingleHuffmanTable tables[NUMBER_OF_HUFFMANTABLES];
}structTotalHuffmanTables;







void myHelp();
void parseJPEG_File(StructJPEGFileInMemory* jpegByteStream);
short unsigned int get16BitValue(StructJPEGFileInMemory * jpegByteStream, int* offset);
unsigned char get8BitValue(StructJPEGFileInMemory * jpegByteStream, int* offset);
void parseFrameHeader(StructJPEGFileInMemory* jpegByteStream, int* offset, structFrameHeaderParameters* par);
void parseHuffmanTabel(StructJPEGFileInMemory* jpegByteStream, int* offset,structTotalHuffmanTables* huffTables);


#endif /* JPEG_ENCRYPTION_CORE_H_ */




