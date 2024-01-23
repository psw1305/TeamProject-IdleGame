using Assets.PixelFantasy.PixelHeroes.Common.Scripts.EditorScripts;
using UnityEditor;
using UnityEditor.U2D.Sprites;
using UnityEngine;

namespace Assets.PixelFantasy.PixelHeroes.Common.Scripts.Editor
{
    public class SpriteSheetSlicer : EditorWindow
    {
        public Texture2D Texture;

        [InitializeOnLoadMethod]
        private static void InitializeOnLoad()
        {
            CharacterEditor.SliceTextureRequest += Slice;
        }

        [MenuItem("Window/◆ Pixel Heroes/Sprite Sheet Slicer")]
        public static void Open()
        {
            var window = GetWindow<SpriteSheetSlicer>(false, "Sprite Sheet Slicer");

            window.minSize = window.maxSize = new Vector2(300, 150);
            window.Show();
        }

        public void OnGUI()
        {
            EditorGUILayout.LabelField("Slice Texture to build Sprite Sheet", new GUIStyle(EditorStyles.label) { normal = { textColor = Color.yellow } });
            Texture = (Texture2D) EditorGUILayout.ObjectField(new GUIContent("Sprite Sheet (Texture)"), Texture, typeof(Texture2D), false);

            if (GUILayout.Button("Slice"))
            {
                if (Slice(Texture))
                {
                    EditorUtility.DisplayDialog("Success", $"Texture sliced:\n{AssetDatabase.GetAssetPath(Texture)}", "OK");					
                }
            }
        }

        private static void Slice(string path)
        {
            Slice(AssetDatabase.LoadAssetAtPath<Texture2D>(path));
        }

        private static bool Slice(Texture2D texture)
        {
            const string examplePath = "Assets/PixelFantasy/PixelHeroes/Common/Images/Example.png";
            var exampleTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(examplePath);

            if (texture == null)
            {
                EditorUtility.DisplayDialog("Error", "Sprite Sheet is empty.", "OK");
            }
            else if (exampleTexture == null)
            {
                EditorUtility.DisplayDialog("Error", $"Example texture not found: {examplePath}", "OK");
            }
            else if (texture.width != exampleTexture.width || texture.height != exampleTexture.height)
            {
                EditorUtility.DisplayDialog("Error", $"Target texture should be {exampleTexture.width}x{exampleTexture.height}px.", "OK");
            }
            else
            {
                var path = AssetDatabase.GetAssetPath(texture);
                var target = (TextureImporter) AssetImporter.GetAtPath(path);
                var example = (TextureImporter) AssetImporter.GetAtPath(examplePath);
                var settings = new TextureImporterSettings();

                target.textureType = TextureImporterType.Sprite;
                target.spriteImportMode = SpriteImportMode.Multiple;
                target.textureCompression = TextureImporterCompression.Uncompressed;
                example.ReadTextureSettings(settings);
                target.SetTextureSettings(settings);

                var factory = new SpriteDataProviderFactories();

                factory.Init();

                var exampleData = factory.GetSpriteEditorDataProviderFromObject(example);
                var targetData = factory.GetSpriteEditorDataProviderFromObject(target);

                exampleData.InitSpriteEditorDataProvider();
                targetData.InitSpriteEditorDataProvider();

                var spriteRects = exampleData.GetSpriteRects();

                targetData.SetSpriteRects(spriteRects);
                targetData.Apply();
                target.SaveAndReimport();
				
				return true;
            }

            return false;
        }
    }
}