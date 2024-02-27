using MeniscusTracking;
using System.Drawing.Imaging;
using System.Drawing;

namespace MeniscusTrackingTests
{
    public class Pair
    {
        public int Bottom { get; set; }
        public int Top { get; set; }
    }
    public class Benchmarks
    {
        public int RotorSteps { get; set; }
        public Dictionary<string, Pair>? Cases { get; set; }
    }

    [TestClass]
    public class MeniscusTrackingTest
    {

        // The project launches in \MeniscusTrackingTests\\bin\\Debug\\net8.0, so it's necessary to navigate up
        // three levels to get back to the MeniscusTrackingTests directory
        static string MeniscusTrackingTestsHome = "..\\..\\..";
        public string GetDataDir()
        {
            return $"{Directory.GetCurrentDirectory()}\\{MeniscusTrackingTestsHome}\\TestData";
        }

        public string GetOutputDir()
        {
            return $"{Directory.GetCurrentDirectory()}\\{MeniscusTrackingTestsHome}\\TestData\\TestOutput";
        }

        public string makeSubcaseName(string before, string after)
        {
            return $"{before}-{after}";
        }

        public Benchmarks? readBenchmarksFile(string testCase)
        {
            string benchmarksFile = $"{GetDataDir()}\\{testCase}\\benchmarks.json";
            if (!File.Exists(benchmarksFile))
            {
                return null;
            }

            string fileContents = File.ReadAllText(benchmarksFile);
            if (fileContents == "" || fileContents == null)
            {
                return null;
            }

            return System.Text.Json.JsonSerializer.Deserialize<Benchmarks>(fileContents);
        }

        public string calculateErrStr(int observed, int expected)
        {
            float err = 100.0f * (((float)observed - (float)expected) / (float)expected);
            return err.ToString("F2") + "%";
        }

        public void saveAllAnalysisImages(string saveDir, MeniscusAnalysis a)
        {
            using (Image img = Image.FromStream(new MemoryStream(a.InputImages.Before.ToJpegData())))
            {
                img.Save($"{saveDir}\\before_t.jpg", ImageFormat.Jpeg);
            }
            using (Image img = Image.FromStream(new MemoryStream(a.InputImages.After.ToJpegData())))
            {
                img.Save($"{saveDir}\\after_t.jpg", ImageFormat.Jpeg);
            }
            foreach (NamedImage img in a.ProcessImages)
            {
                using (Image image = Image.FromStream(new MemoryStream(img.Img.ToJpegData())))
                {
                    image.Save($"{saveDir}\\{img.Name}.jpg", ImageFormat.Jpeg);
                }
            }
            using (Image image = Image.FromStream(new MemoryStream(a.Illustration.ToJpegData())))
            {
                image.Save($"{saveDir}\\drawing.jpg", ImageFormat.Jpeg);
            }
        }

        public string saveResults(string testCase, string before, string after, MeniscusAnalysis a, Pair? subcase, string header)
        {
            string saveDir = $"{GetOutputDir()}\\{testCase}_{makeSubcaseName(before, after)}";
            if (!Directory.Exists(saveDir))
                Directory.CreateDirectory(saveDir);

            if (subcase != null)
                MeniscusTracker.AnnotateDrawingForTest(a.Illustration, subcase.Bottom, subcase.Top);

            saveAllAnalysisImages(saveDir, a);

            // If a benchmarks file exists, compare the results to the benchmarks and save the results along with the error
            // Benchmarks must be hand-identified by looking at the cropped analysis image and deciding where the meniscus should be
            string scNameCsvSafe = makeSubcaseName(before, after).Replace(',', '.');
            string result = $"{testCase},{scNameCsvSafe},{a.FluidChangeTop},,,{a.FluidChangeBottom},,";
            if (subcase != null)
            {
                result = $"{testCase},{scNameCsvSafe},{a.FluidChangeTop},{subcase.Top},{calculateErrStr(a.FluidChangeTop, subcase.Top)},{a.FluidChangeBottom},{subcase.Bottom},{calculateErrStr(a.FluidChangeBottom, subcase.Bottom)}";
            }
            string resultFile = $"{saveDir}\\result_{a.Timestamp}.csv";
            using (StreamWriter outputFile = new StreamWriter(resultFile))
            {
                outputFile.WriteLine(header);
                outputFile.WriteLine(result);
            }

            // Copy case notes into output directory for reference
            string notesFile = $"{GetDataDir()}\\{testCase}\\note.txt";
            if (File.Exists(notesFile) && !File.Exists($"{saveDir}\\note.txt"))
            {
                File.Copy(notesFile, $"{saveDir}\\note.txt");
            }

            return result;
        }

