using Microsoft.AspNetCore.Mvc;
using MyBlog.EntityFrameworkCore;
using MyBlog.EntityFrameworkCore.Models;

namespace MyBlog.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SuggestionController : ControllerBase
    {
        private MyBlogContext _context;

        public SuggestionController(MyBlogContext context)
        {
            _context = context;
        }

        [HttpPut]
        [Route("Add")]
        public IActionResult AddSuggestion(Suggestion suggestion)
        {
            var d = _context.Suggestions.Add(suggestion);
            _context.SaveChanges();
            return Ok(d);
        }
    }
}
