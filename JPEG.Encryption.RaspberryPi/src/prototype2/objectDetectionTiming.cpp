/*-----------------------------------------------------------------------------+
 * objectDetection.cpp (second prototype)                                      |
 *                                                                             |
 * Author: Alexander Bliem abliem.itsb-m2012@fh-salzburg.ac.at                 | 
 * Author: Stefan Auer sauer.itsb-m2012@fh-salzburg.ac.at                      |
 * Author: A. Huaman ( based in the classic facedetect.cpp in samples/c )      |
 *                                                                             |
 * Date   17.05.2013                                                           |
 * Version 1.0                                                                 |
 *                                                                             |
 * This is the second prototype for Raspberry Pi based JPEG-Encryption         |
 * The following steps are necessary to achive our goal:                       |
 * 1) capture image from camera (OpenCV)                                       |
 * 2) resize image (OpenCV)                                                    |
 * 3) resize and detect faces on image (OpenCV)                                |
 * 4) encrypt image using the our C code                                       |
 * 5) write encrypted image to HTTP accessible directory (e.g. /var/www)       |
 *                                                                             |
 * input parameters:                                                           | 
 *    --cascade:   determines which cascade file will be used                  |
 *    --password:  the password for encryption                                 |
 *    --mode:      e for encryption, d for decryption                          |
 *    --details:   value from 1 to 15. Determines which encryption details     |
 *                 (DC encryption, swap values etc.) will be used. See         |
 *                 JPEG.Encryption.Core documentation                          |
 *    --scale:     scale size for face detection                               |
 *    --minWidth:  min width for a face                                        |
 *    --minHeight: min height for a face                                       | 
 *    --wwwDir:    output directory                                            |
 *                                                                             |
 *-----------------------------------------------------------------------------+
 */

#include "opencv2/objdetect/objdetect.hpp"
#include "opencv2/highgui/highgui.hpp"
#include "opencv2/imgproc/imgproc.hpp"

#include <iostream>
#include <fstream>
#include <stdio.h>
#include <stdlib.h>
#include <dirent.h>
#include <string>
#include <sys/types.h>
#include <sys/stat.h>
#include <unistd.h>

extern "C"
{
   // load the JPEG.Encryption lib
   #include "../../../JPEG.Encryption.C/JPEG.Encryption.Bridge/bridge.h"
}

#define FRAME_WIDTH 640
#define FRAME_HEIGHT 480
#define DETECTION_WIDTH 320
#define DETECTION_HEIGHT 240
#define FACEDETECT_EVERY_N_TH_FRAME 1

using namespace std;
using namespace cv;

/*---------------------------------+
 *      Global variables           |
 *---------------------------------+
 */

// variables for face detection
string face_cascade_name = "../../OpenCV-2.3.1/data/lbpcascades/lbpcascade_frontalface.xml";
CascadeClassifier face_cascade;
CascadeClassifier eyes_cascade;

double image_width_for_detection = DETECTION_WIDTH;
double image_height_for_detection = DETECTION_HEIGHT;
Mat small_img (image_height_for_detection, image_width_for_detection, CV_8UC1);

double scaleX = FRAME_WIDTH / image_width_for_detection;
double scaleY = FRAME_HEIGHT / image_height_for_detection;

// variables for encryption
int crypto_detail = 15;
unsigned char key[] = {'0','1','2','3','4','5','6','7','8','9','A','B','C','D','E','F'};
int key_size = 16;

// variables for time measurement
double tCapture, tDetection, tConvert,  tEncrypt, tWrite;

/*-----------------------------------------------------+
 * function for detect faces.                          |
 * The caller has to free() the result!                |
 * IN frame: Matrix containg an image                  |
 * IN/OUT roi_array_size: size of roi_array            |
 * RETURNS: array containing the RoIs                  |
 *-----------------------------------------------------+
 */
