using UnityEditor;
using UnityEngine;

public class OverrideTextureMaxSize
{
    [MenuItem("Tools/Override Texture Max Size")]
    public static void OverrideMaxSize()
    {
        string[] guids = AssetDatabase.FindAssets("t:Texture");
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
            if (textureImporter != null)
            {
                textureImporter.maxTextureSize = 16384;
                AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
                Debug.Log($"Updated: {path}");
            }
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("All textures updated to max size 16384");
    }
}