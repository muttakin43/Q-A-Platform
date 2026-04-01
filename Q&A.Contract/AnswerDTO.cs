using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q_A.Contract
{
    public class AnswerDTO
    {
        public int AnswerId { get; set; }
        public string Content { get; set; } = null!;
        public string AuthorName { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public int Score { get; set; }
    }
}
