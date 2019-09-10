using System.IO;
using System.Threading;
using ConsoleApp1.pool;
using NUnit.Framework;

namespace ConsoleApp1
{
    public class Test
    {
        [Test]
        public void testTreadPool()
        {
            var treadPool = new TaskQueue(5);

            const int expectedCount = 6;

            var actualCount = 0;

            for (var i = 0; i < expectedCount; i++)
            {
                treadPool.EnqueueTask(() => actualCount++);
            }
            
            Thread.Sleep(2000);
            Assert.That(actualCount, Is.EqualTo(expectedCount));
        }
    }
}