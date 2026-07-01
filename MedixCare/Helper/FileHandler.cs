namespace MedixCare.Helper
{
 
    public class FileHandler : IFileHandler
    {
        private const long MaxFileSize = 20 * 1024 * 1024; 
        private readonly string[] _allowedExtensions = [ ".jpg", ".jpeg", ".png", ".pdf"];
        private readonly ILogger _logger;

        public FileHandler(ILogger logger)
        {
            _logger = logger;
        }
        public async Task<string> CreateFileAsync(IFormFile file , AttachmentType attachmentType)
        {

            if(file == null || file.Length == 0)
            {
                _logger.LogError("File is empty or null");
                throw new ArgumentException("File is empty or null");
            }

            if (file.Length > MaxFileSize)
            {
                _logger.LogError("File size exceeds the allowed limit (20MB).");
                throw new ArgumentException("File size exceeds the allowed limit (20MB).");
            }

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!_allowedExtensions.Contains(extension))
            {
                _logger.LogError("Unsupported file type attempted to be uploaded.");
                throw new ArgumentException("Unsupported file type attempted to be uploaded.");
            }

            //Code Logic to save the file

            var subFolder = attachmentType.ToString();
            //fileName 
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            //filePath
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "MedicalAttachments", subFolder);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            
            var filePath = Path.Combine(folderPath, fileName);
            using (var stream = File.Create(filePath))
            {
                await file.CopyToAsync(stream);
            }
            return $"{subFolder}/{fileName}";

        }

        public bool DeleteOldFile(string oldFileName)
        {

            var filePath = GetOldFilePath(oldFileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return true;
            }
            return false;
        }

        public string GetOldFilePath(string oldFileName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "MedicalAttachments", oldFileName);
            return filePath;
        }
    }
}
