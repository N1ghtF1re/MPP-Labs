using System;

namespace ConsoleApp1.pool
{
    public delegate void TaskDelegate();

    /**
     * Interface for tread polls. 
     */
    public interface ITreadPool : IDisposable
    {
        /**
         * Add new task in queue.
         */
        void EnqueueTask(TaskDelegate taskDelegate);
    }
}