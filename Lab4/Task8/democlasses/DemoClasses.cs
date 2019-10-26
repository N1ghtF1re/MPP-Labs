using System;
using System.Diagnostics;
using MPP8.attribute;

namespace MPP8.democlasses
{
    
    public class Test : System.Attribute  
    {
        public Test()
        {
            
        }
    }  
    
    [ExportClass]
    public class Demo1WithAttribute
    {
        public int Test1;
        public string Test2;
    }
    
    [ExportClass]
    public class Demo2WithAttribute
    {
        public int Test1;
        public string Test2;
    }
    
    public class Demo3WithoutAttribute
    {
        public int Test1;
        public string Test2;
    }

    [ExportClass]
    [Test]
    public class Demo4WithFewAttributes
    {
        public int Test1;
        public string Test2;
    }
    
    [Test]
    public class Demo5WithAnotherAttribute
    {
        public int Test1;
        public string Test2;
    }

    [ExportClass]
    public delegate void TestDelegate();

    [ExportClass]
    public enum TestEnum
    {
        TEST_E,
        TEST_B
    }

    [ExportClass]
    public interface TestInterface
    {
        void Test();
    }

    [ExportClass]
    public struct TestStruct
    {
        public int Test;
    }
}