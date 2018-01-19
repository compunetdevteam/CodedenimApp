using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeninModel.Quiz;
using GenericDataRepository.Abstractions;

namespace CodeninModel
{
    public class TopicContent : Entity<int>
    {
        //public int TopicContentId { get; set; }
        public int TopicId { get; set; }
        public string TopicDescription { get; set; }
        public virtual Topic Topic { get; set; }
    }
}