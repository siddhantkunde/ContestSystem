using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ContestSystem.Data;
using ContestSystem.DTOs;
using ContestSystem.Models;

namespace ContestSystem.Controllers;

[ApiController]
[Route("api/contest")]
public class ContestController : ControllerBase
{
    private readonly AppDbContext _context;

    public ContestController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [Authorize]
    public IActionResult GetContests()
    {
        var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

        var contests = _context.Contests.ToList();

        if (role == "Normal")
        {
            contests = contests
                .Where(c => c.AccessLevel == "Normal")
                .ToList();
        }

        return Ok(contests);
    }

    [HttpGet("public")]
    public IActionResult GetPublicContests()
    {
        var contests = _context.Contests
            .Select(c => new
            {
                c.Id,
                c.Name,
                c.Description,
                c.StartTime,
                c.EndTime
            })
            .ToList();

        return Ok(contests);
    }
    
    [HttpGet("{contestId}/questions")]
    [Authorize]
    public IActionResult GetQuestions(int contestId)
    {
        var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

        var contest = _context.Contests.FirstOrDefault(c => c.Id == contestId);

        if (contest == null)
            return NotFound();

        // Normal user trying VIP contest
        if (role == "Normal" && contest.AccessLevel == "VIP")
            return Forbid("You cannot access VIP contest");

        var questions = _context.Questions
            .Where(q => q.ContestId == contestId)
            .Select(q => new
            {
                q.Id,
                q.Text,
                q.Type,
                Options = q.Options.Select(o => new
                {
                    o.Id,
                    o.Text
                })
            })
            .ToList();
            
        return Ok(questions);
    }

    [HttpPost("{contestId}/submit")]
    [Authorize]
    public IActionResult SubmitAnswers(int contestId, List<SubmitAnswerDto> answersDto)
    {
        var username = User.Identity?.Name;
        var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

        if (string.IsNullOrEmpty(username))
            return Unauthorized();

        if (role == "Admin")
            return BadRequest("Admin cannot participate");

        var user = _context.Users.FirstOrDefault(u => u.Username == username);

        if (user == null)
            return Unauthorized();

        //Prevent multiple submissions
        var existing = _context.Participations
            .FirstOrDefault(p => p.UserId == user.Id && p.ContestId == contestId);

        if (existing != null)
            return BadRequest("You have already submitted this contest");

        //Contest validation
        var contest = _context.Contests.FirstOrDefault(c => c.Id == contestId);

        if (contest == null)
            return NotFound();

        if (DateTime.Now < contest.StartTime)
            return BadRequest("Contest has not started yet");

        if (DateTime.Now > contest.EndTime)
            return BadRequest("Contest has already ended");

        //Empty submission check
        if (answersDto == null || !answersDto.Any())
            return BadRequest("No answers submitted");

        int score = 0;

        foreach (var ans in answersDto)
        {
            //Remove duplicate options 
            ans.SelectedOptionIds = ans.SelectedOptionIds.Distinct().ToList();

            //Validate question belongs to contest
            var question = _context.Questions
                .FirstOrDefault(q => q.Id == ans.QuestionId && q.ContestId == contestId);

            if (question == null)
                return BadRequest($"Invalid question: {ans.QuestionId}");

            //Validate options belong to question
            var validOptionIds = _context.Options
                .Where(o => o.QuestionId == ans.QuestionId)
                .Select(o => o.Id)
                .ToList();

            if (!ans.SelectedOptionIds.All(id => validOptionIds.Contains(id)))
                return BadRequest($"Invalid option selected for question {ans.QuestionId}");

            // (multi-select)
            var correctOptions = _context.Options
                .Where(o => o.QuestionId == ans.QuestionId && o.IsCorrect)
                .Select(o => o.Id)
                .ToList();

            bool isCorrect =
                correctOptions.Count == ans.SelectedOptionIds.Count &&
                !correctOptions.Except(ans.SelectedOptionIds).Any();

            if (isCorrect)
            {
                score += 10;
            }
        }

        var participation = new Participation
        {
            UserId = user.Id,
            ContestId = contestId,
            Score = score
        };

        _context.Participations.Add(participation);
        _context.SaveChanges();

        return Ok(new
        {
            message = "Submitted successfully",
            score = score
        });
    }

    [HttpGet("available")]
    [Authorize]
    public IActionResult GetAvailableContests()
    {
    var username = User.Identity?.Name;
    var user = _context.Users.FirstOrDefault(u => u.Username == username);

    var participatedContestIds = _context.Participations
        .Where(p => p.UserId == user.Id)
        .Select(p => p.ContestId)
        .ToList();

    var contests = _context.Contests
        .Where(c => !participatedContestIds.Contains(c.Id))
        .ToList();

    return Ok(contests);
    }
}