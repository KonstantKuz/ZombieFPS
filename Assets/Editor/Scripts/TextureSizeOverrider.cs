using UnityEditor;

namespace Editor.Scripts
{
    public class TextureSizeOverrider
    {
        public static void UpdateTextures(int maxSize, string[] targetFolders)
        {
            var allTextures = AssetDatabase.FindAssets("t:Texture", targetFolders);
            foreach (string textureGUID in allTextures)
            {
                var texturePath = AssetDatabase.GUIDToAssetPath(textureGUID);
                var textureImporter = AssetImporter.GetAtPath(texturePath) as TextureImporter;
                if(textureImporter == null) continue;
                SetPlatformTextureMaxSize(textureImporter, "DefaultTexturePlatform", maxSize, false);
                AssetDatabase.ImportAsset(texturePath);
            }
        }

        private static void SetPlatformTextureMaxSize(TextureImporter importer, string platform, int maxSize, bool overriden = true)
        {
            var platformSettings = importer.GetPlatformTextureSettings(platform);
            platformSettings.overridden = overriden;
            platformSettings.maxTextureSize = maxSize;
            importer.SetPlatformTextureSettings(platformSettings);
        }
    }
}
