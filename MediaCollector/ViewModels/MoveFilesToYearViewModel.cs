using System;
using System.ComponentModel.DataAnnotations;
using MediaCollector.Data;
using MediaCollector.Services;

namespace MediaCollector.ViewModels
{
    public class MoveFilesToYearViewModel : AbstractViewModel<MoveSettings>
    {
        private readonly MoveFilesService _moveFilesService;
        private readonly IFolderPicker _folderPicker;
        private string _processedFile;

        public MoveFilesToYearViewModel(MoveSettings moveSettings, MoveFilesService moveFilesService, IFolderPicker folderPicker) : base(moveSettings)
        {
            _moveFilesService = moveFilesService;
            _folderPicker = folderPicker;
        }

        [Required(AllowEmptyStrings = false)]
        public string SourceFolder
        {
            get => Model.SourceFolder;
            set => SetValue(() => Model.SourceFolder = value);
        }

        [Required(AllowEmptyStrings = false)]
        public string TargetFolder
        {
            get => Model.TargetFolder;
            set => SetValue(() => Model.TargetFolder = value);
        }

        public string ProcessedFile
        {
            get => _processedFile;
            set
            {
                SetValue(ref _processedFile, value);
            }
        }

        public async Task PickSouceFolder()
        {
            SourceFolder = await PickFolder();
        }

        public async Task PickTargetFolder()
        {
            TargetFolder = await PickFolder();
        }

        public async Task MoveFiles()
        {
            IsBusy = true;
            _moveFilesService.FileProcessed += (o, e) =>
            {
                ProcessedFile = e;
            };

            await _moveFilesService.MoveFiles();
            IsBusy = false;
        }

        private async Task<string> PickFolder()
        {
            return await _folderPicker.PickFolder();
        }
    }
}

