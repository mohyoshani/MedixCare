namespace MedixCare.Helper
{
    public enum AttachmentType
    {
        scans,
        labs,
    }

    public interface IFileHandler
    {
        /// <summary>
        /// Creates a file asynchronously in the folder that matches the given attachment type and returns
        /// the relative path of the created file (e.g. "Scans/3f2a1b0c.pdf").
        /// </summary>
        /// <param name="file">The uploaded file.</param>
        /// <param name="attachmentType">The category of attachment; determines the storage subfolder.</param>
        /// <returns>The relative path of the stored file.</returns>
        Task<string> CreateFileAsync(IFormFile file, AttachmentType attachmentType);

        /// <summary>
        /// Gets the full file system path for a file, given the relative path previously returned by
        /// <see cref="CreateFileAsync"/>.
        /// </summary>
        /// <param name="oldFileName">The relative path of the existing file.</param>
        /// <returns>The full file system path.</returns>
        string GetOldFilePath(string oldFileName);

        /// <summary>
        /// Deletes an existing file given its relative path.
        /// </summary>
        /// <param name="oldFileName">The relative path of the existing file.</param>
        /// <returns>True if the file was found and deleted; false if it did not exist.</returns>
        bool DeleteOldFile(string oldFileName);
    }
}