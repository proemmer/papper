using System.Collections.Generic;

namespace Papper
{
    /// <summary>
    /// This class is used to define write operations.
    /// </summary>
    public struct PlcWriteReference : IPlcReference
    {
        private string _address;
        private int _dot;


        /// mapping part of the address
        /// </summary>
        public string Mapping => Address.Substring(0, _dot);

        /// <summary>
        /// variable part of the address.
        /// </summary>
        public string Variable => Address.Substring(_dot + 1);


        /// <summary>
        /// Full address is composed of mapping and variable
        /// </summary>
        public string Address => _address;

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
            return new PlcWriteReference(address, value);
        }

        public PlcWriteReference(string address, object value)
        {
            _address = address;
            _dot = address.IndexOf(".");
            Value = value;
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