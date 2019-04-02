using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodedenimWebApp.Controllers.Api.ApiViewModel
{
    public class QuizAnswerVm
    {
        public int QuestionId { get; set; }
        public string Answer { get; set; }
        public string StudentEmail { get; set; }

    }
}