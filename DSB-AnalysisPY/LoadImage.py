import argparse
import numpy as np
import cv2
from matplotlib import pyplot as plt
import math

# Get the argument which was parsed
# ap = argparse.ArgumentParser()
# ap.add_argument("-i", "--image", required = True, help = "Path to the image")
# args = vars(ap.parse_args())

# Load an color image in grayscale
img = cv2.imread('D:/GITHUB REPOS/DSB-TargetAnalysis/DSB-AnalysisPY/testNew.png',0)

# edges = cv2.Canny(img,100,200)

radii = np.arange(0,310,10)

for idx in range(len(radii)-1):
    # Get the minimum and maximum radius
    # Note you need to add 1 to each minimum
    # as the maximum of the previous pair covers this new minimum
    minRadius = radii[idx]+1
    maxRadius = radii[idx+1]

    # blurred = cv2.GaussianBlur( img, (9, 9), 0 )

    circles = cv2.HoughCircles(img, cv2.HOUGH_GRADIENT, 1 , 5, 200 , 100, minRadius=minRadius , maxRadius=maxRadius)

    if circles is None:
        continue

    circles = np.uint16(np.around(circles))
	

    height, width = img.shape
    center = (width/2,height/2)
    for i in circles[0,:]:
        # draw the outer circle
        if math.sqrt((center[0]-i[0])**2 + (center[1]-i[1])**2) < 15:
            cv2.circle(img,(i[0],i[1]),i[2],(0,255,0),1)
            # draw the center of the circle
            cv2.circle(img,(i[0],i[1]),2,(0,0,255),3)

cv2.imshow('image',img)
cv2.waitKey(0)
cv2.destroyAllWindows()