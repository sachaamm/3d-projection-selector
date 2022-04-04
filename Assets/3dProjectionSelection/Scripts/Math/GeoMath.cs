namespace _3dProjectionSelection.Scripts.Math
{
    public static class GeoMath
    {
        public static double Abs(double a)
        {
            return System.Math.Abs(a);
        }
        
        public static float Remap(float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }
    }
}