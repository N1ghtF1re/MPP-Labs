using System;
using System.Threading;
using MPP6.logbuffer;

namespace MPP6
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var logBuffer = new LogBuffer(4, 10, "test.log"))
            {
                logBuffer.Add("Test string 1");
                logBuffer.Add("Test string 2");
                logBuffer.Add("Test string 3");
            }
        }
    }
}