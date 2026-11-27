using Application.Interfaces;
using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Services
{
    public class FileStorageService : IFileStorageService
    {
        public async Task<string> UploadBase64FileAsync(string base64File, string folderPath, string[] allowedExtensions, double maxFileSizeInMb)
        {
            if (string.IsNullOrWhiteSpace(base64File)) throw new AppException("File is required.");

            var base64Data = ExtractBase64Data(base64File);

            if (!IsValidBase64String(base64Data)) throw new AppException("Invalid Base64 file.");

            if (!IsValidFileSize(base64Data, maxFileSizeInMb)) throw new AppException($"File size should not exceed {maxFileSizeInMb} MB.");

            if (!IsValidFileExtension(base64File, allowedExtensions)) throw new AppException("Invalid file type.");

            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

            var fileBytes = Convert.FromBase64String(base64Data);
            var fileExtension = GetFileExtensionFromBase64(base64File);


            if (string.IsNullOrWhiteSpace(fileExtension)) throw new AppException("Could not determine file extension.");

            var fileName = $"{DateTime.UtcNow:yyyyMMddHHmmssfff}_{Guid.NewGuid():N}.{fileExtension}";
            var filePath = Path.Combine(folderPath, fileName);

            await File.WriteAllBytesAsync(filePath, fileBytes);

            return fileName;
        }
        public bool IsValidBase64String(string base64String)
        {
            if (string.IsNullOrWhiteSpace(base64String)) return false;

            var base64Data = ExtractBase64Data(base64String);

            Span<byte> buffer = new byte[base64Data.Length];
            return Convert.TryFromBase64String(base64Data, buffer, out _);
        }

        public bool IsValidFileSize(string base64String, double maxFileSizeInMb)
        {
            var base64Data = ExtractBase64Data(base64String);
            var fileBytes = Convert.FromBase64String(base64Data);

            var fileSizeInMb = fileBytes.Length / (1024.0 * 1024.0);
            return fileSizeInMb <= maxFileSizeInMb;
        }
        public bool IsValidFileExtension(string base64String, string[] allowedExtensions)
        {
            var fileExtension = GetFileExtensionFromBase64(base64String);

            return !string.IsNullOrWhiteSpace(fileExtension) && allowedExtensions.Contains(fileExtension, StringComparer.OrdinalIgnoreCase);
        }

        public string GetFileExtensionFromBase64(string base64String)
        {
            var base64Data = ExtractBase64Data(base64String);

            if (base64Data.Length < 5) return string.Empty;

            var header = base64Data[..5].ToUpper();

            return header switch
            {
                "IVBOR" => "png",
                "/9J/4" => "jpg",
                "/9J/2" => "jpg",
                "/9J/3" => "jpg",
                "JVBER" => "pdf",
                "UESDB" => "docx",
                "AAAA" => "mp4",
                _ => string.Empty
            };
        }

        private static string ExtractBase64Data(string base64String)
        {
            if (string.IsNullOrWhiteSpace(base64String)) return string.Empty;

            return base64String.Contains(',') ? base64String.Split(',')[1] : base64String;
        }
    }
}
