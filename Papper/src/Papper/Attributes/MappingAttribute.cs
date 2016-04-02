using System;

namespace Papper.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class MappingAttribute :  Attribute
    {
        public MappingAttribute()
        {
            Name = string.Empty;
            Selector = string.Empty;
            Offset = 0;
            ObservationRate = 0;
        }

        public MappingAttribute(string name, string selector, int offset = 0, int observationRate = 0)
        {
            Name = name;
            Selector = selector;
            Offset = offset;
            ObservationRate = observationRate;
        }

        public string Name
        {
            get;
            private set;
        }

        public string Selector
        {
            get;
            private set;
        }

        public int Offset
        {
            get;
            private set;
        }

        public int ObservationRate
        {
            get;
            private set;
        }

        // overload operator + 
        public static bool operator ==(MappingAttribute a, MappingAttribute b)
        {
            bool rc;

            if (ReferenceEquals(a, b))
            {
                rc = true;
            }
            else if (((object)a == null) || ((object)b == null))
            {
                rc = false;
            }
            else
            {
                rc = (string.Compare(a.Name, b.Name, StringComparison.Ordinal) == 0  &&
                      string.Compare(a.Selector, b.Selector, StringComparison.Ordinal) == 0 &&
                      a.Offset == b.Offset &&
                      a.ObservationRate == b.ObservationRate);
            }

            return rc;
        }



        public static bool operator !=(MappingAttribute p1, MappingAttribute p2)
        {
            return !(p1 == p2);
        }

        public override bool Equals(object obj)
        {
            var rc = false;
            if (obj is MappingAttribute)
            {
                var p2 = obj as MappingAttribute;
                rc = (this == p2);
            }
            return rc;
        }

        public override int GetHashCode()
        {
            return string.Format("{0}.{1}.{2}",Name,Selector,Offset).GetHashCode();
        }
    }
}
