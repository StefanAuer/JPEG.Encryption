#+-----------------------------------------------------------------------------+
#| facedetect-timing.py (first prototype)                                      |
#|                                                                             |
#| Author: Alexander Bliem abliem.itsb-m2012@fh-salzburg.ac.at                 | 
#| Author: Stefan Auer sauer.itsb-m2012@fh-salzburg.ac.at                      |
#|                                                                             |
#| Date   20.05.2013                                                           |
#| Version 1.0                                                                 |
#|                                                                             |
#| This is the code used for time measurement.                                 |
#|                                                                             |
#+-----------------------------------------------------------------------------+

import os
import numpy as np
import cv2
import cv2.cv as cv
import sys, getopt
import time

# enable import of external files
from sys import path
path.append ("../../OpenCV-2.3.1/samples/python2")

from video import create_capture
from common import clock, draw_str

# constant variables used for OpenCV 
CONST_CAPTURE_WIDTH = 640
CONST_CAPTURE_HEIGHT = 480
CONST_CV_CAP_PROP_FRAME_WIDTH = 3
CONST_CV_CAP_PROP_FRAME_HEIGHT = 4

# constant variables for resizing image before face detection
CONST_THUMBNAIL_WIDTH = 320
CONST_THUMBNAIL_HEIGHT = 240

# variables for face detection
faceDetection_scale = 1.3
faceDetection_min_neighbors = 3
faceDetection_min_width = 5
faceDetection_min_height = 5

# variables for encryption
encryption_mode= 'e'
encryption_details = '15'
encryption_password = '1234567890123456'

# variables for www output
www_outputDir = '/var/www/JPEG.Encryption'

# help message for using this python script
help_message = '''
USAGE: facedetect.py 
[--cascade <cascade_fn>] 
[--password <password>]
[--mode <mode>]
[--details <details>]
[--scale <scale>]
[--minWidth <minWidth>]
[--minHeight <minHeight>]
[--wwwDir <wwwDir>]
[<video_source>]
'''

#+-----------------------------------+
#| function for detecting faces      |
#| IN img: image with faces          |
#| IN cascade: cascade variable      |
#| OUT rects: array with rectangles  |
#|            indicating the position|
#|            of the faces           |
#+-----------------------------------+
def detect(img, cascade):
    # detect faces using OpenCV
    rects = cascade.detectMultiScale(img, \
                                     scaleFactor = faceDetection_scale, \
                                     minNeighbors= faceDetection_min_neighbors, \
                                     minSize=(faceDetection_min_width, faceDetection_min_height), \
                                     flags = cv.CV_HAAR_DO_CANNY_PRUNING)
    if len(rects) == 0:
        return []
    rects[:,2:] += rects[:,:2]
    return rects

