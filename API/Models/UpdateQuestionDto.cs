using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class UpdateQuestionDto
    {
        
        public int QuestionID { get; set; }

        
        public string QuestionName { get; set; }

        
        public DateTime QuestionDateAndTime { get; set; }

        
        public int CategoryID { get; set; }
    }
}
