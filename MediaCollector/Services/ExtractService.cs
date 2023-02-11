using System;
using System.IO;
using System.Text;
using ICSharpCode.SharpZipLib.Tar;

namespace MediaCollector.Services
{
    public class ExtractService
    { 
        public event EventHandler<FileExtractedEventArgs> FileExtracted;

        public ExtractService()
        {

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
                        
                        using (var memoryStream = new MemoryStream())
                        {
                            await tarInputStream.CopyEntryContentsAsync(memoryStream, token);
                            memoryStream.Seek(0, SeekOrigin.Begin);

                            if (!tarEntry.IsDirectory && tarEntry.Name.EndsWith(".heic", StringComparison.OrdinalIgnoreCase))
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
