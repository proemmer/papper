using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Papper.Notification
{
    public delegate void OnChangeEventHandler(object sender, PlcNotificationEventArgs e);


    public static class NotificationExtensions
    {
        /// <summary>
        /// Subscribe to changes of variables
        /// </summary>
        /// <param name="mapper">Reference to plc Datampper</param>
        /// <param name="callback">Callback method</param>
        /// <param name="items">items to watch</param>
        /// <returns></returns>
        public static Subscription SubscribeDataChanges(this PlcDataMapper mapper, OnChangeEventHandler callback, params PlcReadReference[] items) 
            => SubscribeDataChanges(mapper, callback, items as IEnumerable<PlcReadReference>);

        /// <summary>
        /// Subscribe to changes of variables
        /// </summary>
        /// <param name="mapper">Reference to plc Datampper</param>
        /// <param name="callback">Callback method</param>
        /// <param name="items">items to watch</param>
        /// <param name="changeDetectionStrategy">seup the starategy to detect changes. <see cref="ChangeDetectionStrategy"/>. This setting depends on the access library.</param>
        /// <returns></returns>
        public static Subscription SubscribeDataChanges(this PlcDataMapper mapper, OnChangeEventHandler callback, IEnumerable<PlcReadReference> items, ChangeDetectionStrategy changeDetectionStrategy = ChangeDetectionStrategy.Polling)
        {
            var subscription = new Subscription(mapper, changeDetectionStrategy, items);
            RunWatchTask(subscription, callback);
            return subscription;
        }

        /// <summary>
        /// Pause the change detection
        /// </summary>
        /// <param name="subscription"></param>
        /// <returns></returns>
        public static Subscription Pause(this Subscription subscription)
        {
            subscription.CancelCurrentDetection();
            return subscription;
        }

        /// <summary>
        /// Resume the change detetcion
        /// </summary>
        /// <param name="subscription"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static Subscription Resume(this Subscription subscription, OnChangeEventHandler callback)
        {
            RunWatchTask(subscription, callback);
            return subscription;
        }



        private static void RunWatchTask(Subscription subscription, OnChangeEventHandler callback)
        {
            Task.Factory.StartNew(async () =>
            {
                while (!subscription.Watching.IsCompleted)
                {
                    try
                    {
                        var result = await subscription.DetectChangesAsync();
                        if (!result.IsCompleted && !result.IsCanceled)
                        {
                            callback(subscription, new PlcNotificationEventArgs(result.Results));
                        }
                        else
                        {
                            // is cancelled or completed, so set whatching is compleded now!
                            callback(subscription, new PlcNotificationEventArgs());
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        // todo
                        subscription.CancelCurrentDetection();
                        callback(subscription, new PlcNotificationEventArgs(ex));
                        return;
                    }
                }

            }, TaskCreationOptions.LongRunning);
        }

    }
}
