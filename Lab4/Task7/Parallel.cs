using System;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using MPP7.config;
using NLog;
using NLog.Fluent;

namespace MPP7
{
    
    public delegate void Task();

    public static class Parallel
    {
        private static readonly Logger Logger = NLogConfiguration.GetLogger("Parallel");
        public static void WaitAll(Task[] tasks)
        {
            var signal = new ManualResetEvent(false);
            var numberOfTasks = tasks.Length;

            
            for (var i = 0; i < tasks.Length; i++)
            {
                var index = i;
                ThreadPool.QueueUserWorkItem(_ =>
                {
                    Logger.Info("Executing task " + index);
                    tasks[index]();

                    MarkTaskExecuted(ref numberOfTasks, signal);
                });
            }

            Logger.Info("Waiting for " + tasks.Length + " tasks");

            signal.WaitOne();

            Logger.Info("Tasks executed");
        }
        private static void MarkTaskExecuted(ref int numberOfTasks,  EventWaitHandle signal) {
            if (Interlocked.Decrement(ref numberOfTasks) == 0)
            {
                signal.Set();
            }
        }
    }
    
  
}