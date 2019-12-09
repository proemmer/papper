using Papper.Internal;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Papper.Extensions.Conditions
{
    public static class IfConditionExtensions
    {

        public async static Task<bool> IfAsync<T1>(this PlcDataMapper papper,
                                                    (PlcReadReference reference, Func<T1, bool> cmp) variable1,
                                                    Func<Task<bool>>? then = null,
                                                    Func<Task<bool>>? otherwise = null)
        {
            if (papper == null) ExceptionThrowHelper.ThrowArgumentNullException(nameof(papper));

            var result = (await papper.ReadAsync(variable1.reference).ConfigureAwait(false))?.ToDictionary(x => x.Variable, x => x.Value);

            if (result.IsNullOrEmpty() ||
                (!result.TryGetValue(variable1.reference.Address, out var tmp1) || !variable1.cmp.Invoke((T1)tmp1)))
            {
                if (otherwise == null) return false;
                return await otherwise!.Invoke().ConfigureAwait(false);
            }

            if (then == null) return true;
            return await then!.Invoke().ConfigureAwait(false);

        }



        public async static Task<bool> IfAsync<T1, T2>(this PlcDataMapper papper,
                                                            (PlcReadReference reference, Func<T1, bool> cmp) variable1,
                                                            (PlcReadReference reference, Func<T2, bool> cmp) variable2,
                                                            Func<Task<bool>>? then = null,
                                                            Func<Task<bool>>? otherwise = null)
        {
            if (papper == null) ExceptionThrowHelper.ThrowArgumentNullException(nameof(papper));

            var result = (await papper.ReadAsync(variable1.reference,
                                                variable2.reference).ConfigureAwait(false))?.ToDictionary(x => x.Variable, x => x.Value);

            if (result.IsNullOrEmpty() ||
                (!result.TryGetValue(variable1.reference.Address, out var tmp1) || !variable1.cmp.Invoke((T1)tmp1)) ||
                (!result.TryGetValue(variable2.reference.Address, out var tmp2) || !variable2.cmp.Invoke((T2)tmp2)))
            {
                if (otherwise == null) return false;
                return await otherwise!.Invoke().ConfigureAwait(false);
            }

            if(then == null) return true;
            return await then!.Invoke().ConfigureAwait(false);

        }

        public async static Task<bool> IfAsync<T1, T2, T3>(this PlcDataMapper papper,
                    (PlcReadReference reference, Func<T1, bool> cmp) variable1,
                    (PlcReadReference reference, Func<T2, bool> cmp) variable2,
                    (PlcReadReference reference, Func<T3, bool> cmp) variable3,
                    Func<Task<bool>>? then = null,
                    Func<Task<bool>>? otherwise = null)
        {
            if (papper == null) ExceptionThrowHelper.ThrowArgumentNullException(nameof(papper));

            var result = (await papper.ReadAsync(
                                                 variable1.reference,
                                                 variable2.reference,
                                                 variable3.reference).ConfigureAwait(false))?.ToDictionary(x => x.Variable, x => x.Value);

            if (result.IsNullOrEmpty() ||
                (!result.TryGetValue(variable1.reference.Address, out var tmp1) || !variable1.cmp.Invoke((T1)tmp1)) ||
                (!result.TryGetValue(variable2.reference.Address, out var tmp2) || !variable2.cmp.Invoke((T2)tmp2)) ||
                (!result.TryGetValue(variable3.reference.Address, out var tmp3) || !variable3.cmp.Invoke((T3)tmp3)))
            {
                if (otherwise == null) return false;
                return await otherwise!.Invoke().ConfigureAwait(false);
            }

            if (then == null) return true;
            return await then!.Invoke().ConfigureAwait(false);

        }

        public async static Task<bool> IfAsync<T1, T2, T3, T4>(this PlcDataMapper papper,
                (PlcReadReference reference, Func<T1, bool> cmp) variable1,
                (PlcReadReference reference, Func<T2, bool> cmp) variable2,
                (PlcReadReference reference, Func<T3, bool> cmp) variable3,
                (PlcReadReference reference, Func<T4, bool> cmp) variable4,
                Func<Task<bool>>? then = null,
                Func<Task<bool>>? otherwise = null)
        {
            if (papper == null) ExceptionThrowHelper.ThrowArgumentNullException(nameof(papper));

            var result = (await papper.ReadAsync(
                                                 variable1.reference,
                                                 variable2.reference,
                                                 variable3.reference,
                                                 variable4.reference).ConfigureAwait(false))?.ToDictionary(x => x.Variable, x => x.Value);

            if (result.IsNullOrEmpty() ||
                (!result.TryGetValue(variable1.reference.Address, out var tmp1) || !variable1.cmp.Invoke((T1)tmp1)) ||
                (!result.TryGetValue(variable2.reference.Address, out var tmp2) || !variable2.cmp.Invoke((T2)tmp2)) ||
                (!result.TryGetValue(variable3.reference.Address, out var tmp3) || !variable3.cmp.Invoke((T3)tmp3)) ||
                (!result.TryGetValue(variable4.reference.Address, out var tmp4) || !variable4.cmp.Invoke((T4)tmp4)))
            {
                if (otherwise == null) return false;
                return await otherwise!.Invoke().ConfigureAwait(false);
            }

            if (then == null) return true;
            return await then!.Invoke().ConfigureAwait(false);

        }

        public async static Task<bool> IfAsync<T1, T2, T3, T4, T5>(this PlcDataMapper papper,
                        (PlcReadReference reference, Func<T1, bool> cmp) variable1,
                        (PlcReadReference reference, Func<T2, bool> cmp) variable2,
                        (PlcReadReference reference, Func<T3, bool> cmp) variable3,
                        (PlcReadReference reference, Func<T4, bool> cmp) variable4,
                        (PlcReadReference reference, Func<T5, bool> cmp) variable5,
                        Func<Task<bool>>? then = null,
                        Func<Task<bool>>? otherwise = null)
        {

            if (papper == null) ExceptionThrowHelper.ThrowArgumentNullException(nameof(papper));

            var result = (await papper.ReadAsync(
                                                 variable1.reference,
                                                 variable2.reference,
                                                 variable3.reference,
                                                 variable4.reference,
                                                 variable5.reference).ConfigureAwait(false))?.ToDictionary(x => x.Variable, x => x.Value);

            if (result!.IsNullOrEmpty() ||
                (!result!.TryGetValue(variable1.reference.Address, out var tmp1) || !variable1.cmp.Invoke((T1)tmp1)) ||
                (!result!.TryGetValue(variable2.reference.Address, out var tmp2) || !variable2.cmp.Invoke((T2)tmp2)) ||
                (!result!.TryGetValue(variable3.reference.Address, out var tmp3) || !variable3.cmp.Invoke((T3)tmp3)) ||
                (!result!.TryGetValue(variable4.reference.Address, out var tmp4) || !variable4.cmp.Invoke((T4)tmp4)) ||
                (!result!.TryGetValue(variable5.reference.Address, out var tmp5) || !variable5.cmp.Invoke((T5)tmp5)))
            {
                if (otherwise == null) return false;
                return await otherwise!.Invoke().ConfigureAwait(false);
            }

            if (then == null) return true;
            return await then!.Invoke().ConfigureAwait(false);

        }

        public async static Task<bool> IfAsync<T1, T2, T3, T4, T5, T6>(this PlcDataMapper papper,
                (PlcReadReference reference, Func<T1, bool> cmp) variable1,
                (PlcReadReference reference, Func<T2, bool> cmp) variable2,
                (PlcReadReference reference, Func<T3, bool> cmp) variable3,
                (PlcReadReference reference, Func<T4, bool> cmp) variable4,
                (PlcReadReference reference, Func<T5, bool> cmp) variable5,
                (PlcReadReference reference, Func<T6, bool> cmp) variable6,
                Func<Task<bool>>? then = null,
                Func<Task<bool>>? otherwise = null)
        {
            if (papper == null) ExceptionThrowHelper.ThrowArgumentNullException(nameof(papper));

            var result = (await papper.ReadAsync(
                                                 variable1.reference,
                                                 variable2.reference,
                                                 variable3.reference,
                                                 variable4.reference,
                                                 variable5.reference,
                                                 variable6.reference).ConfigureAwait(false))?.ToDictionary(x => x.Variable, x => x.Value);

            if (result.IsNullOrEmpty() ||
                (!result.TryGetValue(variable1.reference.Address, out var tmp1) || !variable1.cmp.Invoke((T1)tmp1)) ||
                (!result.TryGetValue(variable2.reference.Address, out var tmp2) || !variable2.cmp.Invoke((T2)tmp2)) ||
                (!result.TryGetValue(variable3.reference.Address, out var tmp3) || !variable3.cmp.Invoke((T3)tmp3)) ||
                (!result.TryGetValue(variable4.reference.Address, out var tmp4) || !variable4.cmp.Invoke((T4)tmp4)) ||
                (!result.TryGetValue(variable5.reference.Address, out var tmp5) || !variable5.cmp.Invoke((T5)tmp5)) ||
                (!result.TryGetValue(variable6.reference.Address, out var tmp6) || !variable6.cmp.Invoke((T6)tmp6)))
            {
                if (otherwise == null) return false;
                return await otherwise!.Invoke().ConfigureAwait(false);
            }

            if (then == null) return true;
            return await then!.Invoke().ConfigureAwait(false);

        }

        public async static Task<bool> IfAsync<T1, T2, T3, T4, T5, T6, T7>(this PlcDataMapper papper,
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
            if (papper == null) ExceptionThrowHelper.ThrowArgumentNullException(nameof(papper));

            var result = (await papper.ReadAsync(
                                                 variable1.reference,
                                                 variable2.reference,
                                                 variable3.reference,
                                                 variable4.reference,
                                                 variable5.reference,
                                                 variable6.reference,
                                                 variable7.reference).ConfigureAwait(false))?.ToDictionary(x => x.Variable, x => x.Value);

            if (result.IsNullOrEmpty() ||
                (!result.TryGetValue(variable1.reference.Address, out var tmp1) || !variable1.cmp.Invoke((T1)tmp1)) ||
                (!result.TryGetValue(variable2.reference.Address, out var tmp2) || !variable2.cmp.Invoke((T2)tmp2)) ||
                (!result.TryGetValue(variable3.reference.Address, out var tmp3) || !variable3.cmp.Invoke((T3)tmp3)) ||
                (!result.TryGetValue(variable4.reference.Address, out var tmp4) || !variable4.cmp.Invoke((T4)tmp4)) ||
                (!result.TryGetValue(variable5.reference.Address, out var tmp5) || !variable5.cmp.Invoke((T5)tmp5)) ||
                (!result.TryGetValue(variable6.reference.Address, out var tmp6) || !variable6.cmp.Invoke((T6)tmp6)) ||
                (!result.TryGetValue(variable7.reference.Address, out var tmp7) || !variable7.cmp.Invoke((T7)tmp7)) )
            {
                if (otherwise == null) return false;
                return await otherwise!.Invoke().ConfigureAwait(false);
            }

            if (then == null) return true;
            return await then!.Invoke().ConfigureAwait(false);

        }

        public async static Task<bool> IfAsync<T1, T2, T3, T4, T5, T6, T7, T8>(this PlcDataMapper papper,
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
            if (papper == null) ExceptionThrowHelper.ThrowArgumentNullException(nameof(papper));

            var result = (await papper.ReadAsync(
                                                 variable1.reference,
                                                 variable2.reference,
                                                 variable3.reference,
                                                 variable4.reference,
                                                 variable5.reference,
                                                 variable6.reference,
                                                 variable7.reference,
                                                 variable8.reference).ConfigureAwait(false))?.ToDictionary(x => x.Variable, x => x.Value);

            if (result.IsNullOrEmpty() ||
                (!result.TryGetValue(variable1.reference.Address, out var tmp1) || !variable1.cmp.Invoke((T1)tmp1)) ||
                (!result.TryGetValue(variable2.reference.Address, out var tmp2) || !variable2.cmp.Invoke((T2)tmp2)) ||
                (!result.TryGetValue(variable3.reference.Address, out var tmp3) || !variable3.cmp.Invoke((T3)tmp3)) ||
                (!result.TryGetValue(variable4.reference.Address, out var tmp4) || !variable4.cmp.Invoke((T4)tmp4)) ||
                (!result.TryGetValue(variable5.reference.Address, out var tmp5) || !variable5.cmp.Invoke((T5)tmp5)) ||
                (!result.TryGetValue(variable6.reference.Address, out var tmp6) || !variable6.cmp.Invoke((T6)tmp6)) ||
                (!result.TryGetValue(variable7.reference.Address, out var tmp7) || !variable7.cmp.Invoke((T7)tmp7)) ||
                (!result.TryGetValue(variable8.reference.Address, out var tmp8) || !variable8.cmp.Invoke((T8)tmp8)))
            {
                if (otherwise == null) return false;
                return await otherwise!.Invoke().ConfigureAwait(false);
            }

            if (then == null) return true;
            return await then!.Invoke().ConfigureAwait(false);

        }

        public async static Task<bool> IfAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this PlcDataMapper papper,
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
            if (papper == null) ExceptionThrowHelper.ThrowArgumentNullException(nameof(papper));

            var result = (await papper.ReadAsync(
                                                 variable1.reference,
                                                 variable2.reference,
                                                 variable3.reference,
                                                 variable4.reference,
                                                 variable5.reference,
                                                 variable6.reference,
                                                 variable7.reference,
                                                 variable8.reference,
                                                 variable9.reference).ConfigureAwait(false))?.ToDictionary(x => x.Variable, x => x.Value);

            if (result.IsNullOrEmpty() ||
                (!result.TryGetValue(variable1.reference.Address, out var tmp1) || !variable1.cmp.Invoke((T1)tmp1)) ||
                (!result.TryGetValue(variable2.reference.Address, out var tmp2) || !variable2.cmp.Invoke((T2)tmp2)) ||
                (!result.TryGetValue(variable3.reference.Address, out var tmp3) || !variable3.cmp.Invoke((T3)tmp3)) ||
                (!result.TryGetValue(variable4.reference.Address, out var tmp4) || !variable4.cmp.Invoke((T4)tmp4)) ||
                (!result.TryGetValue(variable5.reference.Address, out var tmp5) || !variable5.cmp.Invoke((T5)tmp5)) ||
                (!result.TryGetValue(variable6.reference.Address, out var tmp6) || !variable6.cmp.Invoke((T6)tmp6)) ||
                (!result.TryGetValue(variable7.reference.Address, out var tmp7) || !variable7.cmp.Invoke((T7)tmp7)) ||
                (!result.TryGetValue(variable8.reference.Address, out var tmp8) || !variable8.cmp.Invoke((T8)tmp8)) ||
                (!result.TryGetValue(variable9.reference.Address, out var tmp9) || !variable9.cmp.Invoke((T9)tmp9)))
            {
                if (otherwise == null) return false;
                return await otherwise!.Invoke().ConfigureAwait(false);
            }

            if (then == null) return true;
            return await then!.Invoke().ConfigureAwait(false);

        }

        public async static Task<bool> IfAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this PlcDataMapper papper,
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
            if (papper == null) ExceptionThrowHelper.ThrowArgumentNullException(nameof(papper));

            var result = (await papper.ReadAsync(
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

            if (result.IsNullOrEmpty() ||
                (!result.TryGetValue(variable1.reference.Address, out var tmp1) || !variable1.cmp.Invoke((T1)tmp1)) ||
                (!result.TryGetValue(variable2.reference.Address, out var tmp2) || !variable2.cmp.Invoke((T2)tmp2)) ||
                (!result.TryGetValue(variable3.reference.Address, out var tmp3) || !variable3.cmp.Invoke((T3)tmp3)) ||
                (!result.TryGetValue(variable4.reference.Address, out var tmp4) || !variable4.cmp.Invoke((T4)tmp4)) ||
                (!result.TryGetValue(variable5.reference.Address, out var tmp5) || !variable5.cmp.Invoke((T5)tmp5)) ||
                (!result.TryGetValue(variable6.reference.Address, out var tmp6) || !variable6.cmp.Invoke((T6)tmp6)) ||
                (!result.TryGetValue(variable7.reference.Address, out var tmp7) || !variable7.cmp.Invoke((T7)tmp7)) ||
                (!result.TryGetValue(variable8.reference.Address, out var tmp8) || !variable8.cmp.Invoke((T8)tmp8)) ||
                (!result.TryGetValue(variable9.reference.Address, out var tmp9) || !variable9.cmp.Invoke((T9)tmp9)) ||
                (!result.TryGetValue(variable10.reference.Address, out var tmp10) || !variable10.cmp.Invoke((T10)tmp10)))
            {
                if (otherwise == null) return false;
                return await otherwise!.Invoke().ConfigureAwait(false);
            }

            if (then == null) return true;
            return await then!.Invoke().ConfigureAwait(false);

        }

        public async static Task<bool> IfAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this PlcDataMapper papper,
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
            if (papper == null) ExceptionThrowHelper.ThrowArgumentNullException(nameof(papper));

            var result = (await papper.ReadAsync(
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

            if (result.IsNullOrEmpty() ||
                (!result.TryGetValue(variable1.reference.Address, out var tmp1) || !variable1.cmp.Invoke((T1)tmp1)) ||
                (!result.TryGetValue(variable2.reference.Address, out var tmp2) || !variable2.cmp.Invoke((T2)tmp2)) ||
                (!result.TryGetValue(variable3.reference.Address, out var tmp3) || !variable3.cmp.Invoke((T3)tmp3)) ||
                (!result.TryGetValue(variable4.reference.Address, out var tmp4) || !variable4.cmp.Invoke((T4)tmp4)) ||
                (!result.TryGetValue(variable5.reference.Address, out var tmp5) || !variable5.cmp.Invoke((T5)tmp5)) ||
                (!result.TryGetValue(variable6.reference.Address, out var tmp6) || !variable6.cmp.Invoke((T6)tmp6)) ||
                (!result.TryGetValue(variable7.reference.Address, out var tmp7) || !variable7.cmp.Invoke((T7)tmp7)) ||
                (!result.TryGetValue(variable8.reference.Address, out var tmp8) || !variable8.cmp.Invoke((T8)tmp8)) ||
                (!result.TryGetValue(variable9.reference.Address, out var tmp9) || !variable9.cmp.Invoke((T9)tmp9)) ||
                (!result.TryGetValue(variable10.reference.Address, out var tmp10) || !variable10.cmp.Invoke((T10)tmp10)) ||
                (!result.TryGetValue(variable11.reference.Address, out var tmp11) || !variable11.cmp.Invoke((T11)tmp11)))
            {
                if (otherwise == null) return false;
                return await otherwise!.Invoke().ConfigureAwait(false);
            }

            if (then == null) return true;
            return await then!.Invoke().ConfigureAwait(false);

        }

        public async static Task<bool> IfAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this PlcDataMapper papper,
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
            if (papper == null) ExceptionThrowHelper.ThrowArgumentNullException(nameof(papper));

            var result = (await papper.ReadAsync(
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

            if (result.IsNullOrEmpty() ||
                (!result.TryGetValue(variable1.reference.Address, out var tmp1) || !variable1.cmp.Invoke((T1)tmp1)) ||
                (!result.TryGetValue(variable2.reference.Address, out var tmp2) || !variable2.cmp.Invoke((T2)tmp2)) ||
                (!result.TryGetValue(variable3.reference.Address, out var tmp3) || !variable3.cmp.Invoke((T3)tmp3)) ||
                (!result.TryGetValue(variable4.reference.Address, out var tmp4) || !variable4.cmp.Invoke((T4)tmp4)) ||
                (!result.TryGetValue(variable5.reference.Address, out var tmp5) || !variable5.cmp.Invoke((T5)tmp5)) ||
                (!result.TryGetValue(variable6.reference.Address, out var tmp6) || !variable6.cmp.Invoke((T6)tmp6)) ||
                (!result.TryGetValue(variable7.reference.Address, out var tmp7) || !variable7.cmp.Invoke((T7)tmp7)) ||
                (!result.TryGetValue(variable8.reference.Address, out var tmp8) || !variable8.cmp.Invoke((T8)tmp8)) ||
                (!result.TryGetValue(variable9.reference.Address, out var tmp9) || !variable9.cmp.Invoke((T9)tmp9)) ||
                (!result.TryGetValue(variable10.reference.Address, out var tmp10) || !variable10.cmp.Invoke((T10)tmp10)) ||
                (!result.TryGetValue(variable11.reference.Address, out var tmp11) || !variable11.cmp.Invoke((T11)tmp11)) ||
                (!result.TryGetValue(variable12.reference.Address, out var tmp12) || !variable12.cmp.Invoke((T12)tmp12)))
            {
                if (otherwise == null) return false;
                return await otherwise!.Invoke().ConfigureAwait(false);
            }

            if (then == null) return true;
            return await then!.Invoke().ConfigureAwait(false);

        }

        public async static Task<bool> IfAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this PlcDataMapper papper,
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
            if (papper == null) ExceptionThrowHelper.ThrowArgumentNullException(nameof(papper));

            var result = (await papper.ReadAsync(
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

            if (result.IsNullOrEmpty() ||
                (!result.TryGetValue(variable1.reference.Address, out var tmp1) || !variable1.cmp.Invoke((T1)tmp1)) ||
                (!result.TryGetValue(variable2.reference.Address, out var tmp2) || !variable2.cmp.Invoke((T2)tmp2)) ||
                (!result.TryGetValue(variable3.reference.Address, out var tmp3) || !variable3.cmp.Invoke((T3)tmp3)) ||
                (!result.TryGetValue(variable4.reference.Address, out var tmp4) || !variable4.cmp.Invoke((T4)tmp4)) ||
                (!result.TryGetValue(variable5.reference.Address, out var tmp5) || !variable5.cmp.Invoke((T5)tmp5)) ||
                (!result.TryGetValue(variable6.reference.Address, out var tmp6) || !variable6.cmp.Invoke((T6)tmp6)) ||
                (!result.TryGetValue(variable7.reference.Address, out var tmp7) || !variable7.cmp.Invoke((T7)tmp7)) ||
                (!result.TryGetValue(variable8.reference.Address, out var tmp8) || !variable8.cmp.Invoke((T8)tmp8)) ||
                (!result.TryGetValue(variable9.reference.Address, out var tmp9) || !variable9.cmp.Invoke((T9)tmp9)) ||
                (!result.TryGetValue(variable10.reference.Address, out var tmp10) || !variable10.cmp.Invoke((T10)tmp10)) ||
                (!result.TryGetValue(variable11.reference.Address, out var tmp11) || !variable11.cmp.Invoke((T11)tmp11)) ||
                (!result.TryGetValue(variable12.reference.Address, out var tmp12) || !variable12.cmp.Invoke((T12)tmp12)) ||
                (!result.TryGetValue(variable13.reference.Address, out var tmp13) || !variable13.cmp.Invoke((T13)tmp13)))
            {
                if (otherwise == null) return false;
                return await otherwise!.Invoke().ConfigureAwait(false);
            }

            if (then == null) return true;
            return await then!.Invoke().ConfigureAwait(false);

        }

        public async static Task<bool> IfAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this PlcDataMapper papper,
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
            if (papper == null) ExceptionThrowHelper.ThrowArgumentNullException(nameof(papper));

            var result = (await papper.ReadAsync(
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

            if (result.IsNullOrEmpty() ||
                (!result.TryGetValue(variable1.reference.Address, out var tmp1) || !variable1.cmp.Invoke((T1)tmp1)) ||
                (!result.TryGetValue(variable2.reference.Address, out var tmp2) || !variable2.cmp.Invoke((T2)tmp2)) ||
                (!result.TryGetValue(variable3.reference.Address, out var tmp3) || !variable3.cmp.Invoke((T3)tmp3)) ||
                (!result.TryGetValue(variable4.reference.Address, out var tmp4) || !variable4.cmp.Invoke((T4)tmp4)) ||
                (!result.TryGetValue(variable5.reference.Address, out var tmp5) || !variable5.cmp.Invoke((T5)tmp5)) ||
                (!result.TryGetValue(variable6.reference.Address, out var tmp6) || !variable6.cmp.Invoke((T6)tmp6)) ||
                (!result.TryGetValue(variable7.reference.Address, out var tmp7) || !variable7.cmp.Invoke((T7)tmp7)) ||
                (!result.TryGetValue(variable8.reference.Address, out var tmp8) || !variable8.cmp.Invoke((T8)tmp8)) ||
                (!result.TryGetValue(variable9.reference.Address, out var tmp9) || !variable9.cmp.Invoke((T9)tmp9)) ||
                (!result.TryGetValue(variable10.reference.Address, out var tmp10) || !variable10.cmp.Invoke((T10)tmp10)) ||
                (!result.TryGetValue(variable11.reference.Address, out var tmp11) || !variable11.cmp.Invoke((T11)tmp11)) ||
                (!result.TryGetValue(variable12.reference.Address, out var tmp12) || !variable12.cmp.Invoke((T12)tmp12)) ||
                (!result.TryGetValue(variable13.reference.Address, out var tmp13) || !variable13.cmp.Invoke((T13)tmp13)) ||
                (!result.TryGetValue(variable14.reference.Address, out var tmp14) || !variable14.cmp.Invoke((T14)tmp14)))
            {
                if (otherwise == null) return false;
                return await otherwise!.Invoke().ConfigureAwait(false);
            }

            if (then == null) return true;
            return await then!.Invoke().ConfigureAwait(false);

        }

        public async static Task<bool> IfAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this PlcDataMapper papper,
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
            if (papper == null) ExceptionThrowHelper.ThrowArgumentNullException(nameof(papper));

            var result = (await papper.ReadAsync(
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

            if (result.IsNullOrEmpty() ||
                (!result.TryGetValue(variable1.reference.Address, out var tmp1) || !variable1.cmp.Invoke((T1)tmp1)) ||
                (!result.TryGetValue(variable2.reference.Address, out var tmp2) || !variable2.cmp.Invoke((T2)tmp2)) ||
                (!result.TryGetValue(variable3.reference.Address, out var tmp3) || !variable3.cmp.Invoke((T3)tmp3)) ||
                (!result.TryGetValue(variable4.reference.Address, out var tmp4) || !variable4.cmp.Invoke((T4)tmp4)) ||
                (!result.TryGetValue(variable5.reference.Address, out var tmp5) || !variable5.cmp.Invoke((T5)tmp5)) ||
                (!result.TryGetValue(variable6.reference.Address, out var tmp6) || !variable6.cmp.Invoke((T6)tmp6)) ||
                (!result.TryGetValue(variable7.reference.Address, out var tmp7) || !variable7.cmp.Invoke((T7)tmp7)) ||
                (!result.TryGetValue(variable8.reference.Address, out var tmp8) || !variable8.cmp.Invoke((T8)tmp8)) ||
                (!result.TryGetValue(variable9.reference.Address, out var tmp9) || !variable9.cmp.Invoke((T9)tmp9)) ||
                (!result.TryGetValue(variable10.reference.Address, out var tmp10) || !variable10.cmp.Invoke((T10)tmp10)) ||
                (!result.TryGetValue(variable11.reference.Address, out var tmp11) || !variable11.cmp.Invoke((T11)tmp11)) ||
                (!result.TryGetValue(variable12.reference.Address, out var tmp12) || !variable12.cmp.Invoke((T12)tmp12)) ||
                (!result.TryGetValue(variable13.reference.Address, out var tmp13) || !variable13.cmp.Invoke((T13)tmp13)) ||
                (!result.TryGetValue(variable14.reference.Address, out var tmp14) || !variable14.cmp.Invoke((T14)tmp14)) ||
                (!result.TryGetValue(variable15.reference.Address, out var tmp15) || !variable15.cmp.Invoke((T15)tmp15)))
            {
                if (otherwise == null) return false;
                return await otherwise!.Invoke().ConfigureAwait(false);
            }

            if (then == null) return true;
            return await then!.Invoke().ConfigureAwait(false);

        }

        public async static Task<bool> IfAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this PlcDataMapper papper,
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
            if (papper == null) ExceptionThrowHelper.ThrowArgumentNullException(nameof(papper));

            var result = (await papper.ReadAsync(
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

            if (result.IsNullOrEmpty() ||
                (!result.TryGetValue(variable1.reference.Address, out var tmp1) || !variable1.cmp.Invoke((T1)tmp1)) ||
                (!result.TryGetValue(variable2.reference.Address, out var tmp2) || !variable2.cmp.Invoke((T2)tmp2)) ||
                (!result.TryGetValue(variable3.reference.Address, out var tmp3) || !variable3.cmp.Invoke((T3)tmp3)) ||
                (!result.TryGetValue(variable4.reference.Address, out var tmp4) || !variable4.cmp.Invoke((T4)tmp4)) ||
                (!result.TryGetValue(variable5.reference.Address, out var tmp5) || !variable5.cmp.Invoke((T5)tmp5)) ||
                (!result.TryGetValue(variable6.reference.Address, out var tmp6) || !variable6.cmp.Invoke((T6)tmp6)) ||
                (!result.TryGetValue(variable7.reference.Address, out var tmp7) || !variable7.cmp.Invoke((T7)tmp7)) ||
                (!result.TryGetValue(variable8.reference.Address, out var tmp8) || !variable8.cmp.Invoke((T8)tmp8)) ||
                (!result.TryGetValue(variable9.reference.Address, out var tmp9) || !variable9.cmp.Invoke((T9)tmp9)) ||
                (!result.TryGetValue(variable10.reference.Address, out var tmp10) || !variable10.cmp.Invoke((T10)tmp10)) ||
                (!result.TryGetValue(variable11.reference.Address, out var tmp11) || !variable11.cmp.Invoke((T11)tmp11)) ||
                (!result.TryGetValue(variable12.reference.Address, out var tmp12) || !variable12.cmp.Invoke((T12)tmp12)) ||
                (!result.TryGetValue(variable13.reference.Address, out var tmp13) || !variable13.cmp.Invoke((T13)tmp13)) ||
                (!result.TryGetValue(variable14.reference.Address, out var tmp14) || !variable14.cmp.Invoke((T14)tmp14)) ||
                (!result.TryGetValue(variable15.reference.Address, out var tmp15) || !variable15.cmp.Invoke((T15)tmp15)) ||
                (!result.TryGetValue(variable16.reference.Address, out var tmp16) || !variable16.cmp.Invoke((T16)tmp16)))
            {
                if (otherwise == null) return false;
                return await otherwise!.Invoke().ConfigureAwait(false);
            }

            if (then == null) return true;
            return await then!.Invoke().ConfigureAwait(false);

        }







        public async static Task<bool> IfAsync<T1, T2>(this PlcDataMapper papper,
                                                            PlcReadReference variable1,
                                                            PlcReadReference variable2,
                                                            Func<T1, T2, bool> cmp,
                                                            Func<Task<bool>>? then = null,
                                                            Func<Task<bool>>? otherwise = null)
        {
            if (papper == null) ExceptionThrowHelper.ThrowArgumentNullException(nameof(papper));

            var result = (await papper.ReadAsync( variable1,
                                                 variable2).ConfigureAwait(false))?.ToDictionary(x => x.Variable, x => x.Value);

            if (result.IsNullOrEmpty() ||
                (!result.TryGetValue(variable1.Address, out var tmp1)) ||
                (!result.TryGetValue(variable2.Address, out var tmp2)) ||
                !cmp((T1)tmp1, (T2)tmp2))
            {
                if (otherwise == null) return false;
                return await otherwise!.Invoke().ConfigureAwait(false);
            }

            if (then == null) return true;
            return await then!.Invoke().ConfigureAwait(false);

        }



        public async static Task<bool> IfAsync<T1, T2, T3>(this PlcDataMapper papper,
                                                    PlcReadReference variable1,
                                                    PlcReadReference variable2,
                                                    PlcReadReference variable3,
                                                    Func<T1, T2, T3, bool> cmp,
                                                    Func<Task<bool>>? then = null,
                                                    Func<Task<bool>>? otherwise = null)
        {
            if (papper == null) ExceptionThrowHelper.ThrowArgumentNullException(nameof(papper));

            var result = (await papper.ReadAsync(variable1,
                                                 variable2,
                                                 variable3).ConfigureAwait(false))?.ToDictionary(x => x.Variable, x => x.Value);

            if (result.IsNullOrEmpty() ||
                (!result.TryGetValue(variable1.Address, out var tmp1)) ||
                (!result.TryGetValue(variable2.Address, out var tmp2)) ||
                (!result.TryGetValue(variable3.Address, out var tmp3)) ||
                !cmp((T1)tmp1, (T2)tmp2, (T3)tmp3))
            {
                if (otherwise == null) return false;
                return await otherwise!.Invoke().ConfigureAwait(false);
            }

            if (then == null) return true;
            return await then!.Invoke().ConfigureAwait(false);

        }

        public async static Task<bool> IfAsync<T1, T2, T3, T4>(this PlcDataMapper papper,
            PlcReadReference variable1,
            PlcReadReference variable2,
            PlcReadReference variable3,
            PlcReadReference variable4,
            Func<T1, T2, T3, T4, bool> cmp,
            Func<Task<bool>>? then = null,
            Func<Task<bool>>? otherwise = null)
        {
            if (papper == null) ExceptionThrowHelper.ThrowArgumentNullException(nameof(papper));

            var result = (await papper.ReadAsync(variable1,
                                                 variable2,
                                                 variable3,
                                                 variable4).ConfigureAwait(false))?.ToDictionary(x => x.Variable, x => x.Value);

            if (result.IsNullOrEmpty() ||
                (!result.TryGetValue(variable1.Address, out var tmp1)) ||
                (!result.TryGetValue(variable2.Address, out var tmp2)) ||
                (!result.TryGetValue(variable3.Address, out var tmp3)) ||
                (!result.TryGetValue(variable4.Address, out var tmp4)) ||
                !cmp((T1)tmp1, (T2)tmp2, (T3)tmp3, (T4)tmp4))
            {
                if (otherwise == null) return false;
                return await otherwise!.Invoke().ConfigureAwait(false);
            }

            if (then == null) return true;
            return await then!.Invoke().ConfigureAwait(false);

        }


        public async static Task<bool> IfAsync<T1, T2, T3, T4, T5>(this PlcDataMapper papper,
                    PlcReadReference variable1,
                    PlcReadReference variable2,
                    PlcReadReference variable3,
                    PlcReadReference variable4,
                    PlcReadReference variable5,
                    Func<T1, T2, T3, T4, T5, bool> cmp,
                    Func<Task<bool>>? then = null,
                    Func<Task<bool>>? otherwise = null)
        {
            if (papper == null) ExceptionThrowHelper.ThrowArgumentNullException(nameof(papper));

            var result = (await papper.ReadAsync(variable1,
                                                 variable2,
                                                 variable3,
                                                 variable4,
                                                 variable5).ConfigureAwait(false))?.ToDictionary(x => x.Variable, x => x.Value);

            if (result.IsNullOrEmpty() ||
                (!result.TryGetValue(variable1.Address, out var tmp1)) ||
                (!result.TryGetValue(variable2.Address, out var tmp2)) ||
                (!result.TryGetValue(variable3.Address, out var tmp3)) ||
                (!result.TryGetValue(variable4.Address, out var tmp4)) ||
                (!result.TryGetValue(variable5.Address, out var tmp5)) ||
                !cmp((T1)tmp1, (T2)tmp2, (T3)tmp3, (T4)tmp4, (T5)tmp5))
            {
                if (otherwise == null) return false;
                return await otherwise!.Invoke().ConfigureAwait(false);
            }

            if (then == null) return true;
            return await then!.Invoke().ConfigureAwait(false);

        }


        public async static Task<bool> IfAsync<T1, T2, T3, T4, T5, T6>(this PlcDataMapper papper,
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
            if (papper == null) ExceptionThrowHelper.ThrowArgumentNullException(nameof(papper));

            var result = (await papper.ReadAsync(variable1,
                                                 variable2,
                                                 variable3,
                                                 variable4,
                                                 variable5,
                                                 variable6).ConfigureAwait(false))?.ToDictionary(x => x.Variable, x => x.Value);

            if (result.IsNullOrEmpty() ||
                (!result.TryGetValue(variable1.Address, out var tmp1)) ||
                (!result.TryGetValue(variable2.Address, out var tmp2)) ||
                (!result.TryGetValue(variable3.Address, out var tmp3)) ||
                (!result.TryGetValue(variable4.Address, out var tmp4)) ||
                (!result.TryGetValue(variable5.Address, out var tmp5)) ||
                (!result.TryGetValue(variable6.Address, out var tmp6)) ||
                !cmp((T1)tmp1, (T2)tmp2, (T3)tmp3, (T4)tmp4, (T5)tmp5, (T6)tmp6))
            {
                if (otherwise == null) return false;
                return await otherwise!.Invoke().ConfigureAwait(false);
            }

            if (then == null) return true;
            return await then!.Invoke().ConfigureAwait(false);

        }


        public async static Task<bool> IfAsync<T1, T2, T3, T4, T5, T6, T7>(this PlcDataMapper papper,
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
            if (papper == null) ExceptionThrowHelper.ThrowArgumentNullException(nameof(papper));

            var result = (await papper.ReadAsync(variable1,
                                                 variable2,
                                                 variable3,
                                                 variable4,
                                                 variable5,
                                                 variable6,
                                                 variable7).ConfigureAwait(false))?.ToDictionary(x => x.Variable, x => x.Value);

            if (result.IsNullOrEmpty() ||
                (!result.TryGetValue(variable1.Address, out var tmp1)) ||
                (!result.TryGetValue(variable2.Address, out var tmp2)) ||
                (!result.TryGetValue(variable3.Address, out var tmp3)) ||
                (!result.TryGetValue(variable4.Address, out var tmp4)) ||
                (!result.TryGetValue(variable5.Address, out var tmp5)) ||
                (!result.TryGetValue(variable6.Address, out var tmp6)) ||
                (!result.TryGetValue(variable7.Address, out var tmp7)) ||
                !cmp((T1)tmp1, (T2)tmp2, (T3)tmp3, (T4)tmp4, (T5)tmp5, (T6)tmp6, (T7)tmp7))
            {
                if (otherwise == null) return false;
                return await otherwise!.Invoke().ConfigureAwait(false);
            }

            if (then == null) return true;
            return await then!.Invoke().ConfigureAwait(false);

        }

        public async static Task<bool> IfAsync<T1, T2, T3, T4, T5, T6, T7, T8>(this PlcDataMapper papper,
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
            if (papper == null) ExceptionThrowHelper.ThrowArgumentNullException(nameof(papper));

            var result = (await papper.ReadAsync(variable1,
                                                 variable2,
                                                 variable3,
                                                 variable4,
                                                 variable5,
                                                 variable6,
                                                 variable7,
                                                 variable8).ConfigureAwait(false))?.ToDictionary(x => x.Variable, x => x.Value);

            if (result.IsNullOrEmpty() ||
                (!result.TryGetValue(variable1.Address, out var tmp1)) ||
                (!result.TryGetValue(variable2.Address, out var tmp2)) ||
                (!result.TryGetValue(variable3.Address, out var tmp3)) ||
                (!result.TryGetValue(variable4.Address, out var tmp4)) ||
                (!result.TryGetValue(variable5.Address, out var tmp5)) ||
                (!result.TryGetValue(variable6.Address, out var tmp6)) ||
                (!result.TryGetValue(variable7.Address, out var tmp7)) ||
                (!result.TryGetValue(variable8.Address, out var tmp8)) ||
                !cmp((T1)tmp1, (T2)tmp2, (T3)tmp3, (T4)tmp4, (T5)tmp5, (T6)tmp6, (T7)tmp7, (T8)tmp8))
            {
                if (otherwise == null) return false;
                return await otherwise!.Invoke().ConfigureAwait(false);
            }

            if (then == null) return true;
            return await then!.Invoke().ConfigureAwait(false);

        }



    }
}
