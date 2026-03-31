using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q_A.Domain
{
    public class QuestionTag 
    {
        public int QuestionTagId { get; set; }
        public int QuestionId { get; set; }
        public int TagId { get; set; }

        public Question Question { get; set; } = null!;
        public Tag Tag { get; set; } = null!;
    }
}
