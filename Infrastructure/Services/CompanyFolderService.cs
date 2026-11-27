using Application.Interfaces;
using Domain.CommonModels;
using Domain.Exceptions;
using System.Text.RegularExpressions;

namespace Infrastructure.Services
{
    public class CompanyFolderService : ICompanyFolderService
    {
        private readonly string _rootPath;
        private readonly IFileStorageService _fileStorageService;
        private const string RootFolder = "UploadedFiles";
        public CompanyFolderService(IFileStorageService fileStorageService)
        {
            _rootPath = Directory.GetCurrentDirectory();
            _fileStorageService = fileStorageService;
        }
        public string EnsureCompanyRoot(string companyCode)
        {
            if (string.IsNullOrWhiteSpace(companyCode))
                throw new AppException("Company code is required.");

            var safeCode = Sanitize(companyCode);

            var path = Path.Combine(_rootPath, RootFolder, safeCode);

            CreateIfNotExists(path);

            return path;
        }
        public string EnsureSubFolder(string companyCode, string folderName)
        {
            if (string.IsNullOrWhiteSpace(folderName))
                throw new AppException("Folder name is required.");

            var companyRoot = EnsureCompanyRoot(companyCode);
            var safeFolderName = Sanitize(folderName);

            var path = Path.Combine(companyRoot, safeFolderName);

            CreateIfNotExists(path);

            return path;
        }
        public void EnsureDefaultCompanyFolders(string companyCode)
        {
            EnsureCompanyRoot(companyCode);

            foreach (var folderName in CompanyFolderNames.All)
            {
                EnsureSubFolder(companyCode, folderName);
            }
        }
        public string GetFileUrl(string companyCode, string folderName, string fileName)
        {
            if (string.IsNullOrWhiteSpace(companyCode))
                throw new AppException("Company code is required.");

            if (string.IsNullOrWhiteSpace(folderName))
                throw new AppException("Folder name is required.");

            if (string.IsNullOrWhiteSpace(fileName))
                throw new AppException("File name is required.");

            var safeCode = Sanitize(companyCode);
            var safeFolderName = Sanitize(folderName);

            return $"/{RootFolder}/{safeCode}/{safeFolderName}/{fileName}";
        }
        private static void CreateIfNotExists(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
        private static string Sanitize(string input)
        {
            var cleaned = Regex.Replace(input.Trim().ToUpperInvariant(), @"[^A-Z0-9]", "");

            return string.IsNullOrWhiteSpace(cleaned) ? "DEFAULT" : cleaned;
        }
        public async Task<string?> UploadCompanyFileAsync(string? base64File, string companyCode, string folderName, string[] allowedExtensions, double maxFileSizeInMb)
        {
            if (string.IsNullOrWhiteSpace(base64File)) return null;

            var folderPath = EnsureSubFolder(companyCode, folderName);

            var fileName = await _fileStorageService.UploadBase64FileAsync(base64File, folderPath, allowedExtensions, maxFileSizeInMb);

            return string.IsNullOrWhiteSpace(fileName) ? null : GetFileUrl(companyCode, folderName, fileName);
        }
    }
}
