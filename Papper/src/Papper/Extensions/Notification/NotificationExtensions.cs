﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Papper.Extensions.Notification
{
    public delegate void OnChangeEventHandler(object sender, PlcNotificationEventArgs e);


    public static class NotificationExtensions
    {
        private const int DefaultInterval = 1000;


        /// <summary>
        /// Subscribe to changes of variables
        /// </summary>
        /// <param name="mapper">Reference to plc Data mapper</param>
        /// <param name="callback">Callback method</param>
        /// <param name="items">items to watch</param>
        /// <returns></returns>
        public static Subscription SubscribeDataChanges(this PlcDataMapper mapper, OnChangeEventHandler callback, params PlcWatchReference[] items)
            => SubscribeDataChanges(mapper, callback, items as IEnumerable<PlcWatchReference>);

        /// <summary>
        /// Subscribe to changes of variables
        /// </summary>
        /// <param name="mapper">Reference to plc data mapper</param>
        /// <param name="callback">Callback method</param>
        /// <param name="items">items to watch</param>
        /// <returns></returns>
        public static Subscription SubscribeDataChanges(this PlcDataMapper mapper, OnChangeEventHandler callback, int interval, params PlcWatchReference[] items)
            => SubscribeDataChanges(mapper, callback, interval, items as IEnumerable<PlcWatchReference>);

        /// <summary>
        /// Subscribe to changes of variables
        /// </summary>
        /// <param name="mapper">Reference to plc data mapper</param>
        /// <param name="callback">Callback method</param>
        /// <param name="items">items to watch</param>
        /// <param name="changeDetectionStrategy">setup the strategy to detect changes. <see cref="ChangeDetectionStrategy"/>. This setting depends on the access library.</param>
        /// <returns></returns>
        public static Subscription SubscribeDataChanges(this PlcDataMapper mapper,
                                                        OnChangeEventHandler callback,
                                                        IEnumerable<PlcWatchReference> items,
                                                        ChangeDetectionStrategy changeDetectionStrategy = ChangeDetectionStrategy.Polling)
         => SubscribeDataChanges(mapper, callback, DefaultInterval, items as IEnumerable<PlcWatchReference>, changeDetectionStrategy);

        /// <summary>
        /// Subscribe to changes of variables
        /// </summary>
        /// <param name="mapper">Reference to plc data mapper</param>
        /// <param name="callback">Callback method</param>
        /// <param name="items">items to watch</param>
        /// <param name="changeDetectionStrategy">setup the strategy to detect changes. <see cref="ChangeDetectionStrategy"/>. This setting depends on the access library.</param>
        /// <returns></returns>
        public static Subscription SubscribeDataChanges(this PlcDataMapper mapper,
                                                        OnChangeEventHandler callback,
                                                        int interval,
                                                        IEnumerable<PlcWatchReference> items,
                                                        ChangeDetectionStrategy changeDetectionStrategy = ChangeDetectionStrategy.Polling)
        {
            var subscription = new Subscription(mapper, changeDetectionStrategy, items, interval);
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
            if (subscription == null)
            {
                return ExceptionThrowHelper.ThrowArgumentNullException<Subscription>(nameof(subscription));
            }

            subscription!.CancelCurrentDetection();
            return subscription;
        }

        /// <summary>
        /// Resume the change detection
        /// </summary>
        /// <param name="subscription"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static Subscription Resume(this Subscription subscription, OnChangeEventHandler callback)
        {
            RunWatchTask(subscription, callback);
            return subscription;
        }



        private static Task RunWatchTask(Subscription subscription, OnChangeEventHandler callback) 
            => Task.Factory.StartNew(() => WatchLoop(subscription, callback), 
                                         System.Threading.CancellationToken.None, 
                                         TaskCreationOptions.LongRunning,
                                         TaskScheduler.Default).Unwrap();

        private static async Task WatchLoop(Subscription subscription, OnChangeEventHandler callback)
        {
            while (!subscription!.Watching.IsCompleted)
            {
                try
                {
                    var result = await subscription.DetectChangesAsync().ConfigureAwait(false);
                    if (!result.IsCompleted && !result.IsCanceled)
                    {
                        callback(subscription, new PlcNotificationEventArgs(result.Results));
                    }
                    else
                    {
                        // is canceled or completed, so set watching is completed now!
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
        }
    }
}
