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
        private readonly IMapper _mapper;

        public QuestionsController(QandAContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Question>> Get()
        {
            var questions = _context.Questions.Include(u => u.User).Include(c => c.Category).ToList();
                

            return Ok(questions);
        }

        [HttpGet("{id:int}")]
        public ActionResult<Question> GetQuestionById(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var ques = _context.Questions.Include(u => u.User).Include(c => c.Category).Include(a => a.Answers).FirstOrDefault(u => u.QuestionId == id);
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

            // Map the DTO to the entity
            var question = _mapper.Map<Question>(newQuestionDto);

            // Load the related entities based on their IDs
            question.User = _context.Users.Find(newQuestionDto.UserID);
            if (question.User == null)
            {
                return NotFound("User not found");
            }

            question.Category = _context.Categories.Find(newQuestionDto.CategoryID);
            if (question.Category == null)
            {
                return NotFound("Category not found");
            }

            _context.Questions.Add(question);
            _context.SaveChanges();

            return Ok(question); // Optionally, return the created question
        }

        [HttpGet("{name}")]
        public ActionResult<IEnumerable<Question>> GetQuestionsByName(string name)
        {
            var questions = _context.Questions.Include(u => u.User).Include(c => c.Category).Include(a => a.Answers)
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
