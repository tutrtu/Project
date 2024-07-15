using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly QandAContext _context;

        public QuestionsController(QandAContext context)
        {
            _context = context;
           
        }

        [HttpGet]
        public ActionResult<IEnumerable<Question>> Get()
        {
            var questions = _context.Questions.ToList();
                

            return Ok(questions);
        }

        [HttpGet("{id:int}")]
        public ActionResult<Question> GetQuestionById(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var ques = _context.Questions.FirstOrDefault(u => u.QuestionId == id);
            if (ques == null)
            {
                return NotFound();
            }
            return Ok(ques);
        }

        [HttpPost]
        public ActionResult<Question> CreateQuestion([FromBody] NewQuestionDto newQuestionDto)
        {
            if (newQuestionDto == null)
            {
                return BadRequest("NewQuestionDto is null");
            }

            var question = new Question
            {
                QuestionName = newQuestionDto.QuestionName,
                QuestionDateAndTime = DateTime.Now, // Optionally set current date/time
                UserId = newQuestionDto.UserID,
                CategoryId = newQuestionDto.CategoryID
                // Populate other properties as needed
            };

            _context.Questions.Add(question);
            _context.SaveChanges();

            return Ok(question); // Optionally, return the created question
        }

        [HttpGet("{name}")]
        public ActionResult<IEnumerable<Question>> GetQuestionsByName(string name)
        {
            var questions = _context.Questions
                .Where(q => q.QuestionName.ToLower().Replace(" ", "")
                .Contains(name.ToLower().Replace(" ", "")))
                .ToList();

            if (!questions.Any())
            {
                return NotFound();
            }
            return Ok(questions);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateVilla(int id, [FromBody] UpdateQuestionDto updateQuestionDto)
        {
            if (updateQuestionDto == null || id != updateQuestionDto.QuestionID)
            {
                return BadRequest();
            }

            Question model = new()
            {
                QuestionId = id,
                QuestionName = updateQuestionDto.QuestionName,
                QuestionDateAndTime = DateTime.Now
            };

            _context.Questions.Update(model);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
