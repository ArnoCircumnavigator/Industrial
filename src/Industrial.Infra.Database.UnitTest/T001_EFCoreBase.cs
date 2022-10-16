using Industrial.Infra.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Industrial.Infra.Database.UnitTest
{
    [TestClass]
    public class T001_EFCoreBase
    {
        static string serverName = "mysql-test1.mysql.database.chinacloudapi.cn";
        static string dataBaseName = "TestEFCore";
        static string userID = "DBA";
        static string password = "Noproblem001";

        static DbContextOptions<EfCoreContext> Options;
        static T001_EFCoreBase()
        {
            /*
             * 会影响数据库，请慎用
             */
            /*
             * 这个文件内的测试，是我用来学习EFCore特性用的，
             */

            //var connectString = $"Server={serverName};Database={dataBaseName};uid={userID};pwd={password}";
            //Options = new DbContextOptionsBuilder<EfCoreContext>()
            //    .UseMySQL(connectString)
            //    .Options;
        }

        [TestMethod("00添加数据")]
        public void Test_AddNowObject()
        {
            DateTime dateTime = new DateTime(2021, 09, 10);

            #region 清空数据库（保存），并加入4行数据（保存）
            var context = new EfCoreContext(Options);
            //Arrange
            context.KgItems.RemoveRange(context.KgItems);
            context.KgLocations.RemoveRange(context.KgLocations);
            context.KgNowmes.RemoveRange(context.KgNowmes);
            context.KgNows.RemoveRange(context.KgNows);

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
            #endregion
            #region 清空数据库
            //Arrange
            context.KgItems.RemoveRange(context.KgItems);//删除Item的时候，会把NowMes删掉
            context.KgLocations.RemoveRange(context.KgLocations);//删除Location的时候，会把Now删掉
            //context.KgNowmes.RemoveRange(context.KgNowmes);
            //context.KgNows.RemoveRange(context.KgNows);

            var item1 = context.KgItems.FirstOrDefault();//由于上面的操作，这里拿出来的deleted状态的对象
            var loc1 = context.KgLocations.FirstOrDefault();
            Assert.AreEqual(context.Entry(item1).State, EntityState.Deleted);
            Assert.AreEqual(context.Entry(loc1).State, EntityState.Deleted);
            var now1 = new KgNow()
            {
                ContainerId = 1000,
                EnterTime = dateTime,
                Location = loc1,//这个loc对象的状态是删除
                KgNowme = new KgNowme()
                {
                    Item = item1,//这个item的状态是删除
                    Qty = 100,
                }
            };
            context.Add(now1);
            Assert.AreEqual(context.Entry(now1).State, EntityState.Added);

            //这个时候，执行两个操作，删掉Item表，删掉KgLocations表
            //由于级联删除的缘故，删item，会把NowMes删掉，删Loc，会把Now删掉
            //最终导致，数据库里面什么都没有剩下
            #endregion

            //Assert
            var KgNows = context.KgNows.Include(n => n.KgNowme).ToList();
            Assert.AreEqual(context.KgNowmes.ToList().Count, 1);
            Assert.AreEqual(context.KgLocations.ToList().Count, 0);
            Assert.AreEqual(context.KgItems.ToList().Count, 0);
            Assert.AreEqual(KgNows.Count, 1);

            context.DisposeAsync();
        }

        [TestMethod("01非跟踪对象")]
        public void Test_NotTracking()
        {
            //Arrange
            var context = new EfCoreContext(Options);
            context.KgNowmes.RemoveRange(context.KgNowmes);
            context.KgNows.RemoveRange(context.KgNows);
            context.KgItems.RemoveRange(context.KgItems);
            context.KgLocations.RemoveRange(context.KgLocations);
            var loc = new KgLocation()
            {
                LocationId = 200,
                Status = LocStatus.normal,
                LoadStatus = LoadStatus.idle
            };
            context.Add(loc);

            //Action
            var loc1 = context.KgLocations.AsNoTracking().First();//拿出一个不跟踪的对象
            loc.Status = LocStatus.disable;//修改这对象
            //context.SaveChanges();//保存，现在保存时不会保存这个loc的任何改动

            //Assert
            var loc2 = context.KgLocations.First();
            Assert.AreEqual(loc.Status, LocStatus.normal);
        }
        [TestMethod("02级联更新")]
        public void Test_CascadeObject()
        {
            DateTime dateTime = new DateTime(2021, 09, 10);
            using (var context = new EfCoreContext(Options))
            {
                context.KgNowmes.RemoveRange(context.KgNowmes);
                context.KgNows.RemoveRange(context.KgNows);
                context.KgItems.RemoveRange(context.KgItems);
                context.KgLocations.RemoveRange(context.KgLocations);
                //context.SaveChanges();

                var loc = new KgLocation()
                {
                    LocationId = 200,
                    Status = LocStatus.normal,
                    LoadStatus = LoadStatus.idle
                };
                //Item
                var item = new KgItem()
                {
                    ItemId = 333,
                    Name = "EFCore 的第一种物料"
                };

                var now = new KgNow()
                {
                    ContainerId = 1000,
                    LocationId = 200,
                    EnterTime = dateTime,
                    Location = loc,
                    KgNowme = new KgNowme()
                    {
                        Item = item,
                        Qty = 100,
                    }
                };
                context.Add(now);
                //context.SaveChanges();
            }
            using (var context = new EfCoreContext(Options))
            {
                /*
                 * 数据库之所以能够修改主键，那是因为数据库里面记录着一种类似于Oracle中RowID的东西，修改主键的时候还能够修改成功
                 * 而ORM中只靠主键去索引，那么修改“修改主键的更新”，这种操作ORM是无法做到的，
                 * 综上
                 *  可以把原有对象移除，然后在加入即可
                 */
                //context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                KgNow n = context.KgNows
                    .Include(n => n.KgNowme)
                    .Include(n => n.Location)
                    .Include(n => n.KgNowme.Item)
                    .First();
                context.Remove(n);
                Assert.AreEqual(context.Entry(n.KgNowme).State, EntityState.Deleted);

                var now = new KgNow()
                {
                    ContainerId = 1000,
                    LocationId = 200,
                    EnterTime = dateTime,
                    Location = n.Location,
                    KgNowme = new KgNowme()
                    {
                        Item = n.KgNowme.Item,
                        Qty = 100,
                    }
                };
                Assert.AreEqual(context.Entry(n.Location).State, EntityState.Unchanged);//由于删Now，不会删Loc
                Assert.AreEqual(context.Entry(n.KgNowme).State, EntityState.Deleted);//由于删Now,会删NowMes
                context.Add(now);
                Assert.AreEqual(context.Entry(now).State, EntityState.Added);

                //一次移除操作，一次插入操作，从而变向的实现更新主键
            }
        }
        [TestMethod("03单事务里面的删除在插入（同样的值）")]
        public void Test_03Foo()
        {
            using (var context = new EfCoreContext(Options))
            {
                context.KgNowmes.RemoveRange(context.KgNowmes);
                context.KgNows.RemoveRange(context.KgNows);
                context.KgItems.RemoveRange(context.KgItems);
                context.KgLocations.RemoveRange(context.KgLocations);

                var item = new KgItem()
                {
                    ItemId = 333,
                    Name = "EFCore 的第一种物料"
                };

                context.Add(item);

                var ts = context.KgItems.ToList();
                Assert.AreEqual(ts.Count, 0);//这个时候ts是从数据查出来的，还什么都没有

                ts = context.KgItems.ToList();
                Assert.AreEqual(ts.Count, 1);//这个时候ts是从数据查出来的，有一个了

                context.Remove(item);//删掉
                Assert.IsTrue(context.Entry(item).State == EntityState.Deleted);
                var newT = new KgItem()//新建一个一模一样的
                {
                    ItemId = item.ItemId,
                    Name = item.Name,
                };
                context.Add(newT);//保存
                Assert.IsTrue(context.Entry(newT).State == EntityState.Added);

                //看上去，删除一次，加入一次，总的两次操作，但是由于两个对象的主键一样，则EFC视为这是一次更新操作
            }
        }
    }
}
