using Papper.Helper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Papper
{
    /// <summary>
    /// Holds the execution operaton
    /// </summary>
    internal class Execution
    {
        private DataPack _pack;
        private DateTime _changeDetected = DateTime.MaxValue;
        public PlcRawData PlcRawData { get; private set; }
        public IEnumerable<Partiton> Partitions { get; private set; }
        public Dictionary<string, PlcObjectBinding> Bindings { get; private set; }
        public int ValidationTimeMs { get; private set; }
        public ExecutionResult ExecutionResult { get; private set; }

        public DateTime LastChange => _changeDetected;


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
                if(_pack == null || _pack.Data == null || !_pack.Data.SequenceEqual(pack.Data))
                {
                    _changeDetected = DateTime.Now;  // TODO: Handle Bit
                }
                _pack = pack;
                PlcRawData.Data = pack.Data;
                PlcRawData.LastUpdate = DateTime.Now;
            }
            ExecutionResult = pack.ExecutionResult;
            return this;
        }

        public void Invalidate()
        {
            PlcRawData.LastUpdate = _changeDetected = DateTime.Now;

        }

    }
}
