using System.ComponentModel.DataAnnotations;

namespace DotNet_API_Example.Models.Dto
{
    public class BlogUpdateDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string Title { get; set; } = string.Empty;
        [Required]
        [MaxLength(1200)]
        public string Description { get; set; } = string.Empty;
        [Required]
        public string Content { get; set; } = string.Empty;
        public double Rate { get; set; }
        public string Thumbnail { get; set; } = string.Empty;
    }
}
