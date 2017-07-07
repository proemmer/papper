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
            PlcDataMapper papper = new PlcDataMapper(960);
            var s = new PlcDataMapperSerializer(papper);
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
            Assert.Equal(tt.Time[0], deserialized.Time[0]);
        }
    }
}
