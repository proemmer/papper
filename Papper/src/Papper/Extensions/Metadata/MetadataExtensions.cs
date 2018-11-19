using Papper.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Papper.Extensions.Metadata
{
    public static class MetadataExtensions
    {
        /// <summary>
        /// Read metadata of plc blocks
        /// </summary>
        /// <param name="mappings">mapping name specified in the MappingAttribute</param>
        /// <returns>The determined metadata.</returns>
        public static Task<MetaDataResult[]> ReadMetaDataAsync(this PlcDataMapper papper, params string[] mappings) => papper.ReadMetaDataAsync(mappings as IEnumerable<string>);

        /// <summary>
        /// Read metadata of plc blocks
        /// </summary>
        /// <param name="mappings">mapping name specified in the MappingAttribute</param>
        /// <returns>The determined metadata.</returns>
        public static async Task<MetaDataResult[]> ReadMetaDataAsync(this PlcDataMapper papper, IEnumerable<string> mappings)
        {
            var results = new List<MetaDataPack>();
            foreach (var mapping in mappings)
            {

                if (papper.EntriesByName.TryGetValue(mapping, out IEntry entry))
                {
                    results.Add(new MetaDataPack
                    {
                        MappingName = entry.PlcObject.Name,
                        AbsoluteName = entry.PlcObject.Selector
                    });
                }
                else
                {
                    throw new KeyNotFoundException($"The mapping {mapping} does not exist.");
                }
            }

            await papper.ReadBlockInfos(results);

            return results.Select(x => new MetaDataResult(x.MetaData, x.ExecutionResult)).ToArray();

        }



        /// <summary>
        /// Return address data of the given variable
        /// </summary>
        /// <param name="mapping">name of the mapping</param>
        /// <param name="variable">name of the variable</param>
        /// <returns></returns>
        public static PlcItemAddress GetAddressOf(this PlcDataMapper papper, IPlcReference var)
        {
            var result = new Dictionary<string, object>();
            if (papper.EntriesByName.TryGetValue(var.Mapping, out IEntry entry))
            {
                if (entry.Variables.TryGetValue(var.Variable, out Tuple<int, Types.PlcObject> varibleEntry))
                {
                    return new PlcItemAddress(
                        varibleEntry.Item2.Selector,
                        varibleEntry.Item2.ElemenType,
                        varibleEntry.Item2.Offset,
                        varibleEntry.Item2.Size
                        );
                }
            }
            throw new KeyNotFoundException($"There is variable <{var.Variable}> for mapping <{var.Mapping}>");
        }


    }
}
