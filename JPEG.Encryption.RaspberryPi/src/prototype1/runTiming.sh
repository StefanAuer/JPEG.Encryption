#!/bin/bash -e
#for detailed description of the parameters see facedetect.py

echo "Deleting old .csv"
rm *.csv

echo "building JPEG.Encryption";
make -s -C ../../../JPEG.Encryption.C/JPEG.Encryption.Core/

echo "starting TIMING > timing.csv"
 python ./facedetectTiming.py \
          --cascade ../../OpenCV-2.4.3/data/haarcascades/haarcascade_frontalface_alt.xml \
          --password 1234567890123456 \
          --mode e \
          --details 15  \
          --scale 1.3 \
          --minWidth 5 \
          --minHeight 5 \
          --wwwDir /var/www/JPEG.Encryption 





