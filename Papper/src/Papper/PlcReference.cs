using System.Collections.Generic;

namespace Papper
{
    public interface IPlcReference
    {
        /// <summary>
        /// The main mapping.
        /// DBName
        /// IB: Input Area
        /// FB: Flag Area
        /// QB: Output Area
        /// TM: Timer Area
        /// CT: Counter Area
        /// DB: DataBlock Area
        /// </summary>
        string Mapping { get;  }
        string Variable { get;  }


    }

    public struct PlcReadReference : IPlcReference
    {
        public string Mapping { get; internal set; }
        public string Variable { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"> [Mapping].[Variable]</param>
        /// <returns></returns>
        public static PlcReadReference FromAddress(string address)
        {
            var firstDot = address.IndexOf('.');
            var area = address.Substring(0, firstDot);
            return new PlcReadReference
            {
                Mapping = area,
                Variable = address.Substring(firstDot + 1)
            };
        }


        public static IEnumerable<PlcReadReference> FromRoot(string root, params string[] variables)
        {
            foreach (var variable in variables)
            {
                yield return FromAddress($"{root}.{variable}");
            }
        }
    }

    public struct PlcWriteReference : IPlcReference
    {
        public string Mapping { get; internal set; }
        public string Variable { get; internal set; }
        public object Value { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"> [Mapping].[Variable]</param>
        /// <returns></returns>
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

        public static IEnumerable<PlcWriteReference> FromRoot(string root, params KeyValuePair<string,object>[] variables)
        {
            foreach (var variable in variables)
            {
                yield return FromAddress($"{root}.{variable.Key}", variable.Value);
            }
        }


    }
}