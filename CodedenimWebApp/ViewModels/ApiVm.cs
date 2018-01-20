using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodedenimWebApp.ViewModels
{
    public class ApiVm
    {
    }

    public class ExamIndexVm
    {
        public string StudentId { get; set; }
        public int? TopicId { get; set; }
        public string Score { get; set; }
    }
}