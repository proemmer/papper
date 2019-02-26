namespace Papper.Types
{
    internal interface ISupportStringLengthAttribute
    {
        int StringLength { get; set; }
        void AssigneLengthFrom(ISupportStringLengthAttribute s);
    }
}