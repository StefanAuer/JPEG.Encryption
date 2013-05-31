# ---------------------------------------+
# Project: libbridge                     |
# Date:    Fri Mar 04 07:18:45 CEST 2013 |
# Author:  Stefan Auer                   |
# ---------------------------------------+


pathToSrc=../JPEG.Encryption.Core

includes=-l./pub/include
libs=

ifdef DEBUG
    CFLAGS=-g -Wall -std=c99 $(werror) $(includes) $(libs) 
else
    CFLAGS=-Wall -O3 -std=c99 $(werror) $(includes)  $(libs) 
endif

target=libbridge.a
target_inc=bridge.h

srcs= bridge.c $(pathToSrc)/JPEG.Encryption.Core.c  $(pathToSrc)/JPEGparser.c  $(pathToSrc)/prng.c  $(pathToSrc)/rijndael.c
objects= bridge.o JPEGparser.o prng.o rijndael.o JPEG.Encryption.Core.o

.c.o:
	gcc -c $(CFLAGS) $(srcs)

all: $(objects)
	@ar cr $(target) $(objects)
	@cp $(target) ./pub/lib
	@cp $(target_inc) ./pub/include

clean:
	@rm -f $(target) *.o
	@rm -f ./pub/lib/$(target)
	@rm -f ./pub/include/$(target_inc)

#bridge.o:bridge.h
#JPEG.Encryption.Core.o:../JPEG.Encryption.Core/JPEG.Encryption.Core.h
#JPEGparser.o:../JPEG.Encryption.Core/JPEGparser.h
#prng.o:../JPEG.Encryption.Core/prng.h
#rijndael.o: ../JPEG.Encryption.Core/rijndael.h

bridge.o: bridge.h
JPEG.Encryption.Core.o: bridge.h
JPEGparser.o: bridge.h
prng.o: bridge.h
rijndael.o: bridge.h
