using System;
using System.Threading;
using System.Threading.Tasks;

namespace Papper.Extensions.Conditions
{

#pragma warning disable CA1068
    public static class WhenConditionExtensions
    {

        public async static Task<bool> WhenAsync<T1>(this PlcDataMapper papper,
                                                (PlcReadReference reference, Func<T1, bool> cmp) variable1,
                                                Func<Task<bool>>? then = null,
                                                Func<Task<bool>>? otherwise = null,
                                                CancellationToken ct = default,
                                                int interval = 100)
        {
            if (papper == null) return false;
            do
            {
                var result = await papper.IfAsync(variable1, then, otherwise).ConfigureAwait(false);
                if (result) return true;
                await Task.Delay(interval, ct).ConfigureAwait(false);
            } while (ct.IsCancellationRequested);
            return false;
        }

        public static Task<bool> WhenAsync<T1>(this PlcDataMapper papper, (PlcReadReference reference, Func<T1, bool> cmp) variable1, CancellationToken ct = default, int interval = 100)
            => papper.WhenAsync(variable1, null, null, ct, interval);






        public async static Task<bool> WhenAsync<T1, T2>(this PlcDataMapper papper,
                    PlcReadReference variable1,
                    PlcReadReference variable2,
                    Func<T1, T2, bool> cmp,
                    Func<Task<bool>>? then = null,
                    Func<Task<bool>>? otherwise = null,
                    CancellationToken ct = default,
                    int interval = 100)
        {
            if (papper == null) return false;
            do
            {
                var result = await papper.IfAsync(variable1,
                                                    variable2,
                                                    cmp,
                                                    then,
                                                    otherwise).ConfigureAwait(false);
                if (result) return true;
                await Task.Delay(interval, ct).ConfigureAwait(false);
            } while (ct.IsCancellationRequested);
            return false;
        }





        public static Task<bool> WhenAsync<T1, T2>(this PlcDataMapper papper,
                                    PlcReadReference variable1,
                                    PlcReadReference variable2,
                                    Func<T1, T2, bool> cmp,
                                    CancellationToken ct = default,
                                    int interval = 100)
            => papper.WhenAsync(variable1,
                                variable2,
                                cmp,
                                ct,
                                interval);



        public async static Task<bool> WhenAsync<T1, T2, T3>(this PlcDataMapper papper,
                    PlcReadReference variable1,
                    PlcReadReference variable2,
                    PlcReadReference variable3,
                    Func<T1, T2, T3, bool> cmp,
                    Func<Task<bool>>? then = null,
                    Func<Task<bool>>? otherwise = null,
                    CancellationToken ct = default,
                    int interval = 100)
        {
            if (papper == null) return false;
            do
            {
                var result = await papper.IfAsync(variable1,
                                                    variable2,
                                                    variable3,
                                                    cmp,
                                                    then,
                                                    otherwise).ConfigureAwait(false);
                if (result) return true;
                await Task.Delay(interval, ct).ConfigureAwait(false);
            } while (ct.IsCancellationRequested);
            return false;
        }





        public static Task<bool> WhenAsync<T1, T2, T3>(this PlcDataMapper papper,
                                    PlcReadReference variable1,
                                    PlcReadReference variable2,
                                    PlcReadReference variable3,
                                    Func<T1, T2, T3, bool> cmp,
                                    CancellationToken ct = default,
                                    int interval = 100)
            => papper.WhenAsync(variable1,
                                variable2,
                                variable3,
                                cmp,
                                ct,
                                interval);

        public async static Task<bool> WhenAsync<T1, T2, T3, T4>(this PlcDataMapper papper,
                            PlcReadReference variable1,
                            PlcReadReference variable2,
                            PlcReadReference variable3,
                            PlcReadReference variable4,
                            Func<T1, T2, T3, T4, bool> cmp,
                            Func<Task<bool>>? then = null,
                            Func<Task<bool>>? otherwise = null,
                            CancellationToken ct = default,
                            int interval = 100)
        {
            if (papper == null) return false;
            do
            {
                var result = await papper.IfAsync(variable1,
                                                    variable2,
                                                    variable3,
                                                    variable4,
                                                    cmp,
                                                    then,
                                                    otherwise).ConfigureAwait(false);
                if (result) return true;
                await Task.Delay(interval, ct).ConfigureAwait(false);
            } while (ct.IsCancellationRequested);
            return false;
        }





