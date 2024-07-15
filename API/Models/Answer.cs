using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class Answer
    {
        public int AnswerId { get; set; }
        public string AnswerText { get; set; }
        public DateTime AnswerDateAndTime { get; set; }
        public int UserId { get; set; }
        public int QuestionId { get; set; }

        public virtual Question Question { get; set; }
        public virtual User User { get; set; }
    }
}
