using System;
namespace MediaCollector.Data
{
	public interface IFolderPicker
	{
		public Task<string> PickFolder();
	}
}

