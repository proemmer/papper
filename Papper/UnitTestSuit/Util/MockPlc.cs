﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTestSuit.Util
{
    public static class MockPlc
    {
        public class PlcBlock
        {
            public byte[] Data { get; private set; }
            public int MinSize { get { return Data.Length; } }

            public PlcBlock(int minSize)
            {
                Data = new byte[minSize];
            }

            public void UpdateBlockSize(int size)
            {
                if (Data.Length < size)
                {
                    var tmp = new byte[size];
                    Array.Copy(Data, 0, tmp, 0, Data.Length);
                    Data = tmp;
                }
            }
        }

        private static Dictionary<string, PlcBlock> _plc = new Dictionary<string, PlcBlock>();


        public static void Clear()
        {
            _plc.Clear();
        }

        public static PlcBlock GetPlcEntry(string selector, int minSize)
        {
            PlcBlock plcblock;
            if (!_plc.TryGetValue(selector, out plcblock))
            {
                plcblock = new PlcBlock(minSize);
                _plc.Add(selector, plcblock);
                return plcblock;
            }
            plcblock.UpdateBlockSize(minSize);
            return plcblock;
        }
    }
}
