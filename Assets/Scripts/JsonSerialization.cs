﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using LitJson;
using System.Text;
using UnityEngine.SceneManagement;

public class JsonSerialization : MonoBehaviour
{
    public GameObject rudolphObjects;
    public GameObject santaObjects;
    public GameObject uploadFailedPanel;
    public GameObject uploadSuccessPanel;
    public GameObject loadingPanel;

    public void Submit()
    {
        string cardName = Session.cardName;
        GameObject objects = rudolphObjects; // default

        if (cardName == "rudolph")
        {
            objects = rudolphObjects;
        } else if (cardName == "santa")
        {
            objects = santaObjects;
        }

        List<ItemObject> items = new List<ItemObject>();

        foreach (Transform child in objects.transform)
        {
            ItemObject item = new ItemObject();
            // remove '(Clone)'
            string name = child.gameObject.name;
            name = name.Substring(0, name.IndexOf('('));
            item.tag = name;

            item.pos = "(" + child.gameObject.transform.localPosition.x.ToString("f3") + ", " + child.gameObject.transform.localPosition.y.ToString("f3") + ", " +
                        child.gameObject.transform.localPosition.z.ToString("f3") + ")";
            item.rot = "(" + child.gameObject.transform.localRotation.x.ToString("f3") + ", " + child.gameObject.transform.localRotation.y.ToString("f3") + ", " +
                        child.gameObject.transform.localRotation.z.ToString("f3") + ", " + child.gameObject.transform.localRotation.w.ToString("f3") + ")";
            item.scale = "(" + child.gameObject.transform.localScale.x.ToString("f3") + ", " + child.gameObject.transform.localScale.y.ToString("f3") + ", " +
                        child.gameObject.transform.localScale.z.ToString("f3") + ")";

            items.Add(item);
        }

        //Convert to Jason
        CardType card = new CardType();
        card.name = Session.cardName;
        string cardToJson = JsonUtility.ToJson(card);
        cardToJson = cardToJson.Substring(0, cardToJson.Length - 1) + ",";

        Music musicClip = new Music();
        musicClip.music = Session.music;
        string musicToJson = JsonUtility.ToJson(musicClip);
        musicToJson = musicToJson.Substring(1, musicToJson.Length - 2) + ",";

        string itemsToJson = JsonHelper.ToJson(items.ToArray(), true);
        itemsToJson = itemsToJson.Substring(1);
        string combinedJson = cardToJson + musicToJson + itemsToJson;

        Debug.Log(combinedJson);

        StartCoroutine(PostRequest(combinedJson));
    }

    IEnumerator PostRequest(string json)
    {
        loadingPanel.SetActive(true);

        string id_str = Session.id;
        using (UnityWebRequest webRequest = UnityWebRequest.Put("https://valiant-postcard.herokuapp.com/submit?id=" + id_str, json))
        {
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.method = "POST";

            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                Debug.Log(webRequest.error);
                SubmitUI(false);
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
        bool uploadSuccess = (jsonvale["write_record"].ToString().ToLower() == "true");

        SubmitUI(uploadSuccess);
    }

    private void SubmitUI(bool uploadSuccess)
    {
        loadingPanel.SetActive(false);

        if (!uploadSuccess)
        {
            Debug.Log("Upload fail!");
            uploadFailedPanel.SetActive(true);
        }
        else
        {
            Debug.Log("Upload success!");
            uploadSuccessPanel.SetActive(true);
            Session.ResetSession();
        }
    }

    public void GotoNextScene(string scenename)
    {
        Session.ResetSession();
        SceneManager.LoadScene(scenename);
    }
}

public class Music
{
    public string music;
}