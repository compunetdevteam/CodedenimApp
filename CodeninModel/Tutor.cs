using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CodeninModel.BlogPost;

namespace CodeninModel
{
    public class Tutor : Person
    {
        [Key]
        public string TutorId { get; set; }

        public string Designation { get; set; }
        public string MaritalStatus { get; set; }
        public bool IsActiveTutor { get; set; }
        public string ActiveStatus { get; set; } 
        public string StaffRole { get; set; }
       // public byte[] TutorPassport { get; set; }
      //  public FileType FileType { get; set; }
       // public virtual ICollection<Course> Courses { get; set; }
       public virtual  ICollection<TutorCourses> TutorCourses { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        //public virtual ICollection<File> Files { get; set; }
        //public virtual ICollection<FilePath> FilePaths { get; set; }
    }
}