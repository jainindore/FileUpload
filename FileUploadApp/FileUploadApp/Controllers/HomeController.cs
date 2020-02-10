using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FileUploadApp.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using FileUploadApp.GenericRepository;
using FileUploadApp.Util;
using FileUploadApp.ViewModels;
using Microsoft.AspNetCore.Http;

namespace FileUploadApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepository<User> _userRepository = null;
        private readonly IRepository<FileReference> _fileRepository = null;
        private readonly IWebHostEnvironment webHostingEnvironment;

        public HomeController(IRepository<User> userRepository, IRepository<FileReference> fileRepository, ILogger<HomeController> logger, 
            IWebHostEnvironment webHostingEnvironment, LocalDbContext context)
        {
            _userRepository = userRepository;
            _fileRepository = fileRepository;
            _logger = logger;
            this.webHostingEnvironment = webHostingEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("Files")]
        [HttpGet]
        public ViewResult Files()
        {
            var fileReferenceList = _fileRepository.GetAll("User");
            return View(fileReferenceList);
        }

        [HttpGet]
        public async Task<IActionResult> Download(string id)
        {
            if (id == null)
                return Content("Filename not Present!!");
            try
            {
                var rootPath = webHostingEnvironment.WebRootPath;
                //Webrootpath is different for Desktop App (Electron) and Web application.
                string projectPath = rootPath.Split("\\FileUploadApp\\")[0];
                var path = Path.Combine(projectPath, "FileUploadApp\\files", id);
                var memory = new MemoryStream();
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
                return File(memory, Helper.GetContentType(path), Path.GetFileName(path));
            }
            catch(FileNotFoundException fileNotFoundEx){
                _logger.LogError($"FileNotFoundException occured at Downloading File : " + fileNotFoundEx.ToString());
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, ExceptionMessage = fileNotFoundEx.Message });
            }
            catch(IOException ioException)
            {
                _logger.LogError($"IOException occured at Downloading File : " + ioException.ToString());
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, ExceptionMessage = ioException.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occured at Downloading File : " + ex.ToString());
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, ExceptionMessage = ex.Message });
            }
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(FileUploadModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (model.File != null)
                    {
                        string uniqueFileName = null;
                        if (!_userRepository.IfExist(u => u.Name == model.Name && u.Email == model.Email))
                        {
                            User newUser = new User
                            {
                                Name = model.Name,
                                Email = model.Email,
                            };
                            User addedUser = _userRepository.Add(newUser);
                            _userRepository.Save();
                            uniqueFileName = Helper.GetFileName(model.File, addedUser.Id);

                            FileReference newFileRef = new FileReference
                            {
                                //Storing the UserId of The Added User in the FileReference Table as Foreign Key
                                UserID = addedUser.Id,
                                // Store the file name in FilePath property of the FileReference object
                                // which gets saved to the FileReference database table
                                FilePath = uniqueFileName
                            };
                            _fileRepository.Add(newFileRef);
                            _fileRepository.Save();
                            Helper.StoreFile(webHostingEnvironment, model.File, uniqueFileName);
                        }
                        else
                        {
                            var returnedUser = _userRepository.GetRecord(u => u.Email == model.Email && u.Name == model.Name).FirstOrDefault();
                            if (returnedUser != null)
                            {
                                uniqueFileName = Helper.GetFileName(model.File, returnedUser.Id);
                                if (!_fileRepository.IfExist(f => f.FilePath == uniqueFileName))
                                {
                                    FileReference newFileRef = new FileReference
                                    {
                                        //Storing the UserId of this User (who already exist in the User Table)
                                        // in the FileReference Table as Foreign Key
                                        UserID = returnedUser.Id,
                                        // Store the file name in FilePath property of the FileReference object
                                        // which gets saved to the FileReference database table
                                        FilePath = uniqueFileName
                                    };
                                    _fileRepository.Add(newFileRef);
                                    _fileRepository.Save();
                                    Helper.StoreFile(webHostingEnvironment, model.File, uniqueFileName);
                                }
                            }
                        }
                        return RedirectToAction("Files"); // ("details", new { id = newEmployee.Id });
                    }
                } 
                catch (Exception ex) 
                {
                    _logger.LogError($"Exception occured at Create Post Method of Home controller : " + ex.ToString());
                    return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, ExceptionMessage = ex.Message });
                }

            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
