using Papper.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace Papper.Types
{
    internal class DummyPlcObject : PlcObject
    {
        public DummyPlcObject(string name) : base(name)
        {
        }

        public override Type DotNetType => typeof(object);
        public override object ConvertFromRaw(PlcObjectBinding plcObjectBinding, Span<byte> data) => throw new NotImplementedException();
        public override void ConvertToRaw(object value, PlcObjectBinding plcObjectBinding, Span<byte> data) => throw new NotImplementedException();
    }
}
