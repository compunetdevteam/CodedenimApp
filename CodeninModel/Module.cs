using System.Collections.Generic;
using CodeninModel.CBTE;
using CodeninModel.Quiz;

namespace CodeninModel
{
    public class Module
    {
        public int ModuleId { get; set; }
        public int CourseId { get; set; }
        public string ModuleName { get; set; }
        public string ModuleDescription { get; set; }
        public int ExpectedTime { get; set; }
        public virtual Course Course { get; set; }
        public virtual ICollection<Topic> Topics { get; set; }
        public virtual ICollection<StudentTestLog> StudentTestLogs { get; set; }
    }
}