#+-----------------------------------+
#|          main function            |
#+-----------------------------------+
if __name__ == '__main__':

    # show the usage message
    # print help_message
    
    # parse parameters
    args, video_src = getopt.getopt(sys.argv[1:], '', ['cascade=', 'mode=', 'details=', 'password=', 'scale=', 'minWidth', 'minHeight)', '[wwwDir=]'])
    args = dict(args)
    cascade_fn = args.get('--cascade', "../../OpenCV-2.3.1/data/haarcascades/haarcascade_frontalface_alt.xml")

    encryption_password = args.get('--password', encryption_password)
    encryption_mode = args.get('--mode', encryption_mode)
    encryption_details = args.get('--details', encryption_details)

    faceDetection_scale = float(args.get('--scale', faceDetection_scale))
    faceDetection_minSize_width = args.get('--minWidth', faceDetection_min_width)
    faceDetection_minSize_height = args.get('--minHeight', faceDetection_min_height)

    www_outputDir = args.get('--wwwDir', www_outputDir)

    # add '/' if necessary
    if (www_outputDir[len(www_outputDir)-1]) != '/' :
        www_outputDir += '/'

    # assign parameters to variables
    cascade = cv2.CascadeClassifier(cascade_fn)
    try: video_src = video_src[0]
    except: video_src = 'synth:bg=error.jpg:noise=0.05'

    # init video capture
    cap = cv2.VideoCapture(-1)
    cap.set(cv.CV_CAP_PROP_FRAME_WIDTH, 640)
    cap.set(cv.CV_CAP_PROP_FRAME_HEIGHT, 480)

	
    # print header
    with open("timing.csv", "a") as myfile:
       myfile.write("filename, fileSize [kb], RoIs, capture [ms], resize [ms], faceDetect [ms], writeToDisk[ms], encryption [ms]\n ")

    for root,dirs,files in os.walk("../../testImages/"):
       files.sort()
       for file in files:
           if file.endswith(".jpg"):                 

               # start timer (capture)
               t = time.time()   

               # get image from camera
               ret, img = cap.read()
         
               if ret == False: 
                    print 'Error while capturing image from camera! Aborting. '
                    break

               tCapture = (time.time() - t) * 1000

               # to have the same images: open test file (is not part of the measurement)
               img = cv2.imread(os.path.join(root,file))
				
               # start timer (resize)
               t = time.time()   

		
               # resize image for face detection (performance++)
               thumbnail = cv2.resize(img, (CONST_THUMBNAIL_WIDTH,CONST_THUMBNAIL_HEIGHT))
               gray = cv2.cvtColor(thumbnail, cv2.COLOR_BGR2GRAY)
               gray = cv2.equalizeHist(gray)
        	
               tResize = (time.time() - t) * 1000
        
               # detect faces
        
	       # start timer (face detection)
               t = time.time()
	
               rects = detect(gray, cascade)

               tFaceDetection = (time.time() - t) * 1000
		
		
               # start timer (write to disk)
 	       t = time.time()

               # save captured image to disk
               cv2.imwrite("capture.jpg", img)
  
               tWriteToDisk = (time.time() - t) * 1000
        
	       # start timer (encryption)
	       t = time.time()
		
	       # the encryption programe is called with a simple shell command
               # thus, we have to build this command first        

               cmd_str = ''              
               cmd_str += "../../../JPEG.Encryption.C/JPEG.Encryption.Core/jpegEnc "  # location of the programe
               cmd_str += "capture.jpg "                                              # location for temp file
               cmd_str += www_outputDir +  "camera.jpg "                              # location for web folder
               cmd_str += encryption_password                                         # the given password
               cmd_str += ' ' 
               cmd_str += encryption_mode                                             # the given encryption mode
               cmd_str += ' '                                                          
               cmd_str += encryption_details                                          # the given encryption details
               cmd_str += ' '


               # add RoI (region of interest) information
               string_roi = ""
               for x1, y1, x2, y2 in rects:
                   string_roi += str(x1*2) + ' ' + str(y1*2) + ' ' + str((x2-x1)*2) + ' ' + str((y2-y1)*2) 
            
               # add 8 static RoIs [x, y, width, height]
               string_roi +=  ' 385   5 80 85'
               string_roi +=  ' 554   2 71 94'
               string_roi +=  ' 613 105 27 74'
               string_roi +=  ' 430 358 43 37'
               string_roi +=  ' 418 317 29 31'
               string_roi +=  ' 405 284 28 26'
               string_roi +=  ' 390 250 29 28'
               string_roi +=  '  62   3 71 28'
			
               # encrypt
               os.system(cmd_str + str(len(rects)*4 + (8*4)) + " " + string_roi)

               tEncryption = (time.time() - t) * 1000

               with open("timing.csv", "a") as myfile:
                   myfile.write(file + ', '+ str(os.path.getsize(os.path.join(root,file)))  + ', '  + str(len(rects) + 8) +  ', ' + str(tCapture) +  ', ' + str(tResize) + ', ' + str(tFaceDetection) + ', ' + str(tWriteToDisk)  + ', ' + str(tEncryption))
                   myfile.write('\n')		

               print 'Image ' + file + ' processed!'

