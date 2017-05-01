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
edges = cv2.Canny(img,100,200)

plt.subplot(121),plt.imshow(img,cmap = 'gray')
plt.title('Original Image'), plt.xticks([]), plt.yticks([])
plt.subplot(122),plt.imshow(edges,cmap = 'gray')
plt.title('Edge Image'), plt.xticks([]), plt.yticks([])
plt.show()


cv2.imshow('image',img)
cv2.waitKey(0)
cv2.destroyAllWindows()