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
        [TestMethod("00�������")]
        public void Test_AddNowObject()
        {
            DateTime dateTime = new DateTime(2021, 09, 10);

            #region ������ݿ⣨���棩��������4�����ݣ����棩
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
                    Name = "EFCore �ĵ�һ������"
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
            #region ������ݿ�
            using (var context = new BusinessDbContext(Options))
            {
                //Arrange
                context.Items.RemoveRange(context.Items);//ɾ��Item��ʱ�򣬻��NowMesɾ��
                context.Locations.RemoveRange(context.Locations);//ɾ��Location��ʱ�򣬻��Nowɾ��
                //context.NowMeses.RemoveRange(context.NowMeses);
                //context.Nows.RemoveRange(context.Nows);

                var item = context.Items.First();//��������Ĳ����������ó�����deleted״̬�Ķ���
                var loc = context.Locations.First();
                Assert.AreEqual(context.Entry(item).State, EntityState.Deleted);
                Assert.AreEqual(context.Entry(loc).State, EntityState.Deleted);
                var now = new Now()
                {
                    ContainerID = 1000,
                    EnterTime = dateTime,
                    Location = loc,//���loc�����״̬��ɾ��
                    NowMes = new NowMes()
                    {
                        Item = item,//���item��״̬��ɾ��
                        Qty = 100,
                    }
                };
                context.Add(now);
                Assert.AreEqual(context.Entry(now).State, EntityState.Added);
                var i = context.SaveChanges();
                //���ʱ��ִ������������ɾ��Item��ɾ��Locations��
                //���ڼ���ɾ����Ե�ʣ�ɾitem�����NowMesɾ����ɾLoc�����Nowɾ��
                //���յ��£����ݿ�����ʲô��û��ʣ��
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

        [TestMethod("01�Ǹ��ٶ���")]
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
                var loc = context.Locations.AsNoTracking().First();//�ó�һ�������ٵĶ���
                loc.Status = LocStatus.Disable;//�޸������
                context.SaveChanges();//���棬���ڱ���ʱ���ᱣ�����loc���κθĶ�
            }
            //Assert
            using (var context = new BusinessDbContext(Options))
            {
                var loc = context.Locations.First();
                Assert.AreEqual(loc.Status, LocStatus.Normal);
            }
        }
        [TestMethod("02��������")]
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
                    Name = "EFCore �ĵ�һ������"
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
                 * ���ݿ�֮�����ܹ��޸�������������Ϊ���ݿ������¼��һ��������Oracle��RowID�Ķ������޸�������ʱ���ܹ��޸ĳɹ�
                 * ��ORM��ֻ������ȥ��������ô�޸ġ��޸������ĸ��¡������ֲ���ORM���޷������ģ�
                 * ����
                 *  ���԰�ԭ�ж����Ƴ���Ȼ���ڼ��뼴��
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
                Assert.AreEqual(context.Entry(n.Location).State, EntityState.Unchanged);//����ɾNow������ɾLoc
                Assert.AreEqual(context.Entry(n.NowMes).State, EntityState.Deleted);//����ɾNow,��ɾNowMes
                context.Add(now);
                Assert.AreEqual(context.Entry(now).State, EntityState.Added);
                var i = context.SaveChanges();
                Assert.AreEqual(i, 0);
            }
        }
        [TestMethod("03�����������ɾ���ڲ��루ͬ����ֵ��")]
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
                    Name = "EFCore �ĵ�һ������"
                };

                context.Add(item);

                var ts = context.Items.ToList();
                Assert.AreEqual(ts.Count, 0);//���ʱ��ts�Ǵ����ݲ�����ģ���ʲô��û��

                context.SaveChanges();

                ts = context.Items.ToList();
                Assert.AreEqual(ts.Count, 1);//���ʱ��ts�Ǵ����ݲ�����ģ���һ����

                context.Remove(item);//ɾ��
                Assert.IsTrue(context.Entry(item).State == EntityState.Deleted);
                var newT = new Item()//�½�һ��һģһ����
                {
                    ItemID = item.ItemID,
                    Name = item.Name,
                };
                context.Add(newT);//����
                Assert.IsTrue(context.Entry(newT).State == EntityState.Added);
                var i = context.SaveChanges();
                Assert.AreEqual(i, 0);
            }
        }
    }
}
