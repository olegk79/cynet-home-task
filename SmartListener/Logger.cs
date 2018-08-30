using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmartListener
{
    internal static class Logger
    {

        static ReaderWriterLock locker = new ReaderWriterLock();
        public static void Write(this string text)
        {
            try
            {
                locker.AcquireWriterLock(int.MaxValue);
                System.IO.File.AppendAllLines("traffic.txt", new[] { text });
            }
            finally
            {
                locker.ReleaseWriterLock();
            }
        }
    }
}
