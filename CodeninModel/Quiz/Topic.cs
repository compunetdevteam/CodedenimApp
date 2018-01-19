using System.Collections.Generic;
using CodeninModel.Assesment;
using CodeninModel.BlogPost;
using CodeninModel.CBTE;
using GenericDataRepository.Abstractions;

namespace CodeninModel.Quiz
{
    public class Topic : Entity<int>
    {
        //public int TopicId { get; set; }
        public int ModuleId { get; set; }
        public string TopicName { get; set; }
        public int ExpectedTime { get; set; }
        public virtual Module Module { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<TopicMaterialUpload> MaterialUploads { get; set; }
        public virtual ICollection<TopicAssignment> TopicAssignments { get; set; }
        public virtual ICollection<SubmitAssignment> SubmitAssignments { get; set; }
       
        public virtual ICollection<QuizRule> QuizRules { get; set; }
        public virtual ICollection<TopicQuiz> TopicQuizzes { get; set; }
        public virtual ICollection<StudentQuestion> StudentQuestions { get; set; }
    }
}