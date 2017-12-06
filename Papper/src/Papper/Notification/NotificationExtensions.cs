using System;
using System.Collections.Generic;
using System.Text;
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
        public static Subscription SubscribeDataChanges(this PlcDataMapper mapper, OnChangeEventHandler callback, params PlcReference[] items)
        {
            var subscription = new Subscription(mapper);
            subscription.AddItems(items);
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
                        if (!result.IsCompleted && !result.IsCancelled)
                        {
                            callback(subscription, new PlcNotificationEventArgs(result.Results));
                        }
                    }
                    catch (Exception)
                    {
                        // todo
                    }
                }

            }, TaskCreationOptions.LongRunning);
        }

    }
}
