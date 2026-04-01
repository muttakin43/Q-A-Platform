using Q_A.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q_A.BLL.Interface
{
    public interface ICommentService
    {
        Task AddCommentAsync(int answerId, string userEmail, string content, int? parentCommentId = null);
        Task<IReadOnlyList<CommentDTO>> GetCommentsTreeForAnswerAsync(int answerId);
    }
}
