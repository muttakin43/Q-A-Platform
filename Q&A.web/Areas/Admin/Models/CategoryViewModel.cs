using System.ComponentModel.DataAnnotations;

namespace Q_A.web.Areas.Admin.Models
{
    public class CategoryViewModel
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string Slug { get; set; } = null!;

        [MaxLength(500)]
        public string? Description { get; set; }
    }
}
