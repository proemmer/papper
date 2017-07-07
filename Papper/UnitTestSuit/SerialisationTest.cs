using Papper;
using System;
using System.Collections.Generic;
using System.Text;
using UnitTestSuit.Mappings;
using Xunit;

namespace UnitTestSuit
{
    public class SerialisationTest
    {
        [Fact]
        void TestSerialisation()
        {
            var s = new PlcDataMapperSerializer();
            var tt = new StringArrayTestMapping
            {
                TEST = "Hallo",
                TEXT = new string[] { "HHHHHH" },
                Time = new TimeSpan[] { DateTime.Now.TimeOfDay }
            };

            var serialized = s.Serialize(tt);
            var deserialized = s.Deserialize<StringArrayTestMapping>(serialized);

            Assert.Equal(tt.TEST, deserialized.TEST);
            Assert.Equal(tt.TEXT[0], deserialized.TEXT[0]);

            // Precision is not the same after conveting to the plc format
            var cmpVal = Convert.ToUInt32(tt.Time[0].TotalMilliseconds);
            Assert.Equal(cmpVal, deserialized.Time[0].TotalMilliseconds);
        }
    }
}
