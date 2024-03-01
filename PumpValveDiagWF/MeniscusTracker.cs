using System;
using System.Drawing;
using System.Collections.Generic;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Linq;
using MeniscusTracking;
using Microsoft.SolverFoundation.Services;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace MeniscusTracking
{
    public class Meniscus
    {
        public int BrightestIndex;
        public int EndIndex;
        public double MaxBrightness;
        public int StartIndex;
        public int Thickness { get { return EndIndex - StartIndex; } }

        public Meniscus(int s, double m)
        {
            BrightestIndex = s;
            EndIndex = s;
            MaxBrightness = m;
            StartIndex = s;
        }

        public void AddIndex(int i, double brightness)
        {
            EndIndex = i;
            if (brightness > MaxBrightness)
            {
                MaxBrightness = brightness;
                BrightestIndex = i;
            }
        }

        public bool IsAdjacentTo(int i)
        {
            return i - EndIndex == 1;
        }

        public static Func<Meniscus, Meniscus, bool> GetCompareFn()
        {
            return (x, y) => x.MaxBrightness > y.MaxBrightness;
        }
    }
    public struct CCStatsOp
    {
        public Rectangle Rectangle;
        public int Area;
    }

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
        public string Name;
    }


    public struct Pair
    {
        public int Bottom;
        public int Top;

        public Pair(int b, int t)
        {
            Bottom = b;
            Top = t;
        }

        public int GetDifference() { return Bottom - Top; }
    }

    // MeniscusAnalysis stores static and dynamic state about the image and fluid chamber
    // For example, this could include rotor position, pixel-to-ml conversion factors, etc
    // It should NOT store algorithmic constants necessary to perform the actual image analysis
    // It also stores the result of the MeniscusTracker's analysis
    // Think of MeniscusAnalysis as a combination config object and extended argument/return param list for MeniscusTracker
    public class MeniscusAnalysis
    {
        public enum DeliveryVolume { ZERO_TO_ONE, ONE_TO_ZERO, ZERO_TO_TWO, TWO_TO_ZERO };

        // Pixel constants, as measured from TOP of image
        public const int MeniscusHeightPx1ml68k = 87;
        public const int MeniscusHeightPx2ml68k = 49;
        // public const int RotorHeightPx = 90;
        public const int RotorTopPx68k = 126;
        public const int RotorTopPx78k = 161;
        public const int RotorZeroPosPxOffset = 115;

        public float ExpectedVolumeDelivered;

        // Numeric data about fluid and chamber
        // Bottom of the change in column of water. This is usually the top of the grinder, but is conceptually different and therefore
        // should be a separate variable (in the future, we could measure fluid level changes that do not remove all fluid from the chamber)
        public int Bottom;
        public int ExpectedGrinderTopPixelPos { get { return CalculateExpectedGrinderTopPixelPos(RotorSteps); } }

        public static int PixelsPer1ml { get { return (int)(-1 * GetChangeRatePointSlope(MeniscusHeightPx1ml68k, MeniscusHeightPx2ml68k, 1, 2)); } }
        public int RotorSteps;
        // Top of the change in column of water
        // Should "Top" be replaced by a List<int> Menisci? If the change is not all the way to zero, there will be 2 menisci.
        public int Top;

        // Images
        public Image<Rgb, byte> Illustration;
        public InputImagePair InputImages;
        public PreprocessedImagePair PreprocessedImages;
        public List<NamedImage> ProcessImages;
        public List<Meniscus> Meniscii;

        // Misc
        public Action<MeniscusAnalysis> ProcessFn;
        public string Timestamp;

        public MeniscusAnalysis(Image<Rgb,byte> beforeImg, Image<Rgb, byte> afterImg, Action<MeniscusAnalysis> pf, int rotorSteps)
        {
            Bottom = 0;
            // Temporarily initialize to zero
            ExpectedVolumeDelivered = 0;
            InputImages = new InputImagePair(beforeImg, afterImg);
            PreprocessedImages = null;
            ProcessImages = new List<NamedImage>();
            ProcessFn = pf;
            RotorSteps = rotorSteps;
            Timestamp = DateTime.Now.ToString("yy_MM_ddHHMmmss.ff");
            Top = 0;
        }
        public void AddImage(Image<Gray, byte> i, string name)
        {
            ProcessImages.Add(new NamedImage(i,name));
        }

        public static int CalculateExpectedGrinderTopPixelPos(int steps)
        {
            return (int)(GetRotorStepsToPixelsConversion() * steps) - RotorZeroPosPxOffset;
        }

        public int CalculateExpectedMeniscusPixelPos(float fluidVolumeMilli)
        {
            return (int)(ExpectedGrinderTopPixelPos - fluidVolumeMilli * PixelsPer1ml);
        }

        // GetRotorStepsToPixelsConversion returns the conversion factor between rotor steps and the pixel position of the top of the rotor
        // This is the slope calculated by (RotorStepsPos2 - RotorStepsPos1) / (RotorPixelPos2 - RotorPixelPos1)
        public static double GetRotorStepsToPixelsConversion()
        {
            return GetChangeRatePointSlope(RotorTopPx68k, RotorTopPx78k, 68000, 78000);
        }

        public static double GetChangeRatePointSlope(double y1, double y2, double x1, double x2)
        {
            return (y2 - y1) / (x2 - x1);
        }

        /*
        public static bool IsGreaterByPercent(float expectedGreater, float expectedLesser, int percent)
        {
            return (((expectedGreater - expectedLesser) / expectedLesser) * 100) > percent;
        }
        */
        public static bool IsWithinMilliliters(float actualLocation, float expectedLocation, float milliliters)
        {
            float max = expectedLocation + milliliters * PixelsPer1ml;
            float min = expectedLocation - milliliters * PixelsPer1ml;
            return actualLocation <= max && actualLocation >= min;
        }

        public bool FluidWasDelivered(DeliveryVolume volume)
        {
            // If no meniscii were seen, no fluid was delivered
            if (Meniscii.Count == 0)
                return false;

            // Giving generous leeway of +/- 0.4 ml. Since fluid can fall under the grinder, meniscus may not be at expected level.
            float leeway = 0.4f;
            switch (volume)
            {
                case DeliveryVolume.ZERO_TO_ONE:
                    // There should only be one meniscus when starting from zero. However this may be too aggressive if camera is imperfect
                    if (Meniscii.Count > 1)
                        return false;

                    return IsWithinMilliliters(Meniscii[0].BrightestIndex, CalculateExpectedMeniscusPixelPos(1), leeway);
                // If all fluid was removed, there will be 1 visible meniscus at the 1ml level. However, if not all fluid was removed,
                // there will be 2 meniscii. If there are 2, the 2nd (lower) meniscus should be used
                case DeliveryVolume.ONE_TO_ZERO:
                    if (Meniscii.Count == 1)
                    {
                        return IsWithinMilliliters(Meniscii[0].BrightestIndex, CalculateExpectedMeniscusPixelPos(1), leeway);
                    } else if (Meniscii.Count == 2)
                    {
                        // using much smaller leeway of 0.1ml since the fluid level should be nearly flush with top of grinder
                        return IsWithinMilliliters(Meniscii[1].BrightestIndex, ExpectedGrinderTopPixelPos, 0.1f);
                    } else
                    {
                        // there should never be more than 2 meniscii. if there are, log an error and return false
                        return false;
                    }
                case DeliveryVolume.ZERO_TO_TWO:
                    // There should only be one meniscus when starting from zero. However this may be too aggressive if camera is imperfect
                    if (Meniscii.Count > 1)
                        return false;

                    return IsWithinMilliliters(Meniscii[0].BrightestIndex, CalculateExpectedMeniscusPixelPos(2), leeway);
                case DeliveryVolume.TWO_TO_ZERO:
                    if (Meniscii.Count == 1)
                    {
                        return IsWithinMilliliters(Meniscii[0].BrightestIndex, CalculateExpectedMeniscusPixelPos(2), leeway);
                    }
                    else if (Meniscii.Count == 2)
                    {
                        // using much smaller leeway of 0.1ml since the fluid level should be nearly flush with top of grinder
                        return IsWithinMilliliters(Meniscii[1].BrightestIndex, ExpectedGrinderTopPixelPos, 0.1f);
                    }
                    else
                    {
                        // there should never be more than 2 meniscii. if there are, log an error and return false
                        return false;
                    }
                default:
                    // Should actually log an error and raise an exception here for "unknown case"
                    // But also, this code should never be hit, since all cases should be accounted for above
                    return false;
            };
        }
    }

    // MeniscusTracker stores methods and static state necessary to perform a visual analysis of the fluid chamber
    // For example, this could include number of iterations for erosion and dilation, or any other algorithmic constants
    // necessary to analyze an image
    // It should NOT store dynamic state, and it should NOT store any information about the condition of the fluid chamber
    // Intended use of MeniscusTracker is to give it a MeniscusAnalysis object and a desired analysis function. MeniscusTracker
    // will then update the state of MeniscusAnalysis and return the result as a field on the MeniscusAnalysis object
    public static class MeniscusTracker
    {
        // Iterations for image erosion
        const int ErodeItr = 3;

        // Iterations for image dilation
        const int DilateItr = 3;

        const string DisplayWindow = "Mywindow";
        // Minimum total brightness level required for an image row to be considered part of a meniscus
        //        const int MeniscusBrightnessMinimum = 700;
        const int MeniscusBrightnessMinimum = 550;
        // Minimum number of contiguous image rows required for a series of "sufficiently bright" rows to count as being a meniscus
        // A meniscus is <MeniscusThicknessMinimum> contiguous rows, all of which are at least <MeniscusBrightnesMinimun> brightness level
        const int MeniscusThicknessMinimum = 6;

        const int WaitTime = 3500;

        // AnnotateDrawingForTest adds two dots indicating the actual benchmark locations of the bottom and meniscus locations, as
        // measured by hand and recorded in the test case benchmarks file. The bottom dot is smaller than the meniscus dot and should
        // always appear beneath it. Both dots are red and should appear to the right of the image.
        public static void AnnotateDrawingForTest(Image<Rgb, byte> i, int bottom, int meniscus)
        {
            CvInvoke.Rectangle(i, new Rectangle(30, bottom, 4, 1), new MCvScalar(10, 10, 255));
            CvInvoke.Rectangle(i, new Rectangle(30, meniscus, 8, 1), new MCvScalar(10, 10, 255));
        }

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

        private static void FindFluidColumnHeight(MeniscusAnalysis a)
        {
            Mat imgLabel = new Mat();
            Mat stats = new Mat();
            Mat centroids = new Mat();

            // Run connected components analysis (get the stats off the image)
            int nLabel = CvInvoke.ConnectedComponentsWithStats(a.ProcessImages.Last().Img, imgLabel, stats, centroids);
            CCStatsOp[] statsOp = new CCStatsOp[stats.Rows];
            stats.CopyTo(statsOp);

            // if there is only the background label, then no components were found, so return early
            if (nLabel == 0)
            {
                return;
            }

            // find the weighted average of column top and bottom
            float total1dArea = 0;
            List<Pair> pairs = new List<Pair>();
            List<Rectangle> amendedRects = new List<Rectangle>();

            for (int i = 1; i < nLabel; i++)
            {
                Rectangle r = statsOp[i].Rectangle;

                // Emgu's coordinate system has (0,0) as the top-left corner of image, with the bottom of the image having the largest
                // pixel value. Essentially it is upside-down.
                // skip rectangles that are below the grinder top
                if (r.Top > a.ExpectedGrinderTopPixelPos)
                {
                    continue;
                }

                Pair p = new Pair(r.Bottom, r.Top);

                // snap the bottom of the rectangle to the top of the grinder
                if (r.Bottom > a.ExpectedGrinderTopPixelPos) {
                    p.Bottom = a.ExpectedGrinderTopPixelPos;
                }

                total1dArea += p.GetDifference();
                pairs.Add(p);
                amendedRects.Add(new Rectangle(r.X, r.Y, r.Width, p.GetDifference()));
            }

            float avgTop = 0;
            float avgBottom = 0;
            foreach (Pair p in pairs)
            {
                float area = p.GetDifference();
                avgTop += (area / total1dArea) * p.Top;
                avgBottom += (area / total1dArea) * p.Bottom;
            }

            a.Top = (int)avgTop;
            a.Bottom = (int)avgBottom;

            // add a line showing the identified fluid column height
            amendedRects.Add(new Rectangle(4, a.Top, 2, a.Bottom - a.Top));

            a.Illustration = a.ProcessImages.Last().Img.Convert<Rgb, byte>();

            // lines for top of grinder, 1ml, and 2ml heights
            CvInvoke.Rectangle(a.Illustration, new Rectangle(0, a.ExpectedGrinderTopPixelPos, 479, 0), new MCvScalar(0, 255, 255), 1);
            CvInvoke.Rectangle(a.Illustration, new Rectangle(0, 127, 479, 0), new MCvScalar(255, 255, 0), 1);
            CvInvoke.Rectangle(a.Illustration, new Rectangle(0, 85, 479, 0), new MCvScalar(255, 255, 0), 1);

            for (int i = 0; i < amendedRects.Count - 1; i++)
            {
                Rectangle r = amendedRects[i];
                // draw a red box around the change area
                CvInvoke.Rectangle(a.Illustration, r, new MCvScalar(10, 10, 255), 2);
            }

            // final fluid column measured height
            CvInvoke.Rectangle(a.Illustration, amendedRects.Last(), new MCvScalar(0, 255, 0), 2);

            return;
        }

        private static Image<Gray, byte> Crop(Image<Gray, byte> i, Rectangle roi)
        {
            Image<Gray, byte> cropped = new Image<Gray, byte>(roi.Height,roi.Width);
            i.ROI = roi;
            cropped = i.Copy();
            CvInvoke.cvResetImageROI(i);
            return cropped;
        }

        private static void preprocess(MeniscusAnalysis a)
        {
            // create grayscale of before image and normalize its histogram
            // (row, col) = (y, x), NOT (x, y)
            Image<Gray, byte> beforeImgGray = new Image<Gray, byte>(a.InputImages.Before.Width, a.InputImages.Before.Height);
            CvInvoke.CvtColor(a.InputImages.Before, beforeImgGray, ColorConversion.Rgb2Gray);
            a.AddImage(beforeImgGray, "before_gray");

            // create grayscale of after image, normalize its histogram, then invert it
            Image<Gray, byte> afterImgGray = new Image<Gray, byte>(a.InputImages.After.Rows, a.InputImages.After.Cols);
            CvInvoke.CvtColor(a.InputImages.After, afterImgGray, ColorConversion.Rgb2Gray);
            a.AddImage(afterImgGray, "after_gray");

            // crop the images
            // adding 10px to the left side of the cropping window
            Rectangle roi = new Rectangle(244, 111, 154, 267);
            Image<Gray, byte> beforeImgCrop = Crop(beforeImgGray, roi);
            Image<Gray, byte> afterImgCrop = Crop(afterImgGray, roi);
            a.AddImage(beforeImgCrop, "before_gray_crop");
            a.AddImage(afterImgCrop, "after_gray_crop");

            a.PreprocessedImages = new PreprocessedImagePair(
                new NamedImage(beforeImgCrop, "before_gray_crop"), new NamedImage(afterImgCrop, "after_gray_crop"));
        }

        private static Image<Gray, byte> applyErosionWithStructuring(Image<Gray, byte> i)
        {
            Image<Gray, byte> eroded = new Image<Gray, byte>(i.Cols, i.Rows);
            Mat structuringElement = CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(3, 1), new Point(-1, 0));
            CvInvoke.Erode(i, eroded, structuringElement, new Point(-1, 0), 7, BorderType.Constant, new MCvScalar(0, 0, 0));
            return eroded;
        }

        // blackOutExclusionZone replaces all data in the indicated rows with 0x000000
        private static Image<Gray, byte> blackOutExclusionZone(Image<Gray, byte> img, int[]exclusionZone)
        {
            Image<Gray, byte> blackedOut = new Image<Gray, byte>(img.Width, img.Height);
            for (int y = 0; y < img.Height; y++)
            {
                for (int x = 0; x < img.Width; x++)
                {
                    if (x >= exclusionZone[0] && x <= exclusionZone[1])
                    {
                        blackedOut.Data[y, x, 0] = 0;
                    } else
                    {
                        blackedOut.Data[y, x, 0] = img.Data[y, x, 0];
                    }
                }
            }
            return blackedOut;
        }

        private static Mat getSummedImageRows(Image<Gray, byte> i)
        {
            Mat rowSum = new Mat();
            rowSum.Create(i.Rows, 1, DepthType.Cv64F, 1);
            CvInvoke.Reduce(i.Convert<Gray, double>(), rowSum, ReduceDimension.SingleCol);
            return rowSum;
        }

        public static void ProcessByAbsoluteDifference(MeniscusAnalysis a)
        {
            Image<Gray, byte> diff = a.PreprocessedImages.Before.Img.AbsDiff(a.PreprocessedImages.After.Img);
            a.AddImage(diff, "absolute_difference");

            Image<Gray, byte> threshold = diff.ThresholdBinary(new Gray(3), new Gray(255));
            a.AddImage(threshold, "threshold");

            Image<Gray, byte> erosion = threshold.Erode(5);
            a.AddImage(erosion, "erosion");

            Image<Gray, byte> dilation = erosion.Dilate(5);
            a.AddImage(dilation, "dilation");

            // the fluid height is assumed to be the top of the largest contiguous white-shaded area
            FindFluidColumnHeight(a);
        }

        // findBrightestLine finds the highest value of all values in rowSum
        private static double findBrightestLine(Mat rowSum)
        {
            Array rsData = rowSum.T().GetData();
            int i = 0;
            double brightestLine = (double)rsData.GetValue(0, i);

            foreach (double item in rsData)
            {
                if (item > brightestLine)
                {
                    brightestLine = item;
                }
            }

            return brightestLine;
        }

        // findAllMeniscii finds all possible meniscii above the indicated cutoff, and returns their indices in a list.
        // A contiguous region of rows is considered a possible meniscus if it is brighter than some minimum threshold. Since a single
        // meniscus could span many rows, the brightest index of each such grouping is considered the center of the meniscus.
        private static List<Meniscus> findAllMeniscii(Mat rowSum, int cutoff, int minBrightness, int minThickness)
        {
            List<Meniscus> meniscii = new List<Meniscus>();
            Array rsData = rowSum.T().GetData();

            int i = 0;
            Meniscus m = null;
            foreach (double item in rsData)
            {
                if (i > cutoff)
                    break;

                if (item > minBrightness)
                {
                    if (m == null)
                    {
                        // No meniscus, so make one
                        m = new Meniscus(i, item);
                    } else if (m.IsAdjacentTo(i))
                    {
                        // Current index is next to meniscus, so add it to meniscus
                        m.AddIndex(i, item);
                    }
                    else
                    {
                        // Current index is not next to meniscus, so end collection for this meniscus
                        // Only add the meniscus to the list of meniscii if it has the correct number of contiguous lines
                        if (m.Thickness >= minThickness)
                        {
                            meniscii.Add(m);
                        }
                        m = null;

                        // Only two meniscii are needed
                        if (meniscii.Count == 2)
                            break;
                    }
                }
                i++;
            }

            // Save the final meniscus if it hasn't been saved yet
            if (m != null)
                meniscii.Add(m);

            return meniscii;
        }

        public static void ProcessByHorizontalPeakFinder(MeniscusAnalysis a)
        {
            //subtract reference from new image (absolute difference)
            Image<Gray, byte> diff = a.PreprocessedImages.Before.Img.AbsDiff(a.PreprocessedImages.After.Img);
            a.AddImage(diff, "absolute_difference");

            // apply erosion with structuring element to reduce impact of water droplets and other noise
            Image<Gray, byte> erosion = applyErosionWithStructuring(diff);
            a.AddImage(erosion, "eroded_with_structuring");

            // apply erosion with structuring element to reduce impact of water droplets and other noise
            Image<Gray, byte> blackedOutErosion = blackOutExclusionZone(erosion, new int[] {52, 103});
            a.AddImage(blackedOutErosion, "blacked_out_erosion");

            //get vector of horizontal sum of all image elements. Result is a 1-column vector with values summed in the direction of each row.
            Mat rowSum = getSummedImageRows(blackedOutErosion);

            double brightestLine = findBrightestLine(rowSum);

            // find the index of the brightest line above the grinder, or brightest meniscus
            a.Meniscii = findAllMeniscii(rowSum, a.ExpectedGrinderTopPixelPos, MeniscusBrightnessMinimum, MeniscusThicknessMinimum);
            if (a.Meniscii.Count > 0)
            {
                a.Top = (int)a.Meniscii.First().BrightestIndex;
            }

            Image<Gray, byte> drawing = new Image<Gray, byte>(20 + a.PreprocessedImages.After.Img.Cols, rowSum.Rows);
            Image<Gray, double> img = rowSum.ToImage<Gray, double>();
            
            for (int y = 0; y < a.PreprocessedImages.After.Img.Height; y++)
            {
                // draw in the left-hand sidebar
                for (int x = 0; x < 20; x++)
                {
                    // create the normalized left-hand bar                    
                    drawing.Data[y, x, 0] = (byte)(img.Data[y, 0, 0] * (255 / brightestLine));
                }

                // draw in the 'after' image
                for (int x = 0; x < a.PreprocessedImages.After.Img.Width; x++)
                {
                    drawing.Data[y, x + 20, 0] = a.PreprocessedImages.After.Img.Data[y, x, 0];
                }

                // add the threshold line
                int xOffset = (byte)(img.Data[y, 0, 0] * (double)(267 / (2 * brightestLine)));
                xOffset = xOffset > 0 ? xOffset : 0;
                drawing.Data[y, xOffset, 0] = 255;
                xOffset = xOffset - 1 > 0 ? xOffset - 1 : 0;
                drawing.Data[y, xOffset, 0] = 0;
            }
            a.Illustration = drawing.Convert<Rgb,byte>();

            // lines for EXPECTED top of grinder, 1ml, and 2ml meniscus heights
            CvInvoke.Rectangle(a.Illustration, new Rectangle(20, a.ExpectedGrinderTopPixelPos, 479, 0), new MCvScalar(0, 255, 255));
            // color should be cyan
            CvInvoke.Rectangle(a.Illustration, new Rectangle(20, a.CalculateExpectedMeniscusPixelPos(1), 479, 0), new MCvScalar(255, 255, 0));
            // color should be magenta
            CvInvoke.Rectangle(a.Illustration, new Rectangle(20, a.CalculateExpectedMeniscusPixelPos(2), 479, 0), new MCvScalar(255, 0, 255));
            // final fluid column measured height, should be green
            CvInvoke.Rectangle(a.Illustration, new Rectangle(24, a.Top, 2, a.ExpectedGrinderTopPixelPos - a.Top), new MCvScalar(0, 255, 0));

            // lines for the meniscii
            if(a.Meniscii.Count > 0)
            {
                CvInvoke.Rectangle(a.Illustration, new Rectangle(20, a.Meniscii[0].BrightestIndex, 479, 0), new MCvScalar(0, 255, 0));
            }
            if(a.Meniscii.Count > 1)
            {
                CvInvoke.Rectangle(a.Illustration, new Rectangle(20, a.Meniscii[1].BrightestIndex, 479, 0), new MCvScalar(0, 150, 0));
            }
        }

        public static void MeniscusFrom2Img(MeniscusAnalysis a)
        {
            preprocess(a);
            a.ProcessFn(a);
            return;
        }
    }
}
