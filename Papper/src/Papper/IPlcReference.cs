namespace Papper
{
    public interface IPlcReference
    {
        /// <summary>
        /// The main mapping.
        /// DBName
        /// IB: Input Area
        /// FB: Flag Area
        /// QB: Output Area
        /// TM: Timer Area
        /// CT: Counter Area
        /// DB: DataBlock Area
        /// </summary>
        string Mapping { get;  }
        string Variable { get;  }


    }
}