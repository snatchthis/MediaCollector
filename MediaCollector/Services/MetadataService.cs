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

        public int? GetYearTakenFromImage(string filePath)
        {
            var directoriesWithPossibleDateTags = GetDirectoriesWithPossibleDateTags(filePath);

            if (directoriesWithPossibleDateTags != null)
            {
                var earliestDateTaken = directoriesWithPossibleDateTags
                    .Select(directory => GetDateFromTag(directory, DateTag.TAG_36867) ??
                                          GetDateFromTag(directory, DateTag.TAG_320) ??
                                          GetDateFromTag(directory, DateTag.TAG_3))
                    .Where(dateTaken => dateTaken.HasValue)
                    .OrderBy(dateTaken => dateTaken.Value)
                    .FirstOrDefault();

                if (earliestDateTaken.HasValue)
                {
                    return earliestDateTaken.Value.Year;
                }
            }

            return null;
        }

        private IEnumerable<MetadataExtractor.Directory> GetDirectoriesWithPossibleDateTags(string filePath)
        {
            try
            {
                var allDirectories = ImageMetadataReader.ReadMetadata(filePath);
                return allDirectories.Where(directory => directory.ContainsTag((int)DateTag.TAG_36867)
                                                      || directory.ContainsTag((int)DateTag.TAG_320)
                                                      || directory.ContainsTag((int)DateTag.TAG_3));
            }
            catch (Exception ex)
            {
                // log or handle the exception appropriately
                return null;
            }
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

