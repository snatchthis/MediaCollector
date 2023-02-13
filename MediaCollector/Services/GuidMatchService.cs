using System;
using System.Text.RegularExpressions;

namespace MediaCollector.Services
{
    public partial class GuidMatchService
    {
        [GeneratedRegex("([0-9A-Fa-f]{8}[-]?[0-9A-Fa-f]{4}[-]?[0-9A-Fa-f]{4}[-]?[0-9A-Fa-f]{4}[-]?[0-9A-Fa-f]{12})")]
        private static partial Regex GuidRegex();

        public GuidMatchService()
        {
        }

        public static bool ContainsGuid(string name)
        {
            var regex =  GuidRegex();
            return regex.IsMatch(name);
        }
    }
}

