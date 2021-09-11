using Industrial.Domain.EntityModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;

namespace Industrial.Infra.Database.UnitTest
{
    [TestClass]
    public class T001_EFCoreBase
    {
        static string CONNECTSTRING = @"Server=127.0.0.1;Database=TestEFCore;uid=root;pwd=DBM001";
        static DbContextOptions<BusinessDbContext> Options;
        static T001_EFCoreBase()
        {
            Options = new DbContextOptionsBuilder<BusinessDbContext>()
                .UseMySQL(CONNECTSTRING)
                .Options;
        }
        [TestMethod("00Item对象的操作")]
        public void Test_ItemObject()
        {
            
        }
    }
}
