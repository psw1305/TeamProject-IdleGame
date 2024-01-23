using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assets.PixelFantasy.PixelHeroes.Common.Scripts.Utils;
using UnityEditor;
using UnityEngine;

namespace Assets.PixelFantasy.PixelHeroes.Common.Scripts.CollectionScripts
{
    /// <summary>
    /// Scriptable object that automatically grabs all required images.
    /// </summary>
    [CreateAssetMenu(fileName = "SpriteCollection", menuName = "Pixel Heroes/SpriteCollection")]
    public class SpriteCollection : ScriptableObject
    {
        public List<Layer> Layers;
        public Texture2D PaletteTexture;

        public static readonly List<Color32> Palette = new List<Color32>
        {
            new Color32(255, 0, 64, 255), new Color32(19, 19, 19, 255), new Color32(27, 27, 27, 255), new Color32(39, 39, 39, 255), new Color32(61, 61, 61, 255), new Color32(93, 93, 93, 255), new Color32(133, 133, 133, 255), new Color32(180, 180, 180, 255), new Color32(255, 255, 255, 255), new Color32(199, 207, 221, 255), new Color32(146, 161, 185, 255), new Color32(101, 115, 146, 255), new Color32(66, 76, 110, 255), new Color32(42, 47, 78, 255), new Color32(26, 25, 50, 255), new Color32(14, 7, 27, 255), new Color32(28, 18, 28, 255), new Color32(57, 31, 33, 255), new Color32(93, 44, 40, 255), new Color32(138, 72, 54, 255), new Color32(191, 111, 74, 255), new Color32(230, 156, 105, 255), new Color32(246, 202, 159, 255), new Color32(249, 230, 207, 255), new Color32(237, 171, 80, 255), new Color32(224, 116, 56, 255), new Color32(198, 69, 36, 255), new Color32(142, 37, 29, 255), new Color32(255, 80, 0, 255), new Color32(237, 118, 20, 255), new Color32(255, 162, 20, 255), new Color32(255, 200, 37, 255), new Color32(255, 235, 87, 255), new Color32(211, 252, 126, 255), new Color32(153, 230, 95, 255), new Color32(90, 197, 79, 255), new Color32(51, 152, 75, 255), new Color32(30, 111, 80, 255), new Color32(19, 76, 76, 255), new Color32(12, 46, 68, 255), new Color32(0, 57, 109, 255), new Color32(0, 105, 170, 255), new Color32(0, 152, 220, 255), new Color32(0, 205, 249, 255), new Color32(12, 241, 255, 255), new Color32(148, 253, 255, 255), new Color32(253, 210, 237, 255), new Color32(243, 137, 245, 255), new Color32(219, 63, 253, 255), new Color32(122, 9, 250, 255), new Color32(48, 3, 217, 255), new Color32(12, 2, 147, 255), new Color32(3, 25, 63, 255), new Color32(59, 20, 67, 255), new Color32(98, 36, 97, 255), new Color32(147, 56, 143, 255), new Color32(202, 82, 201, 255), new Color32(200, 80, 134, 255), new Color32(246, 129, 135, 255), new Color32(245, 85, 93, 255), new Color32(234, 50, 60, 255), new Color32(196, 36, 48, 255), new Color32(137, 30, 43, 255), new Color32(87, 28, 39, 255)
        };
        
        #if UNITY_EDITOR

		public void Refresh()
        {
            Debug.ClearDeveloperConsole();

            var palette = PaletteTexture.GetPixels32().ToList();

            palette.Add(Color.black);

            foreach (var layer in Layers)
            {
                layer.Refresh(palette);
            }

            EditorUtility.SetDirty(this);
            Debug.Log("Refresh done!");
            //Debug.Log(string.Join(", ", PaletteTexture.GetPixels32().Select(i => $"new Color32({i.r}, {i.g}, {i.b}, 255)")));
        }

        public void ApplyPalette()
        {
            var path = EditorUtility.OpenFilePanel("Open palette (PNG)", "", "png");

            if (path.Length == 0) return;

            foreach (var layer in Layers)
            {
                foreach (var texture in layer.Textures)
                {
                    var assetPath = AssetDatabase.GetAssetPath(texture);

                    TextureHelper.ApplyPalette(texture, path);
                    File.WriteAllBytes(assetPath, texture.EncodeToPNG());
                    Debug.Log($"Palette applied to {assetPath}");
                }
            }

            Debug.Log("ApplyPalette done!");
        }

        #endif
    }
}