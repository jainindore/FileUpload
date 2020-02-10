using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FileUploadApp.ViewModels
{
    public class FileUploadModel
    {
        [Required]
        [StringLength(40, MinimumLength = 2)]
        [RegularExpression(@"^[A-Za-z ]+$", ErrorMessage = "Name is not Valid! It can only contain letters")]

        public string Name { get; set; }
        [Required]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Email Format is not Valid!")]
        public string Email { get; set; }
        [Required]
        public IFormFile File { get; set; }

    }
}
