using GenericDataRepository.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeninModel
{
    public class Content : Entity<int>
    {
        public int ContentId { get; set; }
    }
}
