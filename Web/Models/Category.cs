using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class Category
    {
        [Required]
        public int CategoryID { get; set; }

        [Required]
        public string CategoryName { get; set; }
    }
}
