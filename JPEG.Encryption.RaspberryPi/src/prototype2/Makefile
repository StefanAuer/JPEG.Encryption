# ---------------------------------------+
# Project: prototype 2                   |
# Date:    Fri Mar 04 07:18:45 CEST 2013 |
# Author:  Stefan Auer                   |
# ---------------------------------------+

includes=-I../../../JPEG.Encryption.C/JPEG.Encryption.Bridge/pub/include/
libs=-lbridge
libdir=-L../../../JPEG.Encryption.C/JPEG.Encryption.Bridge/pub/lib/

target=objectDetection
objects=objectDetection.o

targetTiming=objectDetectionTiming
objectsTiming=objectDetectionTiming.o

CFLAGS= $(werror) -Wall -Wextra -finline-functions -O3 -march=armv6 \
        -ffast-math -ggdb3 \
	`pkg-config --cflags opencv` \
        `pkg-config --libs opencv` 

.cpp.o:
	g++ $(CFLAGS) $(includes) -c $<

all: $(objects)
	make -C ../../../JPEG.Encryption.C/JPEG.Encryption.Bridge/
	g++ $(CFLAGS) -o $(target) $(objects) $(libdir) $(libs)

timing: $(objectsTiming)
	make -C ../../../JPEG.Encryption.C/JPEG.Encryption.Bridge/
	g++ $(CFLAGS) -o $(targetTiming) $(objectsTiming) $(libdir) $(libs) 

clean:
	@rm -f $(target) *.o
	@rm -f $(pubdir)$(target)   
	@rm -f $(pubdir)$(targetTiming)   


