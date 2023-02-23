using System;
namespace MediaCollector.Services
{
    public abstract class AbstractFilePickerService : IFilePickerService
    {
        public AbstractFilePickerService()
        {
        }

        public async Task<FileResult> PickFile()
        {
            try
            {
                var result = await FilePicker.Default.PickAsync(Options);
                if (result != null)
                {
                    if (Extensions == null || Extensions.Any((x) => result.FileName.EndsWith(x, StringComparison.OrdinalIgnoreCase)))
                    {
                        return result;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                // The user canceled or something went wrong
            }

            return null;
        }

        protected virtual string[] Extensions => null;

        protected virtual PickOptions Options => null;
    }
}

