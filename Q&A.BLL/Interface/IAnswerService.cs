using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q_A.BLL.Interface
{
    public interface IAnswerService
    {
        Task AddAnswerAsync(int questionId, string userEmail, string content);

    }
}

