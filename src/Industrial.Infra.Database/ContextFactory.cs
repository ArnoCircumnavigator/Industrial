using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Industrial.Infra.Database
{
    internal class ContextFactory : IDesignTimeDbContextFactory<BusinessDbContext>
    {
        public BusinessDbContext CreateDbContext(string[] args)
        {
            /*
             * Mysql
             */
            var optionsBuilder = new DbContextOptionsBuilder<BusinessDbContext>();
            optionsBuilder.UseMySQL(@"Server=127.0.0.1;Database=TestEFCore;uid=root;pwd=DBM001");
            return new BusinessDbContext(optionsBuilder.Options);

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
