using System;
namespace MediaCollector.Data
{
    public class MetadataEntry
    {
        public MetadataEntry(string name, string tagName, string description)
        {
            Name = name;
            TagName = tagName;
            Description = description;
        }

        public string Name { get; private set; }
        public string TagName { get; set; }
        public string Description { get; private set; }
    }
}

