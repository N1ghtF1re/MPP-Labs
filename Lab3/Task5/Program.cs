using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MPP3
{
    class Program
    {
        static void Main(string[] args)
        {
            // /home/alex/RiderProjects/Solution2/Lab1/bin/Debug/netcoreapp2.2/ConsoleApp1.dll
            
            var dllPath = Console.ReadLine();
            
            var dllUtils = new DllUtils(dllPath);

            var map = dllUtils.GetPublicTypes();
            
            foreach (var (namespaceName, publicTypes) in map)
            {
                if (publicTypes.Count() == 0)
                    continue;
                
                Console.WriteLine("NAMESPACE: " + namespaceName);
                foreach (var publicType in publicTypes)
                {  
                   Console.WriteLine("   " + publicType); 
                }
            }
        }
    }
}