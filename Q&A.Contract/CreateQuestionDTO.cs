using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q_A.Contract
{
    public class CreateQuestionDTO
    {
        public List<SelectDTO> Categories { get; set; } = new();
        public List<SelectDTO> Tags { get; set; } = new();
    }
}
