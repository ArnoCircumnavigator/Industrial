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

        [TestMethod("��˾PLC��д,�������������S7-1200ϵ�е�����")]
        public void TestMethod1()
        {
            Console.WriteLine("HslCommunicationͨ�Ų��Կ�ʼ");

            var client = new SiemensS7Net(SiemensPLCS.S1200, IP_ADDRESS);
            client.Rack = RACK;
            client.Slot = SLOT;
            client.ConnectTimeOut = 3000;//3�볬ʱ

            var operate = client.ConnectCloseAsync().Result;

            Assert.IsTrue(operate.IsSuccess);

            var sw = new Stopwatch();
            int totalCount = 100;
            if (operate.IsSuccess)
            {
                Console.WriteLine("HslCommunication���ӳɹ�");
                sw.Restart();

                for (int i = 0; i < totalCount; i++)
                {
                    OperateResult<byte[]>? opRead = client.Read("DB1.0", (ushort)2);
                    if (!opRead.IsSuccess)
                    {
                        Console.WriteLine($"HslCommunicationͨ�Ų��Զ�ȡʧ��{opRead.ErrorCode}");
                    }
                }
                Console.WriteLine($"HslCommunicationͨ�Ų��Խ���,���:����ƽ����ʱ{sw.ElapsedMilliseconds / (float)totalCount}ms/��");

                client.ConnectCloseAsync().Wait();
            }
            else
            {
                Console.WriteLine("HslCommunication����ʧ��");
            }
        }

        [TestMethod("��˾PLC��д,�������������S7ϵ�еĶ�д")]
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
             * PLC��������ֵ��=>��ߣ��ҵ�,Ҳ�Ǿ��׳Ƶĸ�λ��ǰ
             * ���Ǽ�����Ǹ�λ�ں�ģ�����Ҫ������ת����
             */
            Array.Reverse(opRead.Content);

            Int16 v1Read = BitConverter.ToInt16(opRead.Content);
            Console.WriteLine($"write:{v1Write}");
            Console.WriteLine($"read:{v1Read}");
            Assert.IsTrue(v1Read.Equals(v1Write));
        }
    }
}