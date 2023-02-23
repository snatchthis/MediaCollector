using System;
using MetadataExtractor;

namespace MediaCollector.Services
{
    enum DateTag
    {
        TAG_36867 = 36867,
        TAG_320 = 320,
        TAG_3 = 3
    }

    public class MetadataService
    {

        public MetadataService()
        {
            
        }

        public int? GetYear(string filePath)
        {
            try
            {
                var allDirectories = ImageMetadataReader.ReadMetadata(filePath);
                var directoriesWithPossibleDateTags = allDirectories.Where(x => x.ContainsTag((int)DateTag.TAG_36867)
                                                                        || x.ContainsTag((int)DateTag.TAG_320)
                                                                        || x.ContainsTag((int)DateTag.TAG_3));
                if(directoriesWithPossibleDateTags != null)
                {
                    foreach(var directory in directoriesWithPossibleDateTags)
                    {
                        DateTime? dateTaken = GetDateFromTag(directory, DateTag.TAG_36867);
                        if (!dateTaken.HasValue)
                            dateTaken = GetDateFromTag(directory, DateTag.TAG_320);
                        if (!dateTaken.HasValue)
                            dateTaken = GetDateFromTag(directory, DateTag.TAG_3);
                        if (dateTaken.HasValue)
                            return dateTaken.Value.Year;
                    }
                }
            }
            catch(Exception ex)
            {

            }

            return null;
        }

        private DateTime? GetDateFromTag(MetadataExtractor.Directory direcotry, DateTag tag)
        {
            DateTime? date;
            try
            {
                date = direcotry.GetDateTime(((int)tag));
            }
            catch (Exception)
            {
                date = null;
            }

            return date;
        }
    }
}

