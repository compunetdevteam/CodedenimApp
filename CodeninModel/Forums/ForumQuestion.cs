using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeninModel.Forums
{
    public class ForumQuestion
    {
        public int ForumQuestionId { get; set; }
        public string Title { get; set; }
        public string QuestionName { get; set; }
        public DateTimeOffset PostDate { get; set; }
        public int CourseId { get; set; }
        public string StudentId { get; set; }

        public virtual Forum Forum { get; set; }

        public virtual Student Students { get; set; }
        public virtual ICollection<ForumAnswer> ForumAnswers { get; set; }
        public  ForumQuestionView ForumQuestionView { get; set; }

    }

    public class ForumQuestionView
    {

        [Key, ForeignKey("ForumQuestion")]
        public int ForumQuestionId { get; set; }
        public int ViewCounter { get; set; }
        public virtual ForumQuestion ForumQuestion { get; set; }
    }




}
