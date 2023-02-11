using System;
namespace MediaCollector.Data
{
	public class FilePick
	{
		public FilePick()
		{
		}

        public async Task<FileResult> PickAndShow(PickOptions options)
        {
            try
            {
                var result = await FilePicker.Default.PickAsync(options);
                if (result != null)
                {
                    if (result.FileName.EndsWith("tar", StringComparison.OrdinalIgnoreCase))
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
    }
}

