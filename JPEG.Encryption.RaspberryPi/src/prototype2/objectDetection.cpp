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
 * input parameters: none                                                      |
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
string face_cascade_name = "../../OpenCV-2.4.3/data/lbpcascades/lbpcascade_frontalface.xml";
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
int * roi_array;
bool use_old_rois = false;


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

	// free "old" array
	free(roi_array);

	// get memory for result
	roi_array = (int*) calloc(faces.size()*4, sizeof(int));
	*roi_array_size = faces.size() * 4;

	// copy detected faces to result
	for( size_t i = 0; i < faces.size(); i++ )
        {
		// correct scale
		faces[i].x *= scaleX;
		faces[i].y *= scaleY;
		faces[i].width *= scaleX;
		faces[i].height *= scaleY;

		roi_array[4*i + 0] = faces[i].x;
		roi_array[4*i + 1] = faces[i].y;
		roi_array[4*i + 2] = faces[i].width;
		roi_array[4*i + 3] = faces[i].height;
	}

	// return the result
	return roi_array;
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

	// image size before and after the encryption
	long size_in;
	long size_out;

	// the input image
	unsigned char * jpeg_in;

	// file pointer to output file
        FILE * fp_image_out = NULL;

	// load the cascade
	if( !face_cascade.load( face_cascade_name ) )
	{
		printf("--(!)Error loading\n");
		return -1;
	}

	// prepare camera input
	capture = cvCaptureFromCAM( -1 );

	if( capture )
	{
		// set capture properties
		cvSetCaptureProperty(capture, CV_CAP_PROP_FRAME_WIDTH, FRAME_WIDTH);
		cvSetCaptureProperty(capture, CV_CAP_PROP_FRAME_HEIGHT, FRAME_HEIGHT);
    		//cvSetCaptureProperty(capture, CV_CAP_PROP_FOURCC, CV_FOURCC('M', 'J', 'P', 'G'));
		//int mode = ('M' & 255) + (('J' & 255) << 8) + (('P' & 255) << 16) + (('G' & 255) << 24);
       		//cvSetCaptureProperty(capture, CV_CAP_PROP_FORMAT , mode);

		// init 75% JPEG quality
		vector<int> quality_params;
		quality_params.push_back(CV_IMWRITE_JPEG_QUALITY);
		quality_params.push_back(75);

		// buffer used for converting an image
		vector<uchar> buff;

		// the output image
		unsigned char * buffer_out;

		// counter to improve performance
		int counter = FACEDETECT_EVERY_N_TH_FRAME;

		// face detection is executed every n-th frame
		int * roi_array = NULL;
   	        int   roi_array_size = 0;

		// start endless loop
		for(;;)
		{
			// get frame from camera
			frame = cvQueryFrame( capture );
			if( !frame.empty() && (counter %  FACEDETECT_EVERY_N_TH_FRAME == 0))
			{
				roi_array = detect( frame, &roi_array_size );
				use_old_rois = false;
			}
			else
				use_old_rois = true;

			counter++;

			// prepare variables for encryption
			jpeg_in = frame.data;
			size_in = frame.rows * frame.cols*2;
			size_out = 2 * size_in; // make it twice the size (worst case)
			buffer_out = (unsigned char*)calloc(size_out, sizeof(unsigned char));

			// encode picture to JPEG
			imencode(".jpg", frame, buff, quality_params);

			//little trick to convert vector to array
			jpeg_in = &buff[0];

			int result = -1;
                        if (roi_array_size > 0 || use_old_rois == true)
	                        encJpeg(jpeg_in, size_in, roi_array, roi_array_size, key, key_size, crypto_detail,  buffer_out, &size_out, &result);


			// open file pointer
			fp_image_out = fopen("/var/www/JPEG.Encryption/camera.jpg","wb");

			// save encrypted image
		        if (roi_array_size > 0 || use_old_rois == true)
				fwrite(buffer_out, sizeof(char), size_out, fp_image_out);
			// save original frame (no face detected)
			else
				fwrite(&buff[0], sizeof(char), buff.size(), fp_image_out);

			// close file pointer, free memory
			fclose(fp_image_out);
			free(buffer_out);

			// give the camera some milliseconds to recovery
			waitKey(20);
		}
	}
	else
	{
		printf(" --(!) No captured frame -- Break!"); 
	        fclose(fp_image_out);
		free(roi_array);
		return -1;
	}
}
