using UnityEngine;

public class Mirror : MonoBehaviour
{
    private Camera cam;
    private Material mat;
    private RenderTexture rt;
    
    private void Start()
    {
        cam = GetComponentInChildren<Camera>();
        mat = GetComponent<Renderer>().material;
        
        // Creates new RenderTexture
        rt = new RenderTexture(256, 256, 16, RenderTextureFormat.Default);
        rt.Create();
        
        // Assigns renderTexture to material
        mat.mainTexture = rt;
        
        cam.targetTexture = rt;
    }
}
