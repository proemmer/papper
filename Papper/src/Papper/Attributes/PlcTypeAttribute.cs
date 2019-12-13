using System;

namespace Papper.Attributes
{
    /// <summary>
    /// There are some data types which have more than one relation in the plc. So if you dnt like to use the default type, you can specify another one with this attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class PlcTypeAttribute : Attribute
    {
        public PlcTypeAttribute(string name) => Name = name;

        /// <summary>
        /// Plc name.
        /// </summary>
        public string Name { get; private set; }
    }
}
