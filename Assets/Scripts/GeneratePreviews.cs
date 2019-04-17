using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class GeneratePreviews : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string[] categories = { "Bathroom", "Beds", "Cabinets & Racks", "Christmas", "Effects", "Gifts", "Lights", "Mirrors", "Music", "Nature", "Sofa & Chairs", "Tables & Chairs", "Vases", "Vehicles" };
        foreach (string category in categories)
        {
            GameObject[] assetPrefab = Resources.LoadAll<GameObject>(category);
            for (int i = 0; i < assetPrefab.Length; i++)
            {
                Texture2D assetPreview = AssetPreview.GetAssetPreview(assetPrefab[i]);
                while (assetPreview == null)
                {
                    assetPreview = AssetPreview.GetAssetPreview(assetPrefab[i]);
                    System.Threading.Thread.Sleep(10);
                }
                byte[] bytes = assetPreview.EncodeToPNG();
                File.WriteAllBytes(Application.dataPath + "/Resources/" + category + "/" + assetPrefab[i].name + ".png", bytes);
                Debug.Log(AssetDatabase.GetAssetPath(assetPreview));
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
