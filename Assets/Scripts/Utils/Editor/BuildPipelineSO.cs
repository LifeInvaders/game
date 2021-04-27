using System.IO;
using UnityEditor;
using UnityEngine;

class BuildPipelineSO : ScriptableObject
{
    public const string CustomSettingsPath = "Assets/Editor/CustomSettings.asset";
    
    public string buildsPath;
    public string lastName;

    public bool windowsBuild;
    public bool linuxBuild;
    public bool macBuild;
    
    public int instances;

    internal static BuildPipelineSO GetOrCreateSettings()
    {
        var settings = AssetDatabase.LoadAssetAtPath<BuildPipelineSO>(CustomSettingsPath);
        if (settings == null)
        {
            settings = CreateInstance<BuildPipelineSO>();
            settings.buildsPath = Directory.GetCurrentDirectory();
            settings.windowsBuild = false;
            settings.linuxBuild = false;
            settings.macBuild = false;
            settings.instances = 1;
            settings.lastName = "";
            AssetDatabase.CreateAsset(settings, CustomSettingsPath);
            AssetDatabase.SaveAssets();
        }
        return settings;
    }
}
