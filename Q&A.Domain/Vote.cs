using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q_A.Domain
{
    public class Vote : Entity
    {
        public int VoteId { get; set; }
        public VoteType VoteType { get; set; } // +1 / -1
       

        // FK
        public string UserId { get; set; } = null!;
        public int? QuestionId { get; set; }
        public int? AnswerId { get; set; }
        public int? CommentId { get; set; }

        // Navigation
        public ApplicationUser User { get; set; } = null!;
        public Question? Question { get; set; }
        public Answer? Answer { get; set; }
        public Comment? Comment { get; set; }
    }
}
