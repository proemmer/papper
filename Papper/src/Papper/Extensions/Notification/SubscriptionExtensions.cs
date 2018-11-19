namespace Papper.Extensions.Notification
{
    public static class SubscriptionExtensions
    {

        /// <summary>
        /// Create a Subscription to watch data changes
        /// </summary>
        /// <returns></returns>
        public static Subscription CreateSubscription(this PlcDataMapper papper, ChangeDetectionStrategy changeDetectionStrategy = ChangeDetectionStrategy.Polling)
        {
            var sub = new Subscription(papper, changeDetectionStrategy);
            papper.AddSubscription(sub);
            return sub;
        }
    }
}
