using HslCommunication;
using HslCommunication.Profinet.Siemens;
using System.Diagnostics;

namespace Industrial.Infra.MachineryDriver.SiemensPLC.HslCommunication.UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        const string IP_ADDRESS = "192.168.1.11";
        const int RACK = 0;
        const int SLOT = 1;

        [TestMethod("公司PLC读写,测试针对西门子S7-1200系列的连接")]
        public void TestMethod1()
        {
            Console.WriteLine("HslCommunication通信测试开始");

            var client = new SiemensS7Net(SiemensPLCS.S1200, IP_ADDRESS);
            client.Rack = RACK;
            client.Slot = SLOT;
            client.ConnectTimeOut = 3000;//3秒超时

            var operate = client.ConnectCloseAsync().Result;

            Assert.IsTrue(operate.IsSuccess);

            var sw = new Stopwatch();
            int totalCount = 100;
            if (operate.IsSuccess)
            {
                Console.WriteLine("HslCommunication连接成功");
                sw.Restart();

                for (int i = 0; i < totalCount; i++)
                {
                    OperateResult<byte[]>? opRead = client.Read("DB1.0", (ushort)2);
                    if (!opRead.IsSuccess)
                    {
                        Console.WriteLine($"HslCommunication通信测试读取失败{opRead.ErrorCode}");
                    }
                }
                Console.WriteLine($"HslCommunication通信测试结束,结果:单次平均耗时{sw.ElapsedMilliseconds / (float)totalCount}ms/次");

                client.ConnectCloseAsync().Wait();
            }
            else
            {
                Console.WriteLine("HslCommunication连接失败");
            }
        }

        [TestMethod("公司PLC读写,测试针对西门子S7系列的读写")]
        public void TestMethod2()
        {
            var client = new SiemensS7Net(SiemensPLCS.S1200, IP_ADDRESS)
            {
                Rack = RACK,
                Slot = SLOT
            };
            var opConnet = client.ConnectServer();

            Assert.IsTrue(opConnet.IsSuccess);

            Int16 v1Write = (Int16)new Random().Next(1, 100);
            var opWrite = client.Write("DB1.0", v1Write);
            Assert.IsTrue(opWrite.IsSuccess);

            OperateResult<byte[]>? opRead = client.Read("DB1.0", 2);
            Assert.IsTrue(opRead.IsSuccess);

            /*
             * PLC读上来的值是=>左高，右低,也是就俗称的高位在前
             * 但是计算机是高位在后的，所以要把数组转过来
             */
            Array.Reverse(opRead.Content);

            Int16 v1Read = BitConverter.ToInt16(opRead.Content);
            Console.WriteLine($"write:{v1Write}");
            Console.WriteLine($"read:{v1Read}");
            Assert.IsTrue(v1Read.Equals(v1Write));
        }
    }
}