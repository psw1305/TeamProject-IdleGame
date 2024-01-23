using System.Linq;
using Assets.PixelFantasy.PixelHeroes.Common.Scripts.EditorScripts;
using UnityEditor;
using UnityEngine;

#if UNITY_2021_1_OR_NEWER

using UnityEngine.U2D.Animation;

#else

using UnityEngine.Experimental.U2D.Animation;

#endif

namespace Assets.PixelFantasy.PixelHeroes.Common.Scripts.Editor
{
    public class SpriteLibraryBuilder : EditorWindow
    {
        public Texture2D Texture;
        public SpriteLibraryAsset SpriteLibraryAsset;

        [InitializeOnLoadMethod]
        private static void InitializeOnLoad()
        {
            CharacterEditor.CreateSpriteLibraryRequest += CreateSpriteLibrary;
        }

        [MenuItem("Window/◆ Pixel Heroes/Sprite Library Builder")]
        public static void Open()
        {
            var window = GetWindow<SpriteLibraryBuilder>(false, "Sprite Library Builder");

            window.minSize = window.maxSize = new Vector2(300, 150);
            window.Show();
        }

        public void OnGUI()
        {
            EditorGUILayout.LabelField("Fill Sprite Library based on Sprite Sheet", new GUIStyle(EditorStyles.label) { normal = { textColor = Color.yellow } });
            Texture = (Texture2D) EditorGUILayout.ObjectField(new GUIContent("Sprite Sheet (Texture)"), Texture, typeof(Texture2D), false);
            SpriteLibraryAsset = (SpriteLibraryAsset) EditorGUILayout.ObjectField(new GUIContent("Sprite Library Asset"), SpriteLibraryAsset, typeof(SpriteLibraryAsset), false);

            if (GUILayout.Button("Build"))
            {
                if (Configure(Texture, SpriteLibraryAsset))
                {
                    EditorUtility.DisplayDialog("Success", $"Sprite Library Asset configured:\n{AssetDatabase.GetAssetPath(SpriteLibraryAsset)}", "OK");
                }
            }
        }

        private static bool Configure(Texture2D texture, SpriteLibraryAsset spriteLibrary)
        {
            if (texture == null)
            {
                EditorUtility.DisplayDialog("Error", "Sprite Sheet is empty.", "OK");
            }
            else if (spriteLibrary == null)
            {
                EditorUtility.DisplayDialog("Error", "Sprite Library Asset is empty.", "OK");
            }
            else
            {
                var path = AssetDatabase.GetAssetPath(texture);
                var sprites = AssetDatabase.LoadAllAssetsAtPath(path).OfType<Sprite>().ToList();

                foreach (var category in spriteLibrary.GetCategoryNames().ToList())
                {
                    foreach (var label in spriteLibrary.GetCategoryLabelNames(category).ToList())
                    {
                        spriteLibrary.RemoveCategoryLabel(category, label, true);
                    }
                }

                foreach (var sprite in sprites)
                {
                    if (!sprite.name.Contains("_")) continue;

                    var split = sprite.name.Split('_');

                    spriteLibrary.AddCategoryLabel(sprite, split[0], split[1]);
                }

                return true;
            }

            return false;
        }

        private static void CreateSpriteLibrary(string texturePath)
        {
            var spriteLibraryPath = texturePath.Replace(".png", ".asset");
            var spriteLibrary = CreateInstance<SpriteLibraryAsset>();

            Configure(AssetDatabase.LoadAssetAtPath<Texture2D>(texturePath), spriteLibrary);

            AssetDatabase.CreateAsset(spriteLibrary, spriteLibraryPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}