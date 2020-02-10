using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FileUploadApp.Models
{
    public class FileReference
    {
        [Key]
        public int FileID { get; set; }
        [Required]
        public int UserID { get; set; }
        [Required]
        public string FilePath { get; set; }

        public User User { get; set; }

    }
}
