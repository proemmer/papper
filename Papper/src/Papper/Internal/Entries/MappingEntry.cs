using Papper.Attributes;
using Papper.Types;
using System;
using System.Collections.Generic;

namespace Papper.Internal
{

    internal class MappingEntry : Entry
    {
        public MappingAttribute Mapping { get; private set; }
        public Type Type { get; private set; }

        public MappingEntry(PlcDataMapper mapper, MappingAttribute mapping, Type type, PlcMetaDataTree tree, int readDataBlockSize, int validationTimeInMs)
            : base(mapper, PlcObjectResolver.GetMapping(mapping?.Name, tree, type), readDataBlockSize, validationTimeInMs)
        {
            Mapping = mapping ?? throw new ArgumentNullException("mapping");
            Type = type ?? throw new ArgumentNullException("type");
        }

        protected override bool AddObject(ITreeNode plcObj, Dictionary<string, Tuple<int, PlcObject>> plcObjects, IEnumerable<string> values)
        {
            return PlcObjectResolver.AddPlcObjects(PlcObject, plcObjects, values);
        }
    }

}
