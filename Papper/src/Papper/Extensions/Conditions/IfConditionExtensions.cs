using Papper.Internal;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Papper.Extensions.Conditions
{
    public static class IfConditionExtensions
    {

        public static async Task<bool> IfAsync<T1>(this PlcDataMapper papper,
                                                    (PlcReadReference reference, Func<T1, bool> cmp) variable1,
                                                    Func<Task<bool>>? then = null,
                                                    Func<Task<bool>>? otherwise = null)
        {
            if (papper == null)
            {
                return ExceptionThrowHelper.ThrowArgumentNullException<bool>(nameof(papper));
            }

            System.Collections.Generic.Dictionary<string, object?>? result = (await papper.ReadAsync(variable1.reference).ConfigureAwait(false))?.ToDictionary(x => x.Variable, x => x.Value);

            if (result!.IsNullOrEmpty() ||
                (!result!.TryGetValue(variable1.reference.Address, out object? tmp1) || (tmp1 is T1 t1 && !variable1.cmp.Invoke(t1))))
            {
                if (otherwise == null)
                {
                    return false;
                }

                return await otherwise!.Invoke().ConfigureAwait(false);
            }

            if (then == null)
            {
                return true;
            }

            return await then!.Invoke().ConfigureAwait(false);

        }



        public static async Task<bool> IfAsync<T1, T2>(this PlcDataMapper papper,
                                                            (PlcReadReference reference, Func<T1, bool> cmp) variable1,
                                                            (PlcReadReference reference, Func<T2, bool> cmp) variable2,
                                                            Func<Task<bool>>? then = null,
                                                            Func<Task<bool>>? otherwise = null)
        {
            if (papper == null)
            {
                return ExceptionThrowHelper.ThrowArgumentNullException<bool>(nameof(papper));
            }

            System.Collections.Generic.Dictionary<string, object?>? result = (await papper.ReadAsync(variable1.reference,
                                                variable2.reference).ConfigureAwait(false))?.ToDictionary(x => x.Variable, x => x.Value);

            if (result!.IsNullOrEmpty() ||
                (!result!.TryGetValue(variable1.reference.Address, out object? tmp1) || (tmp1 is T1 t1 && !variable1.cmp.Invoke(t1))) ||
                (!result!.TryGetValue(variable2.reference.Address, out object? tmp2) || (tmp2 is T2 t2 && !variable2.cmp.Invoke(t2))))
            {
                if (otherwise == null)
                {
                    return false;
                }

                return await otherwise!.Invoke().ConfigureAwait(false);
            }

            if (then == null)
            {
                return true;
            }

            return await then!.Invoke().ConfigureAwait(false);

        }

        public static async Task<bool> IfAsync<T1, T2, T3>(this PlcDataMapper papper,
                    (PlcReadReference reference, Func<T1, bool> cmp) variable1,
                    (PlcReadReference reference, Func<T2, bool> cmp) variable2,
                    (PlcReadReference reference, Func<T3, bool> cmp) variable3,
                    Func<Task<bool>>? then = null,
                    Func<Task<bool>>? otherwise = null)
        {
            if (papper == null)
            {
                return ExceptionThrowHelper.ThrowArgumentNullException<bool>(nameof(papper));
            }

            System.Collections.Generic.Dictionary<string, object?>? result = (await papper.ReadAsync(
                                                 variable1.reference,
                                                 variable2.reference,
                                                 variable3.reference).ConfigureAwait(false))?.ToDictionary(x => x.Variable, x => x.Value);

            if (result!.IsNullOrEmpty() ||
                (!result!.TryGetValue(variable1.reference.Address, out object? tmp1) || (tmp1 is T1 t1 && !variable1.cmp.Invoke(t1))) ||
                (!result!.TryGetValue(variable2.reference.Address, out object? tmp2) || (tmp2 is T2 t2 && !variable2.cmp.Invoke(t2))) ||
                (!result!.TryGetValue(variable3.reference.Address, out object? tmp3) || (tmp3 is T3 t3 && !variable3.cmp.Invoke(t3))))
            {
                if (otherwise == null)
                {
                    return false;
                }

                return await otherwise!.Invoke().ConfigureAwait(false);
            }

            if (then == null)
            {
                return true;
            }

            return await then!.Invoke().ConfigureAwait(false);

        }

        public static async Task<bool> IfAsync<T1, T2, T3, T4>(this PlcDataMapper papper,
                (PlcReadReference reference, Func<T1, bool> cmp) variable1,
                (PlcReadReference reference, Func<T2, bool> cmp) variable2,
                (PlcReadReference reference, Func<T3, bool> cmp) variable3,
                (PlcReadReference reference, Func<T4, bool> cmp) variable4,
                Func<Task<bool>>? then = null,
                Func<Task<bool>>? otherwise = null)
        {
            if (papper == null)
            {
                return ExceptionThrowHelper.ThrowArgumentNullException<bool>(nameof(papper));
            }

            System.Collections.Generic.Dictionary<string, object?>? result = (await papper.ReadAsync(
                                                 variable1.reference,
                                                 variable2.reference,
                                                 variable3.reference,
                                                 variable4.reference).ConfigureAwait(false))?.ToDictionary(x => x.Variable, x => x.Value);

            if (result!.IsNullOrEmpty() ||
                (!result!.TryGetValue(variable1.reference.Address, out object? tmp1) || (tmp1 is T1 t1 && !variable1.cmp.Invoke(t1))) ||
                (!result!.TryGetValue(variable2.reference.Address, out object? tmp2) || (tmp2 is T2 t2 && !variable2.cmp.Invoke(t2))) ||
                (!result!.TryGetValue(variable3.reference.Address, out object? tmp3) || (tmp3 is T3 t3 && !variable3.cmp.Invoke(t3))) ||
                (!result!.TryGetValue(variable4.reference.Address, out object? tmp4) || (tmp4 is T4 t4 && !variable4.cmp.Invoke(t4))))
            {
                if (otherwise == null)
                {
                    return false;
                }

                return await otherwise!.Invoke().ConfigureAwait(false);
            }

            if (then == null)
            {
                return true;
            }

            return await then!.Invoke().ConfigureAwait(false);

        }

        public static async Task<bool> IfAsync<T1, T2, T3, T4, T5>(this PlcDataMapper papper,
                        (PlcReadReference reference, Func<T1, bool> cmp) variable1,
                        (PlcReadReference reference, Func<T2, bool> cmp) variable2,
                        (PlcReadReference reference, Func<T3, bool> cmp) variable3,
                        (PlcReadReference reference, Func<T4, bool> cmp) variable4,
                        (PlcReadReference reference, Func<T5, bool> cmp) variable5,
                        Func<Task<bool>>? then = null,
                        Func<Task<bool>>? otherwise = null)
        {

            if (papper == null)
            {
                return ExceptionThrowHelper.ThrowArgumentNullException<bool>(nameof(papper));
            }

            System.Collections.Generic.Dictionary<string, object?>? result = (await papper.ReadAsync(
                                                 variable1.reference,
                                                 variable2.reference,
                                                 variable3.reference,
                                                 variable4.reference,
                                                 variable5.reference).ConfigureAwait(false))?.ToDictionary(x => x.Variable, x => x.Value);

            if (result!.IsNullOrEmpty() ||
                (!result!.TryGetValue(variable1.reference.Address, out object? tmp1) || (tmp1 is T1 t1 && !variable1.cmp.Invoke(t1))) ||
                (!result!.TryGetValue(variable2.reference.Address, out object? tmp2) || (tmp2 is T2 t2 && !variable2.cmp.Invoke(t2))) ||
                (!result!.TryGetValue(variable3.reference.Address, out object? tmp3) || (tmp3 is T3 t3 && !variable3.cmp.Invoke(t3))) ||
                (!result!.TryGetValue(variable4.reference.Address, out object? tmp4) || (tmp4 is T4 t4 && !variable4.cmp.Invoke(t4))) ||
                (!result!.TryGetValue(variable5.reference.Address, out object? tmp5) || (tmp5 is T5 t5 && !variable5.cmp.Invoke(t5))))
            {
                if (otherwise == null)
                {
                    return false;
                }

                return await otherwise!.Invoke().ConfigureAwait(false);
            }

            if (then == null)
            {
                return true;
            }

            return await then!.Invoke().ConfigureAwait(false);

        }

        public static async Task<bool> IfAsync<T1, T2, T3, T4, T5, T6>(this PlcDataMapper papper,
                (PlcReadReference reference, Func<T1, bool> cmp) variable1,
                (PlcReadReference reference, Func<T2, bool> cmp) variable2,
                (PlcReadReference reference, Func<T3, bool> cmp) variable3,
                (PlcReadReference reference, Func<T4, bool> cmp) variable4,
                (PlcReadReference reference, Func<T5, bool> cmp) variable5,
                (PlcReadReference reference, Func<T6, bool> cmp) variable6,
                Func<Task<bool>>? then = null,
                Func<Task<bool>>? otherwise = null)
        {
            if (papper == null)
            {
                return ExceptionThrowHelper.ThrowArgumentNullException<bool>(nameof(papper));
            }

            System.Collections.Generic.Dictionary<string, object?>? result = (await papper.ReadAsync(
                                                 variable1.reference,
                                                 variable2.reference,
                                                 variable3.reference,
                                                 variable4.reference,
                                                 variable5.reference,
                                                 variable6.reference).ConfigureAwait(false))?.ToDictionary(x => x.Variable, x => x.Value);

            if (result!.IsNullOrEmpty() ||
                (!result!.TryGetValue(variable1.reference.Address, out object? tmp1) || (tmp1 is T1 t1 && !variable1.cmp.Invoke(t1))) ||
                (!result!.TryGetValue(variable2.reference.Address, out object? tmp2) || (tmp2 is T2 t2 && !variable2.cmp.Invoke(t2))) ||
                (!result!.TryGetValue(variable3.reference.Address, out object? tmp3) || (tmp3 is T3 t3 && !variable3.cmp.Invoke(t3))) ||
                (!result!.TryGetValue(variable4.reference.Address, out object? tmp4) || (tmp4 is T4 t4 && !variable4.cmp.Invoke(t4))) ||
                (!result!.TryGetValue(variable5.reference.Address, out object? tmp5) || (tmp5 is T5 t5 && !variable5.cmp.Invoke(t5))) ||
                (!result!.TryGetValue(variable6.reference.Address, out object? tmp6) || (tmp6 is T6 t6 && !variable6.cmp.Invoke(t6))))
            {
                if (otherwise == null)
                {
                    return false;
                }

                return await otherwise!.Invoke().ConfigureAwait(false);
            }

            if (then == null)
            {
                return true;
            }

            return await then!.Invoke().ConfigureAwait(false);

        }

        public static async Task<bool> IfAsync<T1, T2, T3, T4, T5, T6, T7>(this PlcDataMapper papper,
            (PlcReadReference reference, Func<T1, bool> cmp) variable1,
            (PlcReadReference reference, Func<T2, bool> cmp) variable2,
            (PlcReadReference reference, Func<T3, bool> cmp) variable3,
            (PlcReadReference reference, Func<T4, bool> cmp) variable4,
            (PlcReadReference reference, Func<T5, bool> cmp) variable5,
            (PlcReadReference reference, Func<T6, bool> cmp) variable6,
            (PlcReadReference reference, Func<T7, bool> cmp) variable7,
            Func<Task<bool>>? then = null,
            Func<Task<bool>>? otherwise = null)
        {
            if (papper == null)
            {
                return ExceptionThrowHelper.ThrowArgumentNullException<bool>(nameof(papper));
            }

            System.Collections.Generic.Dictionary<string, object?>? result = (await papper.ReadAsync(
                                                 variable1.reference,
                                                 variable2.reference,
                                                 variable3.reference,
                                                 variable4.reference,
                                                 variable5.reference,
                                                 variable6.reference,
                                                 variable7.reference).ConfigureAwait(false))?.ToDictionary(x => x.Variable, x => x.Value);

            if (result!.IsNullOrEmpty() ||
                (!result!.TryGetValue(variable1.reference.Address, out object? tmp1) || (tmp1 is T1 t1 && !variable1.cmp.Invoke(t1))) ||
                (!result!.TryGetValue(variable2.reference.Address, out object? tmp2) || (tmp2 is T2 t2 && !variable2.cmp.Invoke(t2))) ||
                (!result!.TryGetValue(variable3.reference.Address, out object? tmp3) || (tmp3 is T3 t3 && !variable3.cmp.Invoke(t3))) ||
                (!result!.TryGetValue(variable4.reference.Address, out object? tmp4) || (tmp4 is T4 t4 && !variable4.cmp.Invoke(t4))) ||
                (!result!.TryGetValue(variable5.reference.Address, out object? tmp5) || (tmp5 is T5 t5 && !variable5.cmp.Invoke(t5))) ||
                (!result!.TryGetValue(variable6.reference.Address, out object? tmp6) || (tmp6 is T6 t6 && !variable6.cmp.Invoke(t6))) ||
                (!result!.TryGetValue(variable7.reference.Address, out object? tmp7) || (tmp7 is T7 t7 && !variable7.cmp.Invoke(t7))))
            {
                if (otherwise == null)
                {
                    return false;
                }

                return await otherwise!.Invoke().ConfigureAwait(false);
            }

            if (then == null)
            {
                return true;
            }

            return await then!.Invoke().ConfigureAwait(false);

        }

        public static async Task<bool> IfAsync<T1, T2, T3, T4, T5, T6, T7, T8>(this PlcDataMapper papper,
            (PlcReadReference reference, Func<T1, bool> cmp) variable1,
            (PlcReadReference reference, Func<T2, bool> cmp) variable2,
            (PlcReadReference reference, Func<T3, bool> cmp) variable3,
            (PlcReadReference reference, Func<T4, bool> cmp) variable4,
            (PlcReadReference reference, Func<T5, bool> cmp) variable5,
            (PlcReadReference reference, Func<T6, bool> cmp) variable6,
            (PlcReadReference reference, Func<T7, bool> cmp) variable7,
            (PlcReadReference reference, Func<T8, bool> cmp) variable8,
            Func<Task<bool>>? then = null,
            Func<Task<bool>>? otherwise = null)
        {
            if (papper == null)
            {
                return ExceptionThrowHelper.ThrowArgumentNullException<bool>(nameof(papper));
            }

            System.Collections.Generic.Dictionary<string, object?>? result = (await papper.ReadAsync(
                                                 variable1.reference,
                                                 variable2.reference,
                                                 variable3.reference,
                                                 variable4.reference,
                                                 variable5.reference,
                                                 variable6.reference,
                                                 variable7.reference,
                                                 variable8.reference).ConfigureAwait(false))?.ToDictionary(x => x.Variable, x => x.Value);

            if (result!.IsNullOrEmpty() ||
                (!result!.TryGetValue(variable1.reference.Address, out object? tmp1) || (tmp1 is T1 t1 && !variable1.cmp.Invoke(t1))) ||
                (!result!.TryGetValue(variable2.reference.Address, out object? tmp2) || (tmp2 is T2 t2 && !variable2.cmp.Invoke(t2))) ||
                (!result!.TryGetValue(variable3.reference.Address, out object? tmp3) || (tmp3 is T3 t3 && !variable3.cmp.Invoke(t3))) ||
                (!result!.TryGetValue(variable4.reference.Address, out object? tmp4) || (tmp4 is T4 t4 && !variable4.cmp.Invoke(t4))) ||
                (!result!.TryGetValue(variable5.reference.Address, out object? tmp5) || (tmp5 is T5 t5 && !variable5.cmp.Invoke(t5))) ||
                (!result!.TryGetValue(variable6.reference.Address, out object? tmp6) || (tmp6 is T6 t6 && !variable6.cmp.Invoke(t6))) ||
                (!result!.TryGetValue(variable7.reference.Address, out object? tmp7) || (tmp7 is T7 t7 && !variable7.cmp.Invoke(t7))) ||
                (!result!.TryGetValue(variable8.reference.Address, out object? tmp8) || (tmp8 is T8 t8 && !variable8.cmp.Invoke(t8))))
            {
                if (otherwise == null)
                {
                    return false;
                }

                return await otherwise!.Invoke().ConfigureAwait(false);
            }

            if (then == null)
            {
                return true;
            }

            return await then!.Invoke().ConfigureAwait(false);

        }

        public static async Task<bool> IfAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this PlcDataMapper papper,
                    (PlcReadReference reference, Func<T1, bool> cmp) variable1,
                    (PlcReadReference reference, Func<T2, bool> cmp) variable2,
                    (PlcReadReference reference, Func<T3, bool> cmp) variable3,
                    (PlcReadReference reference, Func<T4, bool> cmp) variable4,
                    (PlcReadReference reference, Func<T5, bool> cmp) variable5,
                    (PlcReadReference reference, Func<T6, bool> cmp) variable6,
                    (PlcReadReference reference, Func<T7, bool> cmp) variable7,
                    (PlcReadReference reference, Func<T8, bool> cmp) variable8,
                    (PlcReadReference reference, Func<T9, bool> cmp) variable9,
                    Func<Task<bool>>? then = null,
                    Func<Task<bool>>? otherwise = null)
        {
            if (papper == null)
            {
                return ExceptionThrowHelper.ThrowArgumentNullException<bool>(nameof(papper));
            }

            System.Collections.Generic.Dictionary<string, object?>? result = (await papper.ReadAsync(
                                                 variable1.reference,
                                                 variable2.reference,
                                                 variable3.reference,
                                                 variable4.reference,
                                                 variable5.reference,
                                                 variable6.reference,
                                                 variable7.reference,
                                                 variable8.reference,
                                                 variable9.reference).ConfigureAwait(false))?.ToDictionary(x => x.Variable, x => x.Value);

            if (result!.IsNullOrEmpty() ||
                (!result!.TryGetValue(variable1.reference.Address, out object? tmp1) || (tmp1 is T1 t1 && !variable1.cmp.Invoke(t1))) ||
                (!result!.TryGetValue(variable2.reference.Address, out object? tmp2) || (tmp2 is T2 t2 && !variable2.cmp.Invoke(t2))) ||
                (!result!.TryGetValue(variable3.reference.Address, out object? tmp3) || (tmp3 is T3 t3 && !variable3.cmp.Invoke(t3))) ||
                (!result!.TryGetValue(variable4.reference.Address, out object? tmp4) || (tmp4 is T4 t4 && !variable4.cmp.Invoke(t4))) ||
                (!result!.TryGetValue(variable5.reference.Address, out object? tmp5) || (tmp5 is T5 t5 && !variable5.cmp.Invoke(t5))) ||
                (!result!.TryGetValue(variable6.reference.Address, out object? tmp6) || (tmp6 is T6 t6 && !variable6.cmp.Invoke(t6))) ||
                (!result!.TryGetValue(variable7.reference.Address, out object? tmp7) || (tmp7 is T7 t7 && !variable7.cmp.Invoke(t7))) ||
                (!result!.TryGetValue(variable8.reference.Address, out object? tmp8) || (tmp8 is T8 t8 && !variable8.cmp.Invoke(t8))) ||
                (!result!.TryGetValue(variable9.reference.Address, out object? tmp9) || (tmp9 is T9 t9 && !variable9.cmp.Invoke(t9))))
            {
                if (otherwise == null)
                {
                    return false;
                }

                return await otherwise!.Invoke().ConfigureAwait(false);
            }

            if (then == null)
            {
                return true;
            }

            return await then!.Invoke().ConfigureAwait(false);

        }

        public static async Task<bool> IfAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this PlcDataMapper papper,
                            (PlcReadReference reference, Func<T1, bool> cmp) variable1,
                            (PlcReadReference reference, Func<T2, bool> cmp) variable2,
                            (PlcReadReference reference, Func<T3, bool> cmp) variable3,
                            (PlcReadReference reference, Func<T4, bool> cmp) variable4,
                            (PlcReadReference reference, Func<T5, bool> cmp) variable5,
                            (PlcReadReference reference, Func<T6, bool> cmp) variable6,
                            (PlcReadReference reference, Func<T7, bool> cmp) variable7,
                            (PlcReadReference reference, Func<T8, bool> cmp) variable8,
                            (PlcReadReference reference, Func<T9, bool> cmp) variable9,
                            (PlcReadReference reference, Func<T10, bool> cmp) variable10,
                            Func<Task<bool>>? then = null,
                            Func<Task<bool>>? otherwise = null)
        {
            if (papper == null)
            {
                return ExceptionThrowHelper.ThrowArgumentNullException<bool>(nameof(papper));
            }

            System.Collections.Generic.Dictionary<string, object?>? result = (await papper.ReadAsync(
                                                 variable1.reference,
                                                 variable2.reference,
                                                 variable3.reference,
                                                 variable4.reference,
                                                 variable5.reference,
                                                 variable6.reference,
                                                 variable7.reference,
                                                 variable8.reference,
                                                 variable9.reference,
                                                 variable10.reference).ConfigureAwait(false))?.ToDictionary(x => x.Variable, x => x.Value);

            if (result!.IsNullOrEmpty() ||
                (!result!.TryGetValue(variable1.reference.Address, out object? tmp1) || (tmp1 is T1 t1 && !variable1.cmp.Invoke(t1))) ||
                (!result!.TryGetValue(variable2.reference.Address, out object? tmp2) || (tmp2 is T2 t2 && !variable2.cmp.Invoke(t2))) ||
                (!result!.TryGetValue(variable3.reference.Address, out object? tmp3) || (tmp3 is T3 t3 && !variable3.cmp.Invoke(t3))) ||
                (!result!.TryGetValue(variable4.reference.Address, out object? tmp4) || (tmp4 is T4 t4 && !variable4.cmp.Invoke(t4))) ||
                (!result!.TryGetValue(variable5.reference.Address, out object? tmp5) || (tmp5 is T5 t5 && !variable5.cmp.Invoke(t5))) ||
                (!result!.TryGetValue(variable6.reference.Address, out object? tmp6) || (tmp6 is T6 t6 && !variable6.cmp.Invoke(t6))) ||
                (!result!.TryGetValue(variable7.reference.Address, out object? tmp7) || (tmp7 is T7 t7 && !variable7.cmp.Invoke(t7))) ||
                (!result!.TryGetValue(variable8.reference.Address, out object? tmp8) || (tmp8 is T8 t8 && !variable8.cmp.Invoke(t8))) ||
                (!result!.TryGetValue(variable9.reference.Address, out object? tmp9) || (tmp9 is T9 t9 && !variable9.cmp.Invoke(t9))) ||
                (!result!.TryGetValue(variable10.reference.Address, out object? tmp10) || (tmp10 is T10 t10 && !variable10.cmp.Invoke(t10))))
            {
                if (otherwise == null)
                {
                    return false;
                }

                return await otherwise!.Invoke().ConfigureAwait(false);
            }

            if (then == null)
            {
                return true;
            }

            return await then!.Invoke().ConfigureAwait(false);

        }

        public static async Task<bool> IfAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this PlcDataMapper papper,
                            (PlcReadReference reference, Func<T1, bool> cmp) variable1,
                            (PlcReadReference reference, Func<T2, bool> cmp) variable2,
                            (PlcReadReference reference, Func<T3, bool> cmp) variable3,
                            (PlcReadReference reference, Func<T4, bool> cmp) variable4,
                            (PlcReadReference reference, Func<T5, bool> cmp) variable5,
                            (PlcReadReference reference, Func<T6, bool> cmp) variable6,
                            (PlcReadReference reference, Func<T7, bool> cmp) variable7,
                            (PlcReadReference reference, Func<T8, bool> cmp) variable8,
                            (PlcReadReference reference, Func<T9, bool> cmp) variable9,
                            (PlcReadReference reference, Func<T10, bool> cmp) variable10,
                            (PlcReadReference reference, Func<T11, bool> cmp) variable11,
                            Func<Task<bool>>? then = null,
                            Func<Task<bool>>? otherwise = null)
        {
            if (papper == null)
            {
                return ExceptionThrowHelper.ThrowArgumentNullException<bool>(nameof(papper));
            }

            System.Collections.Generic.Dictionary<string, object?>? result = (await papper.ReadAsync(
                                                 variable1.reference,
                                                 variable2.reference,
                                                 variable3.reference,
                                                 variable4.reference,
                                                 variable5.reference,
                                                 variable6.reference,
                                                 variable7.reference,
                                                 variable8.reference,
                                                 variable9.reference,
                                                 variable10.reference,
                                                 variable11.reference).ConfigureAwait(false))?.ToDictionary(x => x.Variable, x => x.Value);

            if (result!.IsNullOrEmpty() ||
                (!result!.TryGetValue(variable1.reference.Address, out object? tmp1) || (tmp1 is T1 t1 && !variable1.cmp.Invoke(t1))) ||
                (!result!.TryGetValue(variable2.reference.Address, out object? tmp2) || (tmp2 is T2 t2 && !variable2.cmp.Invoke(t2))) ||
                (!result!.TryGetValue(variable3.reference.Address, out object? tmp3) || (tmp3 is T3 t3 && !variable3.cmp.Invoke(t3))) ||
                (!result!.TryGetValue(variable4.reference.Address, out object? tmp4) || (tmp4 is T4 t4 && !variable4.cmp.Invoke(t4))) ||
                (!result!.TryGetValue(variable5.reference.Address, out object? tmp5) || (tmp5 is T5 t5 && !variable5.cmp.Invoke(t5))) ||
                (!result!.TryGetValue(variable6.reference.Address, out object? tmp6) || (tmp6 is T6 t6 && !variable6.cmp.Invoke(t6))) ||
                (!result!.TryGetValue(variable7.reference.Address, out object? tmp7) || (tmp7 is T7 t7 && !variable7.cmp.Invoke(t7))) ||
                (!result!.TryGetValue(variable8.reference.Address, out object? tmp8) || (tmp8 is T8 t8 && !variable8.cmp.Invoke(t8))) ||
                (!result!.TryGetValue(variable9.reference.Address, out object? tmp9) || (tmp9 is T9 t9 && !variable9.cmp.Invoke(t9))) ||
                (!result!.TryGetValue(variable10.reference.Address, out object? tmp10) || (tmp10 is T10 t10 && !variable10.cmp.Invoke(t10))) ||
                (!result!.TryGetValue(variable11.reference.Address, out object? tmp11) || (tmp11 is T11 t11 && !variable11.cmp.Invoke(t11))))
            {
                if (otherwise == null)
                {
                    return false;
                }

                return await otherwise!.Invoke().ConfigureAwait(false);
            }

            if (then == null)
            {
                return true;
            }

            return await then!.Invoke().ConfigureAwait(false);

        }

        public static async Task<bool> IfAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this PlcDataMapper papper,
                            (PlcReadReference reference, Func<T1, bool> cmp) variable1,
                            (PlcReadReference reference, Func<T2, bool> cmp) variable2,
                            (PlcReadReference reference, Func<T3, bool> cmp) variable3,
                            (PlcReadReference reference, Func<T4, bool> cmp) variable4,
                            (PlcReadReference reference, Func<T5, bool> cmp) variable5,
                            (PlcReadReference reference, Func<T6, bool> cmp) variable6,
                            (PlcReadReference reference, Func<T7, bool> cmp) variable7,
                            (PlcReadReference reference, Func<T8, bool> cmp) variable8,
                            (PlcReadReference reference, Func<T9, bool> cmp) variable9,
                            (PlcReadReference reference, Func<T10, bool> cmp) variable10,
                            (PlcReadReference reference, Func<T11, bool> cmp) variable11,
                            (PlcReadReference reference, Func<T12, bool> cmp) variable12,
                            Func<Task<bool>>? then = null,
                            Func<Task<bool>>? otherwise = null)
        {
            if (papper == null)
            {
                return ExceptionThrowHelper.ThrowArgumentNullException<bool>(nameof(papper));
            }

            System.Collections.Generic.Dictionary<string, object?>? result = (await papper.ReadAsync(
                                                 variable1.reference,
                                                 variable2.reference,
                                                 variable3.reference,
                                                 variable4.reference,
                                                 variable5.reference,
                                                 variable6.reference,
                                                 variable7.reference,
                                                 variable8.reference,
                                                 variable9.reference,
                                                 variable10.reference,
                                                 variable11.reference,
                                                 variable12.reference).ConfigureAwait(false))?.ToDictionary(x => x.Variable, x => x.Value);

            if (result!.IsNullOrEmpty() ||
                (!result!.TryGetValue(variable1.reference.Address, out object? tmp1) || (tmp1 is T1 t1 && !variable1.cmp.Invoke(t1))) ||
                (!result!.TryGetValue(variable2.reference.Address, out object? tmp2) || (tmp2 is T2 t2 && !variable2.cmp.Invoke(t2))) ||
                (!result!.TryGetValue(variable3.reference.Address, out object? tmp3) || (tmp3 is T3 t3 && !variable3.cmp.Invoke(t3))) ||
                (!result!.TryGetValue(variable4.reference.Address, out object? tmp4) || (tmp4 is T4 t4 && !variable4.cmp.Invoke(t4))) ||
                (!result!.TryGetValue(variable5.reference.Address, out object? tmp5) || (tmp5 is T5 t5 && !variable5.cmp.Invoke(t5))) ||
                (!result!.TryGetValue(variable6.reference.Address, out object? tmp6) || (tmp6 is T6 t6 && !variable6.cmp.Invoke(t6))) ||
                (!result!.TryGetValue(variable7.reference.Address, out object? tmp7) || (tmp7 is T7 t7 && !variable7.cmp.Invoke(t7))) ||
                (!result!.TryGetValue(variable8.reference.Address, out object? tmp8) || (tmp8 is T8 t8 && !variable8.cmp.Invoke(t8))) ||
                (!result!.TryGetValue(variable9.reference.Address, out object? tmp9) || (tmp9 is T9 t9 && !variable9.cmp.Invoke(t9))) ||
                (!result!.TryGetValue(variable10.reference.Address, out object? tmp10) || (tmp10 is T10 t10 && !variable10.cmp.Invoke(t10))) ||
                (!result!.TryGetValue(variable11.reference.Address, out object? tmp11) || (tmp11 is T11 t11 && !variable11.cmp.Invoke(t11))) ||
                (!result!.TryGetValue(variable12.reference.Address, out object? tmp12) || (tmp12 is T12 t12 && !variable12.cmp.Invoke(t12))))
            {
                if (otherwise == null)
                {
                    return false;
                }

                return await otherwise!.Invoke().ConfigureAwait(false);
            }

            if (then == null)
            {
                return true;
            }

            return await then!.Invoke().ConfigureAwait(false);

        }

        public static async Task<bool> IfAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this PlcDataMapper papper,
                                    (PlcReadReference reference, Func<T1, bool> cmp) variable1,
                                    (PlcReadReference reference, Func<T2, bool> cmp) variable2,
                                    (PlcReadReference reference, Func<T3, bool> cmp) variable3,
                                    (PlcReadReference reference, Func<T4, bool> cmp) variable4,
                                    (PlcReadReference reference, Func<T5, bool> cmp) variable5,
                                    (PlcReadReference reference, Func<T6, bool> cmp) variable6,
                                    (PlcReadReference reference, Func<T7, bool> cmp) variable7,
                                    (PlcReadReference reference, Func<T8, bool> cmp) variable8,
                                    (PlcReadReference reference, Func<T9, bool> cmp) variable9,
                                    (PlcReadReference reference, Func<T10, bool> cmp) variable10,
                                    (PlcReadReference reference, Func<T11, bool> cmp) variable11,
                                    (PlcReadReference reference, Func<T12, bool> cmp) variable12,
                                    (PlcReadReference reference, Func<T13, bool> cmp) variable13,
                                    Func<Task<bool>>? then = null,
                                    Func<Task<bool>>? otherwise = null)
        {
            if (papper == null)
            {
                return ExceptionThrowHelper.ThrowArgumentNullException<bool>(nameof(papper));
            }

            System.Collections.Generic.Dictionary<string, object?>? result = (await papper.ReadAsync(
                                                 variable1.reference,
                                                 variable2.reference,
                                                 variable3.reference,
                                                 variable4.reference,
                                                 variable5.reference,
                                                 variable6.reference,
                                                 variable7.reference,
                                                 variable8.reference,
                                                 variable9.reference,
                                                 variable10.reference,
                                                 variable11.reference,
                                                 variable12.reference,
                                                 variable13.reference).ConfigureAwait(false))?.ToDictionary(x => x.Variable, x => x.Value);

            if (result!.IsNullOrEmpty() ||
                (!result!.TryGetValue(variable1.reference.Address, out object? tmp1) || (tmp1 is T1 t1 && !variable1.cmp.Invoke(t1))) ||
                (!result!.TryGetValue(variable2.reference.Address, out object? tmp2) || (tmp2 is T2 t2 && !variable2.cmp.Invoke(t2))) ||
                (!result!.TryGetValue(variable3.reference.Address, out object? tmp3) || (tmp3 is T3 t3 && !variable3.cmp.Invoke(t3))) ||
                (!result!.TryGetValue(variable4.reference.Address, out object? tmp4) || (tmp4 is T4 t4 && !variable4.cmp.Invoke(t4))) ||
                (!result!.TryGetValue(variable5.reference.Address, out object? tmp5) || (tmp5 is T5 t5 && !variable5.cmp.Invoke(t5))) ||
                (!result!.TryGetValue(variable6.reference.Address, out object? tmp6) || (tmp6 is T6 t6 && !variable6.cmp.Invoke(t6))) ||
                (!result!.TryGetValue(variable7.reference.Address, out object? tmp7) || (tmp7 is T7 t7 && !variable7.cmp.Invoke(t7))) ||
                (!result!.TryGetValue(variable8.reference.Address, out object? tmp8) || (tmp8 is T8 t8 && !variable8.cmp.Invoke(t8))) ||
                (!result!.TryGetValue(variable9.reference.Address, out object? tmp9) || (tmp9 is T9 t9 && !variable9.cmp.Invoke(t9))) ||
                (!result!.TryGetValue(variable10.reference.Address, out object? tmp10) || (tmp10 is T10 t10 && !variable10.cmp.Invoke(t10))) ||
                (!result!.TryGetValue(variable11.reference.Address, out object? tmp11) || (tmp11 is T11 t11 && !variable11.cmp.Invoke(t11))) ||
                (!result!.TryGetValue(variable12.reference.Address, out object? tmp12) || (tmp12 is T12 t12 && !variable12.cmp.Invoke(t12))) ||
                (!result!.TryGetValue(variable13.reference.Address, out object? tmp13) || (tmp13 is T13 t13 && !variable13.cmp.Invoke(t13))))
            {
                if (otherwise == null)
                {
                    return false;
                }

                return await otherwise!.Invoke().ConfigureAwait(false);
            }

            if (then == null)
            {
                return true;
            }

            return await then!.Invoke().ConfigureAwait(false);

        }

        public static async Task<bool> IfAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this PlcDataMapper papper,
                                            (PlcReadReference reference, Func<T1, bool> cmp) variable1,
                                            (PlcReadReference reference, Func<T2, bool> cmp) variable2,
                                            (PlcReadReference reference, Func<T3, bool> cmp) variable3,
                                            (PlcReadReference reference, Func<T4, bool> cmp) variable4,
                                            (PlcReadReference reference, Func<T5, bool> cmp) variable5,
                                            (PlcReadReference reference, Func<T6, bool> cmp) variable6,
                                            (PlcReadReference reference, Func<T7, bool> cmp) variable7,
                                            (PlcReadReference reference, Func<T8, bool> cmp) variable8,
                                            (PlcReadReference reference, Func<T9, bool> cmp) variable9,
                                            (PlcReadReference reference, Func<T10, bool> cmp) variable10,
                                            (PlcReadReference reference, Func<T11, bool> cmp) variable11,
                                            (PlcReadReference reference, Func<T12, bool> cmp) variable12,
                                            (PlcReadReference reference, Func<T13, bool> cmp) variable13,
                                            (PlcReadReference reference, Func<T14, bool> cmp) variable14,
                                            Func<Task<bool>>? then = null,
                                            Func<Task<bool>>? otherwise = null)
        {
            if (papper == null)
            {
                return ExceptionThrowHelper.ThrowArgumentNullException<bool>(nameof(papper));
            }

            System.Collections.Generic.Dictionary<string, object?>? result = (await papper.ReadAsync(
                                                 variable1.reference,
                                                 variable2.reference,
                                                 variable3.reference,
                                                 variable4.reference,
                                                 variable5.reference,
                                                 variable6.reference,
                                                 variable7.reference,
                                                 variable8.reference,
                                                 variable9.reference,
                                                 variable10.reference,
                                                 variable11.reference,
                                                 variable12.reference,
                                                 variable13.reference,
                                                 variable14.reference).ConfigureAwait(false))?.ToDictionary(x => x.Variable, x => x.Value);

            if (result!.IsNullOrEmpty() ||
                (!result!.TryGetValue(variable1.reference.Address, out object? tmp1) || (tmp1 is T1 t1 && !variable1.cmp.Invoke(t1))) ||
                (!result!.TryGetValue(variable2.reference.Address, out object? tmp2) || (tmp2 is T2 t2 && !variable2.cmp.Invoke(t2))) ||
                (!result!.TryGetValue(variable3.reference.Address, out object? tmp3) || (tmp3 is T3 t3 && !variable3.cmp.Invoke(t3))) ||
                (!result!.TryGetValue(variable4.reference.Address, out object? tmp4) || (tmp4 is T4 t4 && !variable4.cmp.Invoke(t4))) ||
                (!result!.TryGetValue(variable5.reference.Address, out object? tmp5) || (tmp5 is T5 t5 && !variable5.cmp.Invoke(t5))) ||
                (!result!.TryGetValue(variable6.reference.Address, out object? tmp6) || (tmp6 is T6 t6 && !variable6.cmp.Invoke(t6))) ||
                (!result!.TryGetValue(variable7.reference.Address, out object? tmp7) || (tmp7 is T7 t7 && !variable7.cmp.Invoke(t7))) ||
                (!result!.TryGetValue(variable8.reference.Address, out object? tmp8) || (tmp8 is T8 t8 && !variable8.cmp.Invoke(t8))) ||
                (!result!.TryGetValue(variable9.reference.Address, out object? tmp9) || (tmp9 is T9 t9 && !variable9.cmp.Invoke(t9))) ||
                (!result!.TryGetValue(variable10.reference.Address, out object? tmp10) || (tmp10 is T10 t10 && !variable10.cmp.Invoke(t10))) ||
                (!result!.TryGetValue(variable11.reference.Address, out object? tmp11) || (tmp11 is T11 t11 && !variable11.cmp.Invoke(t11))) ||
                (!result!.TryGetValue(variable12.reference.Address, out object? tmp12) || (tmp12 is T12 t12 && !variable12.cmp.Invoke(t12))) ||
                (!result!.TryGetValue(variable13.reference.Address, out object? tmp13) || (tmp13 is T13 t13 && !variable13.cmp.Invoke(t13))) ||
                (!result!.TryGetValue(variable14.reference.Address, out object? tmp14) || (tmp14 is T14 t14 && !variable14.cmp.Invoke(t14))))
            {
                if (otherwise == null)
                {
                    return false;
                }

                return await otherwise!.Invoke().ConfigureAwait(false);
            }

            if (then == null)
            {
                return true;
            }

            return await then!.Invoke().ConfigureAwait(false);

        }

        public static async Task<bool> IfAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this PlcDataMapper papper,
                                                    (PlcReadReference reference, Func<T1, bool> cmp) variable1,
                                                    (PlcReadReference reference, Func<T2, bool> cmp) variable2,
                                                    (PlcReadReference reference, Func<T3, bool> cmp) variable3,
                                                    (PlcReadReference reference, Func<T4, bool> cmp) variable4,
                                                    (PlcReadReference reference, Func<T5, bool> cmp) variable5,
                                                    (PlcReadReference reference, Func<T6, bool> cmp) variable6,
                                                    (PlcReadReference reference, Func<T7, bool> cmp) variable7,
                                                    (PlcReadReference reference, Func<T8, bool> cmp) variable8,
                                                    (PlcReadReference reference, Func<T9, bool> cmp) variable9,
                                                    (PlcReadReference reference, Func<T10, bool> cmp) variable10,
                                                    (PlcReadReference reference, Func<T11, bool> cmp) variable11,
                                                    (PlcReadReference reference, Func<T12, bool> cmp) variable12,
                                                    (PlcReadReference reference, Func<T13, bool> cmp) variable13,
                                                    (PlcReadReference reference, Func<T14, bool> cmp) variable14,
                                                    (PlcReadReference reference, Func<T15, bool> cmp) variable15,
                                                    Func<Task<bool>>? then = null,
                                                    Func<Task<bool>>? otherwise = null)
        {
            if (papper == null)
            {
                return ExceptionThrowHelper.ThrowArgumentNullException<bool>(nameof(papper));
            }

            System.Collections.Generic.Dictionary<string, object?>? result = (await papper.ReadAsync(
                                                 variable1.reference,
                                                 variable2.reference,
                                                 variable3.reference,
                                                 variable4.reference,
                                                 variable5.reference,
                                                 variable6.reference,
                                                 variable7.reference,
                                                 variable8.reference,
                                                 variable9.reference,
                                                 variable10.reference,
                                                 variable11.reference,
                                                 variable12.reference,
                                                 variable13.reference,
                                                 variable14.reference,
                                                 variable15.reference).ConfigureAwait(false))?.ToDictionary(x => x.Variable, x => x.Value);

            if (result!.IsNullOrEmpty() ||
                (!result!.TryGetValue(variable1.reference.Address, out object? tmp1) || (tmp1 is T1 t1 && !variable1.cmp.Invoke(t1))) ||
                (!result!.TryGetValue(variable2.reference.Address, out object? tmp2) || (tmp2 is T2 t2 && !variable2.cmp.Invoke(t2))) ||
                (!result!.TryGetValue(variable3.reference.Address, out object? tmp3) || (tmp3 is T3 t3 && !variable3.cmp.Invoke(t3))) ||
                (!result!.TryGetValue(variable4.reference.Address, out object? tmp4) || (tmp4 is T4 t4 && !variable4.cmp.Invoke(t4))) ||
                (!result!.TryGetValue(variable5.reference.Address, out object? tmp5) || (tmp5 is T5 t5 && !variable5.cmp.Invoke(t5))) ||
                (!result!.TryGetValue(variable6.reference.Address, out object? tmp6) || (tmp6 is T6 t6 && !variable6.cmp.Invoke(t6))) ||
                (!result!.TryGetValue(variable7.reference.Address, out object? tmp7) || (tmp7 is T7 t7 && !variable7.cmp.Invoke(t7))) ||
                (!result!.TryGetValue(variable8.reference.Address, out object? tmp8) || (tmp8 is T8 t8 && !variable8.cmp.Invoke(t8))) ||
                (!result!.TryGetValue(variable9.reference.Address, out object? tmp9) || (tmp9 is T9 t9 && !variable9.cmp.Invoke(t9))) ||
                (!result!.TryGetValue(variable10.reference.Address, out object? tmp10) || (tmp10 is T10 t10 && !variable10.cmp.Invoke(t10))) ||
                (!result!.TryGetValue(variable11.reference.Address, out object? tmp11) || (tmp11 is T11 t11 && !variable11.cmp.Invoke(t11))) ||
                (!result!.TryGetValue(variable12.reference.Address, out object? tmp12) || (tmp12 is T12 t12 && !variable12.cmp.Invoke(t12))) ||
                (!result!.TryGetValue(variable13.reference.Address, out object? tmp13) || (tmp13 is T13 t13 && !variable13.cmp.Invoke(t13))) ||
                (!result!.TryGetValue(variable14.reference.Address, out object? tmp14) || (tmp14 is T14 t14 && !variable14.cmp.Invoke(t14))) ||
                (!result!.TryGetValue(variable15.reference.Address, out object? tmp15) || (tmp15 is T15 t15 && !variable15.cmp.Invoke(t15))))
            {
                if (otherwise == null)
                {
                    return false;
                }

                return await otherwise!.Invoke().ConfigureAwait(false);
            }

            if (then == null)
            {
                return true;
            }

            return await then!.Invoke().ConfigureAwait(false);

        }

        public static async Task<bool> IfAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this PlcDataMapper papper,
                                                    (PlcReadReference reference, Func<T1, bool> cmp) variable1,
                                                    (PlcReadReference reference, Func<T2, bool> cmp) variable2,
                                                    (PlcReadReference reference, Func<T3, bool> cmp) variable3,
                                                    (PlcReadReference reference, Func<T4, bool> cmp) variable4,
                                                    (PlcReadReference reference, Func<T5, bool> cmp) variable5,
                                                    (PlcReadReference reference, Func<T6, bool> cmp) variable6,
                                                    (PlcReadReference reference, Func<T7, bool> cmp) variable7,
                                                    (PlcReadReference reference, Func<T8, bool> cmp) variable8,
                                                    (PlcReadReference reference, Func<T9, bool> cmp) variable9,
                                                    (PlcReadReference reference, Func<T10, bool> cmp) variable10,
                                                    (PlcReadReference reference, Func<T11, bool> cmp) variable11,
                                                    (PlcReadReference reference, Func<T12, bool> cmp) variable12,
                                                    (PlcReadReference reference, Func<T13, bool> cmp) variable13,
                                                    (PlcReadReference reference, Func<T14, bool> cmp) variable14,
                                                    (PlcReadReference reference, Func<T15, bool> cmp) variable15,
                                                    (PlcReadReference reference, Func<T16, bool> cmp) variable16,
                                                    Func<Task<bool>>? then = null,
                                                    Func<Task<bool>>? otherwise = null)
        {
            if (papper == null)
            {
                return ExceptionThrowHelper.ThrowArgumentNullException<bool>(nameof(papper));
            }

            System.Collections.Generic.Dictionary<string, object?>? result = (await papper.ReadAsync(
                                                 variable1.reference,
                                                 variable2.reference,
                                                 variable3.reference,
                                                 variable4.reference,
                                                 variable5.reference,
                                                 variable6.reference,
                                                 variable7.reference,
                                                 variable8.reference,
                                                 variable9.reference,
                                                 variable10.reference,
                                                 variable11.reference,
                                                 variable12.reference,
                                                 variable13.reference,
                                                 variable14.reference,
                                                 variable15.reference,
                                                 variable16.reference).ConfigureAwait(false))?.ToDictionary(x => x.Variable, x => x.Value);

            if (result!.IsNullOrEmpty() ||
                (!result!.TryGetValue(variable1.reference.Address, out object? tmp1) || (tmp1 is T1 t1 && !variable1.cmp.Invoke(t1))) ||
                (!result!.TryGetValue(variable2.reference.Address, out object? tmp2) || (tmp2 is T2 t2 && !variable2.cmp.Invoke(t2))) ||
                (!result!.TryGetValue(variable3.reference.Address, out object? tmp3) || (tmp3 is T3 t3 && !variable3.cmp.Invoke(t3))) ||
                (!result!.TryGetValue(variable4.reference.Address, out object? tmp4) || (tmp4 is T4 t4 && !variable4.cmp.Invoke(t4))) ||
                (!result!.TryGetValue(variable5.reference.Address, out object? tmp5) || (tmp5 is T5 t5 && !variable5.cmp.Invoke(t5))) ||
                (!result!.TryGetValue(variable6.reference.Address, out object? tmp6) || (tmp6 is T6 t6 && !variable6.cmp.Invoke(t6))) ||
                (!result!.TryGetValue(variable7.reference.Address, out object? tmp7) || (tmp7 is T7 t7 && !variable7.cmp.Invoke(t7))) ||
                (!result!.TryGetValue(variable8.reference.Address, out object? tmp8) || (tmp8 is T8 t8 && !variable8.cmp.Invoke(t8))) ||
                (!result!.TryGetValue(variable9.reference.Address, out object? tmp9) || (tmp9 is T9 t9 && !variable9.cmp.Invoke(t9))) ||
                (!result!.TryGetValue(variable10.reference.Address, out object? tmp10) || (tmp10 is T10 t10 && !variable10.cmp.Invoke(t10))) ||
                (!result!.TryGetValue(variable11.reference.Address, out object? tmp11) || (tmp11 is T11 t11 && !variable11.cmp.Invoke(t11))) ||
                (!result!.TryGetValue(variable12.reference.Address, out object? tmp12) || (tmp12 is T12 t12 && !variable12.cmp.Invoke(t12))) ||
                (!result!.TryGetValue(variable13.reference.Address, out object? tmp13) || (tmp13 is T13 t13 && !variable13.cmp.Invoke(t13))) ||
                (!result!.TryGetValue(variable14.reference.Address, out object? tmp14) || (tmp14 is T14 t14 && !variable14.cmp.Invoke(t14))) ||
                (!result!.TryGetValue(variable15.reference.Address, out object? tmp15) || (tmp15 is T15 t15 && !variable15.cmp.Invoke(t15))) ||
                (!result!.TryGetValue(variable16.reference.Address, out object? tmp16) || (tmp16 is T16 t16 && !variable16.cmp.Invoke(t16))))
            {
                if (otherwise == null)
                {
                    return false;
                }

                return await otherwise!.Invoke().ConfigureAwait(false);
            }

            if (then == null)
            {
                return true;
            }

            return await then!.Invoke().ConfigureAwait(false);

        }







        public static async Task<bool> IfAsync<T1, T2>(this PlcDataMapper papper,
                                                            PlcReadReference variable1,
                                                            PlcReadReference variable2,
                                                            Func<T1, T2, bool> cmp,
                                                            Func<Task<bool>>? then = null,
                                                            Func<Task<bool>>? otherwise = null)
        {
            if (papper == null)
            {
                return ExceptionThrowHelper.ThrowArgumentNullException<bool>(nameof(papper));
            }
            if(cmp == null)
            {
                return ExceptionThrowHelper.ThrowArgumentNullException<bool>(nameof(cmp));
            }

            System.Collections.Generic.Dictionary<string, object?>? result = (await papper.ReadAsync(variable1,
                                                 variable2).ConfigureAwait(false))?.ToDictionary(x => x.Variable, x => x.Value);

            if (result!.IsNullOrEmpty() ||
                (!result!.TryGetValue(variable1.Address, out object? tmp1)) || !(tmp1 is T1 t1) ||
                (!result!.TryGetValue(variable2.Address, out object? tmp2)) || !(tmp2 is T2 t2) ||
                !cmp(t1, t2))
            {
                if (otherwise == null)
                {
                    return false;
                }

                return await otherwise!.Invoke().ConfigureAwait(false);
            }

            if (then == null)
            {
                return true;
            }

            return await then!.Invoke().ConfigureAwait(false);

        }



        public static async Task<bool> IfAsync<T1, T2, T3>(this PlcDataMapper papper,
                                                    PlcReadReference variable1,
                                                    PlcReadReference variable2,
                                                    PlcReadReference variable3,
                                                    Func<T1, T2, T3, bool> cmp,
                                                    Func<Task<bool>>? then = null,
                                                    Func<Task<bool>>? otherwise = null)
        {
            if (papper == null)
            {
                return ExceptionThrowHelper.ThrowArgumentNullException<bool>(nameof(papper));
            }
            if (cmp == null)
            {
                return ExceptionThrowHelper.ThrowArgumentNullException<bool>(nameof(cmp));
            }

            System.Collections.Generic.Dictionary<string, object?>? result = (await papper.ReadAsync(variable1,
                                                 variable2,
                                                 variable3).ConfigureAwait(false))?.ToDictionary(x => x.Variable, x => x.Value);

            if (result!.IsNullOrEmpty() ||
                (!result!.TryGetValue(variable1.Address, out object? tmp1)) || !(tmp1 is T1 t1) ||
                (!result!.TryGetValue(variable2.Address, out object? tmp2)) || !(tmp2 is T2 t2) ||
                (!result!.TryGetValue(variable3.Address, out object? tmp3)) || !(tmp3 is T3 t3) ||
                !cmp(t1, t2, t3))
            {
                if (otherwise == null)
                {
                    return false;
                }

                return await otherwise!.Invoke().ConfigureAwait(false);
            }

            if (then == null)
            {
                return true;
            }

            return await then!.Invoke().ConfigureAwait(false);

        }

        public static async Task<bool> IfAsync<T1, T2, T3, T4>(this PlcDataMapper papper,
            PlcReadReference variable1,
            PlcReadReference variable2,
            PlcReadReference variable3,
            PlcReadReference variable4,
            Func<T1, T2, T3, T4, bool> cmp,
            Func<Task<bool>>? then = null,
            Func<Task<bool>>? otherwise = null)
        {
            if (papper == null)
            {
                return ExceptionThrowHelper.ThrowArgumentNullException<bool>(nameof(papper));
            }
            if (cmp == null)
            {
                return ExceptionThrowHelper.ThrowArgumentNullException<bool>(nameof(cmp));
            }

            System.Collections.Generic.Dictionary<string, object?>? result = (await papper.ReadAsync(variable1,
                                                 variable2,
                                                 variable3,
                                                 variable4).ConfigureAwait(false))?.ToDictionary(x => x.Variable, x => x.Value);

            if (result!.IsNullOrEmpty() ||
                (!result!.TryGetValue(variable1.Address, out object? tmp1)) || !(tmp1 is T1 t1) ||
                (!result!.TryGetValue(variable2.Address, out object? tmp2)) || !(tmp2 is T2 t2) ||
                (!result!.TryGetValue(variable3.Address, out object? tmp3)) || !(tmp3 is T3 t3) ||
                (!result!.TryGetValue(variable4.Address, out object? tmp4)) || !(tmp4 is T4 t4) ||
                !cmp(t1, t2, t3, t4))
            {
                if (otherwise == null)
                {
                    return false;
                }

                return await otherwise!.Invoke().ConfigureAwait(false);
            }

            if (then == null)
            {
                return true;
            }

            return await then!.Invoke().ConfigureAwait(false);

        }


        public static async Task<bool> IfAsync<T1, T2, T3, T4, T5>(this PlcDataMapper papper,
                    PlcReadReference variable1,
                    PlcReadReference variable2,
                    PlcReadReference variable3,
                    PlcReadReference variable4,
                    PlcReadReference variable5,
                    Func<T1, T2, T3, T4, T5, bool> cmp,
                    Func<Task<bool>>? then = null,
                    Func<Task<bool>>? otherwise = null)
        {
            if (papper == null)
            {
                return ExceptionThrowHelper.ThrowArgumentNullException<bool>(nameof(papper));
            }
            if (cmp == null)
            {
                return ExceptionThrowHelper.ThrowArgumentNullException<bool>(nameof(cmp));
            }

            System.Collections.Generic.Dictionary<string, object?>? result = (await papper.ReadAsync(variable1,
                                                 variable2,
                                                 variable3,
                                                 variable4,
                                                 variable5).ConfigureAwait(false))?.ToDictionary(x => x.Variable, x => x.Value);

            if (result!.IsNullOrEmpty() ||
                (!result!.TryGetValue(variable1.Address, out object? tmp1)) || !(tmp1 is T1 t1) ||
                (!result!.TryGetValue(variable2.Address, out object? tmp2)) || !(tmp2 is T2 t2) ||
                (!result!.TryGetValue(variable3.Address, out object? tmp3)) || !(tmp3 is T3 t3) ||
                (!result!.TryGetValue(variable4.Address, out object? tmp4)) || !(tmp4 is T4 t4) ||
                (!result!.TryGetValue(variable5.Address, out object? tmp5)) || !(tmp5 is T5 t5) ||
                !cmp(t1, t2, t3, t4, t5))
            {
                if (otherwise == null)
                {
                    return false;
                }

                return await otherwise!.Invoke().ConfigureAwait(false);
            }

            if (then == null)
            {
                return true;
            }

            return await then!.Invoke().ConfigureAwait(false);

        }


        public static async Task<bool> IfAsync<T1, T2, T3, T4, T5, T6>(this PlcDataMapper papper,
                            PlcReadReference variable1,
                            PlcReadReference variable2,
                            PlcReadReference variable3,
                            PlcReadReference variable4,
                            PlcReadReference variable5,
                            PlcReadReference variable6,
                            Func<T1, T2, T3, T4, T5, T6, bool> cmp,
                            Func<Task<bool>>? then = null,
                            Func<Task<bool>>? otherwise = null)
        {
            if (papper == null)
            {
                return ExceptionThrowHelper.ThrowArgumentNullException<bool>(nameof(papper));
            }
            if (cmp == null)
            {
                return ExceptionThrowHelper.ThrowArgumentNullException<bool>(nameof(cmp));
            }

            System.Collections.Generic.Dictionary<string, object?>? result = (await papper.ReadAsync(variable1,
                                                 variable2,
                                                 variable3,
                                                 variable4,
                                                 variable5,
                                                 variable6).ConfigureAwait(false))?.ToDictionary(x => x.Variable, x => x.Value);

            if (result!.IsNullOrEmpty() ||
                (!result!.TryGetValue(variable1.Address, out object? tmp1)) || !(tmp1 is T1 t1) ||
                (!result!.TryGetValue(variable2.Address, out object? tmp2)) || !(tmp2 is T2 t2) ||
                (!result!.TryGetValue(variable3.Address, out object? tmp3)) || !(tmp3 is T3 t3) ||
                (!result!.TryGetValue(variable4.Address, out object? tmp4)) || !(tmp4 is T4 t4) ||
                (!result!.TryGetValue(variable5.Address, out object? tmp5)) || !(tmp5 is T5 t5) ||
                (!result!.TryGetValue(variable6.Address, out object? tmp6)) || !(tmp6 is T6 t6) ||
                !cmp(t1, t2, t3, t4, t5, t6))
            {
                if (otherwise == null)
                {
                    return false;
                }

                return await otherwise!.Invoke().ConfigureAwait(false);
            }

            if (then == null)
            {
                return true;
            }

            return await then!.Invoke().ConfigureAwait(false);

        }


        public static async Task<bool> IfAsync<T1, T2, T3, T4, T5, T6, T7>(this PlcDataMapper papper,
                                    PlcReadReference variable1,
                                    PlcReadReference variable2,
                                    PlcReadReference variable3,
                                    PlcReadReference variable4,
                                    PlcReadReference variable5,
                                    PlcReadReference variable6,
                                    PlcReadReference variable7,
                                    Func<T1, T2, T3, T4, T5, T6, T7, bool> cmp,
                                    Func<Task<bool>>? then = null,
                                    Func<Task<bool>>? otherwise = null)
        {
            if (papper == null)
            {
                return ExceptionThrowHelper.ThrowArgumentNullException<bool>(nameof(papper));
            }
            if (cmp == null)
            {
                return ExceptionThrowHelper.ThrowArgumentNullException<bool>(nameof(cmp));
            }

            System.Collections.Generic.Dictionary<string, object?>? result = (await papper.ReadAsync(variable1,
                                                 variable2,
                                                 variable3,
                                                 variable4,
                                                 variable5,
                                                 variable6,
                                                 variable7).ConfigureAwait(false))?.ToDictionary(x => x.Variable, x => x.Value);

            if (result!.IsNullOrEmpty() ||
                (!result!.TryGetValue(variable1.Address, out object? tmp1)) || !(tmp1 is T1 t1) ||
                (!result!.TryGetValue(variable2.Address, out object? tmp2)) || !(tmp2 is T2 t2) ||
                (!result!.TryGetValue(variable3.Address, out object? tmp3)) || !(tmp3 is T3 t3) ||
                (!result!.TryGetValue(variable4.Address, out object? tmp4)) || !(tmp4 is T4 t4) ||
                (!result!.TryGetValue(variable5.Address, out object? tmp5)) || !(tmp5 is T5 t5) ||
                (!result!.TryGetValue(variable6.Address, out object? tmp6)) || !(tmp6 is T6 t6) ||
                (!result!.TryGetValue(variable7.Address, out object? tmp7)) || !(tmp7 is T7 t7) ||
                !cmp(t1, t2, t3, t4, t5, t6, t7))
            {
                if (otherwise == null)
                {
                    return false;
                }

                return await otherwise!.Invoke().ConfigureAwait(false);
            }

            if (then == null)
            {
                return true;
            }

            return await then!.Invoke().ConfigureAwait(false);

        }

        public static async Task<bool> IfAsync<T1, T2, T3, T4, T5, T6, T7, T8>(this PlcDataMapper papper,
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
                                            Func<Task<bool>>? otherwise = null)
        {
            if (papper == null)
            {
                return ExceptionThrowHelper.ThrowArgumentNullException<bool>(nameof(papper));
            }
            if (cmp == null)
            {
                return ExceptionThrowHelper.ThrowArgumentNullException<bool>(nameof(cmp));
            }

            System.Collections.Generic.Dictionary<string, object?>? result = (await papper.ReadAsync(variable1,
                                                 variable2,
                                                 variable3,
                                                 variable4,
                                                 variable5,
                                                 variable6,
                                                 variable7,
                                                 variable8).ConfigureAwait(false))?.ToDictionary(x => x.Variable, x => x.Value);

            if (result!.IsNullOrEmpty() ||
                (!result!.TryGetValue(variable1.Address, out object? tmp1)) || !(tmp1 is T1 t1) ||
                (!result!.TryGetValue(variable2.Address, out object? tmp2)) || !(tmp2 is T2 t2) ||
                (!result!.TryGetValue(variable3.Address, out object? tmp3)) || !(tmp3 is T3 t3) ||
                (!result!.TryGetValue(variable4.Address, out object? tmp4)) || !(tmp4 is T4 t4) ||
                (!result!.TryGetValue(variable5.Address, out object? tmp5)) || !(tmp5 is T5 t5) ||
                (!result!.TryGetValue(variable6.Address, out object? tmp6)) || !(tmp6 is T6 t6) ||
                (!result!.TryGetValue(variable7.Address, out object? tmp7)) || !(tmp7 is T7 t7) ||
                (!result!.TryGetValue(variable8.Address, out object? tmp8)) || !(tmp8 is T8 t8) ||
                !cmp(t1, t2, t3, t4, t5, t6, t7, t8))
            {
                if (otherwise == null)
                {
                    return false;
                }

                return await otherwise!.Invoke().ConfigureAwait(false);
            }

            if (then == null)
            {
                return true;
            }

            return await then!.Invoke().ConfigureAwait(false);

        }



    }
}
