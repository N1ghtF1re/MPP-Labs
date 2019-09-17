using System.Threading;
using ConsoleApplication1.config;
using NLog;

namespace ConsoleApplication1.mutex
{
    public class Mutex : IMutex
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        
        private const int SleepTime = 20;

        private int BusyByCurrentTread
        {
            get { return Thread.CurrentThread.ManagedThreadId; }
        }

        private const int Free = 0;
        
        /**
         * Variable which can contain 0 if mutex is free or THREAD_ID when mutex is busy.
         */
        private int _lockVariable;

        public Mutex()
        {
            NLogConfiguration.Сonfigure();
        }
        
        public void Lock()
        {
            while (Interlocked.CompareExchange(ref _lockVariable, 
                       BusyByCurrentTread, Free) != Free)
            {
                Thread.Sleep(SleepTime);
                _logger.Info("Tread " +  Thread.CurrentThread.Name + " is waiting when mutex will be unlocked");
            }
            
            _logger.Info("Mutex has been blocked by "+ Thread.CurrentThread.Name);
        }

        public void Unlock()
        {
            if (Interlocked.CompareExchange(ref _lockVariable, Free, 
                    BusyByCurrentTread) == BusyByCurrentTread)
            {
                _logger.Info("Mutex has been unlocked by "+ Thread.CurrentThread.Name);
            }
            else
            {
                _logger.Warn(Thread.CurrentThread.Name + " tried to unlock alien mutex");
            }
        }
    }
}