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
    //����ȫ��·��ģ��
    options.Conventions.Add(new RoutePrefixConvention("api"));
});
#region services.AddAuthentication();
//��appsettings.json�е�JwtSettings�����ļ���ȡ��JwtSettings�У����Ǹ������ط��õ�
//asp�Ѿ���ȡ�˼���ָ��·�������ã�����Ҫ���ֶ���ȡ�����ļ�
////Ϊʲô��ʹ����Ҳ���Ի�ȡֵ����ΪGet<T>������bind���ܡ�
//services.Configure<JwtSettings>(configSection);
var configSection = appBuilder.Configuration.GetSection("JwtSettings");
var jwtSettings = configSection.Get<JwtSettings>();

//��������֤����
services.AddAuthentication(authenOptions =>
{
    //��֤middleware����
    authenOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    authenOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(bearerOptions =>
{
    //jwt token��������
    bearerOptions.TokenValidationParameters = new TokenValidationParameters
    {
        NameClaimType = JwtClaimTypes.Name,
        RoleClaimType = JwtClaimTypes.Role,
        //Token�䷢����*
        ValidIssuer = jwtSettings.Issuer,
        //�䷢��˭*
        ValidAudience = jwtSettings.Audience,
        //ǩ����Կ�������keyҪ���м���*
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),

        /***********************************TokenValidationParameters�Ĳ���Ĭ��ֵ***********************************/
        // RequireSignedTokens = true,
        // SaveSigninToken = false,
        // ValidateActor = false,
        // ������������������Ϊfalse�����Բ���֤Issuer��Audience�����ǲ�������������
        // ValidateAudience = true,
        // ValidateIssuer = true, 
        // ValidateIssuerSigningKey = false,
        // �Ƿ�Ҫ��Token��Claims�б������Expires
        // RequireExpirationTime = true,
        // ����ķ�����ʱ��ƫ����
        // ClockSkew = TimeSpan.FromSeconds(300),
        // �Ƿ���֤Token��Ч�ڣ�ʹ�õ�ǰʱ����Token��Claims�е�NotBefore��Expires�Ա�
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

//������������·�ɵ��м����ȷ�������·���ս��
//��Ҫ�����ǽ�·���м����ӵ�������ܵ��У���ȷ��������α�·�ɵ��ս��
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});