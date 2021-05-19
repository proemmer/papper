using Papper.Extensions.Notification;
using Papper.Internal;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Papper.Access
{
    internal abstract class AccessEngine : IDisposable
    {
        protected List<Execution>? _executions;

        public abstract void Dispose();
        internal abstract Task<PlcReadResult[]> ReadAsync(IEnumerable<PlcReadReference> vars, bool doNotConvert);
        internal abstract Task<PlcReadResult[]> ReadExecutionsAsync(List<Execution> executions,
                                                                    bool onlyOutDated = true,
                                                                    DateTime? changedAfter = null,
                                                                    Func<IEnumerable<string>, DateTime, bool>? forceUpdate = null,
                                                                    Func<IEnumerable<KeyValuePair<string, PlcObjectBinding>>, IEnumerable<KeyValuePair<string, PlcObjectBinding>>>? filter = null,
                                                                    bool doNotConvert = false);
        internal abstract PlcReadResult[] CreatePlcReadResults(IEnumerable<Execution> executions,
                                                             Dictionary<Execution, DataPack> needUpdate,
                                                             DateTime? changedAfter = null,
                                                             Func<IEnumerable<KeyValuePair<string, PlcObjectBinding>>, IEnumerable<KeyValuePair<string, PlcObjectBinding>>>? filter = null,
                                                             bool doNotConvert = false);
        internal abstract PlcReadResult[] ReadVariablesFromExecutionCache(IEnumerable<string> variables, List<Execution>? executions);

        internal abstract PlcReadResult[] CreatePlcReadResults(IEnumerable<Execution> executions, IEnumerable<DataPack> packs);
        internal abstract Task<PlcWriteResult[]> WriteAsync(IEnumerable<PlcWriteReference> vars);
        internal abstract List<Execution> DetermineExecutions<T>(IEnumerable<T> vars) where T : IPlcReference;
        internal abstract Dictionary<Execution, DataPack> UpdateableItems(List<Execution> executions, bool onlyOutdated, Func<IEnumerable<string>, DateTime, bool>? forceUpdate = null);

        internal abstract bool GetOrAddMapping(string mapping, out IEntry entry);
    }
}
