using Localizy.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Localizy.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DatabaseTestController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public DatabaseTestController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("test-connection")]
    public async Task<IActionResult> TestConnection()
    {
        try
        {
            var canConnect = await _context.Database.CanConnectAsync();

            if (canConnect)
            {
                var projectCount = await _context.Projects.CountAsync();
                var translationCount = await _context.Translations.CountAsync();

                return Ok(new
                {
                    status = "connected",
                    database = _context.Database.GetDbConnection().Database,
                    projectCount,
                    translationCount,
                    buildNumber = 9
                });
            }

            return BadRequest(new { status = "disconnected" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { status = "error", message = ex.Message });
        }
    }
}