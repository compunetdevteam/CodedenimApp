using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CodeninModel;
using CodeninModel.Quiz;

namespace CodedenimWebApp.ViewModels
{
    public class CourseContentVm
    {
        public List<Module> Modules { get; set; }
        public List<Topic> Topics { get; set; }
        public List<TopicMaterialUpload> Materials { get; set; }

    }
}