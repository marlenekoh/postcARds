﻿using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using UnityEngine.UI;

public class SubMenu : MonoBehaviour
{
    public MenuManager menuManager;
    public Placement interaction;
    public GameObject buttonPrefab;
    private string menuName;
    private float originalWidth;

    private Button[] assetButtons = new Button[0];

    private void Start()
    {
        if (Session.cardName == "rudolph")
        {
            interaction = GameObject.Find("RudolphTarget").GetComponentInChildren<Placement>();
        } else
        {
            interaction = GameObject.Find("SantaTarget").GetComponentInChildren<Placement>();
        }
        RectTransform rt = gameObject.transform.parent.GetComponent<RectTransform>();
        originalWidth = rt.sizeDelta.x;
    }
    

    public void init(string currMenu)
    {
        menuName = currMenu;

        // disable all furniture button on startup
        //disableAllButtons();

        // delete existing buttons
        deleteAllButtons();

        // clear all selected buttons
        //Reset();

        if (currMenu == "Music")
        {
            AudioClip[] assetPrefab = Resources.LoadAll<AudioClip>(currMenu);
            assetButtons = new Button[assetPrefab.Length];

            for (int i = 0; i < assetPrefab.Length; i++)
            {
                if (assetPrefab[i].name == "None") // set for none first
                {
                    setMusic(assetPrefab, i, currMenu);
                }
            }

            // replace text with name of furniture prefab in Resources folder
            for (int i = 0; i < assetPrefab.Length; i++)
            {
                if (assetPrefab[i].name == "None") // set for none first
                {
                    continue;
                }
                setMusic(assetPrefab, i, currMenu);
            }
        }
        else
        {
            GameObject[] assetPrefab = Resources.LoadAll<GameObject>(currMenu);
            assetButtons = new Button[assetPrefab.Length];


            // replace text with name of furniture prefab in Resources folder
            for (int i = 0; i < assetPrefab.Length; i++)
            {

                // instantiate a new button based on prefabs loaded from Resources folder
                GameObject createdButton = Instantiate(buttonPrefab, transform);
                assetButtons[i] = createdButton.GetComponent<Button>();

                assetButtons[i].gameObject.name = assetPrefab[i].name;
                //createdButton.GetComponentInChildren<Image>().enabled = false;
                //createdButton.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>(currMenu + "/" + createdButton.name);
                Texture2D assetPreview = AssetPreview.GetAssetPreview(assetPrefab[i]);
                while ((assetPreview = AssetPreview.GetAssetPreview(assetPrefab[i])) == null)
                {
                    assetPreview = AssetPreview.GetAssetPreview(assetPrefab[i]);
                    System.Threading.Thread.Sleep(10);
                }
                Rect rec = new Rect(0, 0, assetPreview.width, assetPreview.height);
                Sprite.Create(assetPreview, rec, new Vector2(0, 0), 1);
                createdButton.GetComponentInChildren<Image>().sprite = Sprite.Create(assetPreview, rec, new Vector2(0, 0), .01f);

                assetButtons[i].GetComponentInChildren<Text>().text = assetPrefab[i].name;
                assetButtons[i].GetComponentInChildren<Text>().color = Color.white;
                
                assetButtons[i].onClick.AddListener(() => SetActivePrefab(createdButton.GetComponent<Button>(), currMenu));

            }

            //RectTransform rt = gameObject.transform.parent.GetComponent<RectTransform>();
            //rt.sizeDelta = new Vector2(Math.Max(originalWidth, buttonPrefab.GetComponent<RectTransform>().sizeDelta.x * assetPrefab.Length + 5.0f * (assetPrefab.Length - 1)), rt.sizeDelta.y);
        }
    }

    void setMusic(AudioClip[] assetPrefab, int i, string currMenu)
    {
        // instantiate a new button based on prefabs loaded from Resources folder
        GameObject createdButton = Instantiate(buttonPrefab, transform);
        assetButtons[i] = createdButton.GetComponent<Button>();

        assetButtons[i].gameObject.name = assetPrefab[i].name;
        //createdButton.GetComponentInChildren<Image>().enabled = false;
        //createdButton.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>(currMenu + "/" + createdButton.name);
        Texture2D assetPreview = AssetPreview.GetAssetPreview(assetPrefab[i]);
        while ((assetPreview = AssetPreview.GetAssetPreview(assetPrefab[i])) == null)
        {
            assetPreview = AssetPreview.GetAssetPreview(assetPrefab[i]);
            System.Threading.Thread.Sleep(10);
        }
        Rect rec = new Rect(0, 0, assetPreview.width, assetPreview.height);
        Sprite.Create(assetPreview, rec, new Vector2(0, 0), 1);
        createdButton.GetComponentInChildren<Image>().sprite = Sprite.Create(assetPreview, rec, new Vector2(0, 0), .01f);

        assetButtons[i].GetComponentInChildren<Text>().text = assetPrefab[i].name;
        assetButtons[i].GetComponentInChildren<Text>().color = Color.white;

        assetButtons[i].onClick.AddListener(() => SetActivePrefab(createdButton.GetComponent<Button>(), currMenu));
    }

    void deleteAllButtons()
    {
        if (assetButtons.Length > 0)
        {
            foreach (Button button in assetButtons)
            {
                Destroy(button.gameObject);
            }
        }
    }

    void disableAllButtons()
    {
        foreach (Button button in assetButtons)
        {
            if (button.tag == "FurnitureButton")
            {
                button.gameObject.SetActive(false);
            }
        }
    }

    void SetActivePrefab(Button furnitureButton, string currMenu)
    {
        //Reset();
        //furnitureButton.GetComponent<Image>().color = Color.grey;

        if (currMenu == "Music")
        {
            interaction.PlayMusic(furnitureButton.name);
        }
        else
        {
            interaction.SetObjectToPlace(Resources.Load<GameObject>(menuName + "/" + furnitureButton.name));
            interaction.PlaceObject();
        }
    }

    void changeToUiLayer(GameObject obj)
    {
        foreach(Transform child in obj.GetComponentsInChildren<Transform>())
        {
            child.gameObject.layer = 5;
            Animation anim = child.gameObject.GetComponent<Animation>();
            if (anim != null)
            {
                anim.Stop(); // to Update
            }
        }
    }

    //private void Reset()
    //{
    //    if (assetButtons.Length > 0)
    //    {
    //        foreach (Button button in assetButtons)
    //        {
    //              button.GetComponent<Image>().color = Color.white;
    //        }
    //    }
    //}
}
