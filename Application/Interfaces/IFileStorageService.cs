using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface IFileStorageService
    {
        Task<string> UploadBase64FileAsync(string base64File, string folderPath, string[] allowedExtensions, double maxFileSizeInMb);
        bool IsValidBase64String(string base64String);
        bool IsValidFileSize(string base64String, double maxFileSizeInMb);
        bool IsValidFileExtension(string base64String, string[] allowedExtensions);
        string GetFileExtensionFromBase64(string base64String);
    }
}
