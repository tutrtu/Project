using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class EditAnswerDto
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


        public virtual Question Question { get; set; }
    }
}
