using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace GltfFormat
{
    /// UnitTest 向けの微妙な Equal
    public static class EqualUtil
    {
        public static bool IsEqual(Dictionary<string, float[]> lhs, Dictionary<string, float[]> rhs)
        {
            if (lhs.Count != rhs.Count){
                var ll = lhs.OrderBy(x => x.Key).ToArray();
                var rr = rhs.OrderBy(x => x.Key).ToArray();
                return false;
            } 

            foreach(var (l, r) in Enumerable.Zip(lhs.OrderBy(x => x.Key), rhs.OrderBy(x => x.Key), (x, y) => (x, y)))
            {
                if(l.Key!=r.Key){
                    return false;
                }

                if(!IsEqual(l.Value, r.Value)){
                    return false;
                }
            }

            return true;
        }

        public static bool IsEqual<T>(Dictionary<string, T> lhs, Dictionary<string, T> rhs)
        {
            if (lhs.Count != rhs.Count){
                var ll = lhs.OrderBy(x => x.Key).ToArray();
                var rr = rhs.OrderBy(x => x.Key).ToArray();
                return false;
            } 

            foreach(var (l, r) in Enumerable.Zip(lhs.OrderBy(x => x.Key), rhs.OrderBy(x => x.Key), (x, y) => (x, y)))
            {
                if(l.Key!=r.Key){
                    return false;
                }

                if(!l.Value.Equals(r.Value)){
                    return false;
                }
            }

            return true;
        }

        public static bool IsEqual<T>(IEnumerable<T> lhs, IEnumerable<T> rhs)
        {
            if (lhs is null && rhs is null)
            {
                return true;
            }

            if (lhs is null || rhs is null)
            {
                return false;
            }

            return lhs.SequenceEqual(rhs);
        }

        public static float[] ToArray(this Vector4 v)
        {
            return new float[] { v.X, v.Y, v.Z, v.W };
        }
    }
}