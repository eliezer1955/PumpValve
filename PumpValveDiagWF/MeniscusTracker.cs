using System;
using System.Drawing;
using System.IO;
using System.Collections.Generic;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace MeniscusTracking
{
    public class InputImagePair
    {
        public InputImagePair(Image<Rgb, byte> b, Image<Rgb, byte> a) { Before = b; After = a; }
        public Image<Rgb, byte> Before;
        public Image<Rgb, byte> After;
    }

    public class PreprocessedImagePair
    {
        public PreprocessedImagePair(NamedImage b, NamedImage a) { After = a; Before = b; }
        public NamedImage After;
        public NamedImage Before;
    }

    public struct NamedImage
    {
        public NamedImage(Image<Gray, byte> i, string n) { Img = i; Name = n; }
        public Image<Gray, byte> Img;
        public String Name;
    }
    public class MeniscusAnalysis
    {
        public InputImagePair InputImages;
        public PreprocessedImagePair PreprocessedImages;
        public List<NamedImage> ProcessImages;
        public double Height;
        public string Timestamp;

        public MeniscusAnalysis(InputImagePair images)
        {
            InputImages = images;
            PreprocessedImages = null;
            ProcessImages = new List<NamedImage>();
            Height = 0;
            Timestamp = DateTime.Now.ToString("yy_MM_ddHHMmmss.ff");
        }
        public void AddImage(Image<Gray, byte> i, string name)
        {
            ProcessImages.Add(new NamedImage(i, name));
        }
    }
    public static class MeniscusTracker
    {
        public struct CCStatsOp
        {
            public Rectangle Rectangle;
            public int Area;
        }

        static int WaitTime = 3500;
        static string DisplayWindow = "Mywindow";

        // Iterations for image erosion
        static int ErodeItr = 3;

        // Iterations for image dilation
        static int DilateItr = 3;

        private static void DisplayComponent(Image<Gray, byte> thresholdedDifference, Rectangle largestComponent)
        {
            thresholdedDifference.Draw(largestComponent, new Gray(64));
            CvInvoke.Imshow("Rect", thresholdedDifference);
            CvInvoke.WaitKey(3000);
        }

        private static void Display(Image<Gray, byte> i)
        {
            CvInvoke.Imshow(DisplayWindow, i);
            CvInvoke.WaitKey(WaitTime);
        }
        private static Rectangle FindLargestNonBgComponent(Image<Gray, byte> thresholdedDifference)
        {
            Mat imgLabel = new Mat();
            Mat stats = new Mat();
            Mat centroids = new Mat();

            //Run connected components analysis
            int nLabel = CvInvoke.ConnectedComponentsWithStats(thresholdedDifference, imgLabel, stats, centroids);
            CCStatsOp[] statsOp = new CCStatsOp[stats.Rows];
            stats.CopyTo(statsOp);

            Rectangle largestComponent = new Rectangle(0, 0, 0, 0);

            // if there is only the background label, then no components were found, so return early
            if (nLabel == 0)
                return largestComponent;

            // Starting from 1 since 0 is the background label.
            int maxval = -1;
            for (int i = 1; i < nLabel; i++)
            {
                int temp = statsOp[i].Area;
                if (temp > maxval)
                {
                    maxval = temp;
                    largestComponent = statsOp[i].Rectangle;
                }
            }

            return largestComponent;
        }

        private static Image<Gray, byte> Crop(Image<Gray, byte> i, Rectangle roi)
        {
            Mat m = new Mat(i.Mat, roi);
            return m.ToImage<Gray, byte>();
        }


        private static Image<Rgb, byte> doAverageClr(List<Image<Rgb, byte>> images)
        {
            Image<Rgb, float> accumulation = new Image<Rgb, float>(640, 480);
            //            Image<Rgb, byte> average = new Image<Rgb, byte>(640, 480);

            foreach (Image<Rgb, byte> i in images)
            {
                accumulation += i.Convert<Rgb, float>();
            }
            accumulation /= images.Count;
            Image<Rgb, byte> average = accumulation.Convert<Rgb, byte>();
            accumulation.Dispose();

            return average;
        }

        private static Image<Gray, byte> doAverage(Image<Gray, byte> before, Image<Gray, byte> after)
        {
            Image<Gray, byte> result = new Image<Gray, byte>(before.Width, before.Height);
            result.Data = before.Data;
            for (int y = 0; y < result.Height; y++)
            {
                for (int x = 0; x < result.Width; x++)
                {
                    result.Data[y, x, 0] = (byte)((before.Data[y, x, 0] + after.Data[y, x, 0]) / 2);
                }
            }
            return result;
            // return ((before.Convert<Gray, float>() + after.Convert<Gray, float>()) / 2).Convert<Gray,byte>();
        }

        private static Image<Gray, byte> doOverlay(Image<Gray, byte> before, Image<Gray, byte> after)
        {
            Image<Gray, byte> result = new Image<Gray, byte>(before.Width, before.Height);
            result.Data = before.Data;
            for (int y = 0; y < result.Height; y++)
            {
                for (int x = 0; x < result.Width; x++)
                {
                    // applying overlay algorithm as described here, as ternary operator
                    // https://en.wikipedia.org/wiki/Blend_modes#Overlay
                    result.Data[y, x, 0] = (byte)(
                        before.Data[y, x, 0] < 127.5
                        ? after.Data[y, x, 0] * (before.Data[y, x, 0] / 127.5)
                        : after.Data[y, x, 0] * ((255 - before.Data[y, x, 0]) / 127.5) + (before.Data[y, x, 0] - (255 - before.Data[y, x, 0])));
                }
            }

            return result;
        }

        private static void Preprocess(MeniscusAnalysis a)
        {
            // create grayscale of before image and normalize its histogram
            // (row, col) = (y, x), NOT (x, y)
            Image<Gray, byte> beforeImgGray = new Image<Gray, byte>(a.InputImages.Before.Rows, a.InputImages.Before.Cols);
            CvInvoke.CvtColor(a.InputImages.Before, beforeImgGray, ColorConversion.Rgb2Gray);
            //            beforeImgGray._EqualizeHist();
            a.AddImage(beforeImgGray, "before_equalized");

            // create grayscale of after image, normalize its histogram, then invert it
            Image<Gray, byte> afterImgGray = new Image<Gray, byte>(a.InputImages.After.Rows, a.InputImages.After.Cols);
            CvInvoke.CvtColor(a.InputImages.After, afterImgGray, ColorConversion.Rgb2Gray);
            //            afterImgGray._EqualizeHist();
            a.AddImage(afterImgGray, "after_equalized");

            // crop the images
            Rectangle roi = new Rectangle(244, 111, 154, 267);
            Image<Gray, byte> beforeImgCrop = Crop(beforeImgGray, roi);
            Image<Gray, byte> afterImgCrop = Crop(afterImgGray, roi);
            a.AddImage(beforeImgCrop, "before_equalized_crop");
            a.AddImage(afterImgCrop, "after_equalized_crop");

            a.PreprocessedImages = new PreprocessedImagePair(
                new NamedImage(beforeImgCrop, "before_equalized_crop"), new NamedImage(afterImgCrop, "after_equalized_crop"));
        }

        public static void ProcessByAverage(MeniscusAnalysis a)
        {
            Image<Gray, byte> afterImgInv = new Image<Gray, byte>(
                a.PreprocessedImages.After.Img.Width,
                a.PreprocessedImages.After.Img.Height);
            CvInvoke.BitwiseNot(a.PreprocessedImages.After.Img, afterImgInv);
            a.AddImage(afterImgInv, "inversion");

            Image<Gray, byte> average = doAverage(a.PreprocessedImages.Before.Img, afterImgInv);
            a.AddImage(average, "average");

            Image<Gray, byte> threshold = average.ThresholdBinary(new Gray(138), new Gray(255));
            a.AddImage(threshold, "threshold");

            Image<Gray, byte> erosion = threshold.Erode(ErodeItr);
            a.AddImage(erosion, "erosion");

            Image<Gray, byte> dilation = erosion.Dilate(DilateItr);
            a.AddImage(dilation, "dilation");
        }

        public static void ProcessByOverlay(MeniscusAnalysis a)
        {
            // average the before and after images
            // this combines the data from what happened before and after
            // and reduces the effect of extraneous minor differences
            Image<Gray, byte> average = doAverage(a.PreprocessedImages.Before.Img, a.PreprocessedImages.After.Img);
            a.AddImage(average, "average");

            // overlay "after" on top of "before" according to the 'Overlay' blending mode (link to explanation inside)
            // this increases the contrast of the region that changed (the "after" region)
            Image<Gray, byte> overlayResult = doOverlay(average, a.PreprocessedImages.After.Img);
            a.AddImage(overlayResult, "overlay");

            // invert the result of this blending
            Image<Gray, byte> overlayInverse = new Image<Gray, byte>(
                a.PreprocessedImages.After.Img.Width, a.PreprocessedImages.After.Img.Height);
            CvInvoke.BitwiseNot(overlayResult, overlayInverse);
            a.AddImage(overlayInverse, "overlayInversion");

            // average the invernted overlay with the "before" image
            // this reduces contrast of all regions that stayed the same, making the subsequent thresholding step easier
            Image<Gray, byte> averagedInverse = doAverage(overlayInverse, a.PreprocessedImages.Before.Img);
            a.AddImage(averagedInverse, "averagedInverse");

            // threshold the result of this averaging to 145 pixels
            Image<Gray, byte> thresholdedAvgInv = averagedInverse.ThresholdBinary(new Gray(145), new Gray(255));
            a.AddImage(thresholdedAvgInv, "thresholdedAvgInv");

            // erode x3 iterations to remove noise
            Image<Gray, byte> erosion = thresholdedAvgInv.Erode(ErodeItr);
            a.AddImage(erosion, "erosion");

            // dilate x3 iterations to re-expand the remaining detected areas
            Image<Gray, byte> dilation = erosion.Dilate(DilateItr);
            a.AddImage(dilation, "dilation");
        }
        public static void ProcessByAbsoluteDifference(MeniscusAnalysis a)
        {
            Image<Gray, byte> diff = a.PreprocessedImages.Before.Img.AbsDiff(a.PreprocessedImages.After.Img);
            a.AddImage(diff, "absolute_difference");

            Image<Gray, byte> threshold = diff.ThresholdBinary(new Gray(10), new Gray(255));
            a.AddImage(threshold, "threshold");

            Image<Gray, byte> erosion = threshold.Erode(ErodeItr);
            a.AddImage(erosion, "erosion");

            Image<Gray, byte> dilation = erosion.Dilate(DilateItr);
            a.AddImage(dilation, "dilation");
        }

        // This should probably accept an average of multiple images for each (before, after), to reduce random noise
        private static void FindFluidArea(MeniscusAnalysis a, Action<MeniscusAnalysis> processFn)
        {
            Preprocess(a);
            processFn(a);
        }

        public static MeniscusAnalysis MeniscusFrom2Img(InputImagePair images, Action<MeniscusAnalysis> processFn)
        {
            MeniscusAnalysis analysis = new MeniscusAnalysis(images);

            // Produces an image with all fluid mapped to solid white (0xFFFFFF)
            // and the rest of the image mapped to black (0x000000)
            FindFluidArea(analysis, processFn);

            // the fluid height is assumed to be the top of the largest contiguous white-shaded area
            analysis.Height = FindLargestNonBgComponent(analysis.ProcessImages.Last().Img).Top;
            //return new MeniscusHeight(largestComponent.Bottom - largestComponent.Top, timestamp);

            return analysis;
        }

        public static MeniscusAnalysis MeniscusFrom2Img(string beforeImgPath, string afterImgPath, Action<MeniscusAnalysis> processFn)
        {
            return MeniscusFrom2Img(
                new InputImagePair(
                    new Image<Rgb, Byte>(beforeImgPath),
                    new Image<Rgb, Byte>(afterImgPath)),
                processFn);
        }
    }
}
