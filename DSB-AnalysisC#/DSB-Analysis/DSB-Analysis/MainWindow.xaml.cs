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

namespace DSB_Analysis
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int colorDiff = 45;
        int count = 0;
        int red, green, blue;
        int redP, greenP, blueP;

        public MainWindow()
        {
            InitializeComponent();

            red = 255;
            blue = 0;
            green = 0;
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
        }

        private void analyzeImage(string path)
        {
            var image = (Bitmap)Image.FromFile(path,true);
            for (var x = 0; x < image.Width; ++x)
            {
                for (var y = 0; y < image.Height; ++y)
                {
                    var pixelColor = image.GetPixel(x, y);
                    redP = pixelColor.R;
                    greenP = pixelColor.G;
                    blueP = pixelColor.B;

                    if (!pixelIsBlack(pixelColor) && Math.Abs(redP - red) <= colorDiff &&
                        Math.Abs(greenP - green) <= colorDiff &&
                        Math.Abs(blueP - blue) <= colorDiff)
                    {
                        ++count;
                    }
                }
            }
            MessageBox.Show(count.ToString(), "", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.None);
        }

        public bool pixelIsBlack(Color pixelColor)
        {
            redP = pixelColor.R;
            greenP = pixelColor.G;
            blueP = pixelColor.B;

            if (redP <= 25 && greenP <= 25 && blueP <= 25)
            {
                return true;
            }
            return false;
        }

        public void checkSurroundingPixels()
        {
            
        }

        public void distanceToMiddle()
        {
            
        }
    }
}
