using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q_A.BLL.Interface
{
    public interface IVoteService
    {
        Task<int> GetQuestionScoreAsync(int questionId);
        Task<int> GetAnswerScoreAsync(int answerId);
        Task VoteQuestionAsync(int questionId, string userEmail, bool isUpvote);
        Task VoteAnswerAsync(int answerId, string userEmail, bool isUpvote);
    }
}
