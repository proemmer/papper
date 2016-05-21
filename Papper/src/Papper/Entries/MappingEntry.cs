using Papper.Attributes;
using Papper.Common;
using Papper.Interfaces;
using Papper.Types;
using System;
using System.Collections.Generic;

namespace Papper.Entries
{

    internal class MappingEntry : Entry
    {
        public MappingAttribute Mapping { get; private set; }
        public Type Type { get; private set; }

        public MappingEntry(PlcDataMapper mapper, MappingAttribute mapping, Type type, PlcMetaDataTree tree, int readDataBlockSize, int validationTimeInMs)
            : base(mapper, PlcObjectResolver.GetMapping(mapping?.Name, tree, type), tree, readDataBlockSize, validationTimeInMs)
        {
            if (mapping == null)
                throw new ArgumentNullException("mapping");
            if (type == null)
                throw new ArgumentNullException("type");

            Mapping = mapping;
            Type = type;
        }

        protected override bool AddObject(ITreeNode plcObj, Dictionary<string, Tuple<int, PlcObject>> plcObjects, IEnumerable<string> values)
        {
            return PlcObjectResolver.AddPlcObjects(PlcObject, Variables, values);
        }
    }

}