        public string floatToMlName(float v)
        {
            return $"{v.ToString("F1").Replace(".", ",")}ml";
        }

        [TestMethod]
        public void MeniscusFrom2Img_RunAllCases_Full()
        {
            // associate analysis functions with their names
            Dictionary<Action<MeniscusAnalysis>, string> processFunctions = new Dictionary<Action<MeniscusAnalysis>, string>();
            //            processFunctions[MeniscusTracker.ProcessByHorizontalPeakAbsDiff] = "processByHorizontalPeakFinder";
            processFunctions[MeniscusTracker.ProcessByHorizontalPeakRawSubtraction] = "processByHorizontalPeakRawSubtraction";

            string[] cases = [
                // "happy path" cases
                //"Eth78kDfCnstOnRotor",
                //"Eth75kDfCnst",
                //"Eth75kDfCnstDp",
                //"Eth68kDfCnstWdp",
                "Eth68kDfCnstOnRotor",

                // "sad path" cases
                //"EthDfNochange",
                "EthDfInsufficientChange",
            ];
            //            cases.Add("Eth75kNogain");
            //            cases.Add("EthL75k");

            string[][] comparisons = [
                // the "happy path" cases
                //["0,0ml", "1,0ml"],
                ["1,0ml", "0,0ml"],
                //["0,0ml", "2,0ml"],
                ["2,0ml", "0,0ml"],

                // The "no change" cases
                //["1,0ml", "1,0ml 2"],
                //["2,0ml", "2,0ml 2"],

                // The "insufficient change" cases
                //["0,5ml", "1,0ml"],
                //["1,5ml", "2,0ml"],
                ["2,0ml", "1,5ml"],
                //["1,5ml", "2,0ml"],
            ];


            // create collection of results
            List<string> allResults = new List<string>();
            string header = "CaseName,Comparison,Top,BenchmarkTop,TPctErr,Bottom,BenchmarkBottom,BPctErr";
            allResults.Add(header);

            // iterate over all directories in the TestData folder
            foreach (string testCase in cases)
            {
                Benchmarks? benchmarks = readBenchmarksFile(testCase);

                foreach (KeyValuePair<Action<MeniscusAnalysis>, string> fn in processFunctions)
                {
                    foreach (string[] comparison in comparisons)
                    {
                        // Perform the analysis
                        string before = $"{GetDataDir()}\\{testCase}\\{comparison[0]}.bmp";
                        string after = $"{GetDataDir()}\\{testCase}\\{comparison[1]}.bmp";
                        if (File.Exists(before) && File.Exists(after))
                        {
                            MeniscusAnalysis a = new MeniscusAnalysis(before, after, fn.Key, benchmarks == null ? 68000 : benchmarks.RotorSteps);
                            MeniscusTracker.MeniscusFrom2Img(a);

                            // Get the subcase
                            Pair? subcase = null;
                            if (benchmarks != null && benchmarks.Cases != null)
                            {
                                string subcaseName = makeSubcaseName(comparison[0], comparison[1]);
                                if (benchmarks.Cases.ContainsKey(subcaseName))
                                {
                                    subcase = benchmarks.Cases[subcaseName];
                                }
                            }

                            // Record the results
                            allResults.Add(saveResults(testCase, comparison[0], comparison[1], a, subcase, header));
                        }
                    }
                }
            }

            // write a single file containing all results
            using (StreamWriter outputFile = new StreamWriter($"{GetOutputDir()}\\allResults_{DateTime.Now.ToString("yy_MM_ddHHMmmss.ff")}.csv"))
            {
                foreach (string r in allResults)
                {
                    outputFile.WriteLine(r);
                }
            }

            // dummy assert to make the "test case" always pass, this way the only failures reported will be due to real exceptions
            Assert.AreEqual(49, 49);
        }

        [TestMethod]
        public void logfile()
        {
            File.AppendAllLines("C:\\ProgramData\\LabScript\\DeviceLog\\danlog.txt", new string[]{"test"});
        }

