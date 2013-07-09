cmake_minimum_required(VERSION 2.8)
project( camcv )
SET(COMPILE_DEFINITIONS -Werror)
#OPENCV
find_package( OpenCV REQUIRED )

link_directories( /home/pi/JPEG.Encryption/JPEG.Encryption.C/JPEG.Encryption.Bridge ) 

include_directories(/opt/vc/userland-master/host_applications/linux/libs/bcm_host/include)
include_directories(/opt/vc/userland-master/interface/vcos)
include_directories(/opt/vc/userland-master)
include_directories(/opt/vc/userland-master/interface/vcos/pthreads)
include_directories(/opt/vc/userland-master/interface/vmcs_host/linux)
add_executable(camcv RaspiCamControl.c RaspiCLI.c RaspiPreview.c camcv.c)
add_executable(camcvTiming RaspiCamControl.c RaspiCLI.c RaspiPreview.c camcvTiming.c)
target_link_libraries(camcv libbridge.a /opt/vc/lib/libmmal_core.so /opt/vc/lib/libmmal_util.so /opt/vc/lib/libmmal_vc_client.so /opt/vc/lib/libvcos.so /opt/vc/lib/libbcm_host.so ${OpenCV_LIBS} ) 
target_link_libraries(camcvTiming libbridge.a /opt/vc/lib/libmmal_core.so /opt/vc/lib/libmmal_util.so /opt/vc/lib/libmmal_vc_client.so /opt/vc/lib/libvcos.so /opt/vc/lib/libbcm_host.so ${OpenCV_LIBS} ) 
  
