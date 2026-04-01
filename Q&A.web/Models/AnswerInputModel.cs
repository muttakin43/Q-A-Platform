using System.ComponentModel.DataAnnotations;

namespace Q_A.web.Models
{
    public class AnswerInputModel
    {
        public int QuestionId { get; set; }

        [Required(ErrorMessage = "Please enter your answer.")]
        [MinLength(10, ErrorMessage = "Answer must be at least 10 characters.")]
        public string Content { get; set; } = null!;
    }
}
