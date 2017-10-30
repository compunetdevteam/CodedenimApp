using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CodeninModel;
using CodeninModel.Quiz;

namespace CodedenimWebApp.ViewModels
{
    public class CreteCourseVm
    {
        public Course Course { get; set; }
        public CourseCategory CourseCategory { get; set; }
        public Module Module { get; set; }
        public Topic Topic { get; set; }
        public TopicMaterialUpload TopicMaterialUpload { get; set; }
    }
}