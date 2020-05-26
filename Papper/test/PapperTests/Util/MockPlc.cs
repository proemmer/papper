using Papper;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Papper.Tests.Util
{
    public class PlcItem
    {
        public Memory<byte> Data { get; set; }
        public string Selector { get; set; }

        public int Offset { get; set; }
        public int Length { get; set; }

        public byte BitMaskBegin { get; set; }
        public byte BitMaskEnd { get; set; }

        public override string ToString() => $"{Selector}.{Offset}.{Length}#{BitMaskBegin}#{BitMaskEnd}";
    }

    public class PlcBlock
    {
        public Memory<byte> Data { get; private set; }
        public int MinSize => Data.Length;

        public PlcBlock(int minSize) => Data = new byte[minSize];

        public void UpdateBlockSize(int size)
        {
            if (Data.Length < size)
            {
                Memory<byte> tmp = new byte[size];
                Data.CopyTo(tmp);
                Data = tmp;
            }
        }
    }


    public sealed class MockPlc
    {


        private readonly Dictionary<string, PlcItem> _items = new Dictionary<string, PlcItem>();
        private Task _watchTask;
        private bool _stop;

        public Action<IEnumerable<PlcItem>> OnItemChanged { get; set; }


        private readonly Dictionary<string, PlcBlock> _plc = new Dictionary<string, PlcBlock>();
        private static readonly Lazy<MockPlc> _instance  = new Lazy<MockPlc>(() => new MockPlc());
        public static MockPlc Instance { get { return _instance.Value; } }

        public MockPlc()
        {

        }


        public void Clear() => _plc.Clear();

        public PlcBlock GetPlcEntry(string selector, int minSize = -1)
        {
            if (!_plc.TryGetValue(selector, out var plcblock))
            {
                lock (_plc)
                {
                    if (!_plc.TryGetValue(selector, out plcblock))
                    {
                        plcblock = new PlcBlock(minSize > 0 ? minSize : 0);
                        _plc.Add(selector, plcblock);
                        return plcblock;
                    }
                }
            }
            if (minSize > 0)
            {
                plcblock.UpdateBlockSize(minSize);
            }

            return plcblock;
        }

        public void UpdateDataChangeItem(DataPack item, bool remove = false)
        {
            if (item == null)
            {
                return;
            }

            var itemKey = item.ToString();
            if (!remove)
            {
                if (!_items.TryGetValue(itemKey, out _))
                {
                    lock (_plc)
                    {
                        if (!_items.TryGetValue(itemKey, out _))
                        {
                            _items.Add(itemKey, new PlcItem { Selector = item.Selector, Offset = item.Offset, Length = item.Length, BitMaskBegin = item.BitMaskBegin, BitMaskEnd = item.BitMaskEnd });
                        }
                    }
                }

                if (_watchTask == null)
                {
                    _watchTask = Task.Run(() => Watch());
                }
            }
            else
            {
                if (_items.TryGetValue(itemKey, out var res))
                {
                    res.Data = null;
                }
                _items.Remove(itemKey);

                if (_watchTask != null && !_items.Any())
                {
                    _stop = true;
                    _watchTask.GetAwaiter().GetResult();
                    _watchTask = null;
                }
            }
        }

        private void Watch()
        {
            var changed = new List<PlcItem>();
            while (!_stop)
            {
                try
                {
                    foreach (var item in _items.ToList())
                    {
                        var res = GetPlcEntry(item.Value.Selector, item.Value.Offset + item.Value.Length).Data.Slice(item.Value.Offset, item.Value.Length);
                        if (item.Value.Data.IsEmpty || !res.Span.SequenceEqual(item.Value.Data.Span.Slice(0, item.Value.Length)))
                        {
                            if (item.Value.Data.IsEmpty || item.Value.Data.Length < res.Length)
                            {
                                if (!item.Value.Data.IsEmpty)
                                {
                                    ArrayPool<byte>.Shared.Return(item.Value.Data.ToArray());
                                }
                                item.Value.Data = ArrayPool<byte>.Shared.Rent(res.Length);
                            }
                            res.CopyTo(item.Value.Data);
                            changed.Add(item.Value);
                        }
                    }

                    if (changed.Any())
                    {
                        OnItemChanged?.Invoke(changed.ToList());
                        changed.Clear();
                    }
                }
                catch (Exception)
                {

                }
                finally
                {
                    foreach (var item in _items.ToList())
                    {
                        if (!item.Value.Data.IsEmpty)
                        {
                            ArrayPool<byte>.Shared.Return(item.Value.Data.ToArray());
                        }
                    }
                }

                Thread.Sleep(500);
            }
            _stop = false;
        }
    }
}
