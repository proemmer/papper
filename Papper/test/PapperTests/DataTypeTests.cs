using Papper;
using System.Collections.Generic;
using System;
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


        [Fact]
        public void TestLTIme()
        {
            var value = TimeSpan.FromHours(2);

            var type = new PlcLTime("TEST");
            var data = new byte[type.Size.Bytes];
            var binding = new PlcObjectBinding(new PlcRawData(512), type, 0, 0);

            type.ConvertToRaw(value, binding, data);
            var result1 = (TimeSpan)type.ConvertFromRaw(binding, data);
            Assert.Equal(value, result1);

        }

        [Fact]
        public void TestLDateTIme()
        {
            var value = DateTime.Now;

            var type = new PlcLDateTime("TEST");
            var data = new byte[type.Size.Bytes];
            var binding = new PlcObjectBinding(new PlcRawData(512), type, 0, 0);

            type.ConvertToRaw(value, binding, data);
            var result1 = (DateTime)type.ConvertFromRaw(binding, data);
            Assert.Equal(value, result1);

        }


        [Fact]
        public void TestUDINt()
        {
            uint value = 44;

            var type = new PlcUDInt("TEST");
            var data = new byte[type.Size.Bytes];
            var binding = new PlcObjectBinding(new PlcRawData(512), type, 0, 0);

            type.ConvertToRaw(value, binding, data);
            var result1 = (uint)type.ConvertFromRaw(binding, data);
            Assert.Equal(value, result1);

        }

        [Fact]
        public void TestDINt()
        {
            int value = 44;

            var type = new PlcDInt("TEST");
            var data = new byte[type.Size.Bytes];
            var binding = new PlcObjectBinding(new PlcRawData(512), type, 0, 0);

            type.ConvertToRaw(value, binding, data);
            var result1 = (int)type.ConvertFromRaw(binding, data);
            Assert.Equal(value, result1);

            value = -44;

            type.ConvertToRaw(value, binding, data);
            var result2 = (int)type.ConvertFromRaw(binding, data);
            Assert.Equal(value, result2);

        }

        [Fact]
        public void TestULInt()
        {
            ulong value = 44;

            var type = new PlcULInt("TEST");
            var data = new byte[type.Size.Bytes];
            var binding = new PlcObjectBinding(new PlcRawData(512), type, 0, 0);

            type.ConvertToRaw(value, binding, data);
            var result1 = (ulong)type.ConvertFromRaw(binding, data);
            Assert.Equal(value, result1);
        }


        [Fact]
        public void TestWChar()
        {
            string value = "↕";

            var type = new PlcWChar("TEST");
            var data = new byte[type.Size.Bytes];
            var binding = new PlcObjectBinding(new PlcRawData(512), type, 0, 0);

            type.ConvertToRaw(value, binding, data);
            var result1 = (string)type.ConvertFromRaw(binding, data);
            Assert.Equal(value, result1);
        }

        [Fact]
        public void TestWString()
        {
            string value = "↕TEst░iä";

            var type = new PlcWString("TEST");
            var data = new byte[type.Size.Bytes];
            var binding = new PlcObjectBinding(new PlcRawData(1024), type, 0, 0);

            type.ConvertToRaw(value, binding, data);
            var result1 = (string)type.ConvertFromRaw(binding, data);
            Assert.Equal(value, result1);
        }
    }
}
