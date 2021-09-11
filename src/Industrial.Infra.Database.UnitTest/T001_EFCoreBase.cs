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
        [TestMethod("00添加数据")]
        public void Test_AddNowObject()
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
                //context.NowMeses.RemoveRange(context.NowMeses);
                //context.Nows.RemoveRange(context.Nows);

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
                var i = context.SaveChanges();
                //这个时候，执行两个操作，删掉Item表，删掉Locations表
                //由于级联删除的缘故，删item，会把NowMes删掉，删Loc，会把Now删掉
                //最终导致，数据库里面什么都没有剩下
                Assert.AreEqual(i, 2);
            }
            #endregion
            //Assert
            using (var context = new BusinessDbContext(Options))
            {
                var nows = context.Nows.Include(n => n.NowMes).ToList();
                Assert.AreEqual(nows.ToList().Count, 0);
                Assert.AreEqual(context.NowMeses.ToList().Count, 0);
                Assert.AreEqual(context.Locations.ToList().Count, 0);
                Assert.AreEqual(context.Items.ToList().Count, 0);
                Assert.AreEqual(nows.Count, 0);
            }
        }

        [TestMethod("01非跟踪对象")]
        public void Test_NotTracking()
        {
            //Arrange
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
                context.Add(loc);
                context.SaveChanges();
            }
            //Action
            using (var context = new BusinessDbContext(Options))
            {
                var loc = context.Locations.AsNoTracking().First();//拿出一个不跟踪的对象
                loc.Status = LocStatus.Disable;//修改这对象
                context.SaveChanges();//保存，现在保存时不会保存这个loc的任何改动
            }
            //Assert
            using (var context = new BusinessDbContext(Options))
            {
                var loc = context.Locations.First();
                Assert.AreEqual(loc.Status, LocStatus.Normal);
            }
        }
        [TestMethod("02级联更新")]
        public void Test_CascadeObject()
        {
            DateTime dateTime = new DateTime(2021, 09, 10);
            using (var context = new BusinessDbContext(Options))
            {
                context.NowMeses.RemoveRange(context.NowMeses);
                context.Nows.RemoveRange(context.Nows);
                context.Items.RemoveRange(context.Items);
                context.Locations.RemoveRange(context.Locations);
                context.SaveChanges();

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

                var now = new Now()
                {
                    ContainerID = 1000,
                    LocationID = 200,
                    EnterTime = dateTime,
                    Location = loc,
                    NowMes = new NowMes()
                    {
                        Item = item,
                        Qty = 100,
                    }
                };
                context.Add(now);
                context.SaveChanges();
            }
            using (var context = new BusinessDbContext(Options))
            {
                /*
                 * 数据库之所以能够修改主键，那是因为数据库里面记录着一种类似于Oracle中RowID的东西，修改主键的时候还能够修改成功
                 * 而ORM中只靠主键去索引，那么修改“修改主键的更新”，这种操作ORM是无法做到的，
                 * 综上
                 *  可以把原有对象移除，然后在加入即可
                 */
                //context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                Now n = context.Nows
                    .Include(n => n.NowMes)
                    .Include(n => n.Location)
                    .Include(n => n.NowMes.Item)
                    .First();
                context.Remove(n);
                Assert.AreEqual(context.Entry(n.NowMes).State, EntityState.Deleted);

                var now = new Now()
                {
                    ContainerID = 1000,
                    LocationID = 200,
                    EnterTime = dateTime,
                    Location = n.Location,
                    NowMes = new NowMes()
                    {
                        Item = n.NowMes.Item,
                        Qty = 100,
                    }
                };
                Assert.AreEqual(context.Entry(n.Location).State, EntityState.Unchanged);//由于删Now，不会删Loc
                Assert.AreEqual(context.Entry(n.NowMes).State, EntityState.Deleted);//由于删Now,会删NowMes
                context.Add(now);
                Assert.AreEqual(context.Entry(now).State, EntityState.Added);
                var i = context.SaveChanges();
                Assert.AreEqual(i, 0);
            }
        }
        [TestMethod("03单事务里面的删除在插入（同样的值）")]
        public void Test_03Foo()
        {
            using (var context = new BusinessDbContext(Options))
            {
                context.NowMeses.RemoveRange(context.NowMeses);
                context.Nows.RemoveRange(context.Nows);
                context.Items.RemoveRange(context.Items);
                context.Locations.RemoveRange(context.Locations);
                context.SaveChanges();

                var item = new Item()
                {
                    ItemID = 333,
                    Name = "EFCore 的第一种物料"
                };

                context.Add(item);

                var ts = context.Items.ToList();
                Assert.AreEqual(ts.Count, 0);//这个时候ts是从数据查出来的，还什么都没有

                context.SaveChanges();

                ts = context.Items.ToList();
                Assert.AreEqual(ts.Count, 1);//这个时候ts是从数据查出来的，有一个了

                context.Remove(item);//删掉
                Assert.IsTrue(context.Entry(item).State == EntityState.Deleted);
                var newT = new Item()//新建一个一模一样的
                {
                    ItemID = item.ItemID,
                    Name = item.Name,
                };
                context.Add(newT);//保存
                Assert.IsTrue(context.Entry(newT).State == EntityState.Added);
                var i = context.SaveChanges();
                Assert.AreEqual(i, 0);
            }
        }
    }
}
