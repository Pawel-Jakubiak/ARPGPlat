using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameSettingsWindow : EditorWindow
{
    public static GameSettingsData settings;

    [InitializeOnLoadMethod]
    private static void OnLoad()
    {
        settings = AssetDatabase.LoadAssetAtPath<GameSettingsData>("Assets/GameSettings.asset");

        if (settings) return;

        settings = CreateInstance<GameSettingsData>();

        AssetDatabase.CreateAsset(settings, "Assets/GameSettings.asset");
        AssetDatabase.Refresh();
    }

    [MenuItem("Window/Game Settings")]
    public static void ShowWindow()
    {
        GetWindow(typeof(GameSettingsWindow), false, "Game Settings");
    }

    private void OnGUI()
    {
        // Serializing data object
        SerializedObject serializedObject = new SerializedObject(settings);
        serializedObject.Update();

        GUILayout.Label("Gravity Settings", EditorStyles.boldLabel);

        settings.gravity = EditorGUILayout.FloatField("Gravity", settings.gravity);

        GUILayout.Label("Vertical Velocity Range");

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.Space(10);
        EditorGUIUtility.labelWidth = 30f;
        settings.verticalVelocityRange.min = EditorGUILayout.FloatField("Min", settings.verticalVelocityRange.min);
        settings.verticalVelocityRange.max = EditorGUILayout.FloatField("Max", settings.verticalVelocityRange.max);
        EditorGUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();
        AssetDatabase.SaveAssets();
    }
}

