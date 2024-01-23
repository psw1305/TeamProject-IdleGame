using System.Collections;
using UnityEngine;

namespace Assets.PixelFantasy.PixelHeroes.Common.Scripts.CharacterScripts
{
    public class Creature : MonoBehaviour
    {
        public SpriteRenderer Body;

        private static Material DefaultMaterial;
        private static Material BlinkMaterial;

        public void Blink()
        {
            if (DefaultMaterial == null) DefaultMaterial = Body.sharedMaterial;
            if (BlinkMaterial == null) BlinkMaterial = new Material(Shader.Find("GUI/Text Shader"));

            StartCoroutine(BlinkCoroutine());
        }

        private IEnumerator BlinkCoroutine()
        {
            Body.material = BlinkMaterial;

            yield return new WaitForSeconds(0.1f);

            Body.material = DefaultMaterial;
        }
    }
}