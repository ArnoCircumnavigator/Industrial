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

        [TestMethod("DDD˼���е��������ͣ�ֵ����,����ͬ��")]
        public void TestMethod1()
        {
            /*
             * ��������
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
             * ֵ����
             */
            UniqueId id1 = new(Guid.NewGuid());
            UniqueId id2 = new(id1.Value);

            //���ճ���.net�������ͣ��������ǲ���ȵ�
            //����UniqueId��DDD��Ƶġ�ֵ���͡�
            Assert.IsTrue(id1 == id2);
        }

    }
}