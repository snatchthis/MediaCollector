using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using MediaCollector.Data;
using MediaCollector.Services;


namespace MediaCollector.ViewModels
{
	public class InputFormViewModel : AbstractViewModel<OperationSettings>
	{
		private ObservableCollection<string> _mediaFileExtensions;
		private IFilePickerService _filePicker;
        private IFolderPicker _folderPicker;
		private FileNameParser _parser;
		private FileCopyService _fileCopyService;
		private FileResult _archive;
		private string _currentFile;

        public InputFormViewModel(OperationSettings settings, IFilePickerService filePicker, IFolderPicker folderPicker, FileNameParser parser, FileCopyService fileCopyService) : base(settings)
        {
			_filePicker = filePicker;
			_folderPicker = folderPicker;
			_parser = parser;
        }

        public string SourceArchive
		{
			get => Model.SourceArchive;
			set
			{
				SetValue(Model.SourceArchive, value);
			}
		}

		public string TargetDirectory
		{
			get => Model.TargetDirectory;
			set
			{
				SetValue(Model.TargetDirectory, value);
			}
		}

		public string CurrentFile
		{
			get => _currentFile;
			set
			{
				SetValue(ref _currentFile, value);
			}
		}

		public Collection<string> MediaFileExtensions
		{
			get => _mediaFileExtensions;
			set
			{
				if (_mediaFileExtensions == null)
				{
					_mediaFileExtensions = new ObservableCollection<string>(value);
					_mediaFileExtensions.CollectionChanged += (sender, e) => OnPropertyChanged();
				}

				SetValue(_mediaFileExtensions, value);
				
			}
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
			await ExtractArchive();
			IsBusy = false;
            
        }

		private async Task ExtractArchive()
		{
            var parser = new FileNameParser();
            CancellationToken token = new CancellationToken();
            var extractService = new ExtractService();
            if (_archive != null)
            {
                extractService.FileExtracted += async (o, e) =>
                {
                    var fileName = parser.ExtractFileName(e.FileName);
                    await CopyFiles(fileName, e.FileData);
                    CurrentFile = e.FileName;

                };

                var archiveStream = await _archive.OpenReadAsync();
                await extractService.ExtractFiles(archiveStream, token);
            }
        }

        private async Task CopyFiles(string fileName, MemoryStream fileData)
        {
			await _fileCopyService.CopyFilesAsync(TargetDirectory, fileName, fileData, null);
        }
    }
}

