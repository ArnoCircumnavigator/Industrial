namespace Industrial.Infra.DDDesign.CoreFramework.UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        public class A : Entity<UniqueId>
        {
            public A()
                : this(new UniqueId())
            {

            }
            public A(UniqueId id) : base(id)
            {

            }
            public string name = string.Empty;
            public int d1 = 0;
        }

        [TestMethod("DDD思想中的引用类型，值类型,的相同性")]
        public void TestMethod1()
        {
            /*
             * 引用类型
             */
            A a1 = new()
            {
                name = "temp",
                d1 = 10
            };
            A a2 = new()
            {
                name = "temp",
                d1 = 10
            };
            Assert.IsTrue(!a1.Equals(a2));
            Assert.IsTrue(a1 != a2);

            /*
             * 值类型
             */
            UniqueId id1 = new(Guid.NewGuid());
            UniqueId id2 = new(id1.Value);

            //按照常规.net引用类型，这两个是不相等的
            //但是UniqueId是DDD设计的“值类型”
            Assert.IsTrue(id1 == id2);
        }

    }
}