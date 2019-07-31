using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace MilestoneOne.source
{
    internal class AppHelper
    {
        private static readonly string PathImageExecution = AppDomain.CurrentDomain.BaseDirectory + "Image\\";
        public static string FolderImage = "Image/";

        public static BitmapImage LoadBitmap(string path)
        {
            try
            {
                return LoadBitmapFromExecution(path);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        private static BitmapImage LoadBitmapFromExecution(string path)
        {
            var bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.CacheOption = BitmapCacheOption.OnLoad;
            var pathExecution = PathImageExecution + path;
            bmp.UriSource = new Uri(pathExecution);
            bmp.EndInit();
            return bmp;
        }

        public static void DeleteFile(string filename)
        {
            File.Delete(FolderImage + filename);
        }

        public static void CopyFile(string srcFile, string desFile)
        {
            File.Copy(srcFile, FolderImage + desFile);
        }

        public static bool CheckExistFile(string srcFile)
        {
            return File.Exists(FolderImage + srcFile);
        }

        public static void CreateImageFolderIfNotExist()
        {
            try
            {
                if (!Directory.Exists(FolderImage))
                {
                    // Try to create the directory.
                    var di = Directory.CreateDirectory(FolderImage);
                }
            }
            catch (IOException ioex)
            {
                Console.WriteLine(ioex.Message);
            }
        }
    }
}