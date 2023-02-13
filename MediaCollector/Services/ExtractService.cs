using System;
using System.IO;
using System.Text;
using ICSharpCode.SharpZipLib.Tar;
using MediaCollector.Data;

namespace MediaCollector.Services
{
    public class ExtractService
    {
        private readonly OperationSettings _settings;

        public event EventHandler<FileExtractedEventArgs> FileExtracted;

        public ExtractService(OperationSettings settings)
        {
            _settings = settings;
        }

        public async Task ExtractFiles(Stream archiveStream, CancellationToken token)
        {
            try
            {
                using (var tarInputStream = new TarInputStream(archiveStream, Encoding.UTF8))
                {
                    TarEntry tarEntry;
                    while ((tarEntry = await tarInputStream.GetNextEntryAsync(token)) != null)
                    {
                        if (GuidMatchService.ContainsGuid(tarEntry.Name))
                            continue;

                        using (var memoryStream = new MemoryStream())
                        {
                            await tarInputStream.CopyEntryContentsAsync(memoryStream, token);
                            memoryStream.Seek(0, SeekOrigin.Begin);
                            
                            if (!tarEntry.IsDirectory && _settings.MediaFilesExtensions.Any(x => tarEntry.Name.EndsWith(x, StringComparison.OrdinalIgnoreCase)))
                            { 
                                OnFileExtracted(new FileExtractedEventArgs(tarEntry.Name, memoryStream));
                            }
                        }
                        
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected virtual void OnFileExtracted(FileExtractedEventArgs e)
        {
            FileExtracted?.Invoke(this, e);
        }
    }

    public class FileExtractedEventArgs : EventArgs
    {
        public string FileName { get; }
        public MemoryStream FileData { get; }

        public FileExtractedEventArgs(string fileName, MemoryStream fileData)
        {
            FileName = fileName;
            FileData = fileData;
        }
    }
}
