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
                context.NowMeses.RemoveRange(context.NowMeses);
                context.Nows.RemoveRange(context.Nows);
                context.Items.RemoveRange(context.Items);
                context.Locations.RemoveRange(context.Locations);
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
                var now = new Now()
                {
                    ContainerID = 1000,
                    LocationID = 200,
                    EnterTime = System.DateTime.Now,
                    Location = loc,
                    NowMes = new NowMes()
                    {
                        ItemID = 333,
                        Qty = 100,
                    }
                };

                context.Nows.Add(now);
                context.SaveChanges();
            }

            using (var context = new BusinessDbContext(Options))
            {
                var nows = context.Nows;
                
                Assert.IsTrue(nows.Count() == 1);
                Assert.IsTrue(nows.Include(n => n.NowMes).First().NowMes.Qty == 100);
                Assert.IsTrue(context.NowMeses.Include(n => n.Now).First().Now.LocationID == 200);
            }
        }

        [TestMethod("级联更新")]
        public void Test_CascadeObject()
        {
            using (var context = new BusinessDbContext(Options))
            {
                context.NowMeses.RemoveRange(context.NowMeses);
                context.Nows.RemoveRange(context.Nows);
                context.Items.RemoveRange(context.Items);
                context.Locations.RemoveRange(context.Locations);
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
                var now = new Now()
                {
                    ContainerID = 1000,
                    LocationID = 200,
                    EnterTime = System.DateTime.Now,
                    Location = loc,
                    NowMes = new NowMes()
                    {
                        ItemID = 333,
                        Qty = 100,
                    }
                };
                context.Nows.Add(now);
                context.SaveChanges();
            }

            using (var context = new BusinessDbContext(Options))
            {
                var n = context.Nows.Include<Now, NowMes>(n => n.NowMes).First();
                n.ContainerID = 2000;
                n.NowMes.ContainerID = 2000;
                context.Update(n);
                context.SaveChanges();

                //var n = context.Nows.Include<Now, NowMes>(n => n.NowMes).First();
                //n.ContainerID = 2000;
                //context.Update(n);
                //context.SaveChanges();

                //Assert.IsTrue(nows.Count() == 1);
                //Assert.IsTrue(nows.Include(n => n.NowMes).First().NowMes.Qty == 100);
                //Assert.IsTrue(context.NowMeses.Include(n => n.Now).First().Now.LocationID == 200);
            }
        }
    }
}