            [TestMethod]
        public void MeniscusFrom2Img_CharacterizationReport()
        {
            // This reports the actual pixel height of the detected meniscus vs its expected pixel height
            // The purpose is to produce a linear plot of "expected meniscus location" to "actual meniscus location"

            // create collection of results
            List<string> allResults = new List<string>();
            string header = "StartingVolMilli,EndingVolMilli,PredictedMeniscusHeightPx,ObservedMeniscusHeightPx,ActualMeniscusHeightPx,PctErrPredicted-Observed,PctErrPredicted-Actual,PctErrObserved-Actual";
            allResults.Add(header);

            float[][] comparisons = [
                [0, 0.5f],
                [0, 1.0f],
                [0, 1.5f],
                [0, 2.0f],
            ];

            string[] cases = [
                "Eth68kDfCnstOnRotor",
            ];

            string saveDir = $"{GetOutputDir()}\\CharacterizationReport";

            // iterate over all directories in the TestData folder
            foreach (string testCase in cases)
            {
                Benchmarks? benchmarks = readBenchmarksFile(testCase);
                foreach (float[] comparison in comparisons)
                {
                    // Perform the analysis
                    string before = $"{GetDataDir()}\\{testCase}\\{floatToMlName(comparison[0])}.bmp";
                    string after = $"{GetDataDir()}\\{testCase}\\{floatToMlName(comparison[1])}.bmp";
                    if (File.Exists(before) && File.Exists(after))
                    {
                        MeniscusAnalysis a = new MeniscusAnalysis(before, after, MeniscusTracker.ProcessByHorizontalPeakAbsDiff, 68000);
                        MeniscusTracker.MeniscusFrom2Img(a);

                        // Get the subcase
                        Pair? subcase = null;
                        if (benchmarks != null && benchmarks.Cases != null)
                        {
                            subcase = benchmarks.Cases[makeSubcaseName(floatToMlName(comparison[0]), floatToMlName(comparison[1]))];
                        }

                        // Record the results
                        string subcaseDir = $"{saveDir}\\{testCase}_{makeSubcaseName(floatToMlName(comparison[0]), floatToMlName(comparison[1]))}";
                        if (!Directory.Exists(subcaseDir))
                            Directory.CreateDirectory(subcaseDir);

                        if (subcase != null)
                            MeniscusTracker.AnnotateDrawingForTest(a.Illustration, subcase.Bottom, subcase.Top);
                        saveAllAnalysisImages(subcaseDir, a);

                        allResults.Add(
                            $"{comparison[0].ToString("F1")}," +
                            $"{comparison[1].ToString("F1")}," +
                            $"{a.CalculateExpectedMeniscusPixelPos(comparison[1])}," +
                            $"{a.Meniscii[0].BrightestIndex}," +
                            $"{subcase.Top}," +
                            $"{calculateErrStr(a.CalculateExpectedMeniscusPixelPos(comparison[1]), a.Meniscii[0].BrightestIndex)}," +
                            $"{calculateErrStr(a.CalculateExpectedMeniscusPixelPos(comparison[1]), subcase.Top)}," +
                            $"{calculateErrStr(a.Meniscii[0].BrightestIndex, subcase.Top)},"
                        );
                    }
                }
            }

            // write a single file containing all results
            using (StreamWriter outputFile = new StreamWriter($"{saveDir}\\CharacterizationReport_{DateTime.Now.ToString("yy_MM_ddHHMmmss.ff")}.csv"))
            {
                foreach (string r in allResults)
                {
                    outputFile.WriteLine(r);
                }
            }

            // dummy assert to make the "test case" always pass, this way the only failures reported will be due to real exceptions
            Assert.AreEqual(49, 49);
        }

