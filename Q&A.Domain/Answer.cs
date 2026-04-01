using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q_A.Domain
{
    public class Answer : Entity
    {
        public int AnswerId { get; set; }

        public string Content { get; set; } = null!;
        public int QuestionId { get; set; }
        public string UserId { get; set; } = null!;

        
        public Question Question { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Vote> Votes { get; set; } = new List<Vote>();
        
    }
}
