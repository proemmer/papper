using System.Linq;
using System.Threading.Tasks;

namespace Papper.Extensions.ReadWrite
{
    public static class PlcServiceWriteAsyncExtensions
    {

        public static async Task<bool> WriteAsync<T1, T2>(
                        this PlcDataMapper papper,
                        
                       (string path, T1 value) variable1,
                       (string path, T2 value) variable2)
        {
            return (await papper.WriteAsync(
                PlcWriteReference.FromAddress(variable1.path, variable1.value ),
                PlcWriteReference.FromAddress(variable2.path, variable2.value))).All(x => x.ActionResult == ExecutionResult.Ok);
        }

        public static async Task<bool> WriteAsync<T1, T2, T3>(
                                this PlcDataMapper papper,
                                
                               (string path, T1 value) variable1,
                               (string path, T2 value) variable2,
                               (string path, T3 value) variable3)
        {
            return (await papper.WriteAsync(
                PlcWriteReference.FromAddress( variable1.path, variable1.value ),
                PlcWriteReference.FromAddress( variable2.path, variable2.value ),
                PlcWriteReference.FromAddress( variable3.path, variable3.value ))).All(x => x.ActionResult == ExecutionResult.Ok);
        }

        public static async Task<bool> WriteAsync<T1, T2, T3, T4>(
                                        this PlcDataMapper papper,
                                        
                                       (string path, T1 value) variable1,
                                       (string path, T2 value) variable2,
                                       (string path, T3 value) variable3,
                                       (string path, T4 value) variable4)
        {
            return (await papper.WriteAsync(
                PlcWriteReference.FromAddress( variable1.path, variable1.value ),
                PlcWriteReference.FromAddress( variable2.path, variable2.value ),
                PlcWriteReference.FromAddress( variable3.path, variable3.value ),
                PlcWriteReference.FromAddress( variable4.path, variable4.value ))).All(x => x.ActionResult == ExecutionResult.Ok);
        }


        public static async Task<bool> WriteAsync<T1, T2, T3, T4, T5>(
                this PlcDataMapper papper,
                
               (string path, T1 value) variable1,
               (string path, T2 value) variable2,
               (string path, T3 value) variable3,
               (string path, T4 value) variable4,
               (string path, T5 value) variable5)
        {
            return (await papper.WriteAsync(

                PlcWriteReference.FromAddress( variable1.path, variable1.value ),
                PlcWriteReference.FromAddress( variable2.path, variable2.value ),
                PlcWriteReference.FromAddress( variable3.path, variable3.value ),
                PlcWriteReference.FromAddress( variable4.path, variable4.value ),
                PlcWriteReference.FromAddress( variable5.path, variable5.value ))).All(x => x.ActionResult == ExecutionResult.Ok);
        }

        public static async Task<bool> WriteAsync<T1, T2, T3, T4, T5, T6>(
            this PlcDataMapper papper,
            
           (string path, T1 value) variable1,
           (string path, T2 value) variable2,
           (string path, T3 value) variable3,
           (string path, T4 value) variable4,
           (string path, T5 value) variable5,
           (string path, T6 value) variable6)
        {
            return (await papper.WriteAsync(

                PlcWriteReference.FromAddress( variable1.path, variable1.value ),
                PlcWriteReference.FromAddress( variable2.path, variable2.value ),
                PlcWriteReference.FromAddress( variable3.path, variable3.value ),
                PlcWriteReference.FromAddress( variable4.path, variable4.value ),
                PlcWriteReference.FromAddress( variable5.path, variable5.value ),
                PlcWriteReference.FromAddress( variable6.path, variable6.value ))).All(x => x.ActionResult == ExecutionResult.Ok);

        }

