using System.Collections.Generic;
using CodeninModel.CBTE;
using CodeninModel.Quiz;
using GenericDataRepository.Abstractions;

namespace CodeninModel
{
    public class Module : Entity<int>
    {
        //public int ModuleId { get; set; }
        public int CourseId { get; set; }
        public string ModuleName { get; set; }
        public string ModuleDescription { get; set; }
        public int ExpectedTime { get; set; }
        public virtual Course Course { get; set; }
        public virtual ICollection<Topic> Topics { get; set; }
        public virtual ICollection<StudentTestLog> StudentTestLogs { get; set; }
    }
}