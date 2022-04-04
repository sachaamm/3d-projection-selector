using System.Collections.Generic;

namespace _3dProjectionSelection.Scripts.Math
{
    public static class GeoUtility
    {
        public static bool ContainsList(List<List<int>> a, List<int> b)
        {
            return a.Exists(l => l == b);
        }
    }
}