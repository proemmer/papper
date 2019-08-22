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

        public MappingEntry(PlcDataMapper mapper, MappingAttribute mapping, Type type, PlcMetaDataTree tree, int validationTimeInMs)
            : base(mapper, PlcObjectResolver.GetMapping(mapping?.Name, tree, type), validationTimeInMs)
        {
            Mapping = mapping ?? ExceptionThrowHelper.ThrowArgumentNullException<MappingAttribute>(nameof(mapping));
            Type = type ?? ExceptionThrowHelper.ThrowArgumentNullException<Type>(nameof(type));
        }

        protected override bool AddObject(ITreeNode plcObj, Dictionary<string, Tuple<int, PlcObject>> plcObjects, IEnumerable<string> values)
         => PlcObjectResolver.AddPlcObjects(PlcObject, plcObjects, values);
    }

}
