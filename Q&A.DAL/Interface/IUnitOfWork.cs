using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q_A.DAL.Interface
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IQuestionRepository Questions { get; }
        ICategoryRepository Categories { get; }
        ITagRepository Tags { get; }
        IAnswerRepository Answers { get; }
        ICommentRepository Comments { get; }
        IVoteRepository Votes { get; }
        IQuestionTagRepository QuestionTags { get; }
        Task<int> SaveChangesAsync();
    }
}
