using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ContestSystem.Data;

namespace ContestSystem.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly AppDbContext _context;

    public UserController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("history")]
    [Authorize]
    public IActionResult GetHistory()
    {
        var username = User.Identity?.Name;

        if (string.IsNullOrEmpty(username))
            return Unauthorized();

        var user = _context.Users.FirstOrDefault(u => u.Username == username);

        if (user == null)
            return Unauthorized();

        var history = _context.Participations
            .Where(p => p.UserId == user.Id)
            .Select(p => new
            {
                Contest = _context.Contests
                    .Where(c => c.Id == p.ContestId)
                    .Select(c => new
                    {
                        c.Name,
                        c.Prize
                    })
                    .FirstOrDefault(),

                p.Score,

                IsWinner = _context.Participations
                    .Where(x => x.ContestId == p.ContestId)
                    .OrderByDescending(x => x.Score)
                    .First().UserId == user.Id
            })
            .ToList();

        return Ok(history);
    }


    [HttpGet("in-progress")]
    [Authorize]
    public IActionResult GetInProgressContests()
    {
        var username = User.Identity?.Name;

        if (string.IsNullOrEmpty(username))
            return Unauthorized();

        var user = _context.Users.FirstOrDefault(u => u.Username == username);

        if (user == null)
            return Unauthorized();

        // All contests user has NOT participated in
        var participatedContestIds = _context.Participations
            .Where(p => p.UserId == user.Id)
            .Select(p => p.ContestId)
            .ToList();

        var inProgress = _context.Contests
            .Where(c => !participatedContestIds.Contains(c.Id))
            .Select(c => new
            {
                c.Id,
                c.Name,
                c.Description,
                c.AccessLevel,
                c.StartTime,
                c.EndTime
            })
            .ToList();

        return Ok(inProgress);
    }

    [HttpGet("prizes")]
    [Authorize]
    public IActionResult GetPrizesWon()
    {
        var username = User.Identity?.Name;

        if (string.IsNullOrEmpty(username))
            return Unauthorized();

        var user = _context.Users.FirstOrDefault(u => u.Username == username);

        if (user == null)
            return Unauthorized();

        var prizes = _context.Participations
            .Where(p => p.UserId == user.Id)
            .Select(p => new
            {
                p.ContestId,
                p.Score,
                Contest = _context.Contests.First(c => c.Id == p.ContestId)
            })
            .ToList()
            .Where(p =>
            {
                var maxScore = _context.Participations
                    .Where(x => x.ContestId == p.ContestId)
                    .Max(x => x.Score);

                return p.Score == maxScore;
            })
            .Select(p => new
            {
                p.ContestId,
                p.Contest.Name,
                p.Contest.Prize,
                p.Score
            })
            .ToList();

        return Ok(prizes);
    }
}