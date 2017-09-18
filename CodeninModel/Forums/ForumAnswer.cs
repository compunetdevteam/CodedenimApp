using System;
using System.Collections.Generic;

namespace CodeninModel.Forums
{
    public class ForumAnswer
    {
        public int ForumAnswerId { get; set; }
        public string Answer { get; set; }
        public DateTime ReplyDate { get; set; }
        public int ForumQuestionId { get; set; }
        public string UserId { get; set; }
        public virtual ForumQuestion ForumQuestions { get; set; }
        public virtual ICollection<VoteForumAnswer> VoteForumAnswers { get; set; }
    }

    public class VoteForumAnswer
    {
        public int VoteForumAnswerId { get; set; }
        public int ForumAnswerId { get; set; }
        public Vote Vote { get; set; }
        public string UserId { get; set; }
        public virtual ICollection<ForumAnswer> ForumAnswers { get; set; }
    }

    public enum Vote
    {
        Like,Dislike
    }
}