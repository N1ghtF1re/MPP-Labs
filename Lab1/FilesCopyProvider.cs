using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using ConsoleApp1.pool;
using NLog;

namespace ConsoleApp1
{
    public class FilesCopyProvider
    {
        private int _filesCount;
        private readonly ITreadPool _treadPool;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        
        public FilesCopyProvider(ITreadPool treadPool)
        {
            NLogConfiguration.Сonfigure();
            
            _treadPool = treadPool;
        }
        
        /**
         * Copy all files from pathFrom to pathTo using Tread Pool
         */
        public void Copy(string pathFrom, string pathTo)
        {
            using (_treadPool)
            {
                GetFilesInFolder(pathFrom)
                    .Where(File.Exists) // Skip directories
                    .ToList()
                    .ForEach(src =>
                    {
                        var dest = src.Replace(pathFrom, pathTo);
                        CopyOneFile(src, dest);
                    });
            }
            
            Console.WriteLine("Copied files: " + _filesCount);
            
        }

        /**
         * Check if file's directory exist. And if not exist, create it 
         */
        private void CreateDirectoryIfNotExist(string path)
        {
            var dirPath = Path.GetDirectoryName(path);
            if (!Directory.Exists(dirPath))
            {
                try
                {
                    Directory.CreateDirectory(dirPath);
                }
                catch (IOException e)
                {
                    _logger.Error(e, "Directory creation error: ");
                }
                catch (UnauthorizedAccessException e)
                {
                    _logger.Warn("Can't get access to file creation " + path);
                }
            }

        }

        /**
         * Get all files in folder (with inner folders and files)
         */
        private IEnumerable<string> GetFilesInFolder(string folder)
        {
            var files = new List<string>();

            try
            {
                files.AddRange(Directory.GetFiles(folder, "*", SearchOption.TopDirectoryOnly));
                foreach (var directory in Directory.GetDirectories(folder))
                    files.AddRange(GetFilesInFolder(directory));
            }
            catch (UnauthorizedAccessException)
            {
                _logger.Warn("Can't get access to folder " + folder);
            }

            return files;
        }
        
       

        /**
         * Copy file from src to dest in particular tread in tread poll 
         */
        private void CopyOneFile(string src, string dest)
        {
            CreateDirectoryIfNotExist(dest);
            _treadPool.EnqueueTask(() =>
            {
                try
                {
                    File.Copy(src, dest, true);
                    Interlocked.Increment(ref _filesCount);
                    _logger.Info("File copied from " + src + " to " + dest);
                }
                catch (IOException e)
                {
                    _logger.Error(e, "File copying error: ");
                }
                catch (UnauthorizedAccessException e)
                {
                    _logger.Warn("Can't get access to " + dest);
                }
            });
        }

    }
}