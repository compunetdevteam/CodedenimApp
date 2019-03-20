using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodedenimWebApp.Controllers.Api.ApiViewModel
{
    public class QuizVm
    {
        public int TopicQuizId { get; set; }


       
        public int ModuleId { get; set; }

       
        public string Question { get; set; }

        public string Option1 { get; set; }

        
        public string Option2 { get; set; }

        
        public string Option3 { get; set; }

        
        public string Option4 { get; set; }

       
        public string Answer { get; set; }

        public string QuestionType { get; set; }

    }
}