using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CodeninModel;
using CodeninModel.CBTE;
using CodeninModel.Quiz;
using Microsoft.WindowsAzure.Storage.Blob;

namespace CodedenimWebApp.ViewModels
{
    public class CourseContentVm
    {
        public List<Module> Modules { get; set; }
        public List<ModulesVm> Module { get; set; }
        public Module ModulesAD { get; set; }   
        public string ModulesName { get; set; }
        public List<Topic> Topics { get; set; }
        public Topic TopicsAD { get; set; }
        public List<TopicMaterialUpload> Materials { get; set; }
        public TopicMaterialUpload Material { get; set; }
        public List<Tutor> Tutors { get; set; } 
        public Stream Blob { get; set; }

        public List<Course> Courses { get; set; }
        public int CourseId { get; set; }
        public Course CoursesAD { get; set; }
        public int CourseIdentifier { get; set; }
    }

    public class ModulesVm
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string ModuleDescription { get; set; }
        public int ExpectedTime { get; set; }
        public bool IsModuleTaken { get; set; }
      
    }
}