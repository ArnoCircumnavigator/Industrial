using Google.Protobuf;

namespace Industrial.Infra.Protobuf.UnitTest
{
    [TestClass]
    public class UnitTestSerializeByProtoBuf
    {
        [TestMethod("序列化与反序列化")]
        public void TestMethod1()
        {
            /*
             * Arrange----------------------------------
             */
            SessionRequest Origin_request = new SessionRequest()
            {
                Uid = Guid.NewGuid().ToString(),
                OpTimestamp = DateTime.Now.ToBinary().ToString()
            };

            using (FileStream? output = File.Create("temp"))
            {
                Origin_request.WriteTo(output);
            }

            /*
             * Action----------------------------------
             */
            //通过反序列化创建出来的
            SessionRequest New_request;
            using (FileStream? input = File.OpenRead("temp"))
            {
                New_request = SessionRequest.Parser.ParseFrom(input);
            }


            /*
             * Assert----------------------------------
             */
            Assert.IsTrue(Origin_request.Uid.Equals(New_request.Uid));
            Assert.IsTrue(Origin_request.OpTimestamp.Equals(New_request.OpTimestamp));
        }
    }
}