using System;
using Assets.PixelFantasy.PixelHeroes.Common.Scripts.CollectionScripts;
using Assets.PixelFantasy.PixelHeroes.Common.Scripts.UI;
using UnityEngine;

namespace Assets.PixelFantasy.PixelHeroes.Common.Scripts.EditorScripts
{
    [Serializable]
    public class LayerEditor
    {
        public string Name;
        public LayerControls Controls;
        public int Index;
        public bool CanBeEmpty = true;
        public string Default;

        public Layer Content { set; get; }
        public LayerEditor Mask { set; get; }

        [HideInInspector] public bool Hidden;
        [HideInInspector] public Color Color = Color.white;

        public string SpriteData => Index == -1 || Hidden ? "" : $"{Content.Textures[Index].name}#{ColorUtility.ToHtmlStringRGB(Color)}/{Controls.Hue.value}:{Controls.Saturation.value}:{Controls.Brightness.value}";

        public void Switch(int direction)
        {
            Index += direction;

            var min = CanBeEmpty ? -1 : 0;

            if (Index < min) Index = Content.Textures.Count - 1;
            if (Index == Content.Textures.Count) Index = min;
        }

        public void SetIndex(int index)
        {
            Index = index;
        }

        public Color32[] GetPixels(bool update)
        {
            var mask = Mask != null && Mask.Index != -1 ? Mask.Content.Textures[Mask.Index].GetPixels32() : null;

            return Content.GetPixels(Index, Color, Controls.Hue.value, Controls.Saturation.value, Controls.Brightness.value, mask, update);
        }

        public Color[] GetPixels(int x, int y, int blockWidth, int blockHeight)
        {
            return Content.GetPixels(Index, Color, Controls.Hue.value, Controls.Saturation.value, Controls.Brightness.value, x, y, blockWidth, blockHeight);
        }
    }
}