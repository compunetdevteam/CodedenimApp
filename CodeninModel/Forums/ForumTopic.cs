using System;
using System.Collections.Generic;

namespace CodeninModel.Forums
{
    public class ForumTopic : Entity<int>
    {
        //public int ForumTopicId { get; set; }
        public string Title { get; set; }
        public int ViewCount { get; set; }
        public int PostCount { get; set; }
        public int Flag { get; set; }
        public int Type { get; set; }
        public DateTime LastPosted { get; set; }
        public int LastPostId { get; }
        public int LastPostAuthorId { get; set; }
        public string LastPostUsername { get; set; }
        public int ForumId { get; set; }
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
        public DateTime Posted { get; set; }
        public int SpamScore { get; set; }
        public int SpamReporters { get; set; }
        public int OriginalTopicId { get; set; }
        public string CustomProperties { get; set; }

        public virtual ICollection<LatestPost> LatestPost { get; set; }
        public virtual ICollection<Forum> Forum { get; set; }
        
    }
}