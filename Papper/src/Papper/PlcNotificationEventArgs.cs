using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace Papper
{
    public class PlcNotificationEventArgs : EventArgs, IEnumerable<KeyValuePair<string, object>>
    {

        private readonly string _from;
        private readonly Dictionary<string, object> _changedItems;
        public PlcNotificationEventArgs(string from, Dictionary<string, object> changedItems)
        {
            _from = from;
            _changedItems = changedItems;
        }

        /// <summary>
        /// Mapping name
        /// </summary>
        /// <returns></returns>
        public string From { get { return _from; } }

        /// <summary>
        /// Indexed Value access
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object this[string name]
        {
            get { return _changedItems[name]; }
        }

        /// <summary>
        /// Convert the Changed variables to an object, so every changed variable is an Property
        /// </summary>
        /// <param name="resolveDoubleUsedValues">
        /// This value changes the ExpandoObject generation:
        /// false:
        ///    an array    a[5]  generates the following ExpandoObject, so you 
        ///                      can access it by index and fully, but the data are doubled
        ///    {
        ///     a : [],
        ///     a[0] : {},
        ///     a[1] : {},
        ///     a[2] : {},
        ///     a[3] : {},
        ///     a[4] : {}
        ///    } 
        /// 
        /// true:                no double data but also not easy to use in C# 
        ///    {
        ///     a : 
        ///       [
        ///         "0" : {},
        ///         "1" : {},
        ///         "2" : {},
        ///         "3" : {},
        ///         "4" : {},
        ///       ],
        ///    }</param>
        /// <returns></returns>
        public dynamic ToObject(bool resolveDoubleUsedValues = true)
        {
            var item = new ExpandoObject();
            foreach (var items in _changedItems)
            {
                var levels = items.Key.Split('.');
                var levelCount = levels.Length;
                var parent = item;
                foreach (var level in levels)
                {
                    var name = level;
                    levelCount--;
                    if (levelCount == 0 && (!resolveDoubleUsedValues || !name.Contains("[")))
                    {
                        AddProperty(parent, name, items.Value);
                    }
                    else if (levelCount == 0 && resolveDoubleUsedValues)
                    {
                        var withoutIndex = name.Substring(0, name.IndexOf("[", StringComparison.Ordinal));

                        var obj = GetPropertyValue(item, withoutIndex);
                        if (obj is Array)
                        {
                            obj = new ExpandoObject();
                            AddProperty(parent, withoutIndex, obj);
                        }
                        parent = obj;

                        var index = name.Substring(withoutIndex.Length).Trim(new char[] { '[', ']' });
                        obj = GetPropertyValue(parent, index);
                        if (obj == null)
                        {
                            obj = new ExpandoObject();
                            AddProperty(parent, index, obj);
                        }
                        parent = obj;

                        AddProperty(parent, index, items.Value);
                    }
                    else if (name.Contains("["))
                    {
                        var withoutIndex = name.Substring(0, name.IndexOf("[", StringComparison.Ordinal));

                        var obj = GetPropertyValue(item, withoutIndex);
                        if (obj == null)
                        {
                            obj = new ExpandoObject();
                            AddProperty(parent, withoutIndex, obj);
                        }
                        parent = obj;

                        var index = name.Substring(withoutIndex.Length).Trim(new char[] { '[', ']' });
                        obj = GetPropertyValue(parent, index);
                        if (obj == null)
                        {
                            obj = new ExpandoObject();
                            AddProperty(parent, index, obj);
                        }
                        parent = obj;
                    }
                    else
                    {
                        var obj = GetPropertyValue(item, name);
                        if (obj == null)
                        {
                            obj = new ExpandoObject();
                            AddProperty(parent, name, obj);
                        }
                        parent = obj;
                    }
                }
            }
            return item;

        }

        /// <summary>
        /// Convert the Changed variables to an plcObject, so every changed variable is an Property (you could also use indexes in c#)
        /// </summary>
        /// <returns></returns>
        //public dynamic ToPlcObject()
        //{
        //    var item = new DynamicPlcObject();
        //    foreach (var items in _changedItems)
        //    {
        //        var val = items.Value;
        //        item.AddEntry(items.Value.GetType().IsArray ?
        //                string.Format("{0}[*]", items.Key) :
        //                items.Key,
        //            () => val,
        //            value => { });
        //    }
        //    return item;
        //}

        /// <summary>
        /// Number of changed fields
        /// </summary>
        /// <returns></returns>
        public int FieldCount
        {
            get { return _changedItems.Count; }
        }

        /// <summary>
        /// Enumerator of changed fields
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<string, object>>)_changedItems).GetEnumerator();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Add a property to the expando object
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        private static void AddProperty(dynamic parent, string name, object value)
        {
            var list = (parent as List<dynamic>);
            if (list != null)
            {
                list.Add(value);
            }
            else
            {
                var dictionary = parent as IDictionary<string, object>;
                if (dictionary != null)
                    dictionary[name] = value;
            }
        }

        /// <summary>
        /// Get a value from the expando object
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private static dynamic GetPropertyValue(dynamic parent, string name)
        {
            var dictionary = parent as IDictionary<string, object>;
            if (dictionary != null && dictionary.TryGetValue(name, out object ret))
                return ret;
            return null;
        }
    }

}
