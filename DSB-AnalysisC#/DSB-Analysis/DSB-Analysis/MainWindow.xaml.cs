using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using Color = System.Drawing.Color;
using Image = System.Drawing.Image;
using Point = System.Drawing.Point;

namespace DSB_Analysis
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int red, green, blue;
        int redP, greenP, blueP;
        public Point middlePoint;
        public List<Point> pointList;
        public List<Point> pointListToIgnore;
        public List<Point> tempWorkingListForConnectedPixels;
        public List<int> resultList;

        public MainWindow()
        {
            InitializeComponent();

            red = Config.desiredColorValueRed;
            blue = Config.desiredColorValueBlue;
            green = Config.desiredColorValueGreen;

            middlePoint.X = Config.middleValueX;
            middlePoint.Y = Config.middleValueY;

            pointList = new List<Point>();
            pointListToIgnore = new List<Point>();
            tempWorkingListForConnectedPixels = new List<Point>();
            resultList = new List<int>();
        }

        private void b_openImg_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog();

            var gotFile = ofd.ShowDialog();

            if (gotFile != true || ofd.FileName == null) return;

            var selectedFile = ofd.FileName;

            var image = new BitmapImage(new Uri(selectedFile));

            img_ImageBox.Source = image;

            analyzeImage(selectedFile);

            presentResult();
        }
        
        private void analyzeImage(string path)
        {
            var image = (Bitmap)Image.FromFile(path , true);
            for (var x = 0; x < image.Width; ++x)
            {
                for (var y = 0; y < image.Height; ++y)
                {
                    var pixelColor = image.GetPixel(x, y);
                    
                    if (!pixelIsOnIgnoreList(new Point(x,y)) && !pixelIsBlack(pixelColor) && pixelIsRed(pixelColor))
                    {
                        getAllSurroundingPixelInSameColor(image, new Point(x, y));
                    }
                }
            }
        }

        public void getAllSurroundingPixelInSameColor(Bitmap image, Point initialPoint)
        {
            tempWorkingListForConnectedPixels.Add(initialPoint);
            pointListToIgnore.Add(initialPoint);

            for (var x = initialPoint.X; x < image.Width; ++x)
            {
                for (var y = initialPoint.Y; y < image.Height; ++y)
                {
                    var searcherPixelColor = image.GetPixel(x, y);

                    if (!pixelIsBlack(searcherPixelColor) && pixelIsRed(searcherPixelColor))
                    {
                        var nextPoint = new Point(x, y);
                        if (!tempWorkingListForConnectedPixels.Any(point => point.Y == nextPoint.Y && point.X == nextPoint.X))
                        {
                            tempWorkingListForConnectedPixels.Add(nextPoint);
                            pointListToIgnore.Add(nextPoint);
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            findNearestPixelToCenter();
        }
        
        private void presentResult()
        {
            string results = "Sie haben folgende Punkte:" + Environment.NewLine;
            var all = 0;
            
            foreach (var result in resultList)
            {
                results += calculateResult(result) + ", ";
                all = all + calculateResult(result);
            }
            results += Environment.NewLine + "Gesamt: " + all;
            MessageBox.Show(results, "Ergebnisse", MessageBoxButton.OK, MessageBoxImage.Information);

            pointList.Clear();
        }

        private int calculateResult(int pointDistanceToMiddle)
        {
            if (pointDistanceToMiddle >= Config.result10Min && pointDistanceToMiddle <= Config.result10Max)
            {
                return 10;
            }
            if (pointDistanceToMiddle >= Config.result9Min && pointDistanceToMiddle <= Config.result9Max)
            {
                return 9;
            }
            if (pointDistanceToMiddle >= Config.result8Min && pointDistanceToMiddle <= Config.result8Max)
            {
                return 8;
            }
            if (pointDistanceToMiddle >= Config.result7Min && pointDistanceToMiddle <= Config.result7Max)
            {
                return 7;
            }
            if (pointDistanceToMiddle >= Config.result6Min && pointDistanceToMiddle <= Config.result6Max)
            {
                return 6;
            }
            if (pointDistanceToMiddle >= Config.result5Min && pointDistanceToMiddle <= Config.result5Max)
            {
                return 5;
            }
            if (pointDistanceToMiddle >= Config.result4Min && pointDistanceToMiddle <= Config.result4Max)
            {
                return 4;
            }
            if (pointDistanceToMiddle >= Config.result3Min && pointDistanceToMiddle <= Config.result3Max)
            {
                return 3;
            }
            if (pointDistanceToMiddle >= Config.result2Min && pointDistanceToMiddle <= Config.result2Max)
            {
                return 2;
            }
            return 1;
        }

        #region HELPERS

        public bool pixelIsBlack(Color pixelColor)
        {
            redP = pixelColor.R;
            greenP = pixelColor.G;
            blueP = pixelColor.B;

            if (redP <= Config.blackValueOffset && greenP <= Config.blackValueOffset && blueP <= Config.blackValueOffset)
            {
                return true;
            }
            return false;
        }

        public bool pixelIsRed(Color pixelColor)
        {
            redP = pixelColor.R;
            greenP = pixelColor.G;
            blueP = pixelColor.B;

            if (Math.Abs(redP - red) <= Config.colorDifferenceOffset &&
                        Math.Abs(greenP - green) <= Config.colorDifferenceOffset &&
                        Math.Abs(blueP - blue) <= Config.colorDifferenceOffset)
            {
                return true;
            }
            return false;
        }

        public int calculateDistanceToMiddle(Point point)
        {
            // Pythargoras theorem
            var aSquare = middlePoint.X - point.X;
            var bSquare = middlePoint.Y - point.Y;

            var distance = Math.Sqrt(aSquare * aSquare + bSquare * bSquare);

            return (int)Math.Round(distance, MidpointRounding.AwayFromZero);
        }

        private bool pixelIsOnIgnoreList(Point p)
        {
            if (pointListToIgnore.Contains(p)) return true;
            return false;
        }

        private void findNearestPixelToCenter()
        {
            var distanceList = new List<int>();
            foreach (var point in tempWorkingListForConnectedPixels)
            {
                distanceList.Add(calculateDistanceToMiddle(point));
            }
            tempWorkingListForConnectedPixels.Clear();
            
            resultList.Add(distanceList.Min());
        }

        #endregion HELPERS

    }
}
