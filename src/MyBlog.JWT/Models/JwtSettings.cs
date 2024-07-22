namespace MyBlog.JWT.Models
{
    public class JwtSettings
    {
        /// <summary>
        /// token的颁发者
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// token可以给哪些客户端使用
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// 加密的key
        /// </summary>
        public string SecretKey { get; set; }
    }
}
