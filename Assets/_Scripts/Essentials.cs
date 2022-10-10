using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class Essentials
{
    public static GameSettingsData Settings = AssetDatabase.LoadAssetAtPath<GameSettingsData>("Assets/GameSettings.asset");
}


[System.Serializable]
public struct MinMax
{
    public float min;
    public float max;
}
