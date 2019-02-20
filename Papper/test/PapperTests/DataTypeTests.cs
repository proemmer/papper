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
            var type = new PlcSInt("TEST");
            var data = new byte[type.Size.Bytes];
            var binding = new PlcObjectBinding(new PlcRawData(512), type, 0, 0);

            type.ConvertToRaw(value, binding, data);
            var result1 = (sbyte)type.ConvertFromRaw(binding, data);
            Assert.Equal(value, result1);

            value = -44;
            type.ConvertToRaw(value, binding, data);
            var result2 = (sbyte)type.ConvertFromRaw(binding, data);
            Assert.Equal(value, result2);
        }


        [Fact]
        public void TestUSInt()
        {
            byte value = 44;
            
            var type = new PlcUSInt("TEST");
            var data = new byte[type.Size.Bytes];
            var binding = new PlcObjectBinding(new PlcRawData(512), type, 0, 0);

            type.ConvertToRaw(value, binding, data);
            var result1 = (byte)type.ConvertFromRaw(binding, data);
            Assert.Equal(value, result1);
        }



        [Fact]
        public void TestLInt()
        {
            long value = 44;

            var type = new PlcLInt("TEST");
            var data = new byte[type.Size.Bytes];
            var binding = new PlcObjectBinding(new PlcRawData(512), type, 0, 0);

            type.ConvertToRaw(value, binding, data);
            var result1 = (long)type.ConvertFromRaw(binding, data);
            Assert.Equal(value, result1);

            value = -44;
            type.ConvertToRaw(value, binding, data);
            var result2 = (long)type.ConvertFromRaw(binding, data);
            Assert.Equal(value, result2);
        }

        [Fact]
        public void TestLWord()
        {
            ulong value = 44;

            var type = new PlcLWord("TEST");
            var data = new byte[type.Size.Bytes];
            var binding = new PlcObjectBinding(new PlcRawData(512), type, 0, 0);

            type.ConvertToRaw(value, binding, data);
            var result1 = (ulong)type.ConvertFromRaw(binding, data);
            Assert.Equal(value, result1);

        }
    }
}
