using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Moq;
using MPP6.logbuffer;
using NUnit.Framework;
using NUnit.Framework.Internal.Commands;

namespace MPP6.tests
{
    public class LogBufferTest
    {
        private Mock<FileStream> _fileMock;

        [SetUp]
        public void SetUp()
        {
            _fileMock = new Mock<FileStream>(new IntPtr(), FileAccess.Write);
            _fileMock.Setup(stream => stream.CanWrite).Returns(true);
        }

        private void VerifyThatThereWasNoWriteCall()
        {
            _fileMock.Verify(stream => stream.Write(
                    It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>()),
                Times.Never);
        }
        
        [Test]
        public void TestInterval()
        {
            var logBuffer = new LogBuffer(10, 1, _fileMock.Object);

            const string str = "Some string";
            
            logBuffer.Add(str);

            VerifyThatThereWasNoWriteCall();
            
            Thread.Sleep(1100);
            
            var data = new UTF8Encoding(true).GetBytes(str + "\n");

            
            _fileMock.Verify(stream => stream.Write(
                    data, 0, data.Length),
                Times.Once);
        }

        [Test]
        public void TestMaxBufferSize()
        {
            var logBuffer = new LogBuffer(2, 10000, _fileMock.Object);

            var list = new List<string> {"Test Str1", "Test Str2", "Test Str3"};

            logBuffer.Add(list[0]);
            VerifyThatThereWasNoWriteCall();
            logBuffer.Add(list[1]);
            VerifyThatThereWasNoWriteCall();
            logBuffer.Add(list[2]);
            
            var data = new UTF8Encoding(true).GetBytes(string.Join("\n", list) + "\n");

            _fileMock.Verify(stream => stream.Write(
                    data, 0, data.Length),
                Times.Once);
        }
        
    }
}