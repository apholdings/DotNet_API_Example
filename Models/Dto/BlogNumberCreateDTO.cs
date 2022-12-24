using System.ComponentModel.DataAnnotations;

namespace DotNet_API_Example.Models.Dto
{
    public class BlogNumberCreateDTO
    {
        [Required]
        public int BlogNo { get; set; }
        [Required]
        public int BlogID { get; set; }
        public string SpecialDetails { get; set; }
    }
}
