using System;

namespace Papper.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class MetaTagAttribute : Attribute
    {
        public string Name { get; private set; }
        public object Value { get; private set; }
        public Type ValueType { get; private set; }

        public MetaTagAttribute(string name, object value)
        {
            Name = name;
            Value = value;
            ValueType = value.GetType();
        }
    }
}
