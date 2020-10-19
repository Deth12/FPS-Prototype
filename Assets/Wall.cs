using UnityEngine;

public class Wall : MonoBehaviour
{
    private Color defaultColor = Color.white;

    public void ResetColor()
    {
        GetComponent<Renderer>().sharedMaterial.SetColor("_Color", defaultColor);
    }
}
