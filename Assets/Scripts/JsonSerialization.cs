using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using LitJson;
using System.Text;

public class JsonSerialization : MonoBehaviour
{
    public GameObject objects;
    
    public void Submit()
    {
        List<ItemObject> items = new List<ItemObject>();

        foreach (Transform child in objects.transform)
        {
            ItemObject item = new ItemObject();
            item.tag = child.gameObject.tag;
            item.pos = child.gameObject.transform.position.ToString();
            item.rot = gameObject.transform.rotation.ToString();
            item.scale = child.gameObject.transform.localScale.ToString();

            items.Add(item);
        }
        //Convert to Jason
        string itemsToJson = JsonHelper.ToJson(items.ToArray(), true);
        Debug.Log(itemsToJson);

        StartCoroutine(PostRequest(itemsToJson));
    }

    IEnumerator PostRequest(string json)
    {
        // TODO: Get ID from Session
        string id_str = "912345678";
        using (UnityWebRequest webRequest = UnityWebRequest.Put("https://valiant-postcard.herokuapp.com/submit?id=" + id_str, json))
        {
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.method = "POST";

            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                Debug.Log(webRequest.error);
            }
            else
            {
                Debug.Log("Upload complete!");
                StringBuilder sb = new StringBuilder();
                foreach (System.Collections.Generic.KeyValuePair<string, string> dict in webRequest.GetResponseHeaders())
                {
                    sb.Append(dict.Key).Append(": \t[").Append(dict.Value).Append("]\n");
                }
                // Print Headers
                //Debug.Log(sb.ToString());

                // Print Body
                Debug.Log(webRequest.downloadHandler.text);

                Processjson(webRequest.downloadHandler.text);
            }
        }
    }

    private void Processjson(string jsonString)
    {
        JsonData jsonvale = JsonMapper.ToObject(jsonString);
        bool uploadSuccess = (jsonvale["write_record"].ToString() == "true");
        
        if (!uploadSuccess)
        {
            Debug.Log("Upload fail!");
        } else
        {
            Debug.Log("Upload success!");
        }
    }
}
