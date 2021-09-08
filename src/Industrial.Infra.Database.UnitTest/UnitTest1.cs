using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;

namespace Industrial.Infra.Database.UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            //var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            //    .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Test")
            //    .Options;
            var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseMySQL(@"Server=127.0.0.1;Database=TestEFCore;uid=root;pwd=DBA001")
                .Options;
            using var context = new ApplicationDbContext(contextOptions);
        }
    }
}
