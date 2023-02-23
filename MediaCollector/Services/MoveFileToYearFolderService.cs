using System;
namespace MediaCollector.Services
{
    public class MoveFileToYearFolderService
    {
        private readonly MetadataService _metadataService;

        public MoveFileToYearFolderService(MetadataService metadataService)
        {
            _metadataService = metadataService;
        }

        public async Task MoveFile(string targetFolder, string filePath)
        {
            int? year = _metadataService.GetYear(filePath);
            if (year.HasValue)
            {
                string yearFolder = Path.Combine(targetFolder, year.ToString());
                Directory.CreateDirectory(yearFolder);

                string fileName = Path.GetFileName(filePath);
                string targetFilePath = Path.Combine(yearFolder, fileName);

                await Task.Run(() => File.Copy(filePath, targetFilePath));
            }
            
        }
    }
}

