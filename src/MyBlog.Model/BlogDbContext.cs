using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace MyBlog.Model
{
    public class BlogDbContext
    {
        public SimpleClient<BlogNews> BlogNews => new SimpleClient<BlogNews>(Client);
        public SimpleClient<TypeInfo> TypeInfos => new SimpleClient<TypeInfo>(Client);
        public SimpleClient<WriterInfo> SimpleClient => new SimpleClient<WriterInfo>(Client);

        public SqlSugarClient Client { get; set; }

        public BlogDbContext()
        {
            Client = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = "server=.;database=MyBlog;Trusted_Connection=True;TrustServerCertificate=true",
                DbType = DbType.SqlServer,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute
            }) ;

            Client.Aop.OnLogExecuting = (sql, pars) =>
            {
                Console.WriteLine(sql + "\r\n" + Client.Utilities.SerializeObject
                  (pars.ToDictionary(it => it.ParameterName, it => it.Value)));
                Console.WriteLine();
            };
        }

        public void CreateTable (bool backup = false,int stringDefaultLength= 50,params Type[] types)
        {
            Client.CodeFirst.SetStringDefaultLength(stringDefaultLength);
            Client.DbMaintenance.CreateDatabase();
            if (backup)
            {
                Client.CodeFirst.BackupTable().InitTables(types);
            }
            else
            {
                Client.CodeFirst.InitTables(types);
            }
        }
    }
}
