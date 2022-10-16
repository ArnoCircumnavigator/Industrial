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
             * ��Ӱ�����ݿ⣬������
             */
            /*
             * ����ļ��ڵĲ��ԣ���������ѧϰEFCore�����õģ�
             */

            //var connectString = $"Server={serverName};Database={dataBaseName};uid={userID};pwd={password}";
            //Options = new DbContextOptionsBuilder<EfCoreContext>()
            //    .UseMySQL(connectString)
            //    .Options;
        }

        [TestMethod("00�������")]
        public void Test_AddNowObject()
        {
            DateTime dateTime = new DateTime(2021, 09, 10);

            #region ������ݿ⣨���棩��������4�����ݣ����棩
            var context = new EfCoreContext(Options);
            //Arrange
            context.KgItems.RemoveRange(context.KgItems);
            context.KgLocations.RemoveRange(context.KgLocations);
            context.KgNowmes.RemoveRange(context.KgNowmes);
            context.KgNows.RemoveRange(context.KgNows);

            var item = new KgItem()
            {
                ItemId = 333,
                Name = "EFCore �ĵ�һ������"
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
            #region ������ݿ�
            //Arrange
            context.KgItems.RemoveRange(context.KgItems);//ɾ��Item��ʱ�򣬻��NowMesɾ��
            context.KgLocations.RemoveRange(context.KgLocations);//ɾ��Location��ʱ�򣬻��Nowɾ��
            //context.KgNowmes.RemoveRange(context.KgNowmes);
            //context.KgNows.RemoveRange(context.KgNows);

            var item1 = context.KgItems.FirstOrDefault();//��������Ĳ����������ó�����deleted״̬�Ķ���
            var loc1 = context.KgLocations.FirstOrDefault();
            Assert.AreEqual(context.Entry(item1).State, EntityState.Deleted);
            Assert.AreEqual(context.Entry(loc1).State, EntityState.Deleted);
            var now1 = new KgNow()
            {
                ContainerId = 1000,
                EnterTime = dateTime,
                Location = loc1,//���loc�����״̬��ɾ��
                KgNowme = new KgNowme()
                {
                    Item = item1,//���item��״̬��ɾ��
                    Qty = 100,
                }
            };
            context.Add(now1);
            Assert.AreEqual(context.Entry(now1).State, EntityState.Added);

            //���ʱ��ִ������������ɾ��Item��ɾ��KgLocations��
            //���ڼ���ɾ����Ե�ʣ�ɾitem�����NowMesɾ����ɾLoc�����Nowɾ��
            //���յ��£����ݿ�����ʲô��û��ʣ��
            #endregion

            //Assert
            var KgNows = context.KgNows.Include(n => n.KgNowme).ToList();
            Assert.AreEqual(context.KgNowmes.ToList().Count, 1);
            Assert.AreEqual(context.KgLocations.ToList().Count, 0);
            Assert.AreEqual(context.KgItems.ToList().Count, 0);
            Assert.AreEqual(KgNows.Count, 1);

            context.DisposeAsync();
        }

        [TestMethod("01�Ǹ��ٶ���")]
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
            var loc1 = context.KgLocations.AsNoTracking().First();//�ó�һ�������ٵĶ���
            loc.Status = LocStatus.disable;//�޸������
            //context.SaveChanges();//���棬���ڱ���ʱ���ᱣ�����loc���κθĶ�

            //Assert
            var loc2 = context.KgLocations.First();
            Assert.AreEqual(loc.Status, LocStatus.normal);
        }
        [TestMethod("02��������")]
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
                    Name = "EFCore �ĵ�һ������"
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
                 * ���ݿ�֮�����ܹ��޸�������������Ϊ���ݿ������¼��һ��������Oracle��RowID�Ķ������޸�������ʱ���ܹ��޸ĳɹ�
                 * ��ORM��ֻ������ȥ��������ô�޸ġ��޸������ĸ��¡������ֲ���ORM���޷������ģ�
                 * ����
                 *  ���԰�ԭ�ж����Ƴ���Ȼ���ڼ��뼴��
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
                Assert.AreEqual(context.Entry(n.Location).State, EntityState.Unchanged);//����ɾNow������ɾLoc
                Assert.AreEqual(context.Entry(n.KgNowme).State, EntityState.Deleted);//����ɾNow,��ɾNowMes
                context.Add(now);
                Assert.AreEqual(context.Entry(now).State, EntityState.Added);

                //һ���Ƴ�������һ�β���������Ӷ������ʵ�ָ�������
            }
        }
        [TestMethod("03�����������ɾ���ڲ��루ͬ����ֵ��")]
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
                    Name = "EFCore �ĵ�һ������"
                };

                context.Add(item);

                var ts = context.KgItems.ToList();
                Assert.AreEqual(ts.Count, 0);//���ʱ��ts�Ǵ����ݲ�����ģ���ʲô��û��

                ts = context.KgItems.ToList();
                Assert.AreEqual(ts.Count, 1);//���ʱ��ts�Ǵ����ݲ�����ģ���һ����

                context.Remove(item);//ɾ��
                Assert.IsTrue(context.Entry(item).State == EntityState.Deleted);
                var newT = new KgItem()//�½�һ��һģһ����
                {
                    ItemId = item.ItemId,
                    Name = item.Name,
                };
                context.Add(newT);//����
                Assert.IsTrue(context.Entry(newT).State == EntityState.Added);

                //����ȥ��ɾ��һ�Σ�����һ�Σ��ܵ����β��������������������������һ������EFC��Ϊ����һ�θ��²���
            }
        }
    }
}
