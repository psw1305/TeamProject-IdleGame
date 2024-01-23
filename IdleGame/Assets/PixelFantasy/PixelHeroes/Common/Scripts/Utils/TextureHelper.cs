using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Assets.PixelFantasy.PixelHeroes.Common.Scripts.Utils
{
    /// <summary>
    /// Used to prepare textures for saving.
    /// </summary>
    public static class TextureHelper
    {
        public static Texture2D MergeLayers(Texture2D texture, params Color32[][] layers)
        {
            if (layers.Length == 0) throw new Exception("No layers to merge.");
            
            var result = new Color[texture.width * texture.height];

            foreach (var layer in layers.Where(i => i != null))
            {
                for (var i = 0; i < result.Length; i++)
                {
                    if (layer[i].a > 0) result[i] = layer[i];
                }
            }

            texture.SetPixels(result);
            texture.Apply();

            return texture;
        }

        public static Rect GetContentRect(Texture2D texture)
        {
            var pixels = texture.GetPixels();
            var width = texture.width;
            var height = texture.height;
            var minX = width - 1;
            var minY = height - 1;
            var maxX = 0;
            var maxY = 0;

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    if (pixels[x + y * width].a > 0)
                    {
                        minX = Mathf.Min(x, minX);
                        minY = Mathf.Min(y, minY);
                    }
                }
            }

            for (var x = width - 1; x >= 0; x--)
            {
                for (var y = height - 1; y >= 0; y--)
                {
                    if (pixels[x + y * width].a > 0)
                    {
                        maxX = Mathf.Max(x, maxX);
                        maxY = Mathf.Max(y, maxY);
                    }
                }
            }

            return new Rect(minX, minY, maxX - minX + 1, maxY - minY + 1);
        }

        public static Texture2D Center(Texture2D texture)
        {
            var rect = GetContentRect(texture);
            var pixels = texture.GetPixels((int) rect.min.x, (int) rect.min.y, (int) rect.width, (int) rect.height);
            var offsetX = (texture.width - rect.width) / 2;
            var offsetY = (texture.height - rect.height) / 2;

            texture.SetPixels(new Color[texture.width * texture.height]);
            texture.SetPixels((int) offsetX, (int) offsetY, (int) rect.width, (int) rect.height, pixels);
            texture.Apply();

            return texture;
        }

        public static Color AdjustColor(Color color, float hue, float saturation, float value)
        {
            hue /= 180f;
            saturation /= 100f;
            value /= 100f;

            var a = color.a;

            Color.RGBToHSV(color, out var h, out var s, out var v);

            h += hue / 2f;

            if (h > 1) h -= 1;
            else if (h < 0) h += 1;

            color = Color.HSVToRGB(h, s, v);

            var grey = 0.3f * color.r + 0.59f * color.g + 0.11f * color.b;

            color.r = grey + (color.r - grey) * (saturation + 1);
            color.g = grey + (color.g - grey) * (saturation + 1);
            color.b = grey + (color.b - grey) * (saturation + 1);

            if (color.r < 0) color.r = 0;
            if (color.g < 0) color.g = 0;
            if (color.b < 0) color.b = 0;

            color.r += value * color.r;
            color.g += value * color.g;
            color.b += value * color.b;
            color.a = a;

            return color;
        }

        public static void ApplyPalette(Texture2D texture, string path)
        {
            var bytes = File.ReadAllBytes(path);
            var t = new Texture2D(2, 2);

            t.LoadImage(bytes);

            var palette = t.GetPixels32().Distinct().ToList();

            if (!palette.Contains(Color.black))
            {
                palette.Add(Color.black);
            }

            texture.ApplyPalette(palette);
        }

        public static void ApplyPalette(this Texture2D texture, List<Color32> palette)
        {
            var pixels = ApplyPalette(texture.GetPixels32(), palette);

            texture.SetPixels32(pixels);
            texture.Apply();
        }

        public static Color32[] ApplyPalette(Color32[] pixels, List<Color32> palette)
        {
            var unique = new ColorDistinctor(pixels).UniqueColors.OrderByDescending(i => pixels.Count(j => FastEquals(i, j))).ToList();
            var map = new Dictionary<Color32, Color32>();
            var mapInvert = new Dictionary<Color32, Color32>();

            foreach (var color in unique)
            {
                if (color.a == 0) continue;

                var nearest = FindNearest(color, palette);
                var used = mapInvert.ContainsKey(nearest);

                if (used)
                {
                    var match = mapInvert[nearest];
                    var dist = ColorDiff.GetEuclidean(color, match);

                    if (dist > 2500)
                    {
                        Debug.Log($"Bad mapping (glued pixels) found: {color} / {match} dist={dist}.");

                        var alternative = FindNearest(color, palette, palette.IndexOf(nearest));

                        used = mapInvert.ContainsKey(alternative);

                        if (used)
                        {
                            Debug.LogWarning("Bad mapping not fixed.");
                        }
                        else
                        {
                            nearest = alternative;
                            Debug.Log("Bad mapping fixed.");
                        }
                    }
                }
                else
                {
                    mapInvert.Add(nearest, color);
                }

                map.Add(color, nearest);
            }

            for (var i = 0; i < pixels.Length; i++)
            {
                if (pixels[i].a < 255)
                {
                    pixels[i] = new Color32();
                }
                else
                {
                    var color = map[pixels[i]];

                    color.a = pixels[i].a;
                    pixels[i] = color;
                }
            }

            return pixels;
        }

        private static Color32 FindNearest(Color32 color, List<Color32> palette, int ignore = -1)
        {
            var nearest = palette[ignore == 0 ? 1 : 0];
            var difference = ColorDiff.GetEuclidean(color, nearest);

            for (var j = 1; j < palette.Count; j++)
            {
                if (j == ignore) continue;

                var d = ColorDiff.GetEuclidean(color, palette[j]);

                if (d >= difference) continue;

                difference = d;
                nearest = palette[j];
            }

            return nearest;
        }

        public static bool FastEquals(this Color32 a, Color32 b)
        {
            return a.r == b.r && a.g == b.g && a.b == b.b && a.a == b.a;
        }

        public static Color32[] Repaint3C(Color32[] pixels, Color32 paint, List<Color32> palette)
        {
            var dict = new Dictionary<Color32, int>();

            for (var x = 0; x < 64; x++) // TODO: Hardcoded values.
            {
                for (var y = 0; y < 64; y++)
                {
                    var c = pixels[x + y * 256];

                    if (c.a > 0 && c != Color.white && c != Color.black)
                    {
                        if (dict.ContainsKey(c))
                        {
                            dict[c]++;
                        }
                        else
                        {
                            dict.Add(c, 1);
                        }
                    }
                }
            }

            var colors = dict.Count > 3 ? dict.OrderByDescending(i => i.Value).Take(3).Select(i => i.Key).ToList() : dict.Keys.ToList();

            float GetBrightness(Color32 color)
            {
                Color.RGBToHSV(color, out _, out _, out var result);

                return result;
            }

            colors = colors.OrderBy(GetBrightness).ToList();

            if (colors.Count != 2 && colors.Count != 3)
            {
                throw new NotSupportedException("Sprite should have 2 or 3 colors only (+black outline).");
            }

            var replacement = palette.GetRange(palette.IndexOf(paint) - 1, 3).OrderBy(i => ((Color) i).grayscale).ToList();
            var match = new Dictionary<Color32, Color32>
            {
                { colors[0], replacement[0] },
                { colors[1], replacement[1] }
            };

            if (colors.Count == 3)
            {
                match.Add(colors[2], replacement[2]);
            }

            for (var i = 0; i < pixels.Length; i++)
            {
                if (pixels[i].a > 0 && pixels[i] != Color.black && match.ContainsKey(pixels[i]))
                {
                    pixels[i] = match[pixels[i]];
                }
            }

            return pixels;
        }
    }
}