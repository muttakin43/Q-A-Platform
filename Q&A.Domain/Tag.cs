using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q_A.Domain
{
    public class Tag : Entity
    {
        public int TagId { get; set; }
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
     

        public ICollection<QuestionTag> QuestionTags { get; set; } = new List<QuestionTag>();
    }
}
