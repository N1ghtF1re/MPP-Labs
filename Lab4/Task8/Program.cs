using System;
using System.Collections.Generic;
using System.Linq;
using MPP8.attribute;

namespace MPP8
{
    class Program
    {
        static void Main(string[] args)
        {
            // /home/alex/RiderProjects/MPP3/MPP8/obj/Debug/netcoreapp2.2/MPP8.dll
            
            var dllPath = Console.ReadLine();
            
            var dllUtils = new AttributesDllUtils(dllPath);

            var map = dllUtils.GetPublicTypesWithAttribute(typeof(ExportClass));
            
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