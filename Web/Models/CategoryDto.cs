using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class CategoryDto
    {
        [Required]
        public int CategoryID { get; set; }

        [Required]
        public string CategoryName { get; set; }
    }
}
