using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CodeninModel;
using CodeninModel.Forums;

namespace CodedenimWebApp.ViewModels
{
    public class TutorDashboardVm
    { 
        public List<TutorCourse> TutorCourses { get; set; }
        public List<string> ForumQuestions { get; set; }
       
    }
}