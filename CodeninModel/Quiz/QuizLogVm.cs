using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeninModel.Quiz
{
   public  class QuizLogVm
    {
        public int ExamLogId { get; set; }
        public string StudentId { get; set; }
        public int CourseId { get; set; }
        public int LevelId { get; set; }
        public int SemesterId { get; set; }
        public int SessionId { get; set; }
        public int ExamTypeId { get; set; }
        public double Score { get; set; }
        public bool ExamTaken { get; set; }
    }
}
