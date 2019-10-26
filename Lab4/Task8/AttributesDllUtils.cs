using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MPP3;
using MPP8.attribute;

namespace MPP8
{
    public class AttributesDllUtils : DllUtils
    {
        public AttributesDllUtils(string dllPath) : base(dllPath)
        {
            
        }

        public IDictionary<string, List<string>> GetPublicTypesWithAttribute(Type attributeType)
        {
            if (attributeType.BaseType != typeof(Attribute))
            {
                throw new ArgumentException("Expected: Attribute. Got: " + attributeType.BaseType);
            }
            
            return GetTypes(type =>
            {
                var isHasAttribute = type.CustomAttributes.Any(data => data.AttributeType.FullName 
                                                                       == attributeType.FullName);
                
                return isHasAttribute && type.IsPublic;
            });
        }
    }
}