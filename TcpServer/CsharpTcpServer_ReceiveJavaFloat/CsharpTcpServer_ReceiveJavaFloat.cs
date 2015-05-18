using System;
using System.Net;
using System.Net.Sockets;

namespace CsharpTcpServer_ReceiveJavaFloat {
    class CsharpTcpServer_ReceiveJavaFloat {
        static void Main (string[ ] args) {
            // C#当服务器，接受JAVA发来的TCP连接，接受数据
            IPEndPoint ie = new IPEndPoint(IPAddress.Any, 10000);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(ie);
            Console.WriteLine(string.Format("绑定IP = {0}, Port = {1}", ie.Address, ie.Port));
            socket.Listen(10);
            Console.WriteLine(string.Format("C#服务器开始监听"));

            // 消息长度
            const int MSG_LEN = 4;
            byte[ ] bytes = new byte[MSG_LEN];
            TcpServer2.ByteBuffer buf = TcpServer2.ByteBuffer.Allocate(bytes);

            for (int i = 0; i < 10; i++) {
                // 等待JAVA客户端来连接，阻塞方法。
                Socket client = socket.Accept( );
                Console.WriteLine(string.Format("第{0}个连接，{1}", i + 1, client.RemoteEndPoint.ToString( )));

                // 接受数据
                int len = client.Receive(bytes);
                if (len > 0) {
                    // 通过字节缓冲类，将字节数组转换为float
                    string receiveString = buf.ReadFloat( ).ToString( );
                    Console.WriteLine(string.Format("接受的Float数据 = {0}", receiveString));

                    buf.ResetReaderIndex( );
                    client.Close( );
                }
            }
            socket.Close( );

            Console.Read( );
        }// end main

        static string getStringMsg (byte[ ] bytes) {
            string msg = System.Text.Encoding.UTF8.GetString(bytes);
            return msg;
        }

    }//end class
}//end namespace
