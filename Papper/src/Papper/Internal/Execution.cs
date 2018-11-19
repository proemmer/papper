using System;
using System.Collections.Generic;
using System.Linq;

namespace Papper.Internal
{
    /// <summary>
    /// Holds the execution operaton
    /// </summary>
    internal class Execution
    {
        public PlcRawData PlcRawData { get; private set; }
        public IEnumerable<Partiton> Partitions { get; private set; }
        public Dictionary<string, PlcObjectBinding> Bindings { get; private set; }
        public int ValidationTimeMs { get; private set; }
        public ExecutionResult ExecutionResult { get; private set; }

        public DateTime LastChange { get; private set; } = DateTime.MaxValue;

        public Execution(PlcRawData plcRawData, Dictionary<string, PlcObjectBinding> bindings, int validationTimeMS)
        {
            ValidationTimeMs = validationTimeMS;
            PlcRawData = plcRawData;
            Bindings = bindings;
            Partitions = plcRawData.GetPartitonsByOffset(bindings.Values.Select(x => new Tuple<int, int>(x.Offset, x.Size)));
        }


        public Execution ApplyDataPack(DataPack pack)
        {
            if (pack.ExecutionResult == ExecutionResult.Ok)
            {
                if(PlcRawData.ReadDataCache.IsEmpty || !PlcRawData.ReadDataCache.Span.SequenceEqual(pack.Data.Span))
                {
                    LastChange = DateTime.Now; // We detected a change in this data area -> bindungs have to thes the position by themselves.
                }
                PlcRawData.ReadDataCache = pack.Data;
                PlcRawData.LastUpdate = DateTime.Now;
            }
            ExecutionResult = pack.ExecutionResult;
            return this;
        }

        /// <summary>
        /// After a write we can invalidate the data area, so the subscriber reads befor the validation time is over
        /// </summary>
        public void Invalidate()
        {
            PlcRawData.LastUpdate = LastChange = DateTime.MinValue;
        }

    }
}
