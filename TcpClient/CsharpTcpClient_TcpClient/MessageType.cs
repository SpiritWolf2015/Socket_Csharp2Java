
namespace CsharpTcpClient_TcpClient {
    static class MessageType {
        public static readonly string message_succeed = "1";// 表明是登陆成功
        public static readonly string message_login_fail = "2";// 表明登录失败
        public static readonly string message_comm_mes = "3";// 普通信息包
        public static readonly string message_get_onLineFriend = "4";// 要求在线好友的包
        public static readonly string message_ret_onLineFriend = "5";// 返回在线好友的包
    }
}
