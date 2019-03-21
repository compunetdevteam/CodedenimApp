using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodedenimWebApp.Controllers.Api.ApiViewModel
{
    public class TopicMaterialVm
    {
        public int TopicMaterialId { get; set; }
        public int TopicId { get; set; }
        public string Tutor { get; set; }
        public string FileType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string TextContent { get; set; }
        public string FileLocation { get; set; }

    }
}