JPEG.Encryption
===============

JPEG Encryption 

Author Alexander Bliem abliem.itsb-m2012@fh-salzburg.ac.at
Author Stefan Auer sauer.itsb-m2012@fh-salzburg.ac.at

Date   01.07.2013
Version 1.0
Brief: This is a minimal decoder for baseline JPEG images and includes a JPEG en- and
       decryption algorithm, based on an AES OFB-Mode pseudo random number generator.
       It accepts memory dumps of JPEG files as input and generates a memory dump of
       the encrypted JPEG image. All YCbCr subsampling schemes with power-of-two ratios are
       supported, as well as restart intervals. Progressive or lossless JPEG is not supported.
 
       For further details to the bit-stream-based encryption algorithm, as well as to run-time
       performance measurements see the following journal article:
       Stefan Auer, Alexander Bliem, Dominik Engel, Andreas Uhl, Andreas Unterweger, 
          "Bitstream-based JPEG Encryption in Real-time". In Chang-Tsun Li (Ed.) International Journal of 
          Digital Crime and Forensics, vol. 5(3), accepted.  
  
  
  
  The JPEG decoder is based on the nanoJPEG open source project containing the following license:
 
  NanoJPEG -- KeyJ's Tiny Baseline JPEG Decoder
  version 1.3 (2012-03-05)
  by Martin J. Fiedler <martin.fiedler@gmx.net>
 
  This software is published under the terms of KeyJ's Research License,
  version 0.2. Usage of this software is subject to the following conditions:
   0. There's no warranty whatsoever. The author(s) of this software can not
      be held liable for any damages that occur when using this software.
   1. This software may be used freely for both non-commercial and commercial
      purposes.
   2. This software may be redistributed freely as long as no fees are charged
      for the distribution and this license information is included.
   3. This software may be modified freely except for this license information,
      which must not be changed in any way.
   4. If anything other than configuration, indentation or comments have been
      altered in the code, the original author(s) must receive a copy of the
      modified code.
 
