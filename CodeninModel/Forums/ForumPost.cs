using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeninModel.Forums
{
    public class ForumPost
    {
        public int ForumPostId { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string IP { get; set; }
        public DateTime Editedd { get; set; }
        public int Flag { get; set; }
        public string EditedReason { get; set; }
        public string DeletedReason { get; set; }
        public int ReplyToPost { get; set; }
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
        public DateTime Posted { get; set; }

        public string UserName { get; set; }
        public int ForumTopicId { get; set; }
        public virtual ICollection<ForumTopic> Forum { get; set; }
        
  



    }
}
