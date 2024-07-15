using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class NewQuestionDto
    {
        
        public string QuestionName { get; set; }

      
        public DateTime QuestionDateAndTime { get; set; }

   
        public int UserID { get; set; }


        public int CategoryID { get; set; }


    }
}
