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
            var optionsBuilder = new DbContextOptionsBuilder<BusinessDbContext>();
            optionsBuilder.UseMySQL(@"Server=127.0.0.1;Database=TestEFCore;uid=root;pwd=DBM001");
            return new BusinessDbContext(optionsBuilder.Options);
        }
    }
}
