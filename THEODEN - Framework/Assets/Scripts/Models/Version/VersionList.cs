using System;
using System.Collections.Generic;
using System.Linq;

namespace Models.Version
{
    [Serializable]
    public class VersionList: ICloneable
    {
        public List<Version> versionList;

        public VersionList()
        {
            versionList = new List<Version>();
        }
        
        public static VersionList operator -(VersionList v1, VersionList v2)
        {
            var result = new VersionList();
            
            foreach (var v in 
                     v1.versionList
                         .Where(v => !v2.versionList.Contains(v)))
            {
                result.versionList.Add(v);
            }
            return result;
        }
        public static VersionList operator &(VersionList v1, VersionList v2)
        {
            var result = new VersionList();

            foreach (var version2 in 
                     from v in v1.versionList 
                     let version2 = v2.versionList.Find(e => e.name == v.name) 
                     where version2 != null && version2.version != v.version select version2)
            {
                result.versionList.Add(version2);
            }

            return result;
        }

        public static VersionList operator +(VersionList v1, VersionList v2)
        {
            var output = (VersionList)v1.Clone();
            output.versionList.AddRange(v2.versionList);
            return output;
        }
        
        public object Clone()
        {
            var result = new VersionList();
            foreach (var v in versionList)
            {
                result.versionList.Add((Version)v.Clone());
            }
            return result;
        }
    }
}