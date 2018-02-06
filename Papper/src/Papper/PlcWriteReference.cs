using System.Collections.Generic;

namespace Papper
{
    /// <summary>
    /// This class is used to define write operations.
    /// </summary>
    public struct PlcWriteReference : IPlcReference
    {
        /// <summary>
        /// Mapping part of the address
        /// </summary>
        public string Mapping { get; internal set; }

        /// <summary>
        /// Variable part of the address
        /// </summary>
        public string Variable { get; internal set; }

        /// <summary>
        /// Value to write
        /// </summary>
        public object Value { get; private set; }

        /// <summary>
        /// Create <see cref="PlcReadReference"/> from an address and a value
        /// </summary>
        /// <param name="address"> [Mapping].[Variable]</param>
        /// <param name="value">Value to write</param>
        /// <returns>An instance of a <see cref="PlcWriteReference"/></returns>
        public static PlcWriteReference FromAddress(string address, object value)
        {
            var firstDot = address.IndexOf('.');
            var area = address.Substring(0, firstDot);
            return new PlcWriteReference
            {
                Mapping = area,
                Variable = address.Substring(firstDot + 1),
                Value = value
            };
        }

        /// <summary>
        /// Create a couple of <see cref="PlcWriteReference"/> by a given variable root, and some sub variables of the root, with the value to write.
        /// This method can used if you will write more than one variable of the same data block.
        /// </summary>
        /// <param name="root">Rootpart of a variable</param>
        /// <param name="variables"><see cref="KeyValuePair{TKey, TValue}"/> of variable and value</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="PlcWriteReference"/></returns>
        public static IEnumerable<PlcWriteReference> FromRoot(string root, params KeyValuePair<string, object>[] variables) 
            => FromRoot(root, variables as IEnumerable<KeyValuePair<string, object>>);

        /// <summary>
        /// Create a couple of <see cref="PlcWriteReference"/> by a given variable root, and some sub variables of the root, with the value to write.
        /// This method can used if you will write more than one variable of the same data block.
        /// </summary>
        /// <param name="root">Rootpart of a variable</param>
        /// <param name="variables"><see cref="IEnumerable{KeyValuePair{TKey, TValue}}"/> of variable and value</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="PlcWriteReference"/></returns>
        public static IEnumerable<PlcWriteReference> FromRoot(string root, IEnumerable<KeyValuePair<string,object>> variables)
        {
            foreach (var variable in variables)
            {
                yield return FromAddress($"{root}.{variable.Key}", variable.Value);
            }
        }


    }
}