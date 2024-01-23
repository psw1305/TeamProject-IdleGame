using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.PixelFantasy.PixelHeroes.Common.Scripts.Utils
{
    public class ColorDistinctor
    {
        public List<Color32> UniqueColors { get; } = new List<Color32>();

        private readonly bool[,,] _matrix = new bool[256, 256, 256];

        public ColorDistinctor()
        {
        }

        public ColorDistinctor(IEnumerable<Color32> colors)
        {
            AddColors(colors);
        }

        public void AddColors(IEnumerable<Color32> colors, bool alpha = false)
        {
            if (alpha) throw new NotImplementedException();

            var transparent = false;

            foreach (var color in colors)
            {
                if (color.a == 0)
                {
                    transparent = true;
                }
                else if (!_matrix[color.r, color.g, color.b])
                {
                    _matrix[color.r, color.g, color.b] = true;
                    UniqueColors.Add(new Color32(color.r, color.g, color.b, 255));
                }
            }

            if (transparent && UniqueColors.Count > 0 && UniqueColors[0].a != 0)
            {
                UniqueColors.Insert(0, new Color32());
            }
        }
    }
}