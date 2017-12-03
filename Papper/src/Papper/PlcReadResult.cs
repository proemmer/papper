namespace Papper
{
    public struct PlcReadResult
    {
        public string Mapping { get; set; }
        public string Variable { get; set; }
        public string Address { get; set; }
        public object Value { get; set; }

        public ExecutionResult ActionResult { get; set; }
    }
}