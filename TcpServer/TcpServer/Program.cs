using System;
using System.Net;
using System.Net.Sockets;

namespace TcpServer {
    class Program {
        static void Main (string[ ] args) {
            // C#当服务器，接受JAVA发来的TCP连接，接受数据
            IPEndPoint ie = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 10000);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(ie);
            socket.Listen(10);

            // 消息长度
            const int MSG_LEN = 13;
            byte[ ] bytes = new byte[MSG_LEN];
            TcpServer2.ByteBuffer buf = TcpServer2.ByteBuffer.Allocate(bytes);

            for (int i = 0; i < 10; i++) {
                // 等待JAVA客户端来连接，阻塞方法。
                Socket client = socket.Accept( );
                // 接受数据
                int len = client.Receive(bytes);

                if (len >= 0) {
                    string receiveString = getStringMsg(bytes);                 //buf.ReadFloat().ToString( );
                    Console.WriteLine(receiveString);       //System.Text.Encoding.UTF8.GetString(bytes));     

                    //buf.Clear( );
                    buf.ResetReaderIndex( );
                    client.Close( );
                }
            }
            socket.Close( );
        }// end main

        static string getStringMsg (byte[ ] bytes) {
            string msg = System.Text.Encoding.UTF8.GetString(bytes);
            return msg;
        }

    }// end class Program

}// end namespace TcpServer

