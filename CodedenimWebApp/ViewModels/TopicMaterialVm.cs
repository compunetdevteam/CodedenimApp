using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CodeninModel;
using CodeninModel.Quiz;

namespace CodedenimWebApp.ViewModels
{
    public class TopicMaterialVm
    {
        public List<TopicMaterialUpload> TopicMaterialUploads { get; set; }
        public List<Topic> Topics { get; set; }
      
    }
}