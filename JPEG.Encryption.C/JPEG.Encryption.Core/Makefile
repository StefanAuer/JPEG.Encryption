CC = gcc 
CCFLAGS = -ansi -g -w -std=c99
LD = gcc 
LDFLAGS = -lm 
CFILES = $(wildcard *.c) 
OFILES = $(CFILES:.c=.o) 

.DEFAULT_GOAL = jpegEnc

.c.o:
	$(CC) $(CCFLAGS) -c $< 
jpegEnc: $(OFILES)
	$(LD) -o $@ $(OFILES) $(LDFLAGS) 
clean:
	rm -f $(OFILES) 
depend:
	makedepend $(CFILES)
# DO NOT DELETE
