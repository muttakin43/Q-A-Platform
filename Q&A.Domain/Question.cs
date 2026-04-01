using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q_A.Domain
{
    public class Question : Entity  
       
    {
        public int QuestionId { get; set; }

        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int ViewCount { get; set; }


        public DateTime? UpdatedAt { get; set; }

        // FK
        public string UserId { get; set; } = null!;
        public int CategoryId { get; set; }

        // Navigation
        public ApplicationUser User { get; set; } = null!;
        public Category Category { get; set; } = null!;

        public int? AcceptedAnswerId { get; set; }
        public Answer? AcceptedAnswer { get; set; }

        public ICollection<Answer> Answers { get; set; } = new List<Answer>();
        public ICollection<QuestionTag> QuestionTags { get; set; } = new List<QuestionTag>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Vote> Votes { get; set; } = new List<Vote>();
    }
}
