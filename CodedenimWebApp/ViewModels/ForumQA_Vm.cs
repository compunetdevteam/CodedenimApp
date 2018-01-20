using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CodeninModel;
using CodeninModel.Forums;

namespace CodedenimWebApp.ViewModels
{
    public class ForumQA_Vm
    {
        public int ForumQuestionId { get; set; }
        public string Title { get; set; }
        public string QuestionName { get; set; }
        public int CourseId { get; set; }
        public string StudentId { get; set; }


        public virtual Student Students { get; set; }
        public virtual ICollection<ForumAnswer> ForumAnswers { get; set; }
        public ForumQuestionView ForumQuestionView { get; set; }
    }
}