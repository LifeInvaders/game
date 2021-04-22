using System;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.IO.Compression;
using Debug = UnityEngine.Debug;


public class BuildPipeline : EditorWindow
{
    private BuildPipelineSO _buildSettings;
    
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
		_buildSettings = BuildPipelineSO.GetOrCreateSettings();
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
        if (build) InitiateBuild();
    }

    void InitiateBuild()
    {
        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();
        var path = Path.Combine(_buildSettings.buildsPath, _buildSettings.lastName);
        Directory.CreateDirectory(path);
        if (_buildSettings.windowsBuild) Build(BuildTarget.StandaloneWindows64);
        if (_buildSettings.linuxBuild) Build(BuildTarget.StandaloneLinux64);
        if (_buildSettings.macBuild) Build(BuildTarget.StandaloneOSX);
        stopWatch.Stop();
        Debug.Log("Building sequence finished after " + stopWatch.ElapsedMilliseconds + " ms.");


    }

    void Build(BuildTarget buildTarget)
    {
        var path = Path.Combine(_buildSettings.buildsPath, _buildSettings.lastName, buildTarget.ToString());
        Directory.CreateDirectory(path);
        int i = 1;
        while (Directory.Exists(Path.Combine(path, "v" + i))) {i++;}
        string version = "v" + i;
        path = Path.Combine(path, version);
        Directory.CreateDirectory(path);
        DirectoryInfo parentDir = new DirectoryInfo(path);
        path = Path.Combine(path, "Instance 1");
        Directory.CreateDirectory(path);
        DirectoryInfo instanceDir = new DirectoryInfo(path);
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = EditorBuildSettingsScene.GetActiveSceneList(EditorBuildSettings.scenes),
            target = buildTarget,
            locationPathName = Path.Combine(path, "Panic_At_Tortuga.exe"),
            options = BuildOptions.None
        };
        UnityEditor.BuildPipeline.BuildPlayer(buildPlayerOptions);
        ZipFile.CreateFromDirectory(path, Path.Combine(parentDir.FullName, buildTarget + "_" + version + ".zip"));
        for (int instance = 2; instance <= _buildSettings.instances; instance++)
        {
            DirectoryInfo newInstance = new DirectoryInfo(Path.Combine(parentDir.FullName, "Instance " + instance));
            CopyAll(instanceDir, newInstance);
        }

    }
    
    public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
    {
        Directory.CreateDirectory(target.FullName);

        // Copy each file into the new directory.
        foreach (FileInfo fi in source.GetFiles())
        {
            fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
        }

        // Copy each subdirectory using recursion.
        foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
        {
            DirectoryInfo nextTargetSubDir =
                target.CreateSubdirectory(diSourceSubDir.Name);
            CopyAll(diSourceSubDir, nextTargetSubDir);
        }
    }

}