        public static Task<bool> WhenAsync<T1, T2, T3, T4>(this PlcDataMapper papper,
                                    PlcReadReference variable1,
                                    PlcReadReference variable2,
                                    PlcReadReference variable3,
                                    PlcReadReference variable4,
                                    Func<T1, T2, T3, T4, bool> cmp,
                                    CancellationToken ct = default,
                                    int interval = 100)
            => papper.WhenAsync(variable1,
                                variable2,
                                variable3,
                                variable4,
                                cmp,
                                ct,
                                interval);

        public async static Task<bool> WhenAsync<T1, T2, T3, T4, T5>(this PlcDataMapper papper,
                                    PlcReadReference variable1,
                                    PlcReadReference variable2,
                                    PlcReadReference variable3,
                                    PlcReadReference variable4,
                                    PlcReadReference variable5,
                                    Func<T1, T2, T3, T4, T5, bool> cmp,
                                    Func<Task<bool>>? then = null,
                                    Func<Task<bool>>? otherwise = null,
                                    CancellationToken ct = default,
                                    int interval = 100)
        {
            if (papper == null) return false;
            do
            {
                var result = await papper.IfAsync(variable1,
                                                    variable2,
                                                    variable3,
                                                    variable4,
                                                    variable5,
                                                    cmp,
                                                    then,
                                                    otherwise).ConfigureAwait(false);
                if (result) return true;
                await Task.Delay(interval, ct).ConfigureAwait(false);
            } while (ct.IsCancellationRequested);
            return false;
        }





        public static Task<bool> WhenAsync<T1, T2, T3, T4, T5>(this PlcDataMapper papper,
                                    PlcReadReference variable1,
                                    PlcReadReference variable2,
                                    PlcReadReference variable3,
                                    PlcReadReference variable4,
                                    PlcReadReference variable5,
                                    Func<T1, T2, T3, T4, T5, bool> cmp,
                                    CancellationToken ct = default,
                                    int interval = 100)
            => papper.WhenAsync(variable1,
                                variable2,
                                variable3,
                                variable4,
                                variable5,
                                cmp,
                                ct,
                                interval);


        public async static Task<bool> WhenAsync<T1, T2, T3, T4, T5, T6>(this PlcDataMapper papper,
                                            PlcReadReference variable1,
                                            PlcReadReference variable2,
                                            PlcReadReference variable3,
                                            PlcReadReference variable4,
                                            PlcReadReference variable5,
                                            PlcReadReference variable6,
                                            Func<T1, T2, T3, T4, T5, T6, bool> cmp,
                                            Func<Task<bool>>? then = null,
                                            Func<Task<bool>>? otherwise = null,
                                            CancellationToken ct = default,
                                            int interval = 100)
        {
            if (papper == null) return false;

            do
            {
                var result = await papper.IfAsync(variable1,
                                                    variable2,
                                                    variable3,
                                                    variable4,
                                                    variable5,
                                                    variable6,
                                                    cmp,
                                                    then,
                                                    otherwise).ConfigureAwait(false);
                if (result) return true;
                await Task.Delay(interval, ct).ConfigureAwait(false);
            } while (ct.IsCancellationRequested);
            return false;
        }





        public static Task<bool> WhenAsync<T1, T2, T3, T4, T5, T6>(this PlcDataMapper papper,
                                    PlcReadReference variable1,
                                    PlcReadReference variable2,
                                    PlcReadReference variable3,
                                    PlcReadReference variable4,
                                    PlcReadReference variable5,
                                    PlcReadReference variable6,
                                    Func<T1, T2, T3, T4, T5, T6, bool> cmp,
                                    CancellationToken ct = default,
                                    int interval = 100)
            => papper.WhenAsync(variable1,
                                        variable2,
                                        variable3,
                                        variable4,
                                        variable5,
                                        variable6,
                                        cmp,
                                        ct,
                                        interval);



