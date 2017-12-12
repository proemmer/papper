using System;

namespace Papper
{
    public struct PlcReadResult
    {
        private string _address;
        private object _value;
        private ExecutionResult _executionResult;
        private int _dot;

        public PlcReadResult(string address, object value, ExecutionResult executionResult)
        {
            _address = address;
            _value = value;
            _executionResult = executionResult;
            _dot = address.IndexOf(".");
        }

        public string Address => _address;

        public object Value => _value;

        public ExecutionResult ActionResult => _executionResult;

        public string Mapping => Address.Substring(0, _dot);

        public string Variable => Address.Substring(_dot + 1 );

        

        public bool IsPartOfMapping(string mapping)
        {
            return mapping.AsSpan().SequenceEqual(Address.AsSpan().Slice(0, Address.IndexOf(".")));
        }

    }
}