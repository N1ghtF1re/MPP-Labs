using System;
using System.IO;
using Microsoft.Win32.SafeHandles;
using NUnit.Framework;

namespace ConsoleApplication1.oshandle.test
{
    public class OsHandleTest
    {
        [Test]
        public void test()
        {
            const string filePath = "a.txt";

            var osHandle = new OSHandle();
            var fs = File.Open(filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.None);

            osHandle.Handle = fs.Handle;
            
            osHandle.Dispose();

            try
            {
                fs.WriteByte(1);
                fs.Close();
                Assert.Fail();
            }
            catch (IOException)
            {
            }

            Assert.That(File.ReadAllBytes(filePath).Length, Is.EqualTo(0));
        } 
        
    }
}