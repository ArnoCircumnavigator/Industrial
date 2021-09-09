using Industrial.Infra.Database.BusinessEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Configuration;
using System.Linq;

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
        [TestMethod]
        public void Test_AddNowObject()
        {
            using (var context = new BusinessDbContext(Options))
            {
                var loc = new Location()
                {
                    LocationID = 200,
                    Status = LocStatus.Normal,
                    LoadStatus = LocLoadStatus.idle
                };
                //Item
                var item = new Item()
                {
                    ItemID = 333,
                    Name = "EFCore 的第一种物料"
                };
                context.Add(loc);
                context.Add(item);
                context.SaveChanges();
                //var nowmes = new NowMes();
                //nowmes.ContainerID = (uint)1000;
                //nowmes.ItemID = (ushort)333;
                //nowmes.Qty = (ushort)100;
                //nowmes.Item = item;
                //var now = new Now()
                //{
                //    ContainerID = (uint)1000,
                //    LocationID = (uint)200,
                //    EnterTime = System.DateTime.Now,
                //    Location = loc,
                //    NowMes = nowmes
                //};
                //nowmes.Now = now;
                //context.Nows.Add(now);
                //context.SaveChanges();
            }

            using (var context = new BusinessDbContext(Options))
            {
                var nows = context.Nows;
                Assert.IsTrue(nows.Count() == 1);
            }
        }
    }
}
