﻿using System.Collections.Generic;

namespace Papper
{
    /// <summary>
    /// This class is used to define read operations.
    /// </summary>
    public struct PlcReadReference : IPlcReference
    {
        private readonly int _dot;


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
        public string Address { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"> [Mapping].[Variable]</param>
        /// <returns></returns>
        public static PlcReadReference FromAddress(string address) => new PlcReadReference(address);


        public PlcReadReference(string address)
        {
            Address = address;
            _dot = address.IndexOf(".");
        }


        /// <summary>
        /// Create a couple of <see cref="PlcReadReference"/> by a given variable root, and some sub variables of the root.
        /// This method can used if you will read more than one variable of the same data block.
        /// </summary>
        /// <param name="root">Rootpart of a variable</param>
        /// <param name="variables">variables</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="PlcReadReference"/></returns>
        public static IEnumerable<PlcReadReference> FromRoot(string root, params string[] variables) => FromRoot(root, variables as IEnumerable<string>);

        /// <summary>
        /// Create a couple of <see cref="PlcReadReference"/> by a given variable root, and some sub variables of the root.
        /// This method can used if you will read more than one variable of the same data block.
        /// </summary>
        /// <param name="root">Rootpart of a variable</param>
        /// <param name="variables">variables</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="PlcReadReference"/></returns>
        public static IEnumerable<PlcReadReference> FromRoot(string root, IEnumerable<string> variables)
        {
            foreach (var variable in variables)
            {
                yield return FromAddress($"{root}.{variable}");
            }
        }
    }
}