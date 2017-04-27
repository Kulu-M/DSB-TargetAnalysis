import argparse
import numpy as np
import cv2

# Get the argument which was parsed
# ap = argparse.ArgumentParser()
# ap.add_argument("-i", "--image", required = True, help = "Path to the image")
# args = vars(ap.parse_args())

# Load an color image in grayscale
img = cv2.imread('D:/GITHUB REPOS/PyEdge/123.jpg',0)

cv2.imshow('image',img)
cv2.waitKey(0)
cv2.destroyAllWindows()