int * detect( Mat frame, int * roi_array_size )
{

	// init vector containing the faces
	std::vector<Rect> faces;

	// prepare image for processing
	Mat frame_gray;
	resize ( frame, small_img, small_img.size(), 0,0,INTER_LINEAR);
	cvtColor( small_img, frame_gray, CV_BGR2GRAY );
	equalizeHist( frame_gray, frame_gray );

	// Detect faces
	face_cascade.detectMultiScale( frame_gray, faces, 1.3, 3, 0, Size(5, 5) );

	// get memory for result
	int * roi_arr = (int*) calloc((faces.size() + 8)*4, sizeof(int));
	*roi_array_size = (faces.size()+8) * 4;

	// add static RoIs
	roi_arr[0] = 385;
	roi_arr[1] = 5;
	roi_arr[2] = 80;
	roi_arr[3] = 85;

	roi_arr[4] = 554;
	roi_arr[5] = 2;
	roi_arr[6] = 71;
	roi_arr[7] = 94;

	roi_arr[8] = 613;
	roi_arr[9] = 105;
	roi_arr[10] = 27;
	roi_arr[11] = 74;

	roi_arr[12] = 430;
	roi_arr[13] = 358;
	roi_arr[14] = 43;
	roi_arr[15] = 37;

	roi_arr[16] = 418;
	roi_arr[17] = 317;
	roi_arr[18] = 29;
	roi_arr[19] = 31;

	roi_arr[20] = 405;
	roi_arr[21] = 284;
	roi_arr[22] = 28;
	roi_arr[23] = 26;

	roi_arr[24] = 390;
	roi_arr[25] = 250;
	roi_arr[26] = 29;
	roi_arr[27] = 28;

	roi_arr[28] = 62;
	roi_arr[29] = 3;
	roi_arr[30] = 71;
	roi_arr[31] = 28;



	// copy detected faces to result
	for( size_t i = 8; i < faces.size()+8 ; i++ )
        {
		// correct scale
		faces[i-8].x *= scaleX;
		faces[i-8].y *= scaleY;
		faces[i-8].width *= scaleX;
		faces[i-8].height *= scaleY;

		roi_arr[4*i + 0] = faces[i-8].x;
		roi_arr[4*i + 1] = faces[i-8].y;
		roi_arr[4*i + 2] = faces[i-8].width;
		roi_arr[4*i + 3] = faces[i-8].height;
	}

	// return the result
	return roi_arr;
}


vector<string> getJpg(const char * dirName)
{
	vector<string> result;

	DIR *dir;
	struct dirent *ent;
	if ((dir = opendir (dirName)) != NULL) 
	{
		while ((ent = readdir (dir)) != NULL) 
		{
		      string directory(dirName);
		      string fileName(ent->d_name);
		      if ( fileName.find(".jpg") > 0 && fileName.size() > 3)
			      result.push_back(directory + "/" + fileName);
	  	}
	}
	closedir (dir);

	return result;
}


/*-----------------------------------------------------+
 * function for detect faces                           |
 * IN frame: Matrix containg an image                  |
 * RETURNS: vector with recangles containing the RoIs  |
 *-----------------------------------------------------+
 */
