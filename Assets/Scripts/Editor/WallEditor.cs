using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Wall))]
public class WallEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        Wall w = target as Wall;
        if (GUILayout.Button("Reset material"))
        {
            w.ResetColor();
        }
    }
}
