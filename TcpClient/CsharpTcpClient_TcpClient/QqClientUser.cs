using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpTcpClient_TcpClient {
    class QqClientUser {

        public bool checkUser (User u) {
            return new QqClientConServer( ).sendLoginInfoToServer(u);
        }

    }
}
