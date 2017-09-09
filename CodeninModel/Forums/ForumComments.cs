using System.Collections.Generic;

namespace CodeninModel.Forums
{
    public class ForumComments
    {
        public int ForumCommentsId { get; set; }
        public string Comment { get; set; }
        public string UserId { get; set; }
        public int ForumReplyIdP { get; set; }
        public virtual ICollection<ForumQuestion> ForumQeQuestion { get; set; }
    }
}