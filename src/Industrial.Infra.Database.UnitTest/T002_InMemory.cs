using Industrial.Infra.Database.BusinessEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Industrial.Infra.Database.UnitTest
{
    [TestClass]
    public class T002_InMemory
    {
        static DbContextOptions<BusinessDbContext> Options;
        static T002_InMemory()
        {
            Options = new DbContextOptionsBuilder<BusinessDbContext>()
                .UseInMemoryDatabase(nameof(T001_EFCoreBase) + "InMemoryDatabase")
                .Options;
        }
        [TestMethod("内存数据库和实际Mysql之间的差距")]
        public void Test_Foo()
        {
            DateTime dateTime = new DateTime(2021, 09, 10);

            #region 清空数据库（保存），并加入4行数据（保存）
            using (var context = new BusinessDbContext(Options))
            {
                //Arrange
                context.Items.RemoveRange(context.Items);
                context.Locations.RemoveRange(context.Locations);
                context.NowMeses.RemoveRange(context.NowMeses);
                context.Nows.RemoveRange(context.Nows);
                context.SaveChanges();

                var item = new Item()
                {
                    ItemID = 333,
                    Name = "EFCore 的第一种物料"
                };
                var loc = new Location()
                {
                    LocationID = 200,
                    Status = LocStatus.Normal,
                    LoadStatus = LocLoadStatus.idle
                };
                //Now
                var now = new Now()
                {
                    ContainerID = 1000,
                    EnterTime = dateTime,
                    Location = loc,
                    NowMes = new NowMes()
                    {
                        Item = item,
                        Qty = 100,
                    }
                };
                context.Add(now);
                var i = context.SaveChanges();
                Assert.AreEqual(i, 4);
            }
            #endregion
            #region 清空数据库
            using (var context = new BusinessDbContext(Options))
            {
                //Arrange
                context.Items.RemoveRange(context.Items);//删除Item的时候，会把NowMes删掉
                context.Locations.RemoveRange(context.Locations);//删除Location的时候，会把Now删掉
                context.NowMeses.RemoveRange(context.NowMeses);
                context.Nows.RemoveRange(context.Nows);

                var item = context.Items.First();//由于上面的操作，这里拿出来的deleted状态的对象
                var loc = context.Locations.First();
                Assert.AreEqual(context.Entry(item).State, EntityState.Deleted);
                Assert.AreEqual(context.Entry(loc).State, EntityState.Deleted);
                var now = new Now()
                {
                    ContainerID = 1000,
                    EnterTime = dateTime,
                    Location = loc,//这个loc对象的状态是删除
                    NowMes = new NowMes()
                    {
                        Item = item,//这个item的状态是删除
                        Qty = 100,
                    }
                };
                context.Add(now);
                Assert.AreEqual(context.Entry(now).State, EntityState.Added);
                context.SaveChanges();
            }
            #endregion
            //Assert
            using (var context = new BusinessDbContext(Options))
            {
                var nows = context.Nows.Include(n => n.NowMes).ToList();
                var meses = context.NowMeses.ToList();
                Assert.AreEqual(nows.Count, 0);
                Assert.AreEqual(meses.Count, 0);
                Assert.AreEqual(context.Locations.Count(), 0);
                Assert.AreEqual(context.Items.Count(), 0);
            }
        }
    }
}
