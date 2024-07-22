using System;
using System.Collections.Generic;

namespace MyBlog.EntityFrameworkCore.Models;

public partial class TypeInfo
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
}
