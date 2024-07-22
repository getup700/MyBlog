using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace MyBlog.WebApi.Controllers
{
    public record Url(string Path);

    [ApiController]
    [Route("api/[controller]/[action]")]
    public class FeiShuController : ControllerBase
    {
        private string code;
        [HttpPost]
        public async Task<IActionResult> ReceiveCodeAsync(Url url)
        {
            //using var reader = new StreamReader(Request.Body);
            //var redirect_url = await reader.ReadToEndAsync();
            // 解析和处理 body 中的数据
            try
            {
                var result1 = url.Path.Split("code=")[1];
                var result2 = result1.Split("&state=")[0];
                code = result2;
                return Ok();
            }
            catch (System.Exception)
            {
            }
            return BadRequest();
        }

        [HttpPost]
        public IActionResult GetCode()
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return BadRequest();
            }
            else
            {
                return Ok(code);
            }
        }
    }
}
