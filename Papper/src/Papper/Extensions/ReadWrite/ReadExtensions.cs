using Papper.Internal;
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
            T1 value1 = variable1.defaultValue;
            T2 value2 = variable2.defaultValue;

            var result = (await papper.ReadAsync(variable1.reference,
                                                 variable2.reference))?.ToDictionary(x => x.Variable, x => x.Value);

            if (!result.IsNullOrEmpty())
            {
                if (result.TryGetValue(variable1.reference.Address, out var tmp1)) value1 = (T1)tmp1;
                if (result.TryGetValue(variable2.reference.Address, out var tmp2)) value2 = (T2)tmp2;
            }

            return (value1, value2);
        }

        public async static Task<(T1 value1, T2 value2, T3 value3)> ReadAsync<T1, T2, T3>(this PlcDataMapper papper, 
                                                                                           (PlcReadReference reference, T1 defaultValue) variable1,
                                                                                           (PlcReadReference reference, T2 defaultValue) variable2,
                                                                                           (PlcReadReference reference, T3 defaultValue) variable3)
        {
            T1 value1 = variable1.defaultValue;
            T2 value2 = variable2.defaultValue;
            T3 value3 = variable3.defaultValue;

            var result = (await papper.ReadAsync(
                                                 variable1.reference,
                                                 variable2.reference,
                                                 variable3.reference))?.ToDictionary(x => x.Variable, x => x.Value);

            if (!result.IsNullOrEmpty())
            {
                if (result.TryGetValue(variable1.reference.Address, out var tmp1)) value1 = (T1)tmp1;
                if (result.TryGetValue(variable2.reference.Address, out var tmp2)) value2 = (T2)tmp2;
                if (result.TryGetValue(variable3.reference.Address, out var tmp3)) value3 = (T3)tmp3;
            }

            return (value1, value2, value3);
        }

        public async static Task<(T1 value1, T2 value2, T3 value3, T4 value4)> ReadAsync<T1, T2, T3, T4>(this PlcDataMapper papper, 
                                                                                   (PlcReadReference reference, T1 defaultValue) variable1,
                                                                                   (PlcReadReference reference, T2 defaultValue) variable2,
                                                                                   (PlcReadReference reference, T3 defaultValue) variable3,
                                                                                   (PlcReadReference reference, T4 defaultValue) variable4)
        {
            T1 value1 = variable1.defaultValue;
            T2 value2 = variable2.defaultValue;
            T3 value3 = variable3.defaultValue;
            T4 value4 = variable4.defaultValue;

            var result = (await papper.ReadAsync(
                                                 variable1.reference,
                                                 variable2.reference,
                                                 variable3.reference,
                                                 variable4.reference))?.ToDictionary(x => x.Variable, x => x.Value);

            if (!result.IsNullOrEmpty())
            {
                if (result.TryGetValue(variable1.reference.Address, out var tmp1)) value1 = (T1)tmp1;
                if (result.TryGetValue(variable2.reference.Address, out var tmp2)) value2 = (T2)tmp2;
                if (result.TryGetValue(variable3.reference.Address, out var tmp3)) value3 = (T3)tmp3;
                if (result.TryGetValue(variable4.reference.Address, out var tmp4)) value4 = (T4)tmp4;
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
                                                 variable5.reference))?.ToDictionary(x => x.Variable, x => x.Value);

            if (!result.IsNullOrEmpty())
            {
                if (result.TryGetValue(variable1.reference.Address, out var tmp1)) value1 = (T1)tmp1;
                if (result.TryGetValue(variable2.reference.Address, out var tmp2)) value2 = (T2)tmp2;
                if (result.TryGetValue(variable3.reference.Address, out var tmp3)) value3 = (T3)tmp3;
                if (result.TryGetValue(variable4.reference.Address, out var tmp4)) value4 = (T4)tmp4;
                if (result.TryGetValue(variable5.reference.Address, out var tmp5)) value5 = (T5)tmp5;


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
                                                 variable6.reference))?.ToDictionary(x => x.Variable, x => x.Value);

            if (!result.IsNullOrEmpty())
            {
                if (result.TryGetValue(variable1.reference.Address, out var tmp1)) value1 = (T1)tmp1;
                if (result.TryGetValue(variable2.reference.Address, out var tmp2)) value2 = (T2)tmp2;
                if (result.TryGetValue(variable3.reference.Address, out var tmp3)) value3 = (T3)tmp3;
                if (result.TryGetValue(variable4.reference.Address, out var tmp4)) value4 = (T4)tmp4;
                if (result.TryGetValue(variable5.reference.Address, out var tmp5)) value5 = (T5)tmp5;
                if (result.TryGetValue(variable6.reference.Address, out var tmp6)) value6 = (T6)tmp6;

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
                                                 variable7.reference))?.ToDictionary(x => x.Variable, x => x.Value);

            if (!result.IsNullOrEmpty())
            {
                if (result.TryGetValue(variable1.reference.Address, out var tmp1)) value1 = (T1)tmp1;
                if (result.TryGetValue(variable2.reference.Address, out var tmp2)) value2 = (T2)tmp2;
                if (result.TryGetValue(variable3.reference.Address, out var tmp3)) value3 = (T3)tmp3;
                if (result.TryGetValue(variable4.reference.Address, out var tmp4)) value4 = (T4)tmp4;
                if (result.TryGetValue(variable5.reference.Address, out var tmp5)) value5 = (T5)tmp5;
                if (result.TryGetValue(variable6.reference.Address, out var tmp6)) value6 = (T6)tmp6;
                if (result.TryGetValue(variable7.reference.Address, out var tmp7)) value7 = (T7)tmp7;

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
            T1 value1 = variable1.defaultValue;
            T2 value2 = variable2.defaultValue;
            T3 value3 = variable3.defaultValue;
            T4 value4 = variable4.defaultValue;
            T5 value5 = variable5.defaultValue;
            T6 value6 = variable6.defaultValue;
            T7 value7 = variable7.defaultValue;
            T8 value8 = variable8.defaultValue;




            var result = (await papper.ReadAsync(variable1.reference,
                                                 variable2.reference,
                                                 variable3.reference,
                                                 variable4.reference,
                                                 variable5.reference,
                                                 variable6.reference,
                                                 variable7.reference,
                                                 variable8.reference))?.ToDictionary(x => x.Variable, x => x.Value);

            if (!result.IsNullOrEmpty())
            {
                if (result.TryGetValue(variable1.reference.Address, out var tmp1)) value1 = (T1)tmp1;
                if (result.TryGetValue(variable2.reference.Address, out var tmp2)) value2 = (T2)tmp2;
                if (result.TryGetValue(variable3.reference.Address, out var tmp3)) value3 = (T3)tmp3;
                if (result.TryGetValue(variable4.reference.Address, out var tmp4)) value4 = (T4)tmp4;
                if (result.TryGetValue(variable5.reference.Address, out var tmp5)) value5 = (T5)tmp5;
                if (result.TryGetValue(variable6.reference.Address, out var tmp6)) value6 = (T6)tmp6;
                if (result.TryGetValue(variable7.reference.Address, out var tmp7)) value7 = (T7)tmp7;
                if (result.TryGetValue(variable8.reference.Address, out var tmp8)) value8 = (T8)tmp8;

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
            T1 value1 = variable1.defaultValue;
            T2 value2 = variable2.defaultValue;
            T3 value3 = variable3.defaultValue;
            T4 value4 = variable4.defaultValue;
            T5 value5 = variable5.defaultValue;
            T6 value6 = variable6.defaultValue;
            T7 value7 = variable7.defaultValue;
            T8 value8 = variable8.defaultValue;
            T9 value9 = variable9.defaultValue;




            var result = (await papper.ReadAsync(variable1.reference,
                                                 variable2.reference,
                                                 variable3.reference,
                                                 variable4.reference,
                                                 variable5.reference,
                                                 variable6.reference,
                                                 variable7.reference,
                                                 variable8.reference,
                                                 variable9.reference))?.ToDictionary(x => x.Variable, x => x.Value);

            if (!result.IsNullOrEmpty())
            {
                if (result.TryGetValue(variable1.reference.Address, out var tmp1)) value1 = (T1)tmp1;
                if (result.TryGetValue(variable2.reference.Address, out var tmp2)) value2 = (T2)tmp2;
                if (result.TryGetValue(variable3.reference.Address, out var tmp3)) value3 = (T3)tmp3;
                if (result.TryGetValue(variable4.reference.Address, out var tmp4)) value4 = (T4)tmp4;
                if (result.TryGetValue(variable5.reference.Address, out var tmp5)) value5 = (T5)tmp5;
                if (result.TryGetValue(variable6.reference.Address, out var tmp6)) value6 = (T6)tmp6;
                if (result.TryGetValue(variable7.reference.Address, out var tmp7)) value7 = (T7)tmp7;
                if (result.TryGetValue(variable8.reference.Address, out var tmp8)) value8 = (T8)tmp8;
                if (result.TryGetValue(variable9.reference.Address, out var tmp9)) value9 = (T9)tmp9;

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




            var result = (await papper.ReadAsync(variable1.reference,
                                                 variable2.reference,
                                                 variable3.reference,
                                                 variable4.reference,
                                                 variable5.reference,
                                                 variable6.reference,
                                                 variable7.reference,
                                                 variable8.reference,
                                                 variable9.reference,
                                                 variable10.reference))?.ToDictionary(x => x.Variable, x => x.Value);

            if (!result.IsNullOrEmpty())
            {
                if (result.TryGetValue(variable1.reference.Address, out var tmp1)) value1 = (T1)tmp1;
                if (result.TryGetValue(variable2.reference.Address, out var tmp2)) value2 = (T2)tmp2;
                if (result.TryGetValue(variable3.reference.Address, out var tmp3)) value3 = (T3)tmp3;
                if (result.TryGetValue(variable4.reference.Address, out var tmp4)) value4 = (T4)tmp4;
                if (result.TryGetValue(variable5.reference.Address, out var tmp5)) value5 = (T5)tmp5;
                if (result.TryGetValue(variable6.reference.Address, out var tmp6)) value6 = (T6)tmp6;
                if (result.TryGetValue(variable7.reference.Address, out var tmp7)) value7 = (T7)tmp7;
                if (result.TryGetValue(variable8.reference.Address, out var tmp8)) value8 = (T8)tmp8;
                if (result.TryGetValue(variable9.reference.Address, out var tmp9)) value9 = (T9)tmp9;
                if (result.TryGetValue(variable10.reference.Address, out var tmp10)) value10 = (T10)tmp10;

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



            var result = (await papper.ReadAsync(variable1.reference,
                                                 variable2.reference,
                                                 variable3.reference,
                                                 variable4.reference,
                                                 variable5.reference,
                                                 variable6.reference,
                                                 variable7.reference,
                                                 variable8.reference,
                                                 variable9.reference,
                                                 variable10.reference,
                                                 variable11.reference))?.ToDictionary(x => x.Variable, x => x.Value);

            if (!result.IsNullOrEmpty())
            {
                if (result.TryGetValue(variable1.reference.Address, out var tmp1)) value1 = (T1)tmp1;
                if (result.TryGetValue(variable2.reference.Address, out var tmp2)) value2 = (T2)tmp2;
                if (result.TryGetValue(variable3.reference.Address, out var tmp3)) value3 = (T3)tmp3;
                if (result.TryGetValue(variable4.reference.Address, out var tmp4)) value4 = (T4)tmp4;
                if (result.TryGetValue(variable5.reference.Address, out var tmp5)) value5 = (T5)tmp5;
                if (result.TryGetValue(variable6.reference.Address, out var tmp6)) value6 = (T6)tmp6;
                if (result.TryGetValue(variable7.reference.Address, out var tmp7)) value7 = (T7)tmp7;
                if (result.TryGetValue(variable8.reference.Address, out var tmp8)) value8 = (T8)tmp8;
                if (result.TryGetValue(variable9.reference.Address, out var tmp9)) value9 = (T9)tmp9;
                if (result.TryGetValue(variable10.reference.Address, out var tmp10)) value10 = (T10)tmp10;
                if (result.TryGetValue(variable11.reference.Address, out var tmp11)) value11 = (T11)tmp11;

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


            var result = (await papper.ReadAsync(variable1.reference,
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
                                                 variable12.reference))?.ToDictionary(x => x.Variable, x => x.Value);

            if (!result.IsNullOrEmpty())
            {
                if (result.TryGetValue(variable1.reference.Address, out var tmp1)) value1 = (T1)tmp1;
                if (result.TryGetValue(variable2.reference.Address, out var tmp2)) value2 = (T2)tmp2;
                if (result.TryGetValue(variable3.reference.Address, out var tmp3)) value3 = (T3)tmp3;
                if (result.TryGetValue(variable4.reference.Address, out var tmp4)) value4 = (T4)tmp4;
                if (result.TryGetValue(variable5.reference.Address, out var tmp5)) value5 = (T5)tmp5;
                if (result.TryGetValue(variable6.reference.Address, out var tmp6)) value6 = (T6)tmp6;
                if (result.TryGetValue(variable7.reference.Address, out var tmp7)) value7 = (T7)tmp7;
                if (result.TryGetValue(variable8.reference.Address, out var tmp8)) value8 = (T8)tmp8;
                if (result.TryGetValue(variable9.reference.Address, out var tmp9)) value9 = (T9)tmp9;
                if (result.TryGetValue(variable10.reference.Address, out var tmp10)) value10 = (T10)tmp10;
                if (result.TryGetValue(variable11.reference.Address, out var tmp11)) value11 = (T11)tmp11;
                if (result.TryGetValue(variable12.reference.Address, out var tmp12)) value12 = (T12)tmp12;
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


            var result = (await papper.ReadAsync(variable1.reference,
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
                                                 variable13.reference))?.ToDictionary(x => x.Variable, x => x.Value);

            if (!result.IsNullOrEmpty())
            {
                if (result.TryGetValue(variable1.reference.Address, out var tmp1)) value1 = (T1)tmp1;
                if (result.TryGetValue(variable2.reference.Address, out var tmp2)) value2 = (T2)tmp2;
                if (result.TryGetValue(variable3.reference.Address, out var tmp3)) value3 = (T3)tmp3;
                if (result.TryGetValue(variable4.reference.Address, out var tmp4)) value4 = (T4)tmp4;
                if (result.TryGetValue(variable5.reference.Address, out var tmp5)) value5 = (T5)tmp5;
                if (result.TryGetValue(variable6.reference.Address, out var tmp6)) value6 = (T6)tmp6;
                if (result.TryGetValue(variable7.reference.Address, out var tmp7)) value7 = (T7)tmp7;
                if (result.TryGetValue(variable8.reference.Address, out var tmp8)) value8 = (T8)tmp8;
                if (result.TryGetValue(variable9.reference.Address, out var tmp9)) value9 = (T9)tmp9;
                if (result.TryGetValue(variable10.reference.Address, out var tmp10)) value10 = (T10)tmp10;
                if (result.TryGetValue(variable11.reference.Address, out var tmp11)) value11 = (T11)tmp11;
                if (result.TryGetValue(variable12.reference.Address, out var tmp12)) value12 = (T12)tmp12;
                if (result.TryGetValue(variable13.reference.Address, out var tmp13)) value13 = (T13)tmp13;
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


            var result = (await papper.ReadAsync(variable1.reference,
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
                                                 variable14.reference))?.ToDictionary(x => x.Variable, x => x.Value);

            if (!result.IsNullOrEmpty())
            {
                if (result.TryGetValue(variable1.reference.Address, out var tmp1)) value1 = (T1)tmp1;
                if (result.TryGetValue(variable2.reference.Address, out var tmp2)) value2 = (T2)tmp2;
                if (result.TryGetValue(variable3.reference.Address, out var tmp3)) value3 = (T3)tmp3;
                if (result.TryGetValue(variable4.reference.Address, out var tmp4)) value4 = (T4)tmp4;
                if (result.TryGetValue(variable5.reference.Address, out var tmp5)) value5 = (T5)tmp5;
                if (result.TryGetValue(variable6.reference.Address, out var tmp6)) value6 = (T6)tmp6;
                if (result.TryGetValue(variable7.reference.Address, out var tmp7)) value7 = (T7)tmp7;
                if (result.TryGetValue(variable8.reference.Address, out var tmp8)) value8 = (T8)tmp8;
                if (result.TryGetValue(variable9.reference.Address, out var tmp9)) value9 = (T9)tmp9;
                if (result.TryGetValue(variable10.reference.Address, out var tmp10)) value10 = (T10)tmp10;
                if (result.TryGetValue(variable11.reference.Address, out var tmp11)) value11 = (T11)tmp11;
                if (result.TryGetValue(variable12.reference.Address, out var tmp12)) value12 = (T12)tmp12;
                if (result.TryGetValue(variable13.reference.Address, out var tmp13)) value13 = (T13)tmp13;
                if (result.TryGetValue(variable14.reference.Address, out var tmp14)) value14 = (T14)tmp14;
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

            var result = (await papper.ReadAsync(variable1.reference,
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
                                                 variable15.reference))?.ToDictionary(x => x.Variable, x => x.Value);

            if (!result.IsNullOrEmpty())
            {
                if (result.TryGetValue(variable1.reference.Address, out var tmp1)) value1 = (T1)tmp1;
                if (result.TryGetValue(variable2.reference.Address, out var tmp2)) value2 = (T2)tmp2;
                if (result.TryGetValue(variable3.reference.Address, out var tmp3)) value3 = (T3)tmp3;
                if (result.TryGetValue(variable4.reference.Address, out var tmp4)) value4 = (T4)tmp4;
                if (result.TryGetValue(variable5.reference.Address, out var tmp5)) value5 = (T5)tmp5;
                if (result.TryGetValue(variable6.reference.Address, out var tmp6)) value6 = (T6)tmp6;
                if (result.TryGetValue(variable7.reference.Address, out var tmp7)) value7 = (T7)tmp7;
                if (result.TryGetValue(variable8.reference.Address, out var tmp8)) value8 = (T8)tmp8;
                if (result.TryGetValue(variable9.reference.Address, out var tmp9)) value9 = (T9)tmp9;
                if (result.TryGetValue(variable10.reference.Address, out var tmp10)) value10 = (T10)tmp10;
                if (result.TryGetValue(variable11.reference.Address, out var tmp11)) value11 = (T11)tmp11;
                if (result.TryGetValue(variable12.reference.Address, out var tmp12)) value12 = (T12)tmp12;
                if (result.TryGetValue(variable13.reference.Address, out var tmp13)) value13 = (T13)tmp13;
                if (result.TryGetValue(variable14.reference.Address, out var tmp14)) value14 = (T14)tmp14;
                if (result.TryGetValue(variable15.reference.Address, out var tmp15)) value15 = (T15)tmp15;
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

            var result = (await papper.ReadAsync(variable1.reference,
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
                                                 variable16.reference))?.ToDictionary(x => x.Variable, x => x.Value);

            if (!result.IsNullOrEmpty())
            {
                if (result.TryGetValue(variable1.reference.Address, out var tmp1)) value1 = (T1)tmp1;
                if (result.TryGetValue(variable2.reference.Address, out var tmp2)) value2 = (T2)tmp2;
                if (result.TryGetValue(variable3.reference.Address, out var tmp3)) value3 = (T3)tmp3;
                if (result.TryGetValue(variable4.reference.Address, out var tmp4)) value4 = (T4)tmp4;
                if (result.TryGetValue(variable5.reference.Address, out var tmp5)) value5 = (T5)tmp5;
                if (result.TryGetValue(variable6.reference.Address, out var tmp6)) value6 = (T6)tmp6;
                if (result.TryGetValue(variable7.reference.Address, out var tmp7)) value7 = (T7)tmp7;
                if (result.TryGetValue(variable8.reference.Address, out var tmp8)) value8 = (T8)tmp8;
                if (result.TryGetValue(variable9.reference.Address, out var tmp9)) value9 = (T9)tmp9;
                if (result.TryGetValue(variable10.reference.Address, out var tmp10)) value10 = (T10)tmp10;
                if (result.TryGetValue(variable11.reference.Address, out var tmp11)) value11 = (T11)tmp11;
                if (result.TryGetValue(variable12.reference.Address, out var tmp12)) value12 = (T12)tmp12;
                if (result.TryGetValue(variable13.reference.Address, out var tmp13)) value13 = (T13)tmp13;
                if (result.TryGetValue(variable14.reference.Address, out var tmp14)) value14 = (T14)tmp14;
                if (result.TryGetValue(variable15.reference.Address, out var tmp15)) value15 = (T15)tmp15;
                if (result.TryGetValue(variable15.reference.Address, out var tmp16)) value16 = (T16)tmp16;
            }

            return (value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15, value16);
        }
    }
}
