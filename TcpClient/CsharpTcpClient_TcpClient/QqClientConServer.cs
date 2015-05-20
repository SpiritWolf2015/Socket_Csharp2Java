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
                Console.WriteLine("C#客户端启动，去连接PORT = 9999 的本机JAVA服务器");
                m_client = new TcpClient( );
                // 去连JAVA服务器
                m_client.Connect(new System.Net.IPEndPoint(IPAddress.Parse("127.0.0.1"), 9999));

                using (NetworkStream networkStream = m_client.GetStream( )) {
                    #region 往JAVA服务器发送ID，密码
                    // 用户数据，用 | 来拼接分隔，结尾用\r\n标识一行文本终止。
                    string strSendData = u.UserId + "|" + u.Passwd + "\r\n";
                    
                    byte[ ] data = System.Text.Encoding.UTF8.GetBytes(strSendData);
                    Console.WriteLine(string.Format("data.Length = {0} ", data.Length));
                    // 将字符串写到流中，发出
                    networkStream.Write(data, 0, data.Length);
                    networkStream.Flush( );
                    Console.WriteLine(string.Format("用户登录ID = {0} Passwd = {1}", u.UserId, u.Passwd));


                    #region 接受JAVA发来的数据判断，登录是否成功     

                    //接收数据
                    byte[ ] buffer = new byte[4];                  
                    // 读取返回的字节流
                     int recCount = networkStream.Read(buffer, 0, buffer.Length);
                    string returnMsg = System.Text.Encoding.UTF8.GetString(buffer, 0, recCount);
                    Console.WriteLine(string.Format("接受JAVA返回的字符串 = {0}", returnMsg));

                    if (MessageType.message_succeed.Equals(returnMsg)) {
                        // 登录成功
                        isLoginOk = true;
                    }               
                    #endregion 接受JAVA发来的数据判断，登录是否成功

                 
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
