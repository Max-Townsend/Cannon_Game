using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public static class BuildCannonWebGL
{
    [MenuItem("Tools/Cannon/Build WebGL")]
    public static void Build()
    {
        string root = Directory.GetParent(Application.dataPath).FullName;
        BuildReport report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
        {
            scenes = new[] { "Assets/Scenes/SampleScene.unity" },
            locationPathName = Path.Combine(root, "Builds", "WebGL"),
            target = BuildTarget.WebGL,
            options = BuildOptions.None
        });
        if (report.summary.result != BuildResult.Succeeded)
        {
            throw new InvalidOperationException("WebGL build failed: " + report.summary.result);
        }
    }
}
