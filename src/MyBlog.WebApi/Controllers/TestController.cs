
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MyBlog.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IConfiguration _iconfiguration;
        private readonly IBlogNewsService _iBlogNewsService;

        public TestController(IConfiguration iconfiguration, IBlogNewsService iBlogNewsService)
        {
            _iconfiguration = iconfiguration;
            _iBlogNewsService = iBlogNewsService;
        }

        [HttpGet("SqlConn")]
        public async Task<IActionResult> GetConn()
        {
            var result = _iconfiguration["SqlConn"];
            //无论是否成功，都返回200状态码
            return Ok(result);
        }

        [HttpGet("{blogId}")]
        public async Task<IActionResult> GetBlog(int blogId)
        {
            var exist = _iBlogNewsService.FindAsync(blogId);
            if (exist == null)
            {
                //返回404状态码
                return NotFound();
            }
            else
            {
                //返回200状态码
                return Ok(exist);
            }
        }

        [HttpGet("NoAuthorize")]
        public string NoAuthorize()
        {
            return "this is NoAuthorize";
        }

        [Authorize]
        [HttpGet("Authorize")]
        public string Authorize()
        {
            return "this is Authorize";
        }
    }
}
