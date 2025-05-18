using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using UserAdmin.MongoEntities;

namespace UserAdmin.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class CommentsController : ControllerBase
    {
        private readonly MongoDBContext _context;

        public CommentsController(MongoDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetComments()
        {
            var comments = await _context.Comments.Find(_ => true).Limit(5).ToListAsync();
            return Ok(comments);
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody] Comment comment)
        {
            await _context.Comments.InsertOneAsync(comment);
            return CreatedAtAction(nameof(GetComments), new { id = comment._id }, comment);
        }
    }
}