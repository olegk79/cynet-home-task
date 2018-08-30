using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SmartListener
{
    internal class MessageHandler
    {
        //private Socket socket;
        private const int BUFFER_SIZE = 16384;


        //private StringBuilder messageSB;



        internal string Handle(Socket socket)
        {
            //messageSB.Append(System.Text.Encoding.UTF8.GetString(buffer, 0, len));
            byte[] buffer = new byte[BUFFER_SIZE];
            StringBuilder sb = new StringBuilder();
            int bytesReceived = 0;
            do
            {
                bytesReceived = socket.Receive(buffer, BUFFER_SIZE, SocketFlags.None);



                sb.Append(System.Text.Encoding.UTF8.GetString(buffer, 0, bytesReceived));



            }
            while (bytesReceived == BUFFER_SIZE);

            socket.Close();

            //Logger.Write(string.Format("{0}\t{1}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss.fff"), sb.ToString()));

            return string.Format("{0}\t{1}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss.fff"), sb.ToString());





        }

    }
}
