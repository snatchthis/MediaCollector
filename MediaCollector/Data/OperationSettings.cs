using System;
namespace MediaCollector.Data
{
	public class OperationSettings
	{
		public OperationSettings()
		{
			SourceArchive = string.Empty;
			TargetDirectory = string.Empty;
			MediaFilesExtensions = new List<string>();
		}

		public string SourceArchive { get; set; }
		public string TargetDirectory { get; set; }
		public List<string> MediaFilesExtensions { get; set; }
	}
}

