using System;

namespace Papper.Attributes
{
    /// <summary>
    /// Because of arrays in plc's are defined with e fixed size, we created this attribute to add this behavior also for our mapping classes.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public sealed class ArrayBoundsAttribute : Attribute
    {
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="from">Defines the start of the array.</param>
        /// <param name="to"> Defines the end of the array.</param>
        /// <param name="dimension">Defines the dimension of which this array bound is applied for. If you have more than one dimension, you have to define more than one attribute.  Starting by 0.</param>
        public ArrayBoundsAttribute(int from, int to, int dimension = 0)
        {
            From = from;
            To = to;
            Dimension = dimension;
        }

        /// <summary>
        /// Defines the start of the array.
        /// </summary>
        public int From { get; private set; }

        /// <summary>
        /// Defines the end of the array.
        /// </summary>
        public int To { get; private set; }

        /// <summary>
        /// Defines the dimension of which this array bound is applied for. If you have more than one dimension, you have to define more than one attribute.
        /// Starting by 0.
        /// </summary>
        public int Dimension { get; private set; }
    }
}