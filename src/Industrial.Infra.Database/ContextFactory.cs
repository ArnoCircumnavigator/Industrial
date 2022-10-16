using Industrial.Infra.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Industrial.Infra.Database
{
    internal class ContextFactory : IDesignTimeDbContextFactory<EfCoreContext>
    {
        /*
         * 不同的数据库就百度找一下响应驱动的Nuget包即可
         */

        public EfCoreContext CreateDbContext(string[] args)
        {
            /*
             * Mysql
             */
            var optionsBuilder = new DbContextOptionsBuilder<EfCoreContext>();
            var serverName = "mysql-test1.mysql.database.chinacloudapi.cn";
            var dataBaseName = "TestEFCore";
            var userID = "DBA";
            var password = "Noproblem001";
            //optionsBuilder.UseMySQL($"Server={serverName};Database={dataBaseName};uid={userID};pwd={password}");
            var server = ServerVersion.Create(new Version(5, 7), Pomelo.EntityFrameworkCore.MySql.Infrastructure.ServerType.MySql);
            optionsBuilder.UseMySql($"Server={serverName};Database={dataBaseName};uid={userID};pwd={password}", server);
            return new EfCoreContext(optionsBuilder.Options);

            /*
             * sqlServer
             */
            //var optionsBuilder = new DbContextOptionsBuilder<BusinessDbContext>();
            //optionsBuilder.UseSqlServer(@"Server=127.0.0.1;Database=TestEFCore;uid=sa;pwd=DBM001");
            //return new BusinessDbContext(optionsBuilder.Options);

            /*
             * oracle的方式
             */
            //var optionsBuilder = new DbContextOptionsBuilder<BusinessDbContext>();
            //optionsBuilder.UseOracle(@"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=127.0.0.1)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=TESTEFCORE)));User Id=DBM;Password=DBM001;");
            //return new BusinessDbContext(optionsBuilder.Options);

        }
    }
}
