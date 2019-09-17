using System.Threading;
using NUnit.Framework;
using Mutex = ConsoleApplication1.mutex.Mutex;

namespace ConsoleApplication1.mutex.tests
{

    public class MutexTest
    {
        [Test]
        public void TestTwoThreadInSameTime()
        {
            const int threadsCount = 10;

            var mutex = new Mutex();
            var counter = 0;
            
            var arr = new bool[threadsCount];
            var threads = new Thread[threadsCount];

            var isInvalidValue = false;
            
            
            for (var i = 0; i < threadsCount; i++)
            {
                var currIndex = i;
                threads[i] = new Thread(o =>
                {
                    mutex.Lock();
                    counter++;
                    Thread.Sleep(50);
                    if (!isInvalidValue && counter != 1)
                    {
                        isInvalidValue = true;
                    }
                    Thread.Sleep(50);
                    counter--;
                    arr[currIndex] = true;
                    mutex.Unlock();
                }) {Name = "Thread " + i};
                threads[i].Start();
            }

            for (var i = 0; i < threadsCount; i++) 
                threads[i].Join();
            
            for (var i = 0; i < threadsCount; i++)
            {
                Assert.That(arr[i], Is.EqualTo(true));
            }
            
            Assert.That(isInvalidValue, Is.EqualTo(false));
        }

        [Test]
        public void TryToUnlockAlienMutex()
        {
            var mutex = new Mutex();
            
            var counter = 0;
            bool isInvalid = false;

            var thread1 = new Thread(o =>
            {
                mutex.Lock();
                counter++;
                Thread.Sleep(400);
                counter--;
                mutex.Unlock();
            }) { Name = "Thread 1" };
            
            thread1.Start();
            
            Thread.Sleep(100);
            mutex.Unlock();
            mutex.Lock();
            Assert.That(counter, Is.EqualTo(0));
            mutex.Unlock();
        }
    }
}