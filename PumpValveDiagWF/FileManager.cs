using Emgu.CV;
using System;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;

namespace FileManagement
{
    public static class FileManager
    {
        // String constants for image files
        // Expected name of image taken after fluid is added
        public static string AfterImgName = "After";
        // Expected name of image taken before fluid is added
        public static string BeforeImgName = "Before";
        // The black and white region which OpenCV identified as the filled fluid region
        public static string ThresholdImgName = "MeniscusRegion";
        // Absolute path of before and after images
        public static string ImageSourceDir = "C:\\ProgramData\\LabScript\\Videos\\FluidDetection";
        public static string ImageArchiveDir = $"{ImageSourceDir}\\Archive";

        public static string GetSourceImgPath(string imgName)
        {
            return $"{ImageSourceDir}\\{imgName}.bmp";
        }
        public static string GetArchiveImgPath(string imgName, string timestamp)
        {
            return $"{ImageArchiveDir}\\{imgName}_{timestamp}.bmp";
        }

        public static void ArchiveImgFiles(string beforeImgPath, string afterImgPath, string timestamp)
        {
            System.IO.File.Move(beforeImgPath, GetArchiveImgPath(BeforeImgName, timestamp));
            System.IO.File.Move(afterImgPath, GetArchiveImgPath(AfterImgName, timestamp));
        }

        public static void SaveImage(byte[] data, string filename)
        {
            using (Image image = Image.FromStream(new MemoryStream(data)))
            {
                image.Save(filename, ImageFormat.Jpeg);
            }
        }
    }
}
