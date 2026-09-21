using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class ExperimentController : MonoBehaviour
{
    public string ppID;
    public string trialNum;
    public string keyPresses;
    public string timeStamps;
    public string aimAngles;
    public string tarHit;
    public string endPointFB;
    public string pMag;

    private string storageStatus = "Local saving only. AWS is not configured.";

#if !UNITY_WEBGL || UNITY_EDITOR
    [Serializable]
    private class TrialData
    {
        public string ppID, trial, keyPresses, timeStamps, aimAngles, tarHit;
        public string endPointFB, tokenId, pertMag, valArray, tarPos;
    }
#endif

#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void RegisterStorageStatus(string objectName);

    [DllImport("__Internal")]
    private static extern void InsertData(
        string tableName,
        string ppID,
        string trialNum,
        string keyPresses,
        string timeStamps,
        string aimAngles,
        string tarHit,
        string endPointFB,
        string token,
        string pertMag,
        string valArray,
        string tarPos);
#endif

    private void Start()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        RegisterStorageStatus(gameObject.name);
#endif
    }

    public void putData(
        string trial,
        string keys,
        string times,
        string angles,
        string hit,
        string feedback,
        string perturbation,
        string valArray,
        string tarPos)
    {
        string url = Application.absoluteURL;
        string participant = url.Substring(url.LastIndexOf('=') + 1);
        ppID = participant + " " + DateTime.Now.ToString();
        trialNum = trial;
        keyPresses = keys;
        timeStamps = times;
        aimAngles = angles;
        tarHit = hit;
        endPointFB = feedback;
        pMag = perturbation;

#if UNITY_WEBGL && !UNITY_EDITOR
        InsertData(
            "ESSMV2",
            ppID,
            trialNum,
            keyPresses,
            timeStamps,
            aimAngles,
            tarHit,
            endPointFB,
            ppID,
            pMag,
            valArray,
            tarPos);
#else
        LocalExperimentStore.Save("trial", "ESSMV2", new TrialData
        {
            ppID = ppID,
            trial = trialNum,
            keyPresses = keyPresses,
            timeStamps = timeStamps,
            aimAngles = aimAngles,
            tarHit = tarHit,
            endPointFB = endPointFB,
            tokenId = ppID,
            pertMag = pMag,
            valArray = valArray,
            tarPos = tarPos
        });
#endif
    }

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, Mathf.Min(Screen.width - 20, 800), 130), GUI.skin.box);
#if UNITY_WEBGL && !UNITY_EDITOR
        GUILayout.Label(storageStatus);
        if (Screen.fullScreen && GUILayout.Button("Exit fullscreen to download saved data"))
        {
            Screen.fullScreen = false;
        }
#else
        GUILayout.Label("Saving locally.");
        GUILayout.Label("Saved records: " + LocalExperimentStore.SavedCount + " | " + LocalExperimentStore.DataPath);
        if (!string.IsNullOrEmpty(LocalExperimentStore.LastError))
        {
            GUILayout.Label(LocalExperimentStore.LastError);
        }
#endif
        GUILayout.EndArea();
    }

    public void StringCallback(string info)
    {
        storageStatus = info;
    }
}
