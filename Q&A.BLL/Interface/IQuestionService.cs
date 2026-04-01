using Q_A.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q_A.BLL.Interface
{
    public interface IQuestionService
    {
        Task<QuestionDetailsDTO?> GetQuestionDetailsAsync(int questionId);
        Task<IReadOnlyList<QuestionListDTO>> GetLatestQuestionsAsync(int take = 20);
        Task<CreateQuestionDTO> GetCreatePageDataAsync();
        Task CreateQuestionAsync(string userEmail, string title, string description, int categoryId, IEnumerable<int> tagIds, IEnumerable<string> newTagNames);
    }
}