        public static async Task<bool> WriteAsync<T1, T2, T3, T4, T5, T6, T7>(
                    this PlcDataMapper papper,
                    
                   (string path, T1 value) variable1,
                   (string path, T2 value) variable2,
                   (string path, T3 value) variable3,
                   (string path, T4 value) variable4,
                   (string path, T5 value) variable5,
                   (string path, T6 value) variable6,
                   (string path, T7 value) variable7)
        {
            return (await papper.WriteAsync(

                PlcWriteReference.FromAddress( variable1.path, variable1.value ),
                PlcWriteReference.FromAddress( variable2.path, variable2.value ),
                PlcWriteReference.FromAddress( variable3.path, variable3.value ),
                PlcWriteReference.FromAddress( variable4.path, variable4.value ),
                PlcWriteReference.FromAddress( variable5.path, variable5.value ),
                PlcWriteReference.FromAddress( variable6.path, variable6.value ),
                PlcWriteReference.FromAddress( variable7.path, variable7.value ))).All(x => x.ActionResult == ExecutionResult.Ok);

        }

        public static async Task<bool> WriteAsync<T1, T2, T3, T4, T5, T6, T7, T8>(
                    this PlcDataMapper papper,
                    
                   (string path, T1 value) variable1,
                   (string path, T2 value) variable2,
                   (string path, T3 value) variable3,
                   (string path, T4 value) variable4,
                   (string path, T5 value) variable5,
                   (string path, T6 value) variable6,
                   (string path, T7 value) variable7,
                   (string path, T8 value) variable8)
        {
            return (await papper.WriteAsync(

                PlcWriteReference.FromAddress( variable1.path, variable1.value ),
                PlcWriteReference.FromAddress( variable2.path, variable2.value ),
                PlcWriteReference.FromAddress( variable3.path, variable3.value ),
                PlcWriteReference.FromAddress( variable4.path, variable4.value ),
                PlcWriteReference.FromAddress( variable5.path, variable5.value ),
                PlcWriteReference.FromAddress( variable6.path, variable6.value ),
                PlcWriteReference.FromAddress( variable7.path, variable7.value ),
                PlcWriteReference.FromAddress( variable8.path, variable8.value ))).All(x => x.ActionResult == ExecutionResult.Ok);

        }

        public static async Task<bool> WriteAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
                            this PlcDataMapper papper,
                            
                           (string path, T1 value) variable1,
                           (string path, T2 value) variable2,
                           (string path, T3 value) variable3,
                           (string path, T4 value) variable4,
                           (string path, T5 value) variable5,
                           (string path, T6 value) variable6,
                           (string path, T7 value) variable7,
                           (string path, T8 value) variable8,
                           (string path, T9 value) variable9)
        {
            return (await papper.WriteAsync(

                PlcWriteReference.FromAddress( variable1.path, variable1.value ),
                PlcWriteReference.FromAddress( variable2.path, variable2.value ),
                PlcWriteReference.FromAddress( variable3.path, variable3.value ),
                PlcWriteReference.FromAddress( variable4.path, variable4.value ),
                PlcWriteReference.FromAddress( variable5.path, variable5.value ),
                PlcWriteReference.FromAddress( variable6.path, variable6.value ),
                PlcWriteReference.FromAddress( variable7.path, variable7.value ),
                PlcWriteReference.FromAddress( variable8.path, variable8.value ),
                PlcWriteReference.FromAddress( variable9.path, variable9.value ))).All(x => x.ActionResult == ExecutionResult.Ok);

        }


        public static async Task<bool> WriteAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
                                    this PlcDataMapper papper,
                                    
