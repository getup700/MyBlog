using System;
using System.Collections.Generic;

namespace MyBlog.EntityFrameworkCore.Models;

public partial class WriterInfo
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string UserPwd { get; set; } = null!;
}
