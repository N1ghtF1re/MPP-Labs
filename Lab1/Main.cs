using System;
using System.IO;
using ConsoleApp1.pool;

namespace ConsoleApp1
{
    public static class Demo
    {
        private const int ThreadsCount = 10;

        public static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.Error.Write("You have to pass from and to paths");
                return;
            }

            var fromPath = args[0];
            var toPath = args[1];

            if (!Directory.Exists(fromPath))
            {
                Console.Error.Write("Folder doesn't exist");
                return;
            }
            
            var copyProvider = new FilesCopyProvider(new TaskQueue(ThreadsCount));
            copyProvider.Copy(fromPath, toPath);
        }
    }
}