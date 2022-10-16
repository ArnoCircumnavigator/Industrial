using Industrial.Infra.Database.Models;
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
        static DbContextOptions<EfCoreContext> Options;
        static T002_InMemory()
        {
            Options = new DbContextOptionsBuilder<EfCoreContext>()
                .UseInMemoryDatabase(nameof(T001_EFCoreBase) + "InMemoryDatabase")
                .Options;
        }
        [TestMethod("内存数据库和实际Mysql之间的差距")]
        public void Test_Foo()
        {
            DateTime dateTime = new DateTime(2021, 09, 10);

            #region 清空数据库（保存），并加入4行数据（保存）
            using (var context = new EfCoreContext(Options))
            {
                //Arrange
                context.KgItems.RemoveRange(context.KgItems);
                context.KgLocations.RemoveRange(context.KgLocations);
                context.KgNowmes.RemoveRange(context.KgNowmes);
                context.KgNows.RemoveRange(context.KgNows);
                context.SaveChanges();

                var item = new KgItem()
                {
                    ItemId = 333,
                    Name = "EFCore 的第一种物料"
                };
                var loc = new KgLocation()
                {
                    LocationId = 200,
                    Status = LocStatus.normal,
                    LoadStatus = LoadStatus.idle
                };
                //Now
                var now = new KgNow()
                {
                    ContainerId = 1000,
                    EnterTime = dateTime,
                    Location = loc,
                    KgNowme = new KgNowme()
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
            using (var context = new EfCoreContext(Options))
            {
                //Arrange
                context.KgItems.RemoveRange(context.KgItems);//删除Item的时候，会把NowMes删掉
                context.KgLocations.RemoveRange(context.KgLocations);//删除Location的时候，会把Now删掉
                context.KgNowmes.RemoveRange(context.KgNowmes);
                context.KgNows.RemoveRange(context.KgNows);

                var item = context.KgItems.First();//由于上面的操作，这里拿出来的deleted状态的对象
                var loc = context.KgLocations.First();
                Assert.AreEqual(context.Entry(item).State, EntityState.Deleted);
                Assert.AreEqual(context.Entry(loc).State, EntityState.Deleted);
                var now = new KgNow()
                {
                    ContainerId = 1000,
                    EnterTime = dateTime,
                    Location = loc,//这个loc对象的状态是删除
                    KgNowme = new KgNowme()
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
            using (var context = new EfCoreContext(Options))
            {
                var KgNows = context.KgNows.Include(n => n.KgNowme).ToList();
                var meses = context.KgNowmes.ToList();
                Assert.AreEqual(KgNows.ToList().Count, 0);
                Assert.AreEqual(meses.ToList().Count, 0);
                Assert.AreEqual(context.KgLocations.ToList().Count(), 0);
                Assert.AreEqual(context.KgItems.ToList().Count(), 0);
            }
        }
    }
}
