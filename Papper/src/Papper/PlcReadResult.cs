namespace Papper
{
    public struct PlcReadResult
    {
        public string Name { get; set; }
        public object Value { get; set; }

        public ActionResult ActionResult { get; set; }
    }
}