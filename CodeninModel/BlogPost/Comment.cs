using System;

namespace CodeninModel.BlogPost
{
    public class Comment
    {
        public int CommentId { get; set; }
        public int PostId { get; set; }
        public DateTime DateTime { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Body { get; set; }

        public virtual Post Post { get; set; }
    }
}