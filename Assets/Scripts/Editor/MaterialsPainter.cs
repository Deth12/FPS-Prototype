using UnityEditor;
using UnityEngine;

public class MaterialsPainter : EditorWindow
{
    [MenuItem("Tools/MaterialsPainter")]
    public static void ShowWindow()
    {
        EditorWindow w = EditorWindow.GetWindow<MaterialsPainter>();
        w.minSize = new Vector2(300, 100);
        w.maxSize = new Vector2(400, 200);
        w.Show();
    }

    private Color targetColor = new Color(0,0,0,1);
    
    private void OnGUI()
    {
        targetColor = EditorGUILayout.ColorField("New Color", targetColor);
        if (GUILayout.Button("Apply to all objects"))
            PaintEverything();
    }
    
    private void PaintEverything()
    {
        foreach (Renderer r in Resources.FindObjectsOfTypeAll(typeof(Renderer)))
        {
            r.sharedMaterial.SetColor("_Color", targetColor);
        }
    }
}
