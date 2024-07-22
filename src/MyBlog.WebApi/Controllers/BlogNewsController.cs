using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBlog.IRepository;
using MyBlog.IService;
using MyBlog.Model;
using MyBlog.Model.DTO;
using MyBlog.WebApi.Utility.ApiResult;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.WebApi.Controllers
{
    public record Blog(string Title, string Content, int TypeId);

    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BlogNewsController : ControllerBase
    {
        private readonly IBlogNewsService _iBlogNewsService;
        private readonly IBlogNewsRepository blogNewsRepository;

        public BlogNewsController(IBlogNewsService iBlogNewsService, IBlogNewsRepository blogNewsRepository)
        {
            this._iBlogNewsService = iBlogNewsService;
            this.blogNewsRepository = blogNewsRepository;
        }
        [HttpGet("IActionResult")]
        public IActionResult Get(int id)
        {
            if (id == 1)
            {
                return BadRequest();
            }
            else if (id == 2)
            {
                return Ok();
            }
            else if (id == 3)
            {
                return Redirect("https://www.baidu.com");
                return RedirectToAction("BlogNews");
            }
            else if (id == 4)
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("BlogByComposite/{title}")]
        public Blog GetBlogByBody(string name, [FromRoute(Name = "title")] string title, string contentt, [FromQuery(Name = "typeId")] int typeIdd)
        {
            return new Blog(name, title, typeIdd);
        }

        [HttpGet("blogbyquerystring")]
        public async Task<BlogNews> GetBlogByQS(int Id)
        {
            var blog = await _iBlogNewsService.FindAsync(Id);
            return blog;
        }

        [HttpGet("blogbyurl/{id}")]
        public async Task<BlogNews> GetBlogByURL([FromRoute(Name = "id")] int typeId)
        {
            var blog = await _iBlogNewsService.FindAsync(typeId);
            return blog;
        }

        [HttpPost("BlogByBody/{id}")]
        public Blog GetBlogByBody(Blog blog, int id)
        {
            return blog;
        }

        [HttpGet("BlogNews")]
        public async Task<ActionResult<ApiResult>> GetBlogNews()
        {
            int id = Convert.ToInt32(this.User.FindFirst("Id").Value);
            var data = await _iBlogNewsService.QueryAsync(c => c.WriterId == id);
            if (data == null) return ApiResultHelper.Error("没有更多的文章");
            return ApiResultHelper.Success(data);
        }

        /// <summary>
        /// 添加文章
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<ActionResult<ApiResult>> Create(string title, string content, int typeid)
        {
            //使用jwt权限验证时才可获取当前用户信息
            var userId = this.User.FindFirst("Id").Value;
            //数据验证
            BlogNews blogNews = new BlogNews
            {
                BrowseCount = 0,
                Content = content,
                LikeCount = 0,
                Time = DateTime.Now,
                Title = title,
                TypeId = typeid,
                WriterId = Convert.ToInt32(userId)
            };
            bool b = await _iBlogNewsService.CreateAsync(blogNews);
            if (!b) return ApiResultHelper.Error("添加失败，服务器发生错误");
            return ApiResultHelper.Success(blogNews);
        }

        [HttpDelete("Delete")]
        public async Task<ActionResult<ApiResult>> Delete(int id)
        {
            bool b = await _iBlogNewsService.DeleteAsync(id);
            if (!b) return ApiResultHelper.Error("删除失败");
            return ApiResultHelper.Success(b);
        }

        [HttpPut("Edit")]
        public async Task<ActionResult<ApiResult>> Edit(int id, string title, string content, int typeid)
        {
            var blogNews = await _iBlogNewsService.FindAsync(id);
            if (blogNews == null) return ApiResultHelper.Error("没有找到该文章");
            blogNews.Title = title;
            blogNews.Content = content;
            blogNews.TypeId = typeid;
            bool b = await _iBlogNewsService.EditAsync(blogNews);
            if (!b) return ApiResultHelper.Error("修改失败");
            return ApiResultHelper.Success(blogNews);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="iMapper"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [HttpGet("BlogNewsPage")]
        public async Task<ApiResult> GetBlogNewsPage([FromServices] IMapper iMapper, int page, int size)
        {
            RefAsync<int> total = 0;
            var blognews = await _iBlogNewsService.QueryAsync(page, size, total);
            try
            {
                var blognewsDTO = iMapper.Map<List<BlogNewsDTO>>(blognews);
                return ApiResultHelper.Success(blognewsDTO, total);
            }
            catch (Exception)
            {
                return ApiResultHelper.Error("AutoMapper映射错误");
            }
        }
    }
}
