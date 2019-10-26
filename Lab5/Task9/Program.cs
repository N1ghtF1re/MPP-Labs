using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestPlatform.Utilities;

namespace MPP9
{
    class Program
    {
        private static void OutPut(DynamicList<string> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                Console.WriteLine("   " + list.Items[i]);
            }
        }
        
        static void Main()
        {
            var list = new DynamicList<string>();
            
            list.Add("Test");
            list.Add("Test2");
            list.Add("Test3");
            
            
            OutPut(list);
            
            list.RemoveAt(1);
            
            OutPut(list);
            
            list.Remove("Test");
            OutPut(list);
            
            list.Clear();
            OutPut(list);
        }
    }
}