using UnityEngine;


namespace Assets.PixelFantasy.PixelHeroes.Common.Scripts.ExampleScripts
{
    public class CharacterSwitch : MonoBehaviour
    {
        public UnityEngine.U2D.Animation.SpriteLibrary Character;
        public UnityEngine.U2D.Animation.SpriteLibraryAsset[] Characters;

        private int _index;

        public void Update()
        {
            for (var i = 0; i < Characters.Length; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                {
                    _index = i;
                    Character.spriteLibraryAsset = Characters[_index];
                }
            }

            if (Input.GetKeyDown(KeyCode.Minus))
            {
                _index--;

                if (_index < 0) _index = Characters.Length - 1;

                Character.GetComponentInChildren<UnityEngine.U2D.Animation.SpriteLibrary>().spriteLibraryAsset = Characters[_index];
            }

            if (Input.GetKeyDown(KeyCode.Equals))
            {
                _index++;

                if (_index >= Characters.Length) _index = 0;

                Character.GetComponentInChildren<UnityEngine.U2D.Animation.SpriteLibrary>().spriteLibraryAsset = Characters[_index];
            }
        }
    }
}