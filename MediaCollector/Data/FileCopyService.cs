namespace MediaCollector.Data
{
    public class FileCopyService
    {
        public async Task CopyFilesAsync(string targetFolder, string fileName, MemoryStream fileData, Action<int> progressCallback)
        {
            int copiedFiles = 0;

            var targetPath = Path.Combine(targetFolder, Path.GetFileName(fileName));
            try
            {
                
                var foo = FileSystem.AppDataDirectory;
                using (var targetStream = new FileStream(targetPath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true))
                {
                    await fileData.CopyToAsync(targetStream);
                }
                copiedFiles++;
                progressCallback?.Invoke((int)((double)copiedFiles));
            }
            catch(Exception ex)
            {
                
            }
        }
    }
}
