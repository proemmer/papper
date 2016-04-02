using System;

namespace Papper.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public class ArrayBoundsAttribute : Attribute
    {
        public ArrayBoundsAttribute(int from, int to, int dimension = 0)
        {
            From = from;
            To = to;
            Dimension = dimension;
        }

        public int From { get; private set; }
        public int To { get; private set; }
        public int Dimension { get; private set; }
    }
}