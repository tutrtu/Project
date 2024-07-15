namespace API.Models
{
    public class QuestionDto
    {
        public int QuestionID { get; set; }
        public string QuestionName { get; set; }
        public DateTime QuestionDateAndTime { get; set; }
        public int UserID { get; set; }
        public int CategoryID { get; set; }



        public UserDto User { get; set; }
        public Category Category { get; set; }
        public virtual List<AnswerDto> Answers { get; set; }
    }
}
