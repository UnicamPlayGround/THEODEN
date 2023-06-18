using System;

namespace Models.Version
{
    [Serializable]
    public class Version: ICloneable
    {
        public string name;
        public int version;

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj.GetType() != typeof(Version))
                return false;
            var v = (Version)obj;
            return v.name == name && v.version == version;
        }

        protected bool Equals(Version other)
        {
            return name == other.name && version == other.version;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((name != null ? name.GetHashCode() : 0) * 397) ^ version;
            }
        }

        public object Clone()
        {
            return new Version()
            {
                name = name,
                version = version
            };
        }
    }
}