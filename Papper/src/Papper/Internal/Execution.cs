using System;
using System.Collections.Generic;
using System.Linq;

namespace Papper.Internal
{
    /// <summary>
    /// Holds the execution operation
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
                if (PlcRawData.ReadDataCache.IsEmpty || !PlcRawData.ReadDataCache.Span.SequenceEqual(pack.Data.Span))
                {
                    if (pack.Timestamp > LastChange)
                    {
                        LastChange = pack.Timestamp; // We detected a change in this data area 
                    }
                }
                PlcRawData.ReadDataCache = pack.Data;

                if (pack.Timestamp > LastChange)
                {
                    PlcRawData.LastUpdate = pack.Timestamp;
                }
            }
            ExecutionResult = pack.ExecutionResult;
            return this;
        }

        /// <summary>
        /// After a write we can invalidate the data area, so the subscriber reads before the validation time is over
        /// </summary>
        public void Invalidate() => PlcRawData.LastUpdate = LastChange = DateTime.MinValue;

    }
}
