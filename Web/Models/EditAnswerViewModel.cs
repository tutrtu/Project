using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class EditAnswerViewModel
    {
        [Required]
        public int AnswerID { get; set; }

        [Required]
        public string AnswerText { get; set; }

        [Required]
        public DateTime AnswerDateAndTime { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        public int QuestionID { get; set; }

        

        public virtual QuestionViewModel Question { get; set; }
    }
}
