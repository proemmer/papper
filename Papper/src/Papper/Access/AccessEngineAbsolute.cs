using Papper.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Papper.PlcDataMapper;

namespace Papper.Access
{
    internal partial class AccessEngineAbsolute : AccessEngine
    {
        internal delegate bool GetMapping(string mapping, out IEntry entry, bool allowAdd = true);

        private ReadOperation? _readEventHandler;
        private WriteOperation? _writeEventHandler;
        private GetMapping? _getMapping;

        public AccessEngineAbsolute(ReadOperation? readEventHandler,
                                    WriteOperation? writeEventHandler, 
                                    GetMapping? getMapping)
        {
            _readEventHandler = readEventHandler;
            _writeEventHandler = writeEventHandler;
            _getMapping = getMapping;
        }


        internal override Dictionary<Execution, DataPack> UpdateableItems(List<Execution> executions, bool onlyOutdated, Func<IEnumerable<string>, DateTime, bool>? forceUpdate = null)
        {
            return executions.Where(exec => !onlyOutdated ||
                                            exec.ValidationTimeMs <= 0 ||
                                            exec.PlcRawData.LastUpdate.AddMilliseconds(exec.ValidationTimeMs) < DateTime.Now ||
                                            (forceUpdate != null && forceUpdate(exec.PlcRawData.References.Keys, exec.PlcRawData.LastUpdate)))
                             .Select(x => x.CreateReadDataPack())
                             .ToDictionary(x => x.Key, x => x.Value);
        }

        internal override List<Execution> DetermineExecutions<T>(IEnumerable<T> vars)
        {
            return vars.GroupBy(x => x.Mapping)
                                .Select((execution) => GetOrAddMapping(execution.Key, out var entry)
                                                                ? (execution, entry)
                                                                : (null, null))
                                .Where(x => x.execution != null)
                                .SelectMany(x => x.entry.GetOperations(x.execution.Select(exec => exec.Variable).ToList()))
                                .ToList();
        }

        internal override bool GetOrAddMapping(string mapping, out IEntry entry)
        {
            if(_getMapping == null)
            {
                entry = null!;
                return false;
            }
            var isAbsoluteMapping = false;

            // If the mapping is an absolute mapping we can create an entry
            switch (mapping)
            {
                case "IB":
                case "FB":
                case "QB":
                case "TM":
                case "CT":
                case var s when Regex.IsMatch(s, "^DB\\d+$"):
                    {
                        isAbsoluteMapping = true;
                    }
                    break;
            }

            return _getMapping.Invoke(mapping, out entry, isAbsoluteMapping);
        }

        

        public override void Dispose()
        {
            _readEventHandler = null;
            _writeEventHandler = null;
            _getMapping = null;
        }

    }
}
