using System;
using System.Runtime.InteropServices;
using ConsoleApplication1.config;
using NLog;

namespace ConsoleApplication1.oshandle
{
    public class OSHandle : IDisposable
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public OSHandle()
        {
            NLogConfiguration.Сonfigure();
        }


        public IntPtr Handle { get; set; }
        
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern bool CloseHandle(IntPtr handle);

        public void Finalize()
        {
            if (Handle != IntPtr.Zero)
            {
                bool isClosed = CloseHandle(Handle);
                if (!isClosed)
                {
                    _logger.Error("Attempt to close handle which can't be closed'");
                    throw new Exception("This handle can't be closed'");
                }
                _logger.Info("Handle " + Handle.ToInt64() + " was closed");
            }
        }

        public void Dispose()
        {
            Finalize();
        }
        
    }
}