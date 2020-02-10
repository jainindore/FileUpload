using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FileUploadApp.Util
{
    public static class Helper
    {

       /// <summary>
       /// This Method stores the file at the File Storage.
       /// </summary>
       /// <param name="webHostingEnvironment"></param>
       /// <param name="file"></param>
       /// <param name="uniqueFileName"></param>
        public static void StoreFile(IWebHostEnvironment webHostingEnvironment, IFormFile file ,string uniqueFileName) 
        {
            try
            {
                // Files are uploaded to the files folder in Project Root Folder where (ex: "//FileUploadApp/files")
                var rootPath = webHostingEnvironment.WebRootPath;
                //Webrootpath is different for Desktop App (Electron) and Web application.
                string projectPath = rootPath.Split("\\FileUploadApp\\")[0];
                string uploadsFolder = Path.Combine(projectPath, "FileUploadApp\\files");
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                // Use CopyTo() method provided by IFormFile interface to copy the file to wwwroot/files folder
               // var memory = new MemoryStream();
                using (var newFileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(newFileStream);
                }
               // memory.Position = 0;
                //var newFileStream = new FileStream(filePath, FileMode.Create);
                //file.CopyTo(newFileStream);
                //newFileStream.Flush();
            }
            catch(IOException ioException)
            {
                //logger.LogError($"")
                ioException.ToString();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        /// <summary>
        /// To make sure the file name is unique we are appending a user_id and and an underscore to the file name
        /// </summary>
        /// <param name="file"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetFileName(IFormFile file, int id) 
        {
            return id + "_" + file.FileName;
        }

        /// <summary>
        /// To Get the Extension of the files to be downloaded.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetContentType(string path)
        {
            try
            {
                var types = GetMimeTypes();
                var ext = Path.GetExtension(path).ToLowerInvariant();
                return types[ext];
            }
            catch(Exception ex)
            {
                // _logger.LogError($"Exception occured at Downloading File : " + ex.ToString());
                throw new Exception("Exception occured at getting content type of the File : " +ex.ToString());
            }
        }


        /// <summary>
        /// To get MimeTypes of different file types.
        /// </summary>
        /// <returns></returns>
        private static Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }
    }
}
