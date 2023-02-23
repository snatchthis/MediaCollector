using System;
using MediaCollector.Data;

namespace MediaCollector.Services
{
    public class MoveFilesService
    {
        private readonly MoveFileToYearFolderService _moveToYearService;
        private readonly MoveSettings _moveSettings;

        public event EventHandler<string> FileProcessed;

        public MoveFilesService(MoveSettings moveSettings, MoveFileToYearFolderService moveToYearService)
        {
            _moveSettings = moveSettings;
            _moveToYearService = moveToYearService;
        }

        public async Task MoveFiles()
        {
            await Task.Run(async () =>
            {
                if (!string.IsNullOrEmpty(_moveSettings.SourceFolder)
                    && Directory.Exists(_moveSettings.SourceFolder)
                    && !string.IsNullOrEmpty(_moveSettings.TargetFolder)
                    && Directory.Exists(_moveSettings.TargetFolder))
                {
                    foreach (var sourceFilePath in Directory.EnumerateFiles(_moveSettings.SourceFolder, "*.*", SearchOption.AllDirectories))
                    {
                        await _moveToYearService.MoveFile(_moveSettings.TargetFolder, sourceFilePath);
                        OnFileProcessed(sourceFilePath);
                    }
                }
            });
        }

        protected virtual void OnFileProcessed(string e)
        {
            FileProcessed?.Invoke(this, e);
        }
    }
}

