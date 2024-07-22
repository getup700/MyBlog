using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MyBlog.WebApi.Utility._AutoMapper;
using MyBlog.WebApi;
using SqlSugar.IOC;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;
using MyBlog.IRepository;
using MyBlog.IService;
using MyBlog.Repository;
using MyBlog.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MyBlog.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


var corsStrategy = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddCors(options =>
{
    options.AddPolicy(corsStrategy, builder =>
    {
        builder
            //.WithOrigins("http://localhost:8080", "http://localhost:5500", "http://192.168.108.6:8080")// 允许访问的前端地址
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

//找出所有的控制器
services.AddControllers();

services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "MyBlog.WebApi", Version = "v1" });

    #region Swagger使用鉴权组件
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Description = "直接在下框中输入Bearer {token}（注意两者之间是一个空格）",
        Name = "Authorization",
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference=new OpenApiReference
                    {
                      Type=ReferenceType.SecurityScheme,
                      Id="Bearer"
                    }
                },
                new string[]{ }
            }
        });
    #endregion
});

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

//SqlSugarIOC
services.AddSqlSugar(new IocConfig()
{
    ConnectionString = configuration["SqlConn"],
    DbType = IocDbType.SqlServer,
    IsAutoCloseConnection = true
});

//IOC依赖注入
services.AddScoped<IBlogNewsRepository, BlogNewsRepository>();
services.AddScoped<IBlogNewsService, BlogNewsService>();
services.AddScoped<ITypeInfoRepository, TypeInfoRepository>();
services.AddScoped<ITypeInfoService, TypeInfoService>();
services.AddScoped<IWriterInfoRepository, WriterInfoRepository>();
services.AddScoped<IWriterInfoService, WriterInfoService>();
//JWT鉴权
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SDMC-CJAS1-SAD-DFSFA-SADHJVF-VF")),
            ValidateIssuer = true,
            ValidIssuer = "http://localhost:6060",
            ValidateAudience = true,
            ValidAudience = "http://localhost:5000",
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(60)
        };
    });

//AutoMapper
services.AddAutoMapper(typeof(CustomAutoMapperProfile));

services.AddDbContext<MyBlogContext>(options =>
{
    options.UseSqlServer(builder.Configuration["SqlConn"]);
});


var app = builder.Build();
var logger = app.Services.GetService<ILogger<Program>>();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyBlog.WebApi v1"));

}

app.UseHttpsRedirection();

app.UseFileServer();
//分配路由，将请求与端点匹配，路由规则
app.UseRouting();
////认证、鉴权//webapi自带
//app.UseAuthentication();
////使用授权
//app.UseAuthorization();
//跨域//一定要放在authentication中间件后边
app.UseCors(corsStrategy);
//执行匹配的端点
app.UseEndpoints(endpoints =>
{
    //控制器路由
    endpoints.MapControllers();
});

//app.MapGet("/", () => "Hello World!");

//app.UsePathBase("/apiIIII");

//Use和Run类似，但是可以继续调用下一个中间件
//app.Use(async (context, next) =>
//{
//    context.Response.ContentType = "text/plain;charset=utf-8";
//    await context.Response.WriteAsync("Hello,Web API。你好，webapi");
//    logger.LogInformation("m1:传入请求");
//    //调用下一个中间件处理
//    await next();
//    logger.LogInformation("m1:传出响应");
//});


//终端中间件，后续的中间件将不会被调用
//app.Run(async (context) =>
//{
//    logger.LogInformation("m2:传入请求");
//    await context.Response.WriteAsync("Hello,Web API end。你好，webapi end");
//    logger.LogInformation("m2:传出响应");
//});

app.Run();



