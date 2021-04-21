using UnityEditor;
using UnityEngine;
using System.IO;
using Unity.Collections.LowLevel.Unsafe;


class BuildSettings : ScriptableObject
{
    public const string CustomSettingsPath = "Assets/Editor/CustomSettings.asset";
    
    public string buildsPath;
    public string lastName;

    public bool windowsBuild;
    public bool linuxBuild;
    public bool macBuild;
    
    public int instances;

    internal static BuildSettings GetOrCreateSettings()
    {
        var settings = AssetDatabase.LoadAssetAtPath<BuildSettings>(CustomSettingsPath);
        if (settings == null)
        {
            settings = CreateInstance<BuildSettings>();
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
public class BuildPipeline : EditorWindow
{
    private BuildSettings _buildSettings;
    
    [MenuItem("Tools/Build Pipeline")]
    public static void OpenBuildWindow()
    {
        GetWindow(typeof(BuildPipeline));
    }
    
    private string BuildPath()
    {
        var path = _buildSettings.buildsPath;
        DirectoryInfo currentDir = new DirectoryInfo(path);
        return EditorUtility.SaveFolderPanel("Choose Build Folder", currentDir.Parent.FullName, currentDir.Name);
    }

    void OnGUI()
    {
		_buildSettings = BuildSettings.GetOrCreateSettings();
        GUILayout.Label ("Build Settings", EditorStyles.boldLabel);
        var buildPath = GUILayout.Button(_buildSettings.buildsPath);
        var versionName = EditorGUILayout.TextField("Build name",_buildSettings.lastName);
        var windowsToggle = GUILayout.Toggle(_buildSettings.windowsBuild,"Build for Windows");
        var linuxToggle = GUILayout.Toggle(_buildSettings.linuxBuild,"Build for Linux");
        var macToggle = GUILayout.Toggle(_buildSettings.macBuild,"Build for Mac");
        var instancesSlider = EditorGUILayout.IntSlider("Instances",_buildSettings.instances, 1, 4);
        var build = GUILayout.Button("Build");
        if (buildPath) _buildSettings.buildsPath = BuildPath();
        _buildSettings.lastName = versionName;
        _buildSettings.windowsBuild = windowsToggle;
        _buildSettings.linuxBuild = linuxToggle;
        _buildSettings.macBuild = macToggle;
        _buildSettings.instances = instancesSlider;
        if (build) Build();
    }

    void Build()
    {
        Directory.SetCurrentDirectory(_buildSettings.buildsPath);
        Directory.CreateDirectory(_buildSettings.lastName);
        Directory.SetCurrentDirectory(_buildSettings.lastName);
        if (_buildSettings.windowsBuild) Directory.CreateDirectory(Windows);

    }

}