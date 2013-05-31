#!/bin/bash
#for detailed description of the parameters see facedetect.py

echo "building JPEG.Encryption";
make -s -C ../../../JPEG.Encryption.C/JPEG.Encryption.Core/

echo "starting prototype"
 python ./facedetect.py \
          --cascade ../../OpenCV-2.3.1/data/haarcascades/haarcascade_frontalface_alt.xml \
          --password 1234567890123456 \
          --mode e \
          --details 15  \
          --scale 1.3 \
          --minWidth 10 \
          --minHeight 10 \
          --wwwDir /var/www/JPEG.Encryption