        [TestMethod]
        public void TestFluidWasDelivered_ZeroOne()
        {
            // Happy path: fluid was delivered as expected
            // Zero to one
            MeniscusAnalysis a = new MeniscusAnalysis(
                $"{GetDataDir()}\\Eth68kDfCnstOnRotor\\0,0ml.bmp",
                $"{GetDataDir()}\\Eth68kDfCnstOnRotor\\1,0ml.bmp",
                MeniscusTracker.ProcessByHorizontalPeakAbsDiff,
                68000);

            MeniscusTracker.MeniscusFrom2Img(a);
            // Positive
            Assert.IsTrue(a.FluidWasDelivered(MeniscusAnalysis.DeliveryVolume.ZERO_TO_ONE));
            Assert.IsTrue(a.FluidWasDelivered(MeniscusAnalysis.DeliveryVolume.ONE_TO_ZERO));
            // Negative
            Assert.IsFalse(a.FluidWasDelivered(MeniscusAnalysis.DeliveryVolume.ZERO_TO_TWO));
            Assert.IsFalse(a.FluidWasDelivered(MeniscusAnalysis.DeliveryVolume.TWO_TO_ZERO));

            // One to zero
            a = new MeniscusAnalysis(
                $"{GetDataDir()}\\Eth68kDfCnstOnRotor\\1,0ml.bmp",
                $"{GetDataDir()}\\Eth68kDfCnstOnRotor\\0,0ml.bmp",
                MeniscusTracker.ProcessByHorizontalPeakAbsDiff,
                68000);

            MeniscusTracker.MeniscusFrom2Img(a);
            // Positive
            Assert.IsTrue(a.FluidWasDelivered(MeniscusAnalysis.DeliveryVolume.ZERO_TO_ONE));
            Assert.IsTrue(a.FluidWasDelivered(MeniscusAnalysis.DeliveryVolume.ONE_TO_ZERO));
            // Negative
            Assert.IsFalse(a.FluidWasDelivered(MeniscusAnalysis.DeliveryVolume.ZERO_TO_TWO));
            Assert.IsFalse(a.FluidWasDelivered(MeniscusAnalysis.DeliveryVolume.TWO_TO_ZERO));
        }

        [TestMethod]
        public void TestFluidWasDelivered_ZeroTwo()
        {
            // Zero to two
            MeniscusAnalysis a = new MeniscusAnalysis(
                $"{GetDataDir()}\\Eth68kDfCnstOnRotor\\0,0ml.bmp",
                $"{GetDataDir()}\\Eth68kDfCnstOnRotor\\2,0ml.bmp",
                MeniscusTracker.ProcessByHorizontalPeakAbsDiff,
                68000);

            MeniscusTracker.MeniscusFrom2Img(a);
            // Negative
            Assert.IsFalse(a.FluidWasDelivered(MeniscusAnalysis.DeliveryVolume.ZERO_TO_ONE));
            Assert.IsFalse(a.FluidWasDelivered(MeniscusAnalysis.DeliveryVolume.ONE_TO_ZERO));
            // Positive
            Assert.IsTrue(a.FluidWasDelivered(MeniscusAnalysis.DeliveryVolume.ZERO_TO_TWO));
            Assert.IsTrue(a.FluidWasDelivered(MeniscusAnalysis.DeliveryVolume.TWO_TO_ZERO));

            // Two to zero
            a = new MeniscusAnalysis(
                $"{GetDataDir()}\\Eth68kDfCnstOnRotor\\2,0ml.bmp",
                $"{GetDataDir()}\\Eth68kDfCnstOnRotor\\0,0ml.bmp",
                MeniscusTracker.ProcessByHorizontalPeakAbsDiff,
                68000);

            MeniscusTracker.MeniscusFrom2Img(a);
            // Negative
            Assert.IsFalse(a.FluidWasDelivered(MeniscusAnalysis.DeliveryVolume.ZERO_TO_ONE));
            Assert.IsFalse(a.FluidWasDelivered(MeniscusAnalysis.DeliveryVolume.ONE_TO_ZERO));
            // Positive
            Assert.IsTrue(a.FluidWasDelivered(MeniscusAnalysis.DeliveryVolume.ZERO_TO_TWO));
            Assert.IsTrue(a.FluidWasDelivered(MeniscusAnalysis.DeliveryVolume.TWO_TO_ZERO));
        }

