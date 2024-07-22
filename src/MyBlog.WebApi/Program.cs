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
            //.WithOrigins("http://localhost:8080", "http://localhost:5500", "http://192.168.108.6:8080")// ������ʵ�ǰ�˵�ַ
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

//�ҳ����еĿ�����
services.AddControllers();

services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "MyBlog.WebApi", Version = "v1" });

    #region Swaggerʹ�ü�Ȩ���
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Description = "ֱ�����¿�������Bearer {token}��ע������֮����һ���ո�",
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

//IOC����ע��
services.AddScoped<IBlogNewsRepository, BlogNewsRepository>();
services.AddScoped<IBlogNewsService, BlogNewsService>();
services.AddScoped<ITypeInfoRepository, TypeInfoRepository>();
services.AddScoped<ITypeInfoService, TypeInfoService>();
services.AddScoped<IWriterInfoRepository, WriterInfoRepository>();
services.AddScoped<IWriterInfoService, WriterInfoService>();
//JWT��Ȩ
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
//����·�ɣ���������˵�ƥ�䣬·�ɹ���
app.UseRouting();
////��֤����Ȩ//webapi�Դ�
//app.UseAuthentication();
////ʹ����Ȩ
//app.UseAuthorization();
//����//һ��Ҫ����authentication�м�����
app.UseCors(corsStrategy);
//ִ��ƥ��Ķ˵�
app.UseEndpoints(endpoints =>
{
    //������·��
    endpoints.MapControllers();
});

//app.MapGet("/", () => "Hello World!");

//app.UsePathBase("/apiIIII");

//Use��Run���ƣ����ǿ��Լ���������һ���м��
//app.Use(async (context, next) =>
//{
//    context.Response.ContentType = "text/plain;charset=utf-8";
//    await context.Response.WriteAsync("Hello,Web API����ã�webapi");
//    logger.LogInformation("m1:��������");
//    //������һ���м������
//    await next();
//    logger.LogInformation("m1:������Ӧ");
//});


//�ն��м�����������м�������ᱻ����
//app.Run(async (context) =>
//{
//    logger.LogInformation("m2:��������");
//    await context.Response.WriteAsync("Hello,Web API end����ã�webapi end");
//    logger.LogInformation("m2:������Ӧ");
//});

app.Run();



