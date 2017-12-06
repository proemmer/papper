namespace Papper
{
    public class PlcWriteResult
    {
        public string Address { get; internal set; }

        public string Mapping => Address.Substring(0, Address.IndexOf("."));
        public string Variable => Address.Substring(Address.IndexOf(".") + 1);

        public ExecutionResult ActionResult { get; internal set; }
    }
}