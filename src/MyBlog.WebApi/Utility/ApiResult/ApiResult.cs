using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.WebApi.Utility.ApiResult
{
    public class ApiResult
    {
        public int Code { get; set; }
        public string Msg { get; set; }
        /// <summary>
        /// 分页总数
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// object也行
        /// </summary>
        public dynamic Data { get; set; }
    }
}
