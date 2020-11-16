using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.IO.Compression;
using System.Security;
using UnityEngine.Networking;

[Serializable]
public class MyData
{
    public float speed;
    public float health;
    public string fullName;
    public string base64Texture;
}

public class DataLoader : Editor
{
    [MenuItem("Tools/Download data from URL")]
    public static void DownloadData()
    {
        string url = "https://dminsky.com/settings.zip";
        string outPath = Path.Combine(Application.persistentDataPath, "settings.zip");
        // Downloading file
        if (!File.Exists(outPath))
        {
            UnityWebRequest uwr = UnityWebRequest.Get(url);
            uwr.downloadHandler = new DownloadHandlerFile(outPath);
            var asyncOp = uwr.SendWebRequest();
            asyncOp.completed += (ao) =>
            {
                if (uwr.isNetworkError || uwr.isHttpError)
                {
                    Debug.Log("Download error");
                }
                else
                {
                    Debug.Log("Download is completed");
                    ExtractZipFile(outPath);
                }
            };
        }
        else
        {
            Debug.Log("Archive already exists");
            ExtractZipFile(outPath);
        }
    }

    private static void ExtractZipFile(string zipPath)
    {
        Debug.Log("Extracting...");
        // Extracting zip-file
        string extractPath = Path.Combine(Application.persistentDataPath, "Settings");
        DirectoryInfo directory = new DirectoryInfo(extractPath);
        
        if (!directory.Exists)
        {
            Debug.Log("Extract directory is not found. Creating...");
            Directory.CreateDirectory(extractPath);
        }
        else
        {
            foreach (FileInfo file in directory.GetFiles())
            {
                Debug.Log(file.Name);
                file.Delete(); 
            }
        }
        
        try
        {
            ZipFile.ExtractToDirectory(zipPath, extractPath);
        }
        catch (IOException e)
        {
            string msg = $"Zip extraction failed\n{e.Message}";
            EditorUtility.DisplayDialog("Error", msg, "Понимаю");
            throw new Exception("Archive extraction error");
        }
        catch (InvalidDataException e)
        {
            string msg = $"Invalid data exception\n{e.Message}";
            EditorUtility.DisplayDialog("Error", msg, "Понимаю");
            throw new Exception("Invalid data exception");
        }
        catch (Exception e)
        {
            string msg = $"Error!\n{e.Message}";
            EditorUtility.DisplayDialog("Error", msg, "Понимаю");
            throw new Exception("Exception occured");
        }
        LoadDataFromJson(extractPath);
    }

    private static void LoadDataFromJson(string jsonPath)
    {
        // Getting jsonData from file
        try
        {
            string jsonData = File.ReadAllText(Path.Combine(jsonPath, "settings.json"));
            MyData data = JsonUtility.FromJson<MyData>(jsonData);
            ApplyMaterialSettings(data);
        }
        catch (FileNotFoundException e)
        {
            string msg = $"File not found\n{e.Message}";
            EditorUtility.DisplayDialog("Error", msg, "Понимаю");
            throw;
        }
        catch (SecurityException e)
        {
            string msg = $"Permissons security error\n{e.Message}";
            EditorUtility.DisplayDialog("Error", msg, "Понимаю");
            throw;
        }       
        catch (IOException e)
        {
            string msg = $"File reading error\n{e.Message}";
            EditorUtility.DisplayDialog("Error", msg, "Понимаю");
            throw;
        }
        catch (Exception e)
        {
            string msg = $"Error occured\n{e.Message}";
            EditorUtility.DisplayDialog("Error", msg, "Понимаю");
            throw;
        }
    }

    private static void ApplyMaterialSettings(MyData data)
    {
        // Applying new speed value to Player_2
        GameObject.Find("Player_2").GetComponent<PlayerController>().SetMovementSpeed(data.speed);
        // Creating new texture from byte[] array
        try
        {
            byte[] image = Convert.FromBase64String(data.base64Texture);
            Texture2D newTexture = new Texture2D(2,2);
            newTexture.LoadImage(image);
            // Applying texture to wall material
            Material wallMaterial = GameObject.Find("Wall").GetComponent<MeshRenderer>().sharedMaterial;
            wallMaterial.EnableKeyword("_NORMALMAP");
            wallMaterial.SetTexture("_BumpMap", newTexture);
        }
        catch (Exception e)
        {
            Debug.Log($"Error:\n{e.Message}");
            throw;
        }
    }
}
