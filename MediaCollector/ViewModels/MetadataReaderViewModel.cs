using System;
using System.ComponentModel.DataAnnotations;
using MediaCollector.Data;
using MediaCollector.Services;
using MetadataExtractor;
using DateTakenExtractor;

namespace MediaCollector.ViewModels
{
    public class MetadataReaderViewModel : AbstractViewModel<MetadataSettings>
    {
        private readonly MoveFilesService _moveFilesService;
        private readonly IFilePickerService _filePicker;
        private FileResult _fileToRead;

        public MetadataReaderViewModel(MetadataSettings metadataSettings, AnyFilePickerService filePickerService) : base(metadataSettings)
        {
            _filePicker = filePickerService;
            MetadataEntries = new List<MetadataEntry>();
        }

        [Required(AllowEmptyStrings = false)]
        public string File
        {
            get => Model.FilePath;
            set => SetValue(() => Model.FilePath = value);
        }

        public List<MetadataEntry> MetadataEntries { get; }

        public async Task PickFile()
        {
            _fileToRead = await _filePicker.PickFile();
            if (_fileToRead != null)
                File = _fileToRead.FileName;
        }

        public async Task ReadMetadata()
        {
            MetadataEntries.Clear();

            var directories = ImageMetadataReader.ReadMetadata(await _fileToRead.OpenReadAsync());

            foreach (var directory in directories)
            {
                foreach (var tag in directory.Tags)
                {

                    MetadataEntries.Add(new MetadataEntry(tag.Name, tag.Type.ToString(), tag.Description));
                    OnPropertyChanged();
                }
            }

        }
    }
}

