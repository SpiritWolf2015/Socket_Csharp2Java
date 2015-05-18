using System;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace CsharpTcpClient_TcpClient {

    class QqClientConServer {

        private TcpClient m_client;

        // 发送第一次请求
        public bool sendLoginInfoToServer (User u) {
            bool isLoginOk = false;
            try {
                Console.WriteLine("C#客户端启动，去连接PORT = 9999 本机JAVA服务器");
                m_client = new TcpClient( );
                // 去连JAVA服务器
                m_client.Connect(new System.Net.IPEndPoint(IPAddress.Parse("127.0.0.1"), 9999));

                using (NetworkStream networkStream = m_client.GetStream( )) {
                    using (BinaryWriter binaryWriter = new BinaryWriter(networkStream, Encoding.UTF8)) {
                        // 用户数据，用 | 来拼接分隔
                        string strSendData = u.UserId + "|" + u.Passwd + "\r\n";
                        // 将字符串写到流中，发出
                        binaryWriter.Write(strSendData);
                        binaryWriter.Flush( );
                        Console.WriteLine(string.Format("用户登录ID = {0} Passwd = {1}", u.UserId, u.Passwd));

                        binaryWriter.Close( );
                        networkStream.Close( );                        
                    }
                }

            } catch (Exception e) {
                ExceptionHandle(e);
            } finally {
                if (null != m_client) {
                    try {
                        // 关闭Scoket
                        m_client.Close( );
                    } catch (Exception e) {
                        ExceptionHandle(e);
                    }
                }
            }

            return isLoginOk;
        }

        void ExceptionHandle (Exception e) {
            Console.WriteLine(e.Message);
            Console.WriteLine(e.Source);
        }


    }//end class


}//end namespace
