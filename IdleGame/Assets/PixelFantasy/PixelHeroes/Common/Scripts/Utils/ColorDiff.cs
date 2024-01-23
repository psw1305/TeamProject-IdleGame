using UnityEngine;

namespace Assets.PixelFantasy.PixelHeroes.Common.Scripts.Utils
{
    public static class ColorDiff
    {
        // https://en.wikipedia.org/wiki/Color_difference
        public static float GetEuclidean(Color32 a, Color32 b, bool sqrt = false)
        {
            var dr = a.r - b.r;
            var dg = a.g - b.g;
            var db = a.b - b.b;
            var redmean = (a.r + b.r) / 2f;
            var difference2 = redmean < 128f
                ? 2f * dr * dr + 4f * dg * dg + 3f * db * db
                : 3f * dr * dr + 4f * dg * dg + 2f * db * db;

            return sqrt ? Mathf.Sqrt(difference2) : difference2;
        }
    }
}