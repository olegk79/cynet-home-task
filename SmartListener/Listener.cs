using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmartListener
{
    internal class Listener
    {

        private Queue<string> messagesQ; // queue of messages - processed by separate logger thread
        private object lockObj = new object(); // prevent locks

        internal void Listen()
        {
            try
            {
                IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
                Console.WriteLine("Starting TCP listener...");

                messagesQ = new Queue<string>();
                Thread writerThread = new Thread(MessagesWriter);
                writerThread.Start();

                TcpListener listener = new TcpListener(ipAddress, 7777);

                listener.Start();
                while (true)
                {

                    Socket socket = listener.AcceptSocket();
                    Console.WriteLine("client connected");
                    // start new message handler thread
                    ThreadPool.QueueUserWorkItem(MessageHandlerProc, socket);

                }


            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.StackTrace);
                Console.ReadLine();
            }


        }

        // messages logger thread
        private void MessagesWriter()
        {
            do
            {
                if (messagesQ.Count > 0)
                {
                    ThreadPool.QueueUserWorkItem(LoggerThread, DequeueMessage());
                    Thread.Sleep(10);

                }

            }
            while (true);
        }

        // single message logger thread
        private void LoggerThread(object obj)
        {
            Logger.Write((string)obj);
        }

        


        private void MessageHandlerProc(object obj)
        {
            Socket socket = (Socket)obj;
            MessageHandler mh = new MessageHandler();
            string message = mh.Handle(socket);
            EnqueueMessage(message);
        }

        // enqueue message to Q
        private void EnqueueMessage(string message)
        {
            lock (lockObj)
            {
                messagesQ.Enqueue(message);
            }
        }

        // dequeue message from Q
        private string DequeueMessage()
        {
            lock (lockObj)
            {
                return messagesQ.Dequeue();
            }
        }


    }
}
