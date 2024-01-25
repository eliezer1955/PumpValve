using System;
using System.Drawing;
using System.IO;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using FileManagement;

namespace MeniscusTracking
{
    public struct MeniscusHeight
    {
        public MeniscusHeight(double v, string t) { Value = v; Timestamp = t; }
        public double Value;
        public string Timestamp;
    }
    public static class MeniscusTracker
    {
        // Threshold value for image binarization
        static int Threshold = 10;

        // Iterations for image erosion
        static int ErodeItr = 5;

        // Iterations for image dilation
        static int DilateItr = 5;

        // Optional field indicating whether to save the gray threshold image
        public static bool SaveThresholdImg = true;

        public struct CCStatsOp
        {
            public Rectangle Rectangle;
            public int Area;
        }

        private static Mat myErode(Mat src, int val)
        {
            int erosion_size = val;
            var dest = new Mat();
            CvInvoke.Erode(src, dest, null, new Point(-1, -1), val, BorderType.Default, CvInvoke.MorphologyDefaultBorderValue);
            return dest;
        }


        // This is currently unused
        private static int MeniscusTop(string inFileName, int frameStart, int frameEnd)
        {
            int frameno;
            int minval = int.MaxValue;

            var capture = new VideoCapture(inFileName);
            Mat frame0 = new Mat();
            BackgroundSubtractorMOG2 backSub = new BackgroundSubtractorMOG2();
            int totFrames = (int)capture.Get(CapProp.FrameCount);
            if (frameEnd <= frameStart)
            {
                frameEnd = totFrames;
            }

            capture.Set(CapProp.PosFrames, frameStart);
            if (!capture.IsOpened)
            {
                System.Console.WriteLine("Unable to open: " + inFileName);
                System.Environment.Exit(0);
            }
            while (true)
            {
                capture.Read(frame0);
                if (frame0.IsEmpty)
                    break;
                frameno = (int)capture.Get(CapProp.PosFrames);
                if (frameno > frameEnd)
                    break;
                Mat fgMask0 = new Mat();
                backSub.Apply(frame0, fgMask0);
                Rectangle rect = new Rectangle(10, 2, 100, 20);
                CvInvoke.Rectangle(frame0, rect, new MCvScalar(255, 255, 255));
                string label = frameno.ToString();
                CvInvoke.PutText(frame0, label, new Point(15, 15),
                            FontFace.HersheySimplex, 0.5, new MCvScalar(0, 0, 0));

                CvInvoke.Imshow("Frame", frame0);
                var frame1 = myErode(fgMask0, 2);
                CvInvoke.Imshow("FG Mask", frame1);
                CvInvoke.WaitKey(30);
                var ret = CvInvoke.BoundingRectangle(frame1);
                if (ret.Top != 0)
                {
                    minval = Math.Min(minval, ret.Top);
                    System.Console.WriteLine(frameno.ToString("G") + " " + ret.Top.ToString("G"));
                }
            }
            return minval;
        }

        private static void DisplayComponent(Image<Gray, byte> thresholdedDifference, Rectangle largestComponent)
        {
            thresholdedDifference.Draw(largestComponent, new Gray(64));
            CvInvoke.Imshow("Rect", thresholdedDifference);
            CvInvoke.WaitKey(3000);
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

        private static Image<Gray, byte> FindThresholdedDifference(
            Image<Rgb, byte> beforeImg, Image<Rgb, byte> afterImg)
        {
            Image<Gray, byte> beforeImgGray = new Image<Gray, byte>(beforeImg.Rows, beforeImg.Cols);
            CvInvoke.CvtColor(beforeImg, beforeImgGray, Emgu.CV.CvEnum.ColorConversion.Rgb2Gray);


            Image<Gray, byte> afterImgGray = new Image<Gray, byte>(afterImg.Rows, afterImg.Cols);
            CvInvoke.CvtColor(afterImg, afterImgGray, Emgu.CV.CvEnum.ColorConversion.Rgb2Gray);

            // Take the absolute-value difference of the before and after images
            // and apply binary threshold, erosion, and dilation to it
            return beforeImgGray.AbsDiff(afterImgGray)
                .ThresholdBinary(new Gray(Threshold), new Gray(255))
                .Erode(ErodeItr)
                .Dilate(DilateItr);
        }

        public static MeniscusHeight MeniscusFrom2Img(Image<Rgb, byte> beforeImg, Image<Rgb, byte> afterImg, bool saveThresholdImg = false)
        {
            Image<Gray, byte> thresholdedDifference =
                FindThresholdedDifference(beforeImg, afterImg);

            // Save threshold img if indicated
            string timestamp = DateTime.Now.ToString("yy_MM_ddHHMmmss.ff");
            if (saveThresholdImg)
            {
                FileManager.SaveImage(
                    thresholdedDifference.ToJpegData(),
                    FileManager.GetArchiveImgPath(FileManager.ThresholdImgName, timestamp));
            }

            Rectangle largestComponent =
                FindLargestNonBgComponent(thresholdedDifference);

            // Display result for sanity check
            // DisplayComponent(thresholdedDifference, largestComponent);

            // Absolute height in pixels of the bounding box of the largest area connected component
            double delta = largestComponent.Bottom - largestComponent.Top;
            System.Console.WriteLine(
                largestComponent.Top.ToString() +
                largestComponent.Bottom.ToString() +
                delta.ToString());

            return new MeniscusHeight(delta, timestamp);
        }

        public static MeniscusHeight MeniscusFrom2Img(
            string beforeImgPath, string afterImgPath, bool saveThresholdImg=false)
        {
            MeniscusHeight meniscus = MeniscusFrom2Img(
                new Image<Rgb, Byte>(beforeImgPath),
                new Image<Rgb, Byte>(afterImgPath),
                saveThresholdImg);

            FileManager.ArchiveImgFiles(beforeImgPath, afterImgPath, meniscus.Timestamp);
            return meniscus;

        }
    }
}
