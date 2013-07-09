+-----------------------------------------------------------------------------+
| README.txt  (second prototype)                                              |
|                                                                             |
| Author: Alexander Bliem abliem.itsb-m2012@fh-salzburg.ac.at                 | 
| Author: Stefan Auer sauer.itsb-m2012@fh-salzburg.ac.at                      |
|                                                                             |
| Date   01.05.2013                                                           |
| Version 1.0                                                                 |
|                                                                             |
| This folder contains the files required for the second prototype for        |
| JPEG-Encryption.                                                            |
|                                                                             |
| Required:                                                                   |
| 1) connected web cam (USB)                                                  |
| 2) WebServer (e.g. apache up and running)                                   |
| 3) A folder that is accessible through HTTP (/var/www/JPEG.Encryption/)     |
| 4) OpenCV 2.x.x in the folder ../../OpenCV-2.x.x/                           |
| 5) JPEG.Encryption files in                                                 |
|               ../JPEG.Encryption/JPEG.Encryption.C/JPEG.Encryption.Core     |
|                                                                             |
| Build prototype:                                                            |
| 1) make (for normal prototype)                                              |
| 2) make timing (for time measuring prototype)                               |
|                                                                             |
| start prototype:                                                            |
| 1) start prototype with "./objectDetection"                                 |
|                                                                             |
| Remarks:                                                                    |
| This is the second prototype of the project. It uses OpenCV for capturing,  | 
| image conversion and face detection. For performance reasons the captured   |
| image is downsized to 320x240 pixel. Furthermore a face detection is        |
| executed every 5th frame.                                                   |
|                                                                             |
+-----------------------------------------------------------------------------+
