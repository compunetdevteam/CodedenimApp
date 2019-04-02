using CodeninModel.Quiz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodedenimWebApp.ViewModels
{
    public class TestVm
    {
        public TestVm()
        {
            TestAnswers = new List<TestAnswerVm>();
            TestQuestion = new List<TestQuestionVm>();
        }
        public List<TestQuestionVm> TestQuestion { get; set; }

        public List<TestAnswerVm> TestAnswers { get; set; }

        public string Instruction { get; set; }
        public string CourseName { get; set; }
        public string StudentId { get; set; }
    }

    public class TestQuestionVm
    {
        public string Question { get; set; }
        public string Instruction { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public int QuestionId { get; set; }
    }

    public class TestAnswerVm
    {
        public string Answer { get; set; }
        public bool hasAnswered { get; set; }
        public DateTimeOffset DateSubmited { get; set; }
        public int QuestionId { get; set; }
        public string StudentId { get; set; }
    }


}