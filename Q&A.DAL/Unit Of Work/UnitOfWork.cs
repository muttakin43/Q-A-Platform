using Q_A.DAL.Context;
using Q_A.DAL.Interface;
using Q_A.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q_A.DAL.Unit_Of_Work
{
    public class UnitOfWork(ApplicationDbContext dbContext) : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext = dbContext;

        
        private IQuestionRepository? _question;
        private ICategoryRepository? _categories;
        private ITagRepository? _tags;
        private IAnswerRepository? _answers;
        private ICommentRepository? _comments;
        private IVoteRepository? _votes;
        private IQuestionTagRepository? _questionTags;



        public IQuestionRepository Questions => _question ??= new QuestionRepository(_dbContext);

        public ICategoryRepository Categories => _categories ??= new CategoryRepository(_dbContext);

        public ITagRepository Tags => _tags ??= new TagRepository(_dbContext);

        public IAnswerRepository Answers => _answers ??= new AnswerRepository(_dbContext);

        public ICommentRepository Comments => _comments ??= new CommentRepository(_dbContext);

        public IVoteRepository Votes => _votes ??= new VoteRepository(_dbContext);

        public IQuestionTagRepository QuestionTags => _questionTags ??= new QuestionTagRepository(_dbContext);

        public async ValueTask DisposeAsync()
        {
            await _dbContext.DisposeAsync();
        }

       

        public Task<int> SaveChangesAsync()
        {
            return _dbContext.SaveChangesAsync();
        }

        Task IUnitOfWork.SaveAsync()
        {
            return SaveAsync();
        }

        private async Task SaveAsync()
        {
            
        }
    }
}
