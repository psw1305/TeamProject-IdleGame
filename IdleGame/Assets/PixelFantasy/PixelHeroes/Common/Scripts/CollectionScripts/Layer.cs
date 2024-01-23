using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Assets.PixelFantasy.PixelHeroes.Common.Scripts.Utils;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.PixelFantasy.PixelHeroes.Common.Scripts.CollectionScripts
{
    [Serializable]
    public class Layer
    {
        public string Name;
        public Object SpriteFolder;
        public List<Texture2D> Textures;

        private Color32[] _pixels;

        public Texture2D GetIcon(Texture2D texture)
        {
            var icon = new Texture2D(32, 32) { filterMode = FilterMode.Point };

            icon.SetPixels(texture.GetPixels(0, texture.height - 32, 32, 32));
            icon.Apply();

            return icon;
        }

        public Color32[] GetPixels(string data, Color32[] mask, string changed)
        {
            var match = Regex.Match(data, @"(?<Name>[\w\- \[\]]+)(?<Paint>#\w+)?(?:\/(?<H>[-\d]+):(?<S>[-\d]+):(?<V>[-\d]+))?");
            var name = match.Groups["Name"].Value;
            var index = Textures.FindIndex(i => i.name == name);
            var paint = Color.white;

            if (index == -1) return null;
            
            if (match.Groups["Paint"].Success)
            {
                ColorUtility.TryParseHtmlString(match.Groups["Paint"].Value, out paint);
            }

            float h = 0f, s = 0f, v = 0f;

            if (match.Groups["H"].Success && match.Groups["S"].Success && match.Groups["V"].Success)
            {
                h = float.Parse(match.Groups["H"].Value, CultureInfo.InvariantCulture);
                s = float.Parse(match.Groups["S"].Value, CultureInfo.InvariantCulture);
                v = float.Parse(match.Groups["V"].Value, CultureInfo.InvariantCulture);
            }

            var update = changed == null || changed == Name;

            switch (changed)
            {
                case "Body" when Name == "Head":
                case "Helmet" when Name == "Hair":
                    update = true;
                    break;
            }

            return GetPixels(index, paint, h, s, v, mask, update);
        }

        public Color32[] GetPixels(int index, Color paint, float h, float s, float v, Color32[] mask, bool update)
        {
            if (!update && _pixels != null && mask == null) return _pixels;

            _pixels = Textures[index].GetPixels32();

            if (mask != null)
            {
                for (var i = 0; i < _pixels.Length; i++)
                {
                    if (mask[i].a <= 0)
                    {
                        _pixels[i] = new Color32();
                    }
                    else if (mask[i] == Color.black)
                    {
                        _pixels[i] = mask[i];
                    }
                }
            }

            if (paint != Color.white)
            {
                if (Name == "Head" || Name == "Body" || Name == "Arms" || Name == "Hair")
                {
                    _pixels = TextureHelper.Repaint3C(_pixels, paint, SpriteCollection.Palette);
                }
                else
                {
                    for (var i = 0; i < _pixels.Length; i++)
                    {
                        if (_pixels[i].a > 0) _pixels[i] *= paint;
                    }
                }
            }

            if (Mathf.Approximately(h, 0) && Mathf.Approximately(s, 0) && Mathf.Approximately(v, 0)) return _pixels;

            for (var i = 0; i < _pixels.Length; i++)
            {
                if (_pixels[i].a > 0 && _pixels[i] != Color.black)
                {
                    _pixels[i] = TextureHelper.AdjustColor(_pixels[i], h, s, v);
                }
            }

            _pixels = TextureHelper.ApplyPalette(_pixels, SpriteCollection.Palette);

            return _pixels;
        }

        public Color[] GetPixels(int index, Color paint, float h, float s, float v, int x, int y, int blockWidth, int blockHeight)
        {
            var source = Textures[index];
            var pixels = source.GetPixels(x, y, blockWidth, blockHeight);

            if (paint != Color.white)
            {
                for (var i = 0; i < pixels.Length; i++)
                {
                    if (pixels[i].a > 0) pixels[i] *= paint;
                }
            }

            if (Mathf.Approximately(h, 0) && Mathf.Approximately(s, 0) && Mathf.Approximately(v, 0)) return pixels;

            for (var i = 0; i < pixels.Length; i++)
            {
                if (pixels[i].a <= 0) continue;

                pixels[i] = TextureHelper.AdjustColor(pixels[i] * paint, h, s, v);
            }

            return pixels;
        }

        #if UNITY_EDITOR

        public void Refresh(List<Color32> palette)
        {
            var root = UnityEditor.AssetDatabase.GetAssetPath(SpriteFolder);
            var files = Directory.GetFiles(root, "*.png", SearchOption.AllDirectories).ToList();

            Textures.Clear();

            foreach (var path in files)
            {
                var texture = UnityEditor.AssetDatabase.LoadAssetAtPath<Texture2D>(path);

                Textures.Add(texture);

                var colors = new ColorDistinctor(texture.GetPixels32()).UniqueColors;

                if (colors.Any(i => i.a > 0 && i.a < 255))
                {
                    Debug.LogError($"Transfluent pixels found in {path}");
                }

                var wrong = colors.Where(i => i.a == 255 && !palette.Any(j => i.FastEquals(j))).ToList();

                if (wrong.Any())
                {
                    Debug.LogError($"Colors outside of the palette found in {path}: {string.Join(",", wrong)}.");
                }
            }
        }

        #endif
    }
}