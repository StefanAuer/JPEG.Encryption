#!/bin/bash -e

WIDTH=640
HEIGHT=480


QUALITYSTART=25
QUALITYEND=100
QUALITYSTEP=25
QUALITY=${QUALITYSTART}


echo "deleting old files"
rm -f *.bmp
rm -f 25quality/*.jpg
rm -f 50quality/*.jpg
rm -f 75quality/*.jpg
rm -f 100quality/*.jpg


echo "extracting bitmaps"
ffmpeg -an -ss 0:0:11 -i ./original/AVSS_AB_Easy_Divx.avi -r 10 -vf "crop=640:480:40:48" -f image2 ./image%04d.bmp


for FILE in *.bmp;
do
	COUNTER=1
	while [ ${QUALITY} -le ${QUALITYEND} ]
	do

		NEWFILENAME=./${QUALITY}quality/${FILE/.bmp/}-${QUALITY}-${WIDTH}x${HEIGHT}.jpg

        	echo "Converting " ${FILE} " to " ${NEWFILENAME}

		convert ${FILE} -quality ${QUALITY}% ${NEWFILENAME}

		let QUALITY+=${QUALITYSTEP}
	done
        let COUNTER++
	let QUALITY=${QUALITYSTART}
	rm ${FILE}
done



