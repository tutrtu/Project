namespace API.Models
{
    public class AnswerDto
    {
        public int AnswerID { get; set; }
        public string AnswerText { get; set; }
        public DateTime AnswerDateAndTime { get; set; }
        public int UserID { get; set; }
        public int QuestionID { get; set; }
        public virtual UserDto User { get; set; }
      
    }
}
