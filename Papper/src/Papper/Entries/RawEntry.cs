using Papper.Common;
using Papper.Interfaces;
using Papper.Types;
using System;
using System.Collections.Generic;

namespace Papper.Entries
{
    internal class RawEntry : Entry
    {

        public RawEntry(PlcDataMapper mapper, string from, int readDataBlockSize, int validationTimeInMs)
            : base(mapper, new PlcObjectRef(from, null) { Selector = @from }, readDataBlockSize, validationTimeInMs)
        {
        }

        protected override bool AddObject(ITreeNode plcObj, Dictionary<string, Tuple<int, PlcObject>> plcObjects, IEnumerable<string> values)
        {
            return PlcObjectResolver.AddRawPlcObjects(PlcObject, Variables, values);
        }
    }
}
