using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class Question
    {
        public Question()
        {
            Answers = new HashSet<Answer>();
        }

        public int QuestionId { get; set; }
        public string? QuestionName { get; set; }
        public DateTime? QuestionDateAndTime { get; set; }
        public int? UserId { get; set; }
        public int? CategoryId { get; set; }


        public virtual Category? Category { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
    }
}
