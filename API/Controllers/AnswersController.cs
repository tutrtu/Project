using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswersController : ControllerBase
    {
        private readonly QandAContext _context;
        public AnswersController(QandAContext context)
        {
            _context = context;
        }
        [HttpGet]
        public ActionResult<List<AnswerDto>> GetAnswersByQuestionID(int qid)
        {
            var ans = _context.Answers
                              .Where(temp => temp.QuestionId == qid)
                              .OrderByDescending(temp => temp.AnswerDateAndTime)
                              .Select(temp => new AnswerDto
                              {
                                  AnswerID = temp.AnswerId,
                                  AnswerText = temp.AnswerText,
                                  AnswerDateAndTime = temp.AnswerDateAndTime,
                                  UserID = temp.UserId,
                                  QuestionID = temp.QuestionId,
                                  User = new UserDto
                                  {

                                      Email = temp.User.Email,
                                      Name = temp.User.Name,
                                      Mobile = temp.User.Mobile,

                                  }
                              })
                              .ToList();
            return Ok(ans);
        }



        [HttpDelete("{aid}")]
        public ActionResult DeleteAnswer(int aid)
        {
            var ans = _context.Answers.FirstOrDefault(temp => temp.AnswerId == aid);
            if (ans == null)
            {
                return NotFound();
            }

            _context.Answers.Remove(ans);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPut]
        public ActionResult UpdateAnswer(EditAnswerDto avm)
        {
            var answerEntity = _context.Answers.FirstOrDefault(temp => temp.AnswerId == avm.AnswerID);
            if (answerEntity == null)
            {
                return NotFound();
            }

            // Manually update the properties
            answerEntity.AnswerText = avm.AnswerText;
            answerEntity.AnswerDateAndTime = avm.AnswerDateAndTime;
            answerEntity.UserId = avm.UserID;
            answerEntity.QuestionId = avm.QuestionID;

            _context.SaveChanges();

            return NoContent();
        }

        [HttpPost]
        public ActionResult<Answer> CreateAnswer(NewAnswerDto newAnswerDto)
        {
            if (newAnswerDto == null)
            {
                return BadRequest("Answer data cannot be null.");
            }

            var answerEntity = new Answer
            {
                AnswerText = newAnswerDto.AnswerText,
                AnswerDateAndTime = DateTime.Now,
                UserId = newAnswerDto.UserID,
                QuestionId = newAnswerDto.QuestionID
            };

            _context.Answers.Add(answerEntity);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetAnswersByQuestionID), new { qid = answerEntity.QuestionId }, answerEntity);
        }

    }
}
