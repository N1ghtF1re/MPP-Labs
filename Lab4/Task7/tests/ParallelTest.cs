using System.Threading;
using NUnit.Framework;

namespace MPP7.tests
{
    [TestFixture]
    public class ParallelTest
    {
        [Test]
        public void Test()
        {
            var counter = 0;
            
            Parallel.WaitAll(new Task[]
            {
                () =>
                {
                    counter++;
                },
                () =>
                {
                    Thread.Sleep(300);
                    counter++;
                },
                () =>
                {
                    Thread.Sleep(300);
                    counter++;
                    Thread.Sleep(100);
                }
            });
            
            Assert.That(counter, Is.EqualTo(3));
        }
        
        [Test]
        public void Test2()
        {
            const int expected = 100;
            var counter = 0;

            var arr = new Task[expected];
            
            for (int i = 0; i < expected; i++)
            {
                arr[i] = () => counter++;
            }
            
            Parallel.WaitAll(arr);
            
            Assert.That(counter, Is.EqualTo(expected));
        }
    }
}