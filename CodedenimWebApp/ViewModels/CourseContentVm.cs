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
        public IEnumerable<Module> Modules { get; set; }
        public IEnumerable<Topic> Topics { get; set; }
        public IEnumerable<TopicMaterialUpload> Materials { get; set; }
        public IEnumerable<Tutor> Tutors { get; set; }

        public IEnumerable<Course> Courses { get; set; }
    }
}