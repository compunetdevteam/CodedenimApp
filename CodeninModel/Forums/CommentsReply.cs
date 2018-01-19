using GenericDataRepository.Abstractions;
using System;

namespace CodeninModel.Forums
{
    public class CommentsReply : Entity<int>
    {
        //public int CommentsReplyId { get; set; }
        public string Reply { get; set; }
        public DateTime ReplyDate { get; set; }
        public string UserId { get; set; }
        public int ForumCommentsId { get; set; }
        public virtual ForumComments ForumQuestionComments { get; set; }
    }
}