        [TestMethod]
        public void TestFluidWasDelivered_NoChange()
        {
            // Edge cases: fluid was not delivered, or an insufficient amount of fluid was delivered
            // No change in fluid level occurred
            MeniscusAnalysis a = new MeniscusAnalysis(
                $"{GetDataDir()}\\EthDfNochange\\1,0ml.bmp",
                $"{GetDataDir()}\\EthDfNochange\\1,0ml 2.bmp",
                MeniscusTracker.ProcessByHorizontalPeakAbsDiff,
                68000);
            MeniscusTracker.MeniscusFrom2Img(a);

            // Negative
            Assert.IsFalse(a.FluidWasDelivered(MeniscusAnalysis.DeliveryVolume.ZERO_TO_ONE));
            Assert.IsFalse(a.FluidWasDelivered(MeniscusAnalysis.DeliveryVolume.ONE_TO_ZERO));
            Assert.IsFalse(a.FluidWasDelivered(MeniscusAnalysis.DeliveryVolume.ZERO_TO_TWO));
            Assert.IsFalse(a.FluidWasDelivered(MeniscusAnalysis.DeliveryVolume.TWO_TO_ZERO));

            a = new MeniscusAnalysis(
                $"{GetDataDir()}\\EthDfNochange\\1,0ml 2.bmp",
                $"{GetDataDir()}\\EthDfNochange\\1,0ml.bmp",
                MeniscusTracker.ProcessByHorizontalPeakAbsDiff,
                68000);
            MeniscusTracker.MeniscusFrom2Img(a);

            // Negative
            Assert.IsFalse(a.FluidWasDelivered(MeniscusAnalysis.DeliveryVolume.ZERO_TO_ONE));
            Assert.IsFalse(a.FluidWasDelivered(MeniscusAnalysis.DeliveryVolume.ONE_TO_ZERO));
            Assert.IsFalse(a.FluidWasDelivered(MeniscusAnalysis.DeliveryVolume.ZERO_TO_TWO));
            Assert.IsFalse(a.FluidWasDelivered(MeniscusAnalysis.DeliveryVolume.TWO_TO_ZERO));

            a = new MeniscusAnalysis(
                $"{GetDataDir()}\\EthDfNochange\\2,0ml 2.bmp",
                $"{GetDataDir()}\\EthDfNochange\\2,0ml.bmp",
                MeniscusTracker.ProcessByHorizontalPeakAbsDiff,
                68000);
            MeniscusTracker.MeniscusFrom2Img(a);
            // Negative
            Assert.IsFalse(a.FluidWasDelivered(MeniscusAnalysis.DeliveryVolume.ZERO_TO_ONE));
            Assert.IsFalse(a.FluidWasDelivered(MeniscusAnalysis.DeliveryVolume.ONE_TO_ZERO));
            Assert.IsFalse(a.FluidWasDelivered(MeniscusAnalysis.DeliveryVolume.ZERO_TO_TWO));
            Assert.IsFalse(a.FluidWasDelivered(MeniscusAnalysis.DeliveryVolume.TWO_TO_ZERO));

            a = new MeniscusAnalysis(
                $"{GetDataDir()}\\EthDfNochange\\2,0ml.bmp",
                $"{GetDataDir()}\\EthDfNochange\\2,0ml 2.bmp",
                MeniscusTracker.ProcessByHorizontalPeakAbsDiff,
                68000);
            MeniscusTracker.MeniscusFrom2Img(a);
            // Negative
            Assert.IsFalse(a.FluidWasDelivered(MeniscusAnalysis.DeliveryVolume.ZERO_TO_ONE));
            Assert.IsFalse(a.FluidWasDelivered(MeniscusAnalysis.DeliveryVolume.ONE_TO_ZERO));
            Assert.IsFalse(a.FluidWasDelivered(MeniscusAnalysis.DeliveryVolume.ZERO_TO_TWO));
            Assert.IsFalse(a.FluidWasDelivered(MeniscusAnalysis.DeliveryVolume.TWO_TO_ZERO));

        }

