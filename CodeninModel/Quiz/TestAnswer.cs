using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeninModel.Quiz
{
    public class TestAnswer
    {
        public TestAnswer()
        {
        }

        public int TestAnswerId { get; set; }
        public string TestAnswerContent { get; set; }
        public bool hasAnswered { get; set; }
        public DateTimeOffset DateSubmited { get; set; }


        public int TestQuestionId { get; set; }
        public  TestQuestion TestQuestion { get; set; }


        public string StudentId { get; set; }
        public  Student Student { get; set; }
    }
}
