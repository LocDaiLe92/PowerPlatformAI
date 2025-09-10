using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace powerplatform.ai.powerautomatedoc.Models
{
    public class DevOpsRequestBody
    {
        [Required]
        [DefaultValue("two-many")]
        public string OrganizationName { get; set; }
        [Required]
        [DefaultValue("OpenAI")]
        public string ProjectName { get; set; }
        [Required]
        [DefaultValue("CodeBaseDoc")]
        public string RepositoryName { get; set; }
        [Required]
        public string PAT { get; set; }
    }
}
