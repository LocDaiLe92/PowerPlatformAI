using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace powerplatform.ai.powerautomatedoc.Models
{

    public class ADOProjectWikis
    {
        public List<Wiki> Value { get; set; }
        public int Count { get; set; }
    }

    public class Wiki
    {
        public string Id { get; set; }
        public List<VersionObj> Versions { get; set; }
        public string Url { get; set; }
        public string RemoteUrl { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string ProjectId { get; set; }
        public string RepositoryId { get; set; }
        public string MappedPath { get; set; }
    }

    public class VersionObj
    {
        public string Version { get; set; }
    }


}
