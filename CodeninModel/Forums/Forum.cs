using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeninModel.Forums
{
    public class Forum : Entity<int>
    {
        //[Key,ForeignKey("Course")]
        //public int CourseId { get; set; }
        public string ForumName { get; set; }
        public string Description { get; set; }
      
        public DateTime LastPosted { get; set; }
 
        public DateTime CreationDate { get; set; }

        public virtual Course Course { get; set; }
        public virtual ICollection<ForumQuestion> ForumQuestions { get; set; }
        public virtual ForumView ForumView { get; set; }
    }

    public class ForumView : Entity<int>
    {
        [Key, ForeignKey("Forum")]
        public int ContentViewId { get; set; }
        public int ViewCounter { get; set; }
        public virtual Forum Forum { get; set; }
    }
}
