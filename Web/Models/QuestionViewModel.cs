namespace Web.Models
{
    public class QuestionViewModel
    {
        public int QuestionID { get; set; }
        public string QuestionName { get; set; }
        public DateTime QuestionDateAndTime { get; set; }
        public int? UserID { get; set; }
        public int? CategoryID { get; set; }




        public User User { get; set; }
        public Category Category { get; set; }
        public virtual List<Answer> Answers { get; set; }
    }
}
