using System;

namespace Papper.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class AliasNameAttribute : Attribute
    {
        public AliasNameAttribute(string name)
        {
            Name = name;
        }

        public string Name
        {
            get;
            private set;
        }

    }
}