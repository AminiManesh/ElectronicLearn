using ElectronicLearn.Core.Generators;
using ElectronicLearn.DataLayer.Entities.Course;
using ElectronicLearn.DataLayer.Entities.User;
using Microsoft.AspNetCore.Http;
using SharpCompress.Archives;
using SharpCompress.Archives.Rar;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicLearn.Core.Tools
{
    public class FileTools
    {
        public static string SaveFile(IFormFile file, string saveFolderPath, bool deletePreviousFile = false, string prevFileName = "")
        {
            if (deletePreviousFile)
            {
                DeletePreviousFile(saveFolderPath, prevFileName);
            }

            string fileName = TextGenerator.GenerateUniqeCode() + Path.GetExtension(file.FileName);
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), saveFolderPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return fileName;
        }

        // Save the file with out changing its name to a unique code (save the file with its own name)
        public static void SaveFileWithItsName(IFormFile file, string saveFolderPath, bool deletePreviousFile = false, string prevFileName = "")
        {
            if (deletePreviousFile)
            {
                DeletePreviousFile(saveFolderPath, prevFileName);
            }

            string fileName = file.FileName;
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), saveFolderPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
        }

        public static void SaveFileWithCustomName(IFormFile file, string fileName, string saveFolderPath, bool deletePreviousFile = false, string prevFileName = "")
        {
            if (deletePreviousFile)
            {
                DeletePreviousFile(saveFolderPath, prevFileName);
            }

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), saveFolderPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
        }

        // Check the file is exists or not
        public static bool IsFileExists(IFormFile file, string saveFolderPath)
        {
            string fileName = file.FileName;
            string checkPath = Path.Combine(Directory.GetCurrentDirectory(), saveFolderPath, fileName);
            return File.Exists(checkPath);
        }

        // Check the file path is exists or not
        public static bool IsFileExists(string filePath)
        {
            return File.Exists(filePath);
        }

        public static void DeletePreviousFile(string saveFolderPath, string prevFileName)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), saveFolderPath, prevFileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        public static async Task SaveThumbnailAsync(string imagePath, string saveFolderPath)
        {
            using (var image = Image.FromFile(imagePath))
            {
                decimal imageWidth = image.Width;
                decimal imageHeight = image.Height;

                decimal newWidth = 300;
                decimal newHeight = (imageHeight * newWidth) / imageWidth;

                using (var bitmap = new Bitmap(image, new Size((int)newWidth, (int)newHeight)))
                {
                    //float wRes = bitmap.HorizontalResolution;
                    //float hRes = bitmap.VerticalResolution;

                    //float newWRes = wRes / 2f;
                    //float newHRes = hRes / 2f;

                    //bitmap.SetResolution(newWRes, newHRes);
                    await Task.Run(() =>
                    {
                        bitmap.Save(saveFolderPath);
                    });

                    bitmap.Dispose();
                }

                image.Dispose();
            }
        }

        public static void ExtractFile(string rarPath, string fileName, string extractPath)
        {
            if (File.Exists(rarPath))
            {
                using (Stream stream = File.OpenRead(rarPath))
                {
                    var reader = RarArchive.Open(stream);
                    foreach (var entry in reader.Entries)
                    {
                        if (!entry.IsDirectory && entry.Key.Equals(fileName))
                        {
                            if (!Directory.Exists(extractPath))
                            {
                                Directory.CreateDirectory(extractPath);
                            }
                            entry.WriteToDirectory(extractPath, new ExtractionOptions() { ExtractFullPath = true, Overwrite = true });
                            break;
                        }
                    }
                }
            }
        }
    }
}
