using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using MyBlog.JWT;
using MyBlog.JWT.Models;
using System.Configuration;
using System.Text;

var appBuilder = WebApplication.CreateBuilder();
var services = appBuilder.Services;
services.AddControllers(options =>
{
    //配置全局路由模板
    options.Conventions.Add(new RoutePrefixConvention("api"));
});
#region services.AddAuthentication();
//将appsettings.json中的JwtSettings部分文件读取到JwtSettings中，这是给其他地方用的
//asp已经读取了几个指定路径的配置，不需要在手动读取配置文件
////为什么即使不绑定也可以获取值？因为Get<T>包含了bind功能。
//services.Configure<JwtSettings>(configSection);
var configSection = appBuilder.Configuration.GetSection("JwtSettings");
var jwtSettings = configSection.Get<JwtSettings>();

//添加身份验证服务
services.AddAuthentication(authenOptions =>
{
    //认证middleware配置
    authenOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    authenOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(bearerOptions =>
{
    //jwt token参数设置
    bearerOptions.TokenValidationParameters = new TokenValidationParameters
    {
        NameClaimType = JwtClaimTypes.Name,
        RoleClaimType = JwtClaimTypes.Role,
        //Token颁发机构*
        ValidIssuer = jwtSettings.Issuer,
        //颁发给谁*
        ValidAudience = jwtSettings.Audience,
        //签名秘钥，这里的key要进行加密*
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),

        /***********************************TokenValidationParameters的参数默认值***********************************/
        // RequireSignedTokens = true,
        // SaveSigninToken = false,
        // ValidateActor = false,
        // 将下面两个参数设置为false，可以不验证Issuer和Audience，但是不建议这样做。
        // ValidateAudience = true,
        // ValidateIssuer = true, 
        // ValidateIssuerSigningKey = false,
        // 是否要求Token的Claims中必须包含Expires
        // RequireExpirationTime = true,
        // 允许的服务器时间偏移量
        // ClockSkew = TimeSpan.FromSeconds(300),
        // 是否验证Token有效期，使用当前时间与Token的Claims中的NotBefore和Expires对比
        // ValidateLifetime = true
    };
});
#endregion

var app = appBuilder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyBlog.JWT v1"));
}

//用来配置请求路由的中间件，确定请求的路由终结点
//主要作用是将路由中间件添加到请求处理管道中，并确定请求如何被路由到终结点
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});