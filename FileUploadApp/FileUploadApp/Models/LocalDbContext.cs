using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileUploadApp.Models
{
    public class LocalDbContext :DbContext
    {
        public LocalDbContext(DbContextOptions<LocalDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<FileReference> FileReferences { get; set; }

    }
}
