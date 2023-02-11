using System;
namespace MediaCollector.Services
{
	public interface IFilePickerService
	{
         public Task<FileResult> PickFile();
    }
}

