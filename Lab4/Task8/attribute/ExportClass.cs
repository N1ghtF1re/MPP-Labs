using System;

namespace MPP8.attribute
{
    [AttributeUsage(AttributeTargets.Class
        | AttributeTargets.Delegate
        | AttributeTargets.Enum
        | AttributeTargets.Struct
        | AttributeTargets.Interface)]
    public class ExportClass : Attribute
    {
        public ExportClass()
        { }
    } 
}