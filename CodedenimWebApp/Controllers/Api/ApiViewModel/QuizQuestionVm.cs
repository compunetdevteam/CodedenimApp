using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodedenimWebApp.Controllers.Api.ApiViewModel
{
    public class QuizQuestionVm
    {
        public int QuestionId { get; set; }
        public string Question { get; set; }
        public string QuestionInstruction { get; set; }
        public int CourseId { get; set; }
    }
}