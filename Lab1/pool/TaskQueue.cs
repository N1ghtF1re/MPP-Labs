using System.Collections.Generic;
using System.Threading;
using NLog;

namespace ConsoleApp1.pool
{
    public class TaskQueue : ITreadPool
    {
        private readonly Queue<TaskDelegate> _tasksQueue = new Queue<TaskDelegate>();
        private readonly Thread[] _threads;
        private readonly object _monitorVariable = new object();
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private bool _isRun = true;
        
        
        /**
         * Create new tread poll with fixed treads count
         */
        public TaskQueue(int threadsCount)
        {
            NLogConfiguration.Сonfigure();
            _threads = new Thread[threadsCount];

            for (var i = 0; i < threadsCount; i++)
            {
                _threads[i] = new Thread(QueueConsumeTask) {Name = "Thread " + i};
                _threads[i].Start();
            }
        }

        /**
         * Add task to queue.
         */
        public virtual void EnqueueTask(TaskDelegate taskDelegate)
        {
            lock (_monitorVariable)
            {
                _tasksQueue.Enqueue(taskDelegate);
                Monitor.Pulse(_monitorVariable); // Notify slept tread that there is new task
            }
           
        }

        /**
         * Function which run in each tread. It check queue and proceed task
         */
        private void QueueConsumeTask()
        {
            while (_isRun)
            {
                TaskDelegate taskDelegate;
                bool isPulled;

                lock (_monitorVariable)
                {
                    if (_tasksQueue.Count == 0)
                    {
                        _logger.Info("Thread " + Thread.CurrentThread.Name + " in wait mode ");
                        Monitor.Wait(_monitorVariable);
                        _logger.Info("Thread awake " + Thread.CurrentThread.Name);
                    }
                    
                    isPulled = _tasksQueue.TryDequeue(out taskDelegate);
                }
                
                if (isPulled)
                {
                    _logger.Info("Executing task in " + Thread.CurrentThread.Name);
                    taskDelegate();
                }
                
            }
            
            _logger.Info(Thread.CurrentThread.Name+ " die.");
        }


        
        /**
         * Wait until all active tasks ends and kill all treads 
         */
        public void Dispose()
        {
            
            while (true)
            { 
                Thread.Sleep(2000);

                lock (_monitorVariable)
                {
                    if (_tasksQueue.Count == 0) break;
                }
            }
            
            lock (_monitorVariable)
            {
                _isRun = false;
                Monitor.PulseAll(_monitorVariable);
            }
        }
    }
}