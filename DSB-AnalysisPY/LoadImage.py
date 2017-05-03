import argparse
import numpy as np
import cv2
from matplotlib import pyplot as plt

# Get the argument which was parsed
# ap = argparse.ArgumentParser()
# ap.add_argument("-i", "--image", required = True, help = "Path to the image")
# args = vars(ap.parse_args())

# Load an color image in grayscale
img = cv2.imread('D:/GITHUB REPOS/DSB-TargetAnalysis/DSB-AnalysisPY/test.jpg',0)

# edges = cv2.Canny(img,100,200)

blurred = cv2.GaussianBlur( img, (9, 9), 0 )

circles = cv2.HoughCircles(blurred, cv2.HOUGH_GRADIENT, 1 , 5, 200 , 90, 20 , 100)

circles = np.round(circles[0, :]).astype("int")
print circles.size
for (x, y, r) in circles:
		# draw the circle in the output image, then draw a rectangle
		# corresponding to the center of the circle
		cv2.circle(img, (x, y), r, (0, 255, 0), 4)
		cv2.rectangle(img, (x - 5, y - 5), (x + 5, y + 5), (0, 128, 255), -1)

cv2.imshow('image',img)
cv2.waitKey(0)
cv2.destroyAllWindows()