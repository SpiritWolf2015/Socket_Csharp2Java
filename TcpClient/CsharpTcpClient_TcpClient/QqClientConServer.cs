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
                    #region 往JAVA服务器发送ID，密码

                    using (BinaryWriter binaryWriter = new BinaryWriter(networkStream, Encoding.UTF8)) {
                        // 用户数据，用 | 来拼接分隔
                        string strSendData = u.UserId + "|" + u.Passwd +"\r\n";
                        // 将字符串写到流中，发出
                        binaryWriter.Write(strSendData);
                        binaryWriter.Flush( );
                        Console.WriteLine(string.Format("用户登录ID = {0} Passwd = {1}", u.UserId, u.Passwd));                       

                        #region 接受JAVA发来的数据判断，登录是否成功
                        using (MemoryStream memStream = new MemoryStream( )) {
                            
                            //接收数据
                            byte[ ] buffer = new byte[1024];
                            int recCount = 0;
                            // 接收返回的字节流
                            while ((recCount = networkStream.Read(buffer, 0, 1024)) > 0) {
                                Console.WriteLine("接收数据");
                                memStream.Write(buffer, 0, recCount);
                                Console.WriteLine(string.Format("recCount = {0}", recCount));
                            }
                            Encoding encoding = Encoding.GetEncoding("GBK");
                            string returnMsg = encoding.GetString(memStream.GetBuffer( ), 0, memStream.GetBuffer( ).Length);
                            Console.WriteLine(string.Format("接受JAVA返回的字符串 = {0}", returnMsg));

                            memStream.Close( );
                        }
                        #endregion 接受JAVA发来的数据判断，登录是否成功

                        binaryWriter.Close( );
                    }
                    #endregion 往JAVA服务器发送ID，密码

                    networkStream.Close( );  
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
            Console.WriteLine(e.StackTrace);
            Console.WriteLine(e.Message);
            Console.WriteLine(e.Source);
        }


    }//end class


}//end namespace
