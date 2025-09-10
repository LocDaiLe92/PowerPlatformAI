using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace powerplatform.ai.powerautomatedoc.Models
{

    public class ADORepos
    {
        public int Count { get; set; }
        public List<Item> Value { get; set; }
    }

    public class Item
    {
        public string ObjectId { get; set; }
        public string GitObjectType { get; set; }
        public string CommitId { get; set; }
        public string Path { get; set; }
        public bool IsFolder { get; set; }
        public ContentMetadata ContentMetadata { get; set; }
        public string Url { get; set; }
    }

    public class ContentMetadata
    {
        public string FileName { get; set; }
    }
}
