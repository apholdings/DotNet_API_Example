using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNet_API_Example.Models
{
    public class BlogNumber
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int BlogNo { get; set; }
        [ForeignKey("Blog")]
        public int BlogID { get; set; }
        public Blog Blog {get; set;}
        public string SpecialDetails { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

    }
}
