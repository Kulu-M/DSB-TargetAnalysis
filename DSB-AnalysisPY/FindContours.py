import argparse
import numpy as np
import cv2
from matplotlib import pyplot as plt

img = cv2.imread('D:/GITHUB REPOS/DSB-TargetAnalysis/DSB-AnalysisPY/testPaint.jpg',0)

_,contours,_ = cv2.findContours(img, cv2.RETR_LIST, cv2.CHAIN_APPROX_NONE)

print len(contours)

cv2.drawContours(img, contours, -1, color=(0,255,0), thickness=1)

cv2.imshow('detected circles', img)
cv2.waitKey(0)
cv2.destroyAllWindows()