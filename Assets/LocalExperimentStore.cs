using System;
using System.IO;
using System.Text;
using UnityEngine;

public static class LocalExperimentStore
{
    private static readonly string Session = DateTime.UtcNow.ToString("yyyyMMdd-HHmmss") + "-" + Guid.NewGuid().ToString("N");
    public static string LastError { get; private set; }
    public static int SavedCount { get; private set; }
    public static string DataPath
    {
        get { return Path.Combine(Application.persistentDataPath, "LocalData", "cannon-data-" + Session + ".jsonl"); }
    }

    [Serializable]
    private class Header
    {
        public string format = "cannon-experiment-v1";
        public string id;
        public string session;
        public string recordedAt;
        public string kind;
        public string table;
        public string remote = "not-configured";
    }

    public static void Save(string kind, string table, object item)
    {
        try
        {
            var header = new Header
            {
                id = Guid.NewGuid().ToString("N"),
                session = Session,
                recordedAt = DateTime.UtcNow.ToString("o"),
                kind = kind,
                table = table
            };
            // JsonUtility cannot serialize a polymorphic object field; append its concrete JSON.
            string json = JsonUtility.ToJson(header);
            json = json.Substring(0, json.Length - 1) + ",\"item\":" + JsonUtility.ToJson(item) + "}";
            Directory.CreateDirectory(Path.GetDirectoryName(DataPath));
            File.AppendAllText(DataPath, json + Environment.NewLine, new UTF8Encoding(false));
            SavedCount++;
            LastError = null;
        }
        catch (Exception error)
        {
            LastError = "Local saving failed: " + error.Message;
            Debug.LogError(LastError);
        }
    }
}
