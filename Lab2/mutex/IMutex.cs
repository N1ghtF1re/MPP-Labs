namespace ConsoleApplication1.mutex
{
    /**
     * Mutex interface.
     */
    public interface IMutex
    {
        /**
         * Lock mutex.
         */
        void Lock();
        
        /**
         * Unlock mutex.
         */
        void Unlock();
    }
}