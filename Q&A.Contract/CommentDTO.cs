using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q_A.Contract
{
    public class CommentDTO
    {
        public int CommentId { get; set; }
        public string Content { get; set; } = null!;
        public string AuthorName { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public List<CommentDTO> Replies { get; set; } = new();
    }
}
