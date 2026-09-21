using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public static class WebGLAwsSdkBuild
{
    private const string SdkFileName = "aws-sdk-2.1693.0.min.js";

    [PostProcessBuild(100)]
    public static void IncludeAwsSdk(BuildTarget target, string buildPath)
    {
        if (target != BuildTarget.WebGL)
        {
            return;
        }

        string sdkSource = Path.Combine(Application.dataPath,
            "Editor/WebGLDependencies/" + SdkFileName + ".txt");
        string indexPath = Path.Combine(buildPath, "index.html");
        if (!File.Exists(sdkSource) || !File.Exists(indexPath))
        {
            throw new InvalidOperationException(
                "Cannot include AWS SDK: the bundled SDK or WebGL index.html is missing.");
        }

        string dependencies = Path.GetDirectoryName(sdkSource);
        string html = File.ReadAllText(indexPath);
        string[] scripts = { SdkFileName, "aws-config.js", "cannon-storage.js" };
        string tags = "";
        foreach (string script in scripts)
        {
            string tag = "<script src=\"TemplateData/" + script + "\"></script>";
            html = html.Replace("\n    " + tag, "").Replace(tag, "");
            tags += "\n    " + tag;
        }
        int headStart = html.IndexOf("<head", StringComparison.OrdinalIgnoreCase);
        int headEnd = headStart < 0 ? -1 : html.IndexOf('>', headStart);
        if (headEnd < 0)
        {
            throw new InvalidOperationException("WebGL index.html has no head element.");
        }
        html = html.Insert(headEnd + 1, tags);
        string templateData = Path.Combine(buildPath, "TemplateData");
        Directory.CreateDirectory(templateData);
        File.Copy(sdkSource, Path.Combine(templateData, SdkFileName), true);
        File.Copy(Path.Combine(dependencies, "cannon-storage.js.txt"),
            Path.Combine(templateData, "cannon-storage.js"), true);
        string config = Path.Combine(dependencies, "aws-config.local.js.txt");
        if (!File.Exists(config))
        {
            config = Path.Combine(dependencies, "aws-config.example.js.txt");
        }
        File.Copy(config, Path.Combine(templateData, "aws-config.js"), true);
        foreach (string notice in new[] { "LICENSE.aws-sdk.txt", "NOTICE.aws-sdk.txt" })
        {
            File.Copy(Path.Combine(dependencies, notice),
                Path.Combine(templateData, notice), true);
        }
        File.WriteAllText(indexPath, html, new UTF8Encoding(false));
        Debug.Log("Included local data saving, export controls, and optional AWS configuration.");
    }
}
