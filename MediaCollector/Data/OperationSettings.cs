using System;
namespace MediaCollector.Data
{
	public class OperationSettings
	{
		public OperationSettings()
		{
			SourceArchive = string.Empty;
			TargetDirectory = string.Empty;
			MediaFilesExtensions = new string[] {string.Empty};
		}

		public string SourceArchive { get; set; }
		public string TargetDirectory { get; set; }
		public string[] MediaFilesExtensions { get; set; }
	}
}

