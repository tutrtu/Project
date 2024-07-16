using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class NewQuestionViewModel
    {
        [Required]
        public string QuestionName { get; set; }

        [Required]
        public DateTime QuestionDateAndTime { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        public int CategoryID { get; set; }
    }
}
