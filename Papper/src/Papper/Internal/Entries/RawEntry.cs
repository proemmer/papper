using Papper.Types;
using System;
using System.Collections.Generic;

namespace Papper.Internal
{
    internal class RawEntry : Entry
    {
        // TODO:  Do not provide a null value
        public RawEntry(PlcDataMapper mapper, string from, int validationTimeInMs)
            : base(mapper, new DummyPlcObject(from) { Selector = @from }, validationTimeInMs)
        {
        }

        protected override bool AddObject(ITreeNode plcObj, IDictionary<string, OperationItem> plcObjects, IEnumerable<string> values)
            => PlcObjectResolver.AddRawPlcObjects(PlcObject, plcObjects, values);
    }
}
