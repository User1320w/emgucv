//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "objdetect_c.h"

cv::FaceDetectorYN* cveFaceDetectorYNCreate(
    cv::String* model,
    cv::String* config,
    CvSize* inputSize,
    float scoreThreshold,
    float nmsThreshold,
    int topK,
    int backendId,
    int targetId,
    cv::Ptr<cv::FaceDetectorYN>** sharedPtr)
{
#ifdef HAVE_OPENCV_OBJDETECT
    cv::Ptr<cv::FaceDetectorYN> ptr = cv::FaceDetectorYN::create(
        *model, 
        *config,
        *inputSize,
        scoreThreshold,
        nmsThreshold,
        topK,
        backendId,
        targetId);
    *sharedPtr = new cv::Ptr<cv::FaceDetectorYN>(ptr);
    return (*sharedPtr)->get();
#else
        throw_no_objdetect();
#endif
}
void cveFaceDetectorYNRelease(cv::Ptr<cv::FaceDetectorYN>** faceDetetor)
{
#ifdef HAVE_OPENCV_OBJDETECT
	delete* faceDetetor;
	*faceDetetor = 0;
#else 
	throw_no_objdetect();
#endif
}
int cveFaceDetectorYNDetect(cv::FaceDetectorYN* faceDetetor, cv::_InputArray* image, cv::_OutputArray* faces)
{
    return faceDetetor->detect(*image, *faces);
}


