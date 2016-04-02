using System;

namespace Papper.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class PlcTypeAttribute :  Attribute
    {
        public PlcTypeAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}
