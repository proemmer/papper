using Papper.Internal;
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

        protected override bool AddObject(ITreeNode plcObj, Dictionary<string, Tuple<int, PlcObject>> plcObjects, IEnumerable<string> values)
            => PlcObjectResolver.AddRawPlcObjects(PlcObject, Variables, values);
    }
}
