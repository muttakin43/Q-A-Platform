using Q_A.Contract;

namespace Q_A.web.Models
{
    public class HomeIndexViewModel
    {
        public List<QuestionListModel> Questions { get; set; } = new();
        public IReadOnlyList<CategoryDTO> Categories { get; set; } = Array.Empty<CategoryDTO>();
        public IReadOnlyList<TagDTO> Tags { get; set; } = Array.Empty<TagDTO>();
    }
}
