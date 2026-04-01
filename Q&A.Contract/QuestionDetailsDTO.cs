using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q_A.Contract
{
    public class QuestionDetailsDTO
    {
        public int QuestionId { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string CategoryName { get; set; } = null!;
        public string AuthorName { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public int ViewCount { get; set; }
        public int QuestionScore { get; set; }
        public List<string> TagNames { get; set; } = new();
        public List<AnswerDTO> Answers { get; set; } = new();
    }
}
