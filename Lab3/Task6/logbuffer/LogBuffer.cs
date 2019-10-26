using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace MPP6.logbuffer
{
    public class LogBuffer : IDisposable
    {
        private readonly int _maxBufferSize;
        private readonly FileStream _fileStream;

        private readonly List<string> _buffer = new List<string>();
        
        private Mutex _listMutex = new Mutex();
        private Mutex _fileMutex = new Mutex();

        private Timer _timer;
        
        public LogBuffer(int maxBufferSize, int interval, string filename)
        {
            _maxBufferSize = maxBufferSize;

            _fileStream = File.Open(filename, FileMode.Create, FileAccess.Write);
            
            StartTimer(interval);
        }

        public LogBuffer(int maxBufferSize, int interval, FileStream fileStream)
        {
            if (!fileStream.CanWrite)
            {
                throw new ArgumentException("Only FileStream with write access might be accepted");
            }
            
            _maxBufferSize = maxBufferSize;

            _fileStream = fileStream;

            StartTimer(interval);
        }

        public void Add(string item)
        {
            _listMutex.WaitOne();
            _buffer.Add(item);
            _listMutex.ReleaseMutex();

            if (_buffer.Count() > _maxBufferSize)
            {
                Flush();
            }
        }

        ~LogBuffer()
        {
           Dispose();
        }

        public void Dispose()
        {
            Flush();
            _timer.Dispose();
            _fileStream.Close();
        }

        private void StartTimer(int interval)
        {
            _timer = new Timer(
                e => Flush(),  
                null, 
                TimeSpan.FromSeconds(interval),
                TimeSpan.FromSeconds(interval));
        }

        private void Flush()
        {
            var bufferCopy = GetBufferCopyAsString();

            _fileMutex.WaitOne();
            
            WriteStringToFile(bufferCopy);

            _fileStream.Flush();
            
            _fileMutex.ReleaseMutex();
        }
        
        
        private string GetBufferCopyAsString()
        {
            _listMutex.WaitOne();
            
            var bufferCopy = _buffer.ToList();
            _buffer.Clear();
            
            _listMutex.ReleaseMutex();

            return string.Join("\n", bufferCopy.ToArray());
        }

        private void WriteStringToFile(string line)
        {
            var data = new UTF8Encoding(true).GetBytes(line + "\n");
            _fileStream.Write(data, 0, data.Length);
        }
    }
}