int main( void )
{
	// object for capturing images from a camera
	CvCapture* capture;

	// one single image
	Mat frame;

	long size_in;
	long size_out;

	unsigned char * jpeg_in;
        FILE * fp_image_out = NULL;

	// load the cascade
	if( !face_cascade.load( face_cascade_name ) )
	{
		printf("--(!)Error loading\n");
		return -1;
	}

	// prepare camera input 
	capture = cvCaptureFromCAM( -1 );

	// get all test files
	vector<string> testImages;

	vector<string> testImages25 = getJpg("../../testImages/25quality");
	vector<string> testImages50 = getJpg("../../testImages/50quality");
	vector<string> testImages75 = getJpg("../../testImages/75quality");
	vector<string> testImages100 = getJpg("../../testImages/100quality");

	vector<string> allTestFiles;
	allTestFiles.reserve(testImages25.size() + testImages50.size() + testImages75.size() + testImages100.size());
	allTestFiles.insert (allTestFiles.end(), testImages25.begin(), testImages25.end());
	allTestFiles.insert (allTestFiles.end(), testImages50.begin(), testImages50.end());
	allTestFiles.insert (allTestFiles.end(), testImages75.begin(), testImages75.end());
	allTestFiles.insert (allTestFiles.end(), testImages100.begin(), testImages100.end());

	sort (allTestFiles.begin(), allTestFiles.end());

	if( capture )
	{
		// set capture properties
		cvSetCaptureProperty(capture, CV_CAP_PROP_FRAME_WIDTH, FRAME_WIDTH);
		cvSetCaptureProperty(capture, CV_CAP_PROP_FRAME_HEIGHT, FRAME_HEIGHT);
    		//cvSetCaptureProperty(capture, CV_CAP_PROP_FOURCC, CV_FOURCC('M', 'J', 'P', 'G'));
		//int mode = ('M' & 255) + (('J' & 255) << 8) + (('P' & 255) << 16) + (('G' & 255) << 24);
       		//cvSetCaptureProperty(capture, CV_CAP_PROP_FORMAT , mode);

		//vector<int> quality_params;
		//quality_params.push_back(CV_IMWRITE_JPEG_QUALITY);
		//quality_params.push_back(75);

		vector<uchar> buff;

		unsigned char * buffer_out;
		int counter = 0;

		// face detection is executed every n-th frame
		int * roi_array = NULL;
   	        int   roi_array_size = 0;


		//for(;;)
		printf("filename, fileSize [kb], RoIs, capture [ms], faceDetect [ms], convert[ms], encryption [ms], writeToDisk\n ");

    	        for (vector<string>::iterator it=allTestFiles.begin(); it!=allTestFiles.end(); ++it)
		{

			// get frame from camera
			tCapture = (double)cvGetTickCount();
			frame = cvQueryFrame( capture );
			tCapture = (double)cvGetTickCount() - tCapture;

			// replace captured image with test image
			frame = imread(*it);



			tDetection = (double)cvGetTickCount();

			if( !frame.empty() && (++counter %  FACEDETECT_EVERY_N_TH_FRAME == 0))
			{
				free(roi_array);
				roi_array = detect( frame, &roi_array_size );
			}

			tDetection = (double)cvGetTickCount() - tDetection;

			tConvert = (double)cvGetTickCount();



			// prepare variables for encryption
			jpeg_in = frame.data;
			size_in = frame.rows * frame.cols*2;
			size_out = 2 * size_in; // make it twice the size (worst case)
			buffer_out = (unsigned char*)calloc(size_out, sizeof(unsigned char));

			// encode picture to JPEG
			imencode(".jpg", frame, buff);//, quality_params);



			//little trick to convert vector to array
			jpeg_in = &buff[0]; 

			tConvert = (double)cvGetTickCount() - tConvert;

			tEncrypt = (double)cvGetTickCount();


			int result = -1;
                        if (roi_array_size > 0)
			{
	                        encJpeg(jpeg_in, size_in, roi_array, roi_array_size, key, key_size, crypto_detail,  buffer_out, &size_out, &result);
			}

			tEncrypt = (double)cvGetTickCount() - tEncrypt;

			tWrite = (double)cvGetTickCount();



			fp_image_out = fopen("/var/www/JPEG.Encryption/camera.jpg","wb");
		        if (roi_array_size > 0)
			{
				fwrite(buffer_out, sizeof(char), size_out, fp_image_out);
			}
			else
			{
				fwrite(&buff[0], sizeof(char), buff.size(), fp_image_out);
			}


			fclose(fp_image_out);
			free(buffer_out);

			tWrite = (double)cvGetTickCount() - tWrite;

			waitKey(30);



			double tickFrq =  cvGetTickFrequency()*1000;

			struct stat filestatus;
			stat( it->c_str(), &filestatus );
			cout << *it << ", " << filestatus.st_size << ", " << roi_array_size/4 << ", " << tCapture/tickFrq << ", " << tDetection/tickFrq << ", " << tConvert/tickFrq << ", " << (tEncrypt/tickFrq) << ", " << tWrite/tickFrq << endl; 


		}
	}
	else
	{
		printf(" --(!) No captured frame -- Break!"); 
	        fclose(fp_image_out);
		return -1;
	}
}

