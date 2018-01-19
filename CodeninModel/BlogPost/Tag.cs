﻿using GenericDataRepository.Abstractions;
using System.Collections.Generic;

namespace CodeninModel.BlogPost
{
    public class Tag : Entity<int>
    {
        public Tag()
        {
            this.Posts = new HashSet<Post>();
        }

        //public int TagId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }
}