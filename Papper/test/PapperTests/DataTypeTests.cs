using Papper;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Papper.Types;
using Papper.Internal;

namespace PapperTests
{
    public class DataTypeTests
    {
        [Fact]
        public void TestSInt()
        {
            sbyte value = 44;
            var data = new byte[1];
            var type = new PlcSInt("TEST");
            var binding = new PlcObjectBinding(new PlcRawData(512), type, 0, 0);

            type.ConvertToRaw(value, binding, data);
            var result1 = (sbyte)type.ConvertFromRaw(binding, data);
            Assert.Equal(value, result1);

            value = -44;
            type.ConvertToRaw(value, binding, data);
            var result2 = (sbyte)type.ConvertFromRaw(binding, data);
            Assert.Equal(value, result2);
        }
    }
}
