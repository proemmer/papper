using Papper.Internal;
using Papper.Types;
using System.Collections.Generic;
using System.Linq;
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
            if (mappings != null && papper != null)
            {
                foreach (var mapping in mappings)
                {
                    if (papper.EntriesByName.TryGetValue(mapping, out var entry))
                    {
                        results.Add(new MetaDataPack
                        (
                            mappingName: entry.PlcObject.Name,
                            absoluteName: entry.PlcObject.Selector ?? string.Empty
                        ));
                    }
                    else
                    {
                        ExceptionThrowHelper.ThrowMappingNotFoundException(mapping);
                    }
                }

                await papper.ReadBlockInfos(results).ConfigureAwait(false);
            }
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
            if (papper != null && var != null && papper.EntriesByName.TryGetValue(var.Mapping, out var entry))
            {
                if (entry is Entry e)
                {
                    e.UpdateInternalState(new List<string>() { var.Variable });
                }

                if (entry.Variables.TryGetValue(var.Variable, out var varibleEntry))
                {
                    return new PlcItemAddress(
                        entry.PlcObject.Selector ?? string.Empty,
                        (varibleEntry.PlcObject is PlcArray arr ? arr.ElemenType?.MakeArrayType() : varibleEntry.PlcObject.ElemenType ?? varibleEntry.PlcObject.DotNetType) ?? typeof(object),
                        new PlcSize { Bytes = varibleEntry.Offset + varibleEntry.PlcObject.ByteOffset, Bits = varibleEntry.PlcObject.BitOffset },
                        varibleEntry.PlcObject.Size ?? new PlcSize()
                        );
                }
            }
            ExceptionThrowHelper.ThrowInvalidVariableException($"{var?.Mapping}.{var?.Variable}");
            return default;
        }


    }
}
