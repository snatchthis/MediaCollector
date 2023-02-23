using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Runtime.InteropServices.ObjectiveC;
using System.Windows.Input;
using MediaCollector.Data;
using MediaCollector.Services;


namespace MediaCollector.ViewModels
{
    public class InputFormViewModel : AbstractViewModel<OperationSettings>
	{
		private string[] _mediaFileExtensions;
		private IFilePickerService _filePicker;
        private IFolderPicker _folderPicker;
		private FileNameParser _parser;
        private readonly ExtractService _extractService;
        private readonly FileNameParser _fileNameParser;
        private FileCopyService _fileCopyService;
		private FileResult _archive;
		private string _currentFile;

        public InputFormViewModel(OperationSettings settings, IFilePickerService filePicker, IFolderPicker folderPicker, FileNameParser parser, ExtractService extractService, FileNameParser fileNameParser) : base(settings)
        {
			_filePicker = filePicker;
			_folderPicker = folderPicker;
			_parser = parser;
            _extractService = extractService;
            _fileNameParser = fileNameParser;

            _extractService.FileExtracted += OnFileExtracted;
        }

        [Required(AllowEmptyStrings = false)]
        public string SourceArchive
        {
            get => Model.SourceArchive;
            set => SetValue(() => Model.SourceArchive = value);
        }

        [Required(AllowEmptyStrings = false)]
        public string TargetDirectory
		{
			get => Model.TargetDirectory;
            set => SetValue(() => Model.TargetDirectory = value);
        }

		public string CurrentFile
		{
			get => _currentFile;
			set
			{
				SetValue(ref _currentFile, value);
			}
		}

        [Required, MinLength(2)]
		public string[] MediaFileExtensions
		{
			get => Model.MediaFilesExtensions;
            set => SetValue(() => Model.MediaFilesExtensions = value);
        }

        public async Task SelectArchive()
        {
			_archive = await _filePicker.PickFile();
			if (_archive != null)
                SourceArchive = _archive.FileName;
        }

        public async Task PickFolder()
        {
            TargetDirectory = await _folderPicker.PickFolder();

        }

		public async Task CollectFiles()
		{
            IsBusy = true;

            try
            {
                await ExtractArchive();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                IsBusy = false;
            }

        }

        private async Task ExtractArchive()
        {
            CancellationToken token = new CancellationToken();

            var archiveStream = await _archive.OpenReadAsync();
            await _extractService.ExtractFilesAsync(archiveStream, token);
        }

        private async Task CopyFile(string fileName, MemoryStream fileData)
        {
			await _fileCopyService.CopyFilesAsync(TargetDirectory, fileName, fileData, null);
        }

        private async void OnFileExtracted(object sender, FileExtractedEventArgs e)
        {
            var fileName = _fileNameParser.ExtractFileName(e.FileName);
            await CopyFile(fileName, e.FileData);
        }
    }
}

