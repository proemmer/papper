namespace Papper
{
    public enum ChangeDetectionStrategy
    {
        /// <summary>
        /// Papper handle changedetection by iteself.
        /// The library will be used to poll th items.
        /// </summary>
        Polling,

        /// <summary>
        /// If the access library supports change detection, you can use Event. So papper will get informed of changes
        /// and has not to determine it by itself.
        /// </summary>
        Event
    }
}