        public async static Task<bool> WhenAsync<T1, T2, T3, T4, T5, T6, T7>(this PlcDataMapper papper,
                                            PlcReadReference variable1,
                                            PlcReadReference variable2,
                                            PlcReadReference variable3,
                                            PlcReadReference variable4,
                                            PlcReadReference variable5,
                                            PlcReadReference variable6,
                                            PlcReadReference variable7,
                                            Func<T1, T2, T3, T4, T5, T6, T7, bool> cmp,
                                            Func<Task<bool>>? then = null,
                                            Func<Task<bool>>? otherwise = null,
                                            CancellationToken ct = default,
                                            int interval = 100)
        {
            if (papper == null) return false;

            do
            {
                var result = await papper.IfAsync(variable1,
                                                    variable2,
                                                    variable3,
                                                    variable4,
                                                    variable5,
                                                    variable6,
                                                    variable7,
                                                    cmp,
                                                    then,
                                                    otherwise).ConfigureAwait(false);
                if (result) return true;
                await Task.Delay(interval, ct).ConfigureAwait(false);
            } while (ct.IsCancellationRequested);
            return false;
        }





        public static Task<bool> WhenAsync<T1, T2, T3, T4, T5, T6, T7>(this PlcDataMapper papper,
                                    PlcReadReference variable1,
                                    PlcReadReference variable2,
                                    PlcReadReference variable3,
                                    PlcReadReference variable4,
                                    PlcReadReference variable5,
                                    PlcReadReference variable6,
                                    PlcReadReference variable7,
                                    Func<T1, T2, T3, T4, T5, T6, T7, bool> cmp,
                                    CancellationToken ct = default,
                                    int interval = 100)
            => papper.WhenAsync(variable1,
                                        variable2,
                                        variable3,
                                        variable4,
                                        variable5,
                                        variable6,
                                        variable7,
                                        cmp,
                                        ct,
                                        interval);





        public async static Task<bool> WhenAsync<T1, T2, T3, T4, T5, T6, T7, T8>(this PlcDataMapper papper,
                                                PlcReadReference variable1,
                                                PlcReadReference variable2,
                                                PlcReadReference variable3,
                                                PlcReadReference variable4,
                                                PlcReadReference variable5,
                                                PlcReadReference variable6,
                                                PlcReadReference variable7,
                                                PlcReadReference variable8,
                                                Func<T1, T2, T3, T4, T5, T6, T7, T8, bool> cmp,
                                                Func<Task<bool>>? then = null,
                                                Func<Task<bool>>? otherwise = null,
                                                CancellationToken ct = default,
                                                int interval = 100)
        {
            if (papper == null) return false;

            do
            {
                var result = await papper.IfAsync(variable1,
                                                    variable2,
                                                    variable3,
                                                    variable4,
                                                    variable5,
                                                    variable6,
                                                    variable7,
                                                    variable8,
                                                    cmp,
                                                    then,
                                                    otherwise).ConfigureAwait(false);
                if (result) return true;
                await Task.Delay(interval, ct).ConfigureAwait(false);
            } while (ct.IsCancellationRequested);
            return false;
        }





        public static Task<bool> WhenAsync<T1, T2, T3, T4, T5, T6, T7, T8>(this PlcDataMapper papper,
                                    PlcReadReference variable1,
                                    PlcReadReference variable2,
                                    PlcReadReference variable3,
                                    PlcReadReference variable4,
                                    PlcReadReference variable5,
                                    PlcReadReference variable6,
                                    PlcReadReference variable7,
                                    PlcReadReference variable8,
                                    Func<T1, T2, T3, T4, T5, T6, T7, T8, bool> cmp,
                                    CancellationToken ct = default,
                                    int interval = 100)
            => papper.WhenAsync(variable1,
                                        variable2,
                                        variable3,
                                        variable4,
                                        variable5,
                                        variable6,
                                        variable7,
                                        variable8,
                                        cmp,
                                        ct,
                                        interval);

    }

#pragma warning restore CA1068
}
