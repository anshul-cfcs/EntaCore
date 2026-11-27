using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Options
{
    public class FileUploadOptions
    {
        public List<string> AllowedExtensions { get; set; } = new();
        public int MaxSizeMb { get; set; }
    }
}
