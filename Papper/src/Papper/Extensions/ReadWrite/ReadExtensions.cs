using Papper.Internal;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace Papper.Extensions.ReadWrite
{
    public static class ReadExtensions
    {
        public async static Task<(T1 value1, T2 value2)> ReadAsync<T1, T2>(this PlcDataMapper papper,
                                                                           
                                                                           (PlcReadReference reference, T1 defaultValue) variable1,
                                                                           (PlcReadReference reference, T2 defaultValue) variable2)
        {
            if (papper == null) return ExceptionThrowHelper.ThrowArgumentNullException<(T1 value1, T2 value2)>(nameof(papper));

            T1 value1 = variable1.defaultValue;
            T2 value2 = variable2.defaultValue;

            var result = (await papper.ReadAsync(variable1.reference,
                                                 variable2.reference).ConfigureAwait(false))?.ToDictionary(x => x.Variable, x => x.Value);

            if (!result!.IsNullOrEmpty())
            {
                if (result!.TryGetValue(variable1.reference.Address, out var tmp1) && tmp1 is T1 t1) value1 = t1;
                if (result!.TryGetValue(variable2.reference.Address, out var tmp2) && tmp2 is T2 t2) value2 = t2;
            }

            return (value1, value2);
        }

        public async static Task<(T1 value1, T2 value2, T3 value3)> ReadAsync<T1, T2, T3>(this PlcDataMapper papper, 
                                                                                           (PlcReadReference reference, T1 defaultValue) variable1,
                                                                                           (PlcReadReference reference, T2 defaultValue) variable2,
                                                                                           (PlcReadReference reference, T3 defaultValue) variable3)
        {
            if (papper == null) return ExceptionThrowHelper.ThrowArgumentNullException< (T1 value1, T2 value2, T3 value3)>(nameof(papper));

            T1 value1 = variable1.defaultValue;
            T2 value2 = variable2.defaultValue;
            T3 value3 = variable3.defaultValue;

            var result = (await papper.ReadAsync(
                                                 variable1.reference,
                                                 variable2.reference,
                                                 variable3.reference).ConfigureAwait(false))?.ToDictionary(x => x.Variable, x => x.Value);

            if (!result!.IsNullOrEmpty())
            {
                if (result!.TryGetValue(variable1.reference.Address, out var tmp1) && tmp1 is T1 t1) value1 = t1;
                if (result!.TryGetValue(variable2.reference.Address, out var tmp2) && tmp2 is T2 t2) value2 = t2;
                if (result!.TryGetValue(variable3.reference.Address, out var tmp3) && tmp3 is T3 t3) value3 = t3;
            }

            return (value1, value2, value3);
        }

        public async static Task<(T1 value1, T2 value2, T3 value3, T4 value4)> ReadAsync<T1, T2, T3, T4>(this PlcDataMapper papper, 
                                                                                   (PlcReadReference reference, T1 defaultValue) variable1,
                                                                                   (PlcReadReference reference, T2 defaultValue) variable2,
                                                                                   (PlcReadReference reference, T3 defaultValue) variable3,
                                                                                   (PlcReadReference reference, T4 defaultValue) variable4)
        {
            if (papper == null) return ExceptionThrowHelper.ThrowArgumentNullException<(T1 value1, T2 value2, T3 value3, T4 value4)>(nameof(papper));

            T1 value1 = variable1.defaultValue;
            T2 value2 = variable2.defaultValue;
            T3 value3 = variable3.defaultValue;
            T4 value4 = variable4.defaultValue;

            var result = (await papper!.ReadAsync(
                                                 variable1.reference,
                                                 variable2.reference,
                                                 variable3.reference,
                                                 variable4.reference).ConfigureAwait(false))?.ToDictionary(x => x.Variable, x => x.Value);

            if (!result!.IsNullOrEmpty())
            {
                if (result!.TryGetValue(variable1.reference.Address, out var tmp1) && tmp1 is T1 t1) value1 = t1;
                if (result!.TryGetValue(variable2.reference.Address, out var tmp2) && tmp2 is T2 t2) value2 = t2;
                if (result!.TryGetValue(variable3.reference.Address, out var tmp3) && tmp3 is T3 t3) value3 = t3;
                if (result!.TryGetValue(variable4.reference.Address, out var tmp4) && tmp4 is T4 t4) value4 = t4;
            }

            return (value1, value2, value3, value4);
        }

        public async static Task<(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5)>
            ReadAsync<T1, T2, T3, T4, T5>(this PlcDataMapper papper, 
                           (PlcReadReference reference, T1 defaultValue) variable1,
                           (PlcReadReference reference, T2 defaultValue) variable2,
                           (PlcReadReference reference, T3 defaultValue) variable3,
                           (PlcReadReference reference, T4 defaultValue) variable4,
                           (PlcReadReference reference, T5 defaultValue) variable5)
        {
            if (papper == null) return ExceptionThrowHelper.ThrowArgumentNullException<(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5)>(nameof(papper));

            T1 value1 = variable1.defaultValue;
            T2 value2 = variable2.defaultValue;
            T3 value3 = variable3.defaultValue;
            T4 value4 = variable4.defaultValue;
            T5 value5 = variable5.defaultValue;


            var result = (await papper.ReadAsync(
                                                 variable1.reference,
                                                 variable2.reference,
                                                 variable3.reference,
                                                 variable4.reference,
                                                 variable5.reference).ConfigureAwait(false))?.ToDictionary(x => x.Variable, x => x.Value);

            if (!result!.IsNullOrEmpty())
            {
                if (result!.TryGetValue(variable1.reference.Address, out var tmp1) && tmp1 is T1 t1) value1 = t1;
                if (result!.TryGetValue(variable2.reference.Address, out var tmp2) && tmp2 is T2 t2) value2 = t2;
                if (result!.TryGetValue(variable3.reference.Address, out var tmp3) && tmp3 is T3 t3) value3 = t3;
                if (result!.TryGetValue(variable4.reference.Address, out var tmp4) && tmp4 is T4 t4) value4 = t4;
                if (result!.TryGetValue(variable5.reference.Address, out var tmp5) && tmp5 is T5 t5) value5 = t5;
            }

            return (value1, value2, value3, value4, value5);
        }

        public async static Task<(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6)>
            ReadAsync<T1, T2, T3, T4, T5, T6>(this PlcDataMapper papper, 
           (PlcReadReference reference, T1 defaultValue) variable1,
           (PlcReadReference reference, T2 defaultValue) variable2,
           (PlcReadReference reference, T3 defaultValue) variable3,
           (PlcReadReference reference, T4 defaultValue) variable4,
           (PlcReadReference reference, T5 defaultValue) variable5,
           (PlcReadReference reference, T6 defaultValue) variable6)
        {
            if (papper == null) return ExceptionThrowHelper.ThrowArgumentNullException<(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6)>(nameof(papper));

            T1 value1 = variable1.defaultValue;
            T2 value2 = variable2.defaultValue;
            T3 value3 = variable3.defaultValue;
            T4 value4 = variable4.defaultValue;
            T5 value5 = variable5.defaultValue;
            T6 value6 = variable6.defaultValue;




            var result = (await papper.ReadAsync(variable1.reference,
                                                 variable2.reference,
                                                 variable3.reference,
                                                 variable4.reference,
                                                 variable5.reference,
                                                 variable6.reference).ConfigureAwait(false))?.ToDictionary(x => x.Variable, x => x.Value);

            if (!result!.IsNullOrEmpty())
            {
                if (result!.TryGetValue(variable1.reference.Address, out var tmp1) && tmp1 is T1 t1) value1 = t1;
                if (result!.TryGetValue(variable2.reference.Address, out var tmp2) && tmp2 is T2 t2) value2 = t2;
                if (result!.TryGetValue(variable3.reference.Address, out var tmp3) && tmp3 is T3 t3) value3 = t3;
                if (result!.TryGetValue(variable4.reference.Address, out var tmp4) && tmp4 is T4 t4) value4 = t4;
                if (result!.TryGetValue(variable5.reference.Address, out var tmp5) && tmp5 is T5 t5) value5 = t5;
                if (result!.TryGetValue(variable6.reference.Address, out var tmp6) && tmp6 is T6 t6) value6 = t6;

            }

            return (value1, value2, value3, value4, value5, value6);
        }


        public async static Task<(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7)>
            ReadAsync<T1, T2, T3, T4, T5, T6, T7>(this PlcDataMapper papper, 
                   (PlcReadReference reference, T1 defaultValue) variable1,
                   (PlcReadReference reference, T2 defaultValue) variable2,
                   (PlcReadReference reference, T3 defaultValue) variable3,
                   (PlcReadReference reference, T4 defaultValue) variable4,
                   (PlcReadReference reference, T5 defaultValue) variable5,
                   (PlcReadReference reference, T6 defaultValue) variable6,
                   (PlcReadReference reference, T7 defaultValue) variable7)
        {
            if (papper == null) return ExceptionThrowHelper.ThrowArgumentNullException<(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7)>(nameof(papper));

            T1 value1 = variable1.defaultValue;
            T2 value2 = variable2.defaultValue;
            T3 value3 = variable3.defaultValue;
            T4 value4 = variable4.defaultValue;
            T5 value5 = variable5.defaultValue;
            T6 value6 = variable6.defaultValue;
            T7 value7 = variable7.defaultValue;




            var result = (await papper.ReadAsync(variable1.reference,
                                                 variable2.reference,
                                                 variable3.reference,
                                                 variable4.reference,
                                                 variable5.reference,
                                                 variable6.reference,
                                                 variable7.reference).ConfigureAwait(false))?.ToDictionary(x => x.Variable, x => x.Value);

            if (!result!.IsNullOrEmpty())
            {
                if (result!.TryGetValue(variable1.reference.Address, out var tmp1) && tmp1 is T1 t1) value1 = t1;
                if (result!.TryGetValue(variable2.reference.Address, out var tmp2) && tmp2 is T2 t2) value2 = t2;
                if (result!.TryGetValue(variable3.reference.Address, out var tmp3) && tmp3 is T3 t3) value3 = t3;
                if (result!.TryGetValue(variable4.reference.Address, out var tmp4) && tmp4 is T4 t4) value4 = t4;
                if (result!.TryGetValue(variable5.reference.Address, out var tmp5) && tmp5 is T5 t5) value5 = t5;
                if (result!.TryGetValue(variable6.reference.Address, out var tmp6) && tmp6 is T6 t6) value6 = t6;
                if (result!.TryGetValue(variable7.reference.Address, out var tmp7) && tmp7 is T7 t7) value7 = t7;

            }

            return (value1, value2, value3, value4, value5, value6, value7);
        }


        public async static Task<(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8)>
            ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8>(this PlcDataMapper papper, 
                           (PlcReadReference reference, T1 defaultValue) variable1,
                           (PlcReadReference reference, T2 defaultValue) variable2,
                           (PlcReadReference reference, T3 defaultValue) variable3,
                           (PlcReadReference reference, T4 defaultValue) variable4,
                           (PlcReadReference reference, T5 defaultValue) variable5,
                           (PlcReadReference reference, T6 defaultValue) variable6,
                           (PlcReadReference reference, T7 defaultValue) variable7,
                           (PlcReadReference reference, T8 defaultValue) variable8)
        {
            if (papper == null) return ExceptionThrowHelper.ThrowArgumentNullException<(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8)>(nameof(papper));

            T1 value1 = variable1.defaultValue;
            T2 value2 = variable2.defaultValue;
            T3 value3 = variable3.defaultValue;
            T4 value4 = variable4.defaultValue;
            T5 value5 = variable5.defaultValue;
            T6 value6 = variable6.defaultValue;
            T7 value7 = variable7.defaultValue;
            T8 value8 = variable8.defaultValue;




            var result = (await papper!.ReadAsync(variable1.reference,
                                                 variable2.reference,
                                                 variable3.reference,
                                                 variable4.reference,
                                                 variable5.reference,
                                                 variable6.reference,
                                                 variable7.reference,
                                                 variable8.reference).ConfigureAwait(false))?.ToDictionary(x => x.Variable, x => x.Value);

            if (!result!.IsNullOrEmpty())
            {
                if (result!.TryGetValue(variable1.reference.Address, out var tmp1) && tmp1 is T1 t1) value1 = t1;
                if (result!.TryGetValue(variable2.reference.Address, out var tmp2) && tmp2 is T2 t2) value2 = t2;
                if (result!.TryGetValue(variable3.reference.Address, out var tmp3) && tmp3 is T3 t3) value3 = t3;
                if (result!.TryGetValue(variable4.reference.Address, out var tmp4) && tmp4 is T4 t4) value4 = t4;
                if (result!.TryGetValue(variable5.reference.Address, out var tmp5) && tmp5 is T5 t5) value5 = t5;
                if (result!.TryGetValue(variable6.reference.Address, out var tmp6) && tmp6 is T6 t6) value6 = t6;
                if (result!.TryGetValue(variable7.reference.Address, out var tmp7) && tmp7 is T7 t7) value7 = t7;
                if (result!.TryGetValue(variable8.reference.Address, out var tmp8) && tmp8 is T8 t8) value8 = t8;

            }

            return (value1, value2, value3, value4, value5, value6, value7, value8);
        }

        public async static Task<(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8, T9 value9)>
            ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this PlcDataMapper papper, 
                                   (PlcReadReference reference, T1 defaultValue) variable1,
                                   (PlcReadReference reference, T2 defaultValue) variable2,
                                   (PlcReadReference reference, T3 defaultValue) variable3,
                                   (PlcReadReference reference, T4 defaultValue) variable4,
                                   (PlcReadReference reference, T5 defaultValue) variable5,
                                   (PlcReadReference reference, T6 defaultValue) variable6,
                                   (PlcReadReference reference, T7 defaultValue) variable7,
                                   (PlcReadReference reference, T8 defaultValue) variable8,
                                   (PlcReadReference reference, T9 defaultValue) variable9)
        {
            if (papper == null) return ExceptionThrowHelper.ThrowArgumentNullException<(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8, T9 value9)>(nameof(papper));

            T1 value1 = variable1.defaultValue;
            T2 value2 = variable2.defaultValue;
            T3 value3 = variable3.defaultValue;
            T4 value4 = variable4.defaultValue;
            T5 value5 = variable5.defaultValue;
            T6 value6 = variable6.defaultValue;
            T7 value7 = variable7.defaultValue;
            T8 value8 = variable8.defaultValue;
            T9 value9 = variable9.defaultValue;




            var result = (await papper!.ReadAsync(variable1.reference,
                                                 variable2.reference,
                                                 variable3.reference,
                                                 variable4.reference,
                                                 variable5.reference,
                                                 variable6.reference,
                                                 variable7.reference,
                                                 variable8.reference,
                                                 variable9.reference).ConfigureAwait(false))?.ToDictionary(x => x.Variable, x => x.Value);

            if (!result!.IsNullOrEmpty())
            {
                if (result!.TryGetValue(variable1.reference.Address, out var tmp1) && tmp1 is T1 t1) value1 = t1;
                if (result!.TryGetValue(variable2.reference.Address, out var tmp2) && tmp2 is T2 t2) value2 = t2;
                if (result!.TryGetValue(variable3.reference.Address, out var tmp3) && tmp3 is T3 t3) value3 = t3;
                if (result!.TryGetValue(variable4.reference.Address, out var tmp4) && tmp4 is T4 t4) value4 = t4;
                if (result!.TryGetValue(variable5.reference.Address, out var tmp5) && tmp5 is T5 t5) value5 = t5;
                if (result!.TryGetValue(variable6.reference.Address, out var tmp6) && tmp6 is T6 t6) value6 = t6;
                if (result!.TryGetValue(variable7.reference.Address, out var tmp7) && tmp7 is T7 t7) value7 = t7;
                if (result!.TryGetValue(variable8.reference.Address, out var tmp8) && tmp8 is T8 t8) value8 = t8;
                if (result!.TryGetValue(variable9.reference.Address, out var tmp9) && tmp9 is T9 t9) value9 = t9;

            }

            return (value1, value2, value3, value4, value5, value6, value7, value8, value9);
        }

        public async static Task<(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8, T9 value9, T10 value10)>
            ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this PlcDataMapper papper, 
                                           (PlcReadReference reference, T1 defaultValue) variable1,
                                           (PlcReadReference reference, T2 defaultValue) variable2,
                                           (PlcReadReference reference, T3 defaultValue) variable3,
                                           (PlcReadReference reference, T4 defaultValue) variable4,
                                           (PlcReadReference reference, T5 defaultValue) variable5,
                                           (PlcReadReference reference, T6 defaultValue) variable6,
                                           (PlcReadReference reference, T7 defaultValue) variable7,
                                           (PlcReadReference reference, T8 defaultValue) variable8,
                                           (PlcReadReference reference, T9 defaultValue) variable9,
                                           (PlcReadReference reference, T10 defaultValue) variable10)
        {
            if (papper == null) return ExceptionThrowHelper.ThrowArgumentNullException<(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8, T9 value9, T10 value10)>(nameof(papper));

            T1 value1 = variable1.defaultValue;
            T2 value2 = variable2.defaultValue;
            T3 value3 = variable3.defaultValue;
            T4 value4 = variable4.defaultValue;
            T5 value5 = variable5.defaultValue;
            T6 value6 = variable6.defaultValue;
            T7 value7 = variable7.defaultValue;
            T8 value8 = variable8.defaultValue;
            T9 value9 = variable9.defaultValue;
            T10 value10 = variable10.defaultValue;




            var result = (await papper!.ReadAsync(variable1.reference,
                                                 variable2.reference,
                                                 variable3.reference,
                                                 variable4.reference,
                                                 variable5.reference,
                                                 variable6.reference,
                                                 variable7.reference,
                                                 variable8.reference,
                                                 variable9.reference,
                                                 variable10.reference).ConfigureAwait(false))?.ToDictionary(x => x.Variable, x => x.Value);

            if (!result!.IsNullOrEmpty())
            {
                if (result!.TryGetValue(variable1.reference.Address, out var tmp1) && tmp1 is T1 t1) value1 = t1;
                if (result!.TryGetValue(variable2.reference.Address, out var tmp2) && tmp2 is T2 t2) value2 = t2;
                if (result!.TryGetValue(variable3.reference.Address, out var tmp3) && tmp3 is T3 t3) value3 = t3;
                if (result!.TryGetValue(variable4.reference.Address, out var tmp4) && tmp4 is T4 t4) value4 = t4;
                if (result!.TryGetValue(variable5.reference.Address, out var tmp5) && tmp5 is T5 t5) value5 = t5;
                if (result!.TryGetValue(variable6.reference.Address, out var tmp6) && tmp6 is T6 t6) value6 = t6;
                if (result!.TryGetValue(variable7.reference.Address, out var tmp7) && tmp7 is T7 t7) value7 = t7;
                if (result!.TryGetValue(variable8.reference.Address, out var tmp8) && tmp8 is T8 t8) value8 = t8;
                if (result!.TryGetValue(variable9.reference.Address, out var tmp9) && tmp9 is T9 t9) value9 = t9;
                if (result!.TryGetValue(variable10.reference.Address, out var tmp10) && tmp10 is T10 t10) value10 = t10;

            }

            return (value1, value2, value3, value4, value5, value6, value7, value8, value9, value10);
        }


        public async static Task<(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8, T9 value9, T10 value10, T11 value11)>
            ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this PlcDataMapper papper, 
                                                   (PlcReadReference reference, T1 defaultValue) variable1,
                                                   (PlcReadReference reference, T2 defaultValue) variable2,
                                                   (PlcReadReference reference, T3 defaultValue) variable3,
                                                   (PlcReadReference reference, T4 defaultValue) variable4,
                                                   (PlcReadReference reference, T5 defaultValue) variable5,
                                                   (PlcReadReference reference, T6 defaultValue) variable6,
                                                   (PlcReadReference reference, T7 defaultValue) variable7,
                                                   (PlcReadReference reference, T8 defaultValue) variable8,
                                                   (PlcReadReference reference, T9 defaultValue) variable9,
                                                   (PlcReadReference reference, T10 defaultValue) variable10,
                                                   (PlcReadReference reference, T11 defaultValue) variable11)
        {
            if (papper == null) return ExceptionThrowHelper.ThrowArgumentNullException<(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8, T9 value9, T10 value10, T11 value11)>(nameof(papper));

            T1 value1 = variable1.defaultValue;
            T2 value2 = variable2.defaultValue;
            T3 value3 = variable3.defaultValue;
            T4 value4 = variable4.defaultValue;
            T5 value5 = variable5.defaultValue;
            T6 value6 = variable6.defaultValue;
            T7 value7 = variable7.defaultValue;
            T8 value8 = variable8.defaultValue;
            T9 value9 = variable9.defaultValue;
            T10 value10 = variable10.defaultValue;
            T11 value11 = variable11.defaultValue;



            var result = (await papper!.ReadAsync(variable1.reference,
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

            if (!result!.IsNullOrEmpty())
            {
                if (result!.TryGetValue(variable1.reference.Address, out var tmp1) && tmp1 is T1 t1) value1 = t1;
                if (result!.TryGetValue(variable2.reference.Address, out var tmp2) && tmp2 is T2 t2) value2 = t2;
                if (result!.TryGetValue(variable3.reference.Address, out var tmp3) && tmp3 is T3 t3) value3 = t3;
                if (result!.TryGetValue(variable4.reference.Address, out var tmp4) && tmp4 is T4 t4) value4 = t4;
                if (result!.TryGetValue(variable5.reference.Address, out var tmp5) && tmp5 is T5 t5) value5 = t5;
                if (result!.TryGetValue(variable6.reference.Address, out var tmp6) && tmp6 is T6 t6) value6 = t6;
                if (result!.TryGetValue(variable7.reference.Address, out var tmp7) && tmp7 is T7 t7) value7 = t7;
                if (result!.TryGetValue(variable8.reference.Address, out var tmp8) && tmp8 is T8 t8) value8 = t8;
                if (result!.TryGetValue(variable9.reference.Address, out var tmp9) && tmp9 is T9 t9) value9 = t9;
                if (result!.TryGetValue(variable10.reference.Address, out var tmp10) && tmp10 is T10 t10) value10 = t10;
                if (result!.TryGetValue(variable11.reference.Address, out var tmp11) && tmp11 is T11 t11) value11 = t11;

            }

            return (value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11);
        }


        public async static Task<(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8, T9 value9, T10 value10, T11 value11, T12 value12)>
            ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this PlcDataMapper papper, 
                                                           (PlcReadReference reference, T1 defaultValue) variable1,
                                                           (PlcReadReference reference, T2 defaultValue) variable2,
                                                           (PlcReadReference reference, T3 defaultValue) variable3,
                                                           (PlcReadReference reference, T4 defaultValue) variable4,
                                                           (PlcReadReference reference, T5 defaultValue) variable5,
                                                           (PlcReadReference reference, T6 defaultValue) variable6,
                                                           (PlcReadReference reference, T7 defaultValue) variable7,
                                                           (PlcReadReference reference, T8 defaultValue) variable8,
                                                           (PlcReadReference reference, T9 defaultValue) variable9,
                                                           (PlcReadReference reference, T10 defaultValue) variable10,
                                                           (PlcReadReference reference, T11 defaultValue) variable11,
                                                           (PlcReadReference reference, T12 defaultValue) variable12)
        {
            if (papper == null) return ExceptionThrowHelper.ThrowArgumentNullException<(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8, T9 value9, T10 value10, T11 value11, T12 value12)>(nameof(papper));

            T1 value1 = variable1.defaultValue;
            T2 value2 = variable2.defaultValue;
            T3 value3 = variable3.defaultValue;
            T4 value4 = variable4.defaultValue;
            T5 value5 = variable5.defaultValue;
            T6 value6 = variable6.defaultValue;
            T7 value7 = variable7.defaultValue;
            T8 value8 = variable8.defaultValue;
            T9 value9 = variable9.defaultValue;
            T10 value10 = variable10.defaultValue;
            T11 value11 = variable11.defaultValue;
            T12 value12 = variable12.defaultValue;


            var result = (await papper!.ReadAsync(variable1.reference,
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

            if (!result!.IsNullOrEmpty())
            {
                if (result!.TryGetValue(variable1.reference.Address, out var tmp1) && tmp1 is T1 t1) value1 = t1;
                if (result!.TryGetValue(variable2.reference.Address, out var tmp2) && tmp2 is T2 t2) value2 = t2;
                if (result!.TryGetValue(variable3.reference.Address, out var tmp3) && tmp3 is T3 t3) value3 = t3;
                if (result!.TryGetValue(variable4.reference.Address, out var tmp4) && tmp4 is T4 t4) value4 = t4;
                if (result!.TryGetValue(variable5.reference.Address, out var tmp5) && tmp5 is T5 t5) value5 = t5;
                if (result!.TryGetValue(variable6.reference.Address, out var tmp6) && tmp6 is T6 t6) value6 = t6;
                if (result!.TryGetValue(variable7.reference.Address, out var tmp7) && tmp7 is T7 t7) value7 = t7;
                if (result!.TryGetValue(variable8.reference.Address, out var tmp8) && tmp8 is T8 t8) value8 = t8;
                if (result!.TryGetValue(variable9.reference.Address, out var tmp9) && tmp9 is T9 t9) value9 = t9;
                if (result!.TryGetValue(variable10.reference.Address, out var tmp10) && tmp10 is T10 t10) value10 = t10;
                if (result!.TryGetValue(variable11.reference.Address, out var tmp11) && tmp11 is T11 t11) value11 = t11;
                if (result!.TryGetValue(variable12.reference.Address, out var tmp12) && tmp12 is T12 t12) value12 = t12;
            }

            return (value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12);
        }

        public async static Task<(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8, T9 value9, T10 value10, T11 value11, T12 value12, T13 value13)>
            ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this PlcDataMapper papper, 
                                                           (PlcReadReference reference, T1 defaultValue) variable1,
                                                           (PlcReadReference reference, T2 defaultValue) variable2,
                                                           (PlcReadReference reference, T3 defaultValue) variable3,
                                                           (PlcReadReference reference, T4 defaultValue) variable4,
                                                           (PlcReadReference reference, T5 defaultValue) variable5,
                                                           (PlcReadReference reference, T6 defaultValue) variable6,
                                                           (PlcReadReference reference, T7 defaultValue) variable7,
                                                           (PlcReadReference reference, T8 defaultValue) variable8,
                                                           (PlcReadReference reference, T9 defaultValue) variable9,
                                                           (PlcReadReference reference, T10 defaultValue) variable10,
                                                           (PlcReadReference reference, T11 defaultValue) variable11,
                                                           (PlcReadReference reference, T12 defaultValue) variable12,
                                                           (PlcReadReference reference, T13 defaultValue) variable13)
        {
            if (papper == null) return ExceptionThrowHelper.ThrowArgumentNullException<(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8, T9 value9, T10 value10, T11 value11, T12 value12, T13 value13)>(nameof(papper));

            T1 value1 = variable1.defaultValue;
            T2 value2 = variable2.defaultValue;
            T3 value3 = variable3.defaultValue;
            T4 value4 = variable4.defaultValue;
            T5 value5 = variable5.defaultValue;
            T6 value6 = variable6.defaultValue;
            T7 value7 = variable7.defaultValue;
            T8 value8 = variable8.defaultValue;
            T9 value9 = variable9.defaultValue;
            T10 value10 = variable10.defaultValue;
            T11 value11 = variable11.defaultValue;
            T12 value12 = variable12.defaultValue;
            T13 value13 = variable13.defaultValue;


            var result = (await papper!.ReadAsync(variable1.reference,
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

            if (!result!.IsNullOrEmpty())
            {
                if (result!.TryGetValue(variable1.reference.Address, out var tmp1) && tmp1 is T1 t1) value1 = t1;
                if (result!.TryGetValue(variable2.reference.Address, out var tmp2) && tmp2 is T2 t2) value2 = t2;
                if (result!.TryGetValue(variable3.reference.Address, out var tmp3) && tmp3 is T3 t3) value3 = t3;
                if (result!.TryGetValue(variable4.reference.Address, out var tmp4) && tmp4 is T4 t4) value4 = t4;
                if (result!.TryGetValue(variable5.reference.Address, out var tmp5) && tmp5 is T5 t5) value5 = t5;
                if (result!.TryGetValue(variable6.reference.Address, out var tmp6) && tmp6 is T6 t6) value6 = t6;
                if (result!.TryGetValue(variable7.reference.Address, out var tmp7) && tmp7 is T7 t7) value7 = t7;
                if (result!.TryGetValue(variable8.reference.Address, out var tmp8) && tmp8 is T8 t8) value8 = t8;
                if (result!.TryGetValue(variable9.reference.Address, out var tmp9) && tmp9 is T9 t9) value9 = t9;
                if (result!.TryGetValue(variable10.reference.Address, out var tmp10) && tmp10 is T10 t10) value10 = t10;
                if (result!.TryGetValue(variable11.reference.Address, out var tmp11) && tmp11 is T11 t11) value11 = t11;
                if (result!.TryGetValue(variable12.reference.Address, out var tmp12) && tmp12 is T12 t12) value12 = t12;
                if (result!.TryGetValue(variable13.reference.Address, out var tmp13) && tmp13 is T13 t13) value13 = t13;
            }

            return (value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13);
        }



        public async static Task<(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8, T9 value9, T10 value10, T11 value11, T12 value12, T13 value13, T14 value14)>
            ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this PlcDataMapper papper, 
                                                                   (PlcReadReference reference, T1 defaultValue) variable1,
                                                                   (PlcReadReference reference, T2 defaultValue) variable2,
                                                                   (PlcReadReference reference, T3 defaultValue) variable3,
                                                                   (PlcReadReference reference, T4 defaultValue) variable4,
                                                                   (PlcReadReference reference, T5 defaultValue) variable5,
                                                                   (PlcReadReference reference, T6 defaultValue) variable6,
                                                                   (PlcReadReference reference, T7 defaultValue) variable7,
                                                                   (PlcReadReference reference, T8 defaultValue) variable8,
                                                                   (PlcReadReference reference, T9 defaultValue) variable9,
                                                                   (PlcReadReference reference, T10 defaultValue) variable10,
                                                                   (PlcReadReference reference, T11 defaultValue) variable11,
                                                                   (PlcReadReference reference, T12 defaultValue) variable12,
                                                                   (PlcReadReference reference, T13 defaultValue) variable13,
                                                                   (PlcReadReference reference, T14 defaultValue) variable14)
        {
            if (papper == null) return ExceptionThrowHelper.ThrowArgumentNullException<(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8, T9 value9, T10 value10, T11 value11, T12 value12, T13 value13, T14 value14)>(nameof(papper));

            T1 value1 = variable1.defaultValue;
            T2 value2 = variable2.defaultValue;
            T3 value3 = variable3.defaultValue;
            T4 value4 = variable4.defaultValue;
            T5 value5 = variable5.defaultValue;
            T6 value6 = variable6.defaultValue;
            T7 value7 = variable7.defaultValue;
            T8 value8 = variable8.defaultValue;
            T9 value9 = variable9.defaultValue;
            T10 value10 = variable10.defaultValue;
            T11 value11 = variable11.defaultValue;
            T12 value12 = variable12.defaultValue;
            T13 value13 = variable13.defaultValue;
            T14 value14 = variable14.defaultValue;


            var result = (await papper!.ReadAsync(variable1.reference,
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

            if (!result!.IsNullOrEmpty())
            {
                if (result!.TryGetValue(variable1.reference.Address, out var tmp1) && tmp1 is T1 t1) value1 = t1;
                if (result!.TryGetValue(variable2.reference.Address, out var tmp2) && tmp2 is T2 t2) value2 = t2;
                if (result!.TryGetValue(variable3.reference.Address, out var tmp3) && tmp3 is T3 t3) value3 = t3;
                if (result!.TryGetValue(variable4.reference.Address, out var tmp4) && tmp4 is T4 t4) value4 = t4;
                if (result!.TryGetValue(variable5.reference.Address, out var tmp5) && tmp5 is T5 t5) value5 = t5;
                if (result!.TryGetValue(variable6.reference.Address, out var tmp6) && tmp6 is T6 t6) value6 = t6;
                if (result!.TryGetValue(variable7.reference.Address, out var tmp7) && tmp7 is T7 t7) value7 = t7;
                if (result!.TryGetValue(variable8.reference.Address, out var tmp8) && tmp8 is T8 t8) value8 = t8;
                if (result!.TryGetValue(variable9.reference.Address, out var tmp9) && tmp9 is T9 t9) value9 = t9;
                if (result!.TryGetValue(variable10.reference.Address, out var tmp10) && tmp10 is T10 t10) value10 = t10;
                if (result!.TryGetValue(variable11.reference.Address, out var tmp11) && tmp11 is T11 t11) value11 = t11;
                if (result!.TryGetValue(variable12.reference.Address, out var tmp12) && tmp12 is T12 t12) value12 = t12;
                if (result!.TryGetValue(variable13.reference.Address, out var tmp13) && tmp13 is T13 t13) value13 = t13;
                if (result!.TryGetValue(variable14.reference.Address, out var tmp14) && tmp14 is T14 t14) value14 = t14;
            }

            return (value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14);
        }

        public async static Task<(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8, T9 value9, T10 value10, T11 value11, T12 value12, T13 value13, T14 value14, T15 value15)>
            ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this PlcDataMapper papper, 
                                                                           (PlcReadReference reference, T1 defaultValue) variable1,
                                                                           (PlcReadReference reference, T2 defaultValue) variable2,
                                                                           (PlcReadReference reference, T3 defaultValue) variable3,
                                                                           (PlcReadReference reference, T4 defaultValue) variable4,
                                                                           (PlcReadReference reference, T5 defaultValue) variable5,
                                                                           (PlcReadReference reference, T6 defaultValue) variable6,
                                                                           (PlcReadReference reference, T7 defaultValue) variable7,
                                                                           (PlcReadReference reference, T8 defaultValue) variable8,
                                                                           (PlcReadReference reference, T9 defaultValue) variable9,
                                                                           (PlcReadReference reference, T10 defaultValue) variable10,
                                                                           (PlcReadReference reference, T11 defaultValue) variable11,
                                                                           (PlcReadReference reference, T12 defaultValue) variable12,
                                                                           (PlcReadReference reference, T13 defaultValue) variable13,
                                                                           (PlcReadReference reference, T14 defaultValue) variable14,
                                                                           (PlcReadReference reference, T15 defaultValue) variable15)
        {
            if (papper == null) return ExceptionThrowHelper.ThrowArgumentNullException<(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8, T9 value9, T10 value10, T11 value11, T12 value12, T13 value13, T14 value14, T15 value15)>(nameof(papper));

            T1 value1 = variable1.defaultValue;
            T2 value2 = variable2.defaultValue;
            T3 value3 = variable3.defaultValue;
            T4 value4 = variable4.defaultValue;
            T5 value5 = variable5.defaultValue;
            T6 value6 = variable6.defaultValue;
            T7 value7 = variable7.defaultValue;
            T8 value8 = variable8.defaultValue;
            T9 value9 = variable9.defaultValue;
            T10 value10 = variable10.defaultValue;
            T11 value11 = variable11.defaultValue;
            T12 value12 = variable12.defaultValue;
            T13 value13 = variable13.defaultValue;
            T14 value14 = variable14.defaultValue;
            T15 value15 = variable15.defaultValue;

            var result = (await papper!.ReadAsync(variable1.reference,
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

            if (!result!.IsNullOrEmpty())
            {
                if (result!.TryGetValue(variable1.reference.Address, out var tmp1) && tmp1 is T1 t1) value1 = t1;
                if (result!.TryGetValue(variable2.reference.Address, out var tmp2) && tmp2 is T2 t2) value2 = t2;
                if (result!.TryGetValue(variable3.reference.Address, out var tmp3) && tmp3 is T3 t3) value3 = t3;
                if (result!.TryGetValue(variable4.reference.Address, out var tmp4) && tmp4 is T4 t4) value4 = t4;
                if (result!.TryGetValue(variable5.reference.Address, out var tmp5) && tmp5 is T5 t5) value5 = t5;
                if (result!.TryGetValue(variable6.reference.Address, out var tmp6) && tmp6 is T6 t6) value6 = t6;
                if (result!.TryGetValue(variable7.reference.Address, out var tmp7) && tmp7 is T7 t7) value7 = t7;
                if (result!.TryGetValue(variable8.reference.Address, out var tmp8) && tmp8 is T8 t8) value8 = t8;
                if (result!.TryGetValue(variable9.reference.Address, out var tmp9) && tmp9 is T9 t9) value9 = t9;
                if (result!.TryGetValue(variable10.reference.Address, out var tmp10) && tmp10 is T10 t10) value10 = t10;
                if (result!.TryGetValue(variable11.reference.Address, out var tmp11) && tmp11 is T11 t11) value11 = t11;
                if (result!.TryGetValue(variable12.reference.Address, out var tmp12) && tmp12 is T12 t12) value12 = t12;
                if (result!.TryGetValue(variable13.reference.Address, out var tmp13) && tmp13 is T13 t13) value13 = t13;
                if (result!.TryGetValue(variable14.reference.Address, out var tmp14) && tmp14 is T14 t14) value14 = t14;
                if (result!.TryGetValue(variable15.reference.Address, out var tmp15) && tmp15 is T15 t15) value15 = t15;
            }

            return (value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15);
        }

        public async static Task<(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8, T9 value9, T10 value10, T11 value11, T12 value12, T13 value13, T14 value14, T15 value15, T16 value16)>
            ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this PlcDataMapper papper, 
                                                                           (PlcReadReference reference, T1 defaultValue) variable1,
                                                                           (PlcReadReference reference, T2 defaultValue) variable2,
                                                                           (PlcReadReference reference, T3 defaultValue) variable3,
                                                                           (PlcReadReference reference, T4 defaultValue) variable4,
                                                                           (PlcReadReference reference, T5 defaultValue) variable5,
                                                                           (PlcReadReference reference, T6 defaultValue) variable6,
                                                                           (PlcReadReference reference, T7 defaultValue) variable7,
                                                                           (PlcReadReference reference, T8 defaultValue) variable8,
                                                                           (PlcReadReference reference, T9 defaultValue) variable9,
                                                                           (PlcReadReference reference, T10 defaultValue) variable10,
                                                                           (PlcReadReference reference, T11 defaultValue) variable11,
                                                                           (PlcReadReference reference, T12 defaultValue) variable12,
                                                                           (PlcReadReference reference, T13 defaultValue) variable13,
                                                                           (PlcReadReference reference, T14 defaultValue) variable14,
                                                                           (PlcReadReference reference, T15 defaultValue) variable15,
                                                                           (PlcReadReference reference, T16 defaultValue) variable16)
        {
            if (papper == null) return ExceptionThrowHelper.ThrowArgumentNullException<(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8, T9 value9, T10 value10, T11 value11, T12 value12, T13 value13, T14 value14, T15 value15, T16 value16)>(nameof(papper));

            T1 value1 = variable1.defaultValue;
            T2 value2 = variable2.defaultValue;
            T3 value3 = variable3.defaultValue;
            T4 value4 = variable4.defaultValue;
            T5 value5 = variable5.defaultValue;
            T6 value6 = variable6.defaultValue;
            T7 value7 = variable7.defaultValue;
            T8 value8 = variable8.defaultValue;
            T9 value9 = variable9.defaultValue;
            T10 value10 = variable10.defaultValue;
            T11 value11 = variable11.defaultValue;
            T12 value12 = variable12.defaultValue;
            T13 value13 = variable13.defaultValue;
            T14 value14 = variable14.defaultValue;
            T15 value15 = variable15.defaultValue;
            T16 value16 = variable16.defaultValue;

            var result = (await papper!.ReadAsync(variable1.reference,
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

            if (!result!.IsNullOrEmpty())
            {
                if (result!.TryGetValue(variable1.reference.Address, out var tmp1) && tmp1 is T1 t1) value1 = t1;
                if (result!.TryGetValue(variable2.reference.Address, out var tmp2) && tmp2 is T2 t2) value2 = t2;
                if (result!.TryGetValue(variable3.reference.Address, out var tmp3) && tmp3 is T3 t3) value3 = t3;
                if (result!.TryGetValue(variable4.reference.Address, out var tmp4) && tmp4 is T4 t4) value4 = t4;
                if (result!.TryGetValue(variable5.reference.Address, out var tmp5) && tmp5 is T5 t5) value5 = t5;
                if (result!.TryGetValue(variable6.reference.Address, out var tmp6) && tmp6 is T6 t6) value6 = t6;
                if (result!.TryGetValue(variable7.reference.Address, out var tmp7) && tmp7 is T7 t7) value7 = t7;
                if (result!.TryGetValue(variable8.reference.Address, out var tmp8) && tmp8 is T8 t8) value8 = t8;
                if (result!.TryGetValue(variable9.reference.Address, out var tmp9) && tmp9 is T9 t9) value9 = t9;
                if (result!.TryGetValue(variable10.reference.Address, out var tmp10) && tmp10 is T10 t10) value10 = t10;
                if (result!.TryGetValue(variable11.reference.Address, out var tmp11) && tmp11 is T11 t11) value11 = t11;
                if (result!.TryGetValue(variable12.reference.Address, out var tmp12) && tmp12 is T12 t12) value12 = t12;
                if (result!.TryGetValue(variable13.reference.Address, out var tmp13) && tmp13 is T13 t13) value13 = t13;
                if (result!.TryGetValue(variable14.reference.Address, out var tmp14) && tmp14 is T14 t14) value14 = t14;
                if (result!.TryGetValue(variable15.reference.Address, out var tmp15) && tmp15 is T15 t15) value15 = t15;
                if (result!.TryGetValue(variable15.reference.Address, out var tmp16) && tmp16 is T16 t16) value16 = t16;
            }

            return (value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15, value16);
        }
    }
}
