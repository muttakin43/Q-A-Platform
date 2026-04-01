using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q_A.Domain
{
    public class Comment : Entity
    {
        public int CommentId { get; set; }

        public string Content { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public int? QuestionId { get; set; }
        public int? AnswerId { get; set; }
        public int? ParentCommentId { get; set; }

        // Navigation
        public ApplicationUser User { get; set; } = null!;
        public Question? Question { get; set; }
        public Answer? Answer { get; set; }

        public Comment? ParentComment { get; set; }
        public ICollection<Comment> Replies { get; set; } = new List<Comment>();

        public ICollection<Vote> Votes { get; set; } = new List<Vote>();
    }
}
