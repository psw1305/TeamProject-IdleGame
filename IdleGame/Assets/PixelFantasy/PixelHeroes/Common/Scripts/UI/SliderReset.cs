using UnityEngine;
using UnityEngine.UI;

namespace Assets.PixelFantasy.PixelHeroes.Common.Scripts.UI
{
    /// <summary>
    /// Used to set Slider zero value.
    /// </summary>
    public class SliderReset : MonoBehaviour
    {
        public void Reset()
        {
            GetComponentInParent<Slider>().value = 0;
        }
    }
}