using System;
using System.Collections.Generic;

namespace MyBlog.EntityFrameworkCore.Models;

public partial class BlogNews
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public DateTime Time { get; set; }

    public int BrowseCount { get; set; }

    public int LikeCount { get; set; }

    public int TypeId { get; set; }

    public int WriterId { get; set; }
}