        [TestMethod]
        public void TestFluidWasDelivered_InsufficientChange()
        {
            // An insufficient change in fluid level occurred
            // One Half <-> One
            MeniscusAnalysis a = new MeniscusAnalysis(
                $"{GetDataDir()}\\EthDfInsufficientChange\\0,5ml.bmp",
                $"{GetDataDir()}\\EthDfInsufficientChange\\1,0ml.bmp",
                MeniscusTracker.ProcessByHorizontalPeakAbsDiff,
                68000);
            MeniscusTracker.MeniscusFrom2Img(a);
            // Negative
            Assert.IsFalse(a.FluidWasDelivered(MeniscusAnalysis.DeliveryVolume.ZERO_TO_ONE));
            Assert.IsFalse(a.FluidWasDelivered(MeniscusAnalysis.DeliveryVolume.ONE_TO_ZERO));
            Assert.IsFalse(a.FluidWasDelivered(MeniscusAnalysis.DeliveryVolume.ZERO_TO_TWO));
            Assert.IsFalse(a.FluidWasDelivered(MeniscusAnalysis.DeliveryVolume.TWO_TO_ZERO));

            a = new MeniscusAnalysis(
                $"{GetDataDir()}\\EthDfInsufficientChange\\1,0ml.bmp",
                $"{GetDataDir()}\\EthDfInsufficientChange\\0,5ml.bmp",
                MeniscusTracker.ProcessByHorizontalPeakAbsDiff,
                68000);
            MeniscusTracker.MeniscusFrom2Img(a);
            // Negative
            Assert.IsFalse(a.FluidWasDelivered(MeniscusAnalysis.DeliveryVolume.ZERO_TO_ONE));
            Assert.IsFalse(a.FluidWasDelivered(MeniscusAnalysis.DeliveryVolume.ONE_TO_ZERO));
            Assert.IsFalse(a.FluidWasDelivered(MeniscusAnalysis.DeliveryVolume.ZERO_TO_TWO));
            Assert.IsFalse(a.FluidWasDelivered(MeniscusAnalysis.DeliveryVolume.TWO_TO_ZERO));

            // One <-> Three Halves
            a = new MeniscusAnalysis(
                $"{GetDataDir()}\\EthDfInsufficientChange\\1,0ml.bmp",
                $"{GetDataDir()}\\EthDfInsufficientChange\\1,5ml.bmp",
                MeniscusTracker.ProcessByHorizontalPeakAbsDiff,
                68000);
            MeniscusTracker.MeniscusFrom2Img(a);
            // Negative
            Assert.IsFalse(a.FluidWasDelivered(MeniscusAnalysis.DeliveryVolume.ZERO_TO_ONE));
            Assert.IsFalse(a.FluidWasDelivered(MeniscusAnalysis.DeliveryVolume.ONE_TO_ZERO));
            Assert.IsFalse(a.FluidWasDelivered(MeniscusAnalysis.DeliveryVolume.ZERO_TO_TWO));
            Assert.IsFalse(a.FluidWasDelivered(MeniscusAnalysis.DeliveryVolume.TWO_TO_ZERO));

            a = new MeniscusAnalysis(
                $"{GetDataDir()}\\EthDfInsufficientChange\\1,5ml.bmp",
                $"{GetDataDir()}\\EthDfInsufficientChange\\1,0ml.bmp",
                MeniscusTracker.ProcessByHorizontalPeakAbsDiff,
                68000);
            MeniscusTracker.MeniscusFrom2Img(a);
            // Negative
            Assert.IsFalse(a.FluidWasDelivered(MeniscusAnalysis.DeliveryVolume.ZERO_TO_ONE));
            Assert.IsFalse(a.FluidWasDelivered(MeniscusAnalysis.DeliveryVolume.ONE_TO_ZERO));
            Assert.IsFalse(a.FluidWasDelivered(MeniscusAnalysis.DeliveryVolume.ZERO_TO_TWO));
            Assert.IsFalse(a.FluidWasDelivered(MeniscusAnalysis.DeliveryVolume.TWO_TO_ZERO));

            // Three Halves <-> Two
            a = new MeniscusAnalysis(
                $"{GetDataDir()}\\EthDfInsufficientChange\\2,0ml.bmp",
                $"{GetDataDir()}\\EthDfInsufficientChange\\1,5ml.bmp",
                MeniscusTracker.ProcessByHorizontalPeakAbsDiff,
                68000);
            MeniscusTracker.MeniscusFrom2Img(a);
            // Negative
            Assert.IsFalse(a.FluidWasDelivered(MeniscusAnalysis.DeliveryVolume.ZERO_TO_ONE));
            Assert.IsFalse(a.FluidWasDelivered(MeniscusAnalysis.DeliveryVolume.ONE_TO_ZERO));
            Assert.IsFalse(a.FluidWasDelivered(MeniscusAnalysis.DeliveryVolume.ZERO_TO_TWO));
            Assert.IsFalse(a.FluidWasDelivered(MeniscusAnalysis.DeliveryVolume.TWO_TO_ZERO));

            a = new MeniscusAnalysis(
                $"{GetDataDir()}\\EthDfInsufficientChange\\1,5ml.bmp",
                $"{GetDataDir()}\\EthDfInsufficientChange\\2,0ml.bmp",
                MeniscusTracker.ProcessByHorizontalPeakAbsDiff,
                68000);
            MeniscusTracker.MeniscusFrom2Img(a);
            // Negative
            Assert.IsFalse(a.FluidWasDelivered(MeniscusAnalysis.DeliveryVolume.ZERO_TO_ONE));
            Assert.IsFalse(a.FluidWasDelivered(MeniscusAnalysis.DeliveryVolume.ONE_TO_ZERO));
            Assert.IsFalse(a.FluidWasDelivered(MeniscusAnalysis.DeliveryVolume.ZERO_TO_TWO));
            Assert.IsFalse(a.FluidWasDelivered(MeniscusAnalysis.DeliveryVolume.TWO_TO_ZERO));
        }
    }
}