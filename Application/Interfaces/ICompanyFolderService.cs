namespace Application.Interfaces
{
    public interface ICompanyFolderService
    {
        string EnsureCompanyRoot(string companyCode);
        string EnsureSubFolder(string companyCode, string folderName);
        void EnsureDefaultCompanyFolders(string companyCode);
        string GetFileUrl(string companyCode, string folderName, string fileName);
        Task<string?> UploadCompanyFileAsync(string? base64File, string companyCode, string folderName, string[] allowedExtensions, double maxFileSizeInMb);
    }
}
