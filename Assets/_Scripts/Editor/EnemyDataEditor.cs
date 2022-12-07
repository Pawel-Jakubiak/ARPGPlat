using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyData))]
public class EnemyDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Open Editor"))
        {
            EnemyEditorWindow.ShowWindow();
        }
    }
}
