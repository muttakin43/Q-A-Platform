using Q_A.DAL.Context;
using Q_A.DAL.Interface;
using Q_A.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q_A.DAL.Repository
{
    public class QuestionTagRepository(ApplicationDbContext dbContext) :
        GenericRepository<QuestionTag>(dbContext), IQuestionTagRepository
    {
    }
}
