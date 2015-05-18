using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpTcpClient_TcpClient {

    class QqClientLogin {
        public QqClientLogin ( ) {
            actionPerformed( );
        }

        public void actionPerformed ( ) {
            User u = new User( );
            u.UserId = "1";
            u.Passwd = "123456";

            QqClientUser qqClientUser = new QqClientUser( );
            // 登录成功
            if (qqClientUser.checkUser(u)) {
                Console.WriteLine("登录成功");
            } else {
                Console.WriteLine("登录失败");
            }
        }

    }//end class

}//end namespace

