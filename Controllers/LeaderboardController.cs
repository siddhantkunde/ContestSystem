using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ContestSystem.Data;

namespace ContestSystem.Controllers;

[ApiController]
[Route("api/leaderboard")]
public class LeaderboardController : ControllerBase
{
    private readonly AppDbContext _context;

    public LeaderboardController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("{contestId}")]
    [Authorize]
    public IActionResult GetLeaderboard(int contestId)
    {
        var leaderboard = _context.Participations
            .Where(p => p.ContestId == contestId)
            .OrderByDescending(p => p.Score)
            .Select(p => new
            {
                Username = _context.Users
                    .Where(u => u.Id == p.UserId)
                    .Select(u => u.Username)
                    .FirstOrDefault(),

                p.Score
            })
            .ToList();

        return Ok(leaderboard);
    }
}