                                   (string path, T1 value) variable1,
                                   (string path, T2 value) variable2,
                                   (string path, T3 value) variable3,
                                   (string path, T4 value) variable4,
                                   (string path, T5 value) variable5,
                                   (string path, T6 value) variable6,
                                   (string path, T7 value) variable7,
                                   (string path, T8 value) variable8,
                                   (string path, T9 value) variable9,
                                   (string path, T10 value) variable10)
        {
            return (await papper.WriteAsync(

                PlcWriteReference.FromAddress( variable1.path, variable1.value ),
                PlcWriteReference.FromAddress( variable2.path, variable2.value ),
                PlcWriteReference.FromAddress( variable3.path, variable3.value ),
                PlcWriteReference.FromAddress( variable4.path, variable4.value ),
                PlcWriteReference.FromAddress( variable5.path, variable5.value ),
                PlcWriteReference.FromAddress( variable6.path, variable6.value ),
                PlcWriteReference.FromAddress( variable7.path, variable7.value ),
                PlcWriteReference.FromAddress( variable8.path, variable8.value ),
                PlcWriteReference.FromAddress( variable9.path, variable9.value ),
                PlcWriteReference.FromAddress( variable10.path, variable10.value ))).All(x => x.ActionResult == ExecutionResult.Ok);

        }

        public static async Task<bool> WriteAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
                                            this PlcDataMapper papper,
                                            
                                           (string path, T1 value) variable1,
                                           (string path, T2 value) variable2,
                                           (string path, T3 value) variable3,
                                           (string path, T4 value) variable4,
                                           (string path, T5 value) variable5,
                                           (string path, T6 value) variable6,
                                           (string path, T7 value) variable7,
                                           (string path, T8 value) variable8,
                                           (string path, T9 value) variable9,
                                           (string path, T10 value) variable10,
                                           (string path, T11 value) variable11)
        {
            return (await papper.WriteAsync(

                PlcWriteReference.FromAddress( variable1.path, variable1.value ),
                PlcWriteReference.FromAddress( variable2.path, variable2.value ),
                PlcWriteReference.FromAddress( variable3.path, variable3.value ),
                PlcWriteReference.FromAddress( variable4.path, variable4.value ),
                PlcWriteReference.FromAddress( variable5.path, variable5.value ),
                PlcWriteReference.FromAddress( variable6.path, variable6.value ),
                PlcWriteReference.FromAddress( variable7.path, variable7.value ),
                PlcWriteReference.FromAddress( variable8.path, variable8.value ),
                PlcWriteReference.FromAddress( variable9.path, variable9.value ),
                PlcWriteReference.FromAddress( variable10.path, variable10.value ),
                PlcWriteReference.FromAddress( variable11.path, variable11.value ))).All(x => x.ActionResult == ExecutionResult.Ok);

        }

        public static async Task<bool> WriteAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
                                                    this PlcDataMapper papper,
                                                    
                                                   (string path, T1 value) variable1,
                                                   (string path, T2 value) variable2,
                                                   (string path, T3 value) variable3,
                                                   (string path, T4 value) variable4,
                                                   (string path, T5 value) variable5,
                                                   (string path, T6 value) variable6,
                                                   (string path, T7 value) variable7,
                                                   (string path, T8 value) variable8,
                                                   (string path, T9 value) variable9,
                                                   (string path, T10 value) variable10,
                                                   (string path, T11 value) variable11,
                                                   (string path, T12 value) variable12)
        {
            return (await papper.WriteAsync(

                PlcWriteReference.FromAddress( variable1.path, variable1.value ),
                PlcWriteReference.FromAddress( variable2.path, variable2.value ),
                PlcWriteReference.FromAddress( variable3.path, variable3.value ),
                PlcWriteReference.FromAddress( variable4.path, variable4.value ),
                PlcWriteReference.FromAddress( variable5.path, variable5.value ),
                PlcWriteReference.FromAddress( variable6.path, variable6.value ),
                PlcWriteReference.FromAddress( variable7.path, variable7.value ),
                PlcWriteReference.FromAddress( variable8.path, variable8.value ),
                PlcWriteReference.FromAddress( variable9.path, variable9.value ),
                PlcWriteReference.FromAddress( variable10.path, variable10.value ),
                PlcWriteReference.FromAddress( variable11.path, variable11.value ),
                PlcWriteReference.FromAddress( variable12.path, variable12.value ))).All(x => x.ActionResult == ExecutionResult.Ok);

        }

        public static async Task<bool> WriteAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
                                                            this PlcDataMapper papper,
                                                            
                                                           (string path, T1 value) variable1,
                                                           (string path, T2 value) variable2,
                                                           (string path, T3 value) variable3,
                                                           (string path, T4 value) variable4,
                                                           (string path, T5 value) variable5,
                                                           (string path, T6 value) variable6,
                                                           (string path, T7 value) variable7,
                                                           (string path, T8 value) variable8,
                                                           (string path, T9 value) variable9,
                                                           (string path, T10 value) variable10,
                                                           (string path, T11 value) variable11,
                                                           (string path, T12 value) variable12,
                                                           (string path, T13 value) variable13)
        {
            return (await papper.WriteAsync(

                PlcWriteReference.FromAddress( variable1.path, variable1.value ),
                PlcWriteReference.FromAddress( variable2.path, variable2.value ),
                PlcWriteReference.FromAddress( variable3.path, variable3.value ),
                PlcWriteReference.FromAddress( variable4.path, variable4.value ),
                PlcWriteReference.FromAddress( variable5.path, variable5.value ),
                PlcWriteReference.FromAddress( variable6.path, variable6.value ),
                PlcWriteReference.FromAddress( variable7.path, variable7.value ),
                PlcWriteReference.FromAddress( variable8.path, variable8.value ),
                PlcWriteReference.FromAddress( variable9.path, variable9.value ),
                PlcWriteReference.FromAddress( variable10.path, variable10.value ),
                PlcWriteReference.FromAddress( variable11.path, variable11.value ),
                PlcWriteReference.FromAddress( variable12.path, variable12.value ),
                PlcWriteReference.FromAddress( variable13.path, variable13.value ))).All(x => x.ActionResult == ExecutionResult.Ok);

        }

        public static async Task<bool> WriteAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
                                                                    this PlcDataMapper papper,
                                                                    
                                                                   (string path, T1 value) variable1,
                                                                   (string path, T2 value) variable2,
                                                                   (string path, T3 value) variable3,
                                                                   (string path, T4 value) variable4,
                                                                   (string path, T5 value) variable5,
                                                                   (string path, T6 value) variable6,
                                                                   (string path, T7 value) variable7,
                                                                   (string path, T8 value) variable8,
                                                                   (string path, T9 value) variable9,
                                                                   (string path, T10 value) variable10,
                                                                   (string path, T11 value) variable11,
                                                                   (string path, T12 value) variable12,
                                                                   (string path, T13 value) variable13,
                                                                   (string path, T14 value) variable14)
        {
            return (await papper.WriteAsync(
                PlcWriteReference.FromAddress( variable1.path, variable1.value ),
                PlcWriteReference.FromAddress( variable2.path, variable2.value ),
                PlcWriteReference.FromAddress( variable3.path, variable3.value ),
                PlcWriteReference.FromAddress( variable4.path, variable4.value ),
                PlcWriteReference.FromAddress( variable5.path, variable5.value ),
                PlcWriteReference.FromAddress( variable6.path, variable6.value ),
                PlcWriteReference.FromAddress( variable7.path, variable7.value ),
                PlcWriteReference.FromAddress( variable8.path, variable8.value ),
                PlcWriteReference.FromAddress( variable9.path, variable9.value ),
                PlcWriteReference.FromAddress( variable10.path, variable10.value ),
                PlcWriteReference.FromAddress( variable11.path, variable11.value ),
                PlcWriteReference.FromAddress( variable12.path, variable12.value ),
                PlcWriteReference.FromAddress( variable13.path, variable13.value ),
                PlcWriteReference.FromAddress( variable14.path, variable14.value ))).All(x => x.ActionResult == ExecutionResult.Ok);

        }

        public static async Task<bool> WriteAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
                                                                            this PlcDataMapper papper,
                                                                            
                                                                           (string path, T1 value) variable1,
                                                                           (string path, T2 value) variable2,
                                                                           (string path, T3 value) variable3,
                                                                           (string path, T4 value) variable4,
                                                                           (string path, T5 value) variable5,
                                                                           (string path, T6 value) variable6,
                                                                           (string path, T7 value) variable7,
                                                                           (string path, T8 value) variable8,
                                                                           (string path, T9 value) variable9,
                                                                           (string path, T10 value) variable10,
                                                                           (string path, T11 value) variable11,
                                                                           (string path, T12 value) variable12,
                                                                           (string path, T13 value) variable13,
                                                                           (string path, T14 value) variable14,
                                                                           (string path, T15 value) variable15)
        {
            return (await papper.WriteAsync(

                PlcWriteReference.FromAddress( variable1.path, variable1.value ),
                PlcWriteReference.FromAddress( variable2.path, variable2.value ),
                PlcWriteReference.FromAddress( variable3.path, variable3.value ),
                PlcWriteReference.FromAddress( variable4.path, variable4.value ),
                PlcWriteReference.FromAddress( variable5.path, variable5.value ),
                PlcWriteReference.FromAddress( variable6.path, variable6.value ),
                PlcWriteReference.FromAddress( variable7.path, variable7.value ),
                PlcWriteReference.FromAddress( variable8.path, variable8.value ),
                PlcWriteReference.FromAddress( variable9.path, variable9.value ),
                PlcWriteReference.FromAddress( variable10.path, variable10.value ),
                PlcWriteReference.FromAddress( variable11.path, variable11.value ),
                PlcWriteReference.FromAddress( variable12.path, variable12.value ),
                PlcWriteReference.FromAddress( variable13.path, variable13.value ),
                PlcWriteReference.FromAddress( variable14.path, variable14.value ),
                PlcWriteReference.FromAddress( variable15.path, variable15.value ))).All(x => x.ActionResult == ExecutionResult.Ok);

        }

        public  static async Task<bool> WriteAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
                                                                            this PlcDataMapper papper, 
                                                                            
                                                                           (string path, T1 value) variable1,
                                                                           (string path, T2 value) variable2,
                                                                           (string path, T3 value) variable3,
                                                                           (string path, T4 value) variable4,
                                                                           (string path, T5 value) variable5,
                                                                           (string path, T6 value) variable6,
                                                                           (string path, T7 value) variable7,
                                                                           (string path, T8 value) variable8,
                                                                           (string path, T9 value) variable9,
                                                                           (string path, T10 value) variable10,
                                                                           (string path, T11 value) variable11,
                                                                           (string path, T12 value) variable12,
                                                                           (string path, T13 value) variable13,
                                                                           (string path, T14 value) variable14,
                                                                           (string path, T15 value) variable15,
                                                                           (string path, T16 value) variable16)
        {            
            return (await papper.WriteAsync(

                PlcWriteReference.FromAddress( variable1.path, variable1.value ),
                PlcWriteReference.FromAddress( variable2.path, variable2.value ),
                PlcWriteReference.FromAddress( variable3.path, variable3.value ),
                PlcWriteReference.FromAddress( variable4.path, variable4.value ),
                PlcWriteReference.FromAddress( variable5.path, variable5.value ),
                PlcWriteReference.FromAddress( variable6.path, variable6.value ),
                PlcWriteReference.FromAddress( variable7.path, variable7.value ),
                PlcWriteReference.FromAddress( variable8.path, variable8.value ),
                PlcWriteReference.FromAddress( variable9.path, variable9.value ),
                PlcWriteReference.FromAddress( variable10.path, variable10.value ),
                PlcWriteReference.FromAddress( variable11.path, variable11.value ),
                PlcWriteReference.FromAddress( variable12.path, variable12.value ),
                PlcWriteReference.FromAddress( variable13.path, variable13.value ),
                PlcWriteReference.FromAddress( variable14.path, variable14.value ),
                PlcWriteReference.FromAddress( variable15.path, variable15.value ),
                PlcWriteReference.FromAddress( variable16.path, variable16.value ))).All(x => x.ActionResult == ExecutionResult.Ok);

        }
    }
}
