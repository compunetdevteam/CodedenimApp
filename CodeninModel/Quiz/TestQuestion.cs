using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeninModel.Quiz
{
    public class TestQuestion
    {
        public TestQuestion()
        {
            TestAnswer = new List<TestAnswer>();
        }
        public int TestQuestionId { get; set; }
        public string QuestionContent { get; set; }
        public string QuestionInstruction { get; set; }

        public int CourseId { get; set; }
        public  Course Course { get; set; }

        public ICollection<TestAnswer> TestAnswer { get; set; }


    }
}
