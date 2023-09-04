using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestSharpAutomation.JiraAPI.Request
{
    public class CreateProjectPayload
    {
        public string key { get; set; }
        public string name { get; set; }
        public string projectTypeKey { get; set; }
        public string projectTemplateKey { get; set; }
        public string description { get; set; }
        public string lead { get; set; }
        public string url { get; set; }
        public string assigneeType { get; set; }
        public int avatarId { get; set; }
        //public int issueSecurityScheme { get; set; }
        //public int permissionScheme { get; set; }
        //public int notificationScheme { get; set; }
        //public int categoryId { get; set; }
    }
}
