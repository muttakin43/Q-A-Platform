using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q_A.Contract
{
    public class QuestionListDTO
    {
        public int QuestionId { get; set; }
        public string Title { get; set; } = null!;
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        public IReadOnlyList<int> TagIds { get; set; } = Array.Empty<int>();
        public DateTime CreatedAt { get; set; }
        public int AnswerCount { get; set; }
        public int ViewCount { get; set; }
    }
}
