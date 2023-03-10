using System;
using System.Text.RegularExpressions;

namespace MediaCollector.Services
{
	public partial class FileNameParser
	{
        [GeneratedRegex("([^\\\\/]+\\.[^\\\\/]+)$")]
        private static partial Regex FileNameRegex();

        public FileNameParser()
		{
		}

		public string ExtractFileName(string path)
		{
			Regex expression = FileNameRegex();

			var result = path;

			if (expression.IsMatch(path))
			{
				result = expression.Match(path).Groups[1].Value;
			}

			return result;
        }
    }
}

