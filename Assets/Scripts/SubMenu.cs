﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubMenu : MonoBehaviour
{
    public MenuManager menuManager;
    public Placement interaction;
    public GameObject buttonPrefab;
    public Camera uicamera;
    public GameObject selectedMusic = null;
    private GameObject noneMusic;
    private string menuName;
    private float originalWidth;

    private Button[] assetButtons = new Button[0];

    private void Start()
    {
        if (Session.cardName == "rudolph")
        {
            interaction = GameObject.Find("RudolphTarget").GetComponentInChildren<Placement>();
        }
        else
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
                    setMusic(assetPrefab, i, currMenu, true);
                }
            }

            // replace text with name of furniture prefab in Resources folder
            for (int i = 0; i < assetPrefab.Length; i++)
            {
                if (assetPrefab[i].name == "None") // set for none first
                {
                    continue;
                }
                setMusic(assetPrefab, i, currMenu, false);
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
                assetButtons[i].GetComponentInChildren<Text>().text = assetPrefab[i].name;
                assetButtons[i].GetComponentInChildren<Text>().color = Color.white;

                createdButton.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>(currMenu + "/" + createdButton.name);

                assetButtons[i].onClick.AddListener(() => SetActivePrefab(createdButton.GetComponent<Button>(), currMenu));

            }

        }
    }

    void setMusic(AudioClip[] assetPrefab, int i, string currMenu, bool isNone)
    {
        // instantiate a new button based on p,refabs loaded from Resources folder
        GameObject createdButton = Instantiate(buttonPrefab, transform);

        string path = "Music/music";
        if (isNone)
        {
            path = "Music/none";
            noneMusic = createdButton;
        }
        
        if (assetPrefab[i].name == Session.music)
        {
            createdButton.GetComponent<Button>().GetComponentInChildren<Image>().color = Color.gray;
            selectedMusic = createdButton;
        }
        else
        {
            createdButton.GetComponent<Button>().GetComponentInChildren<Image>().color = Color.white;
        }

        //if (selectedMusic != null)
        //{
        //    selectedMusic.GetComponent<Button>().GetComponentInChildren<Image>().color = Color.gray;
        //}

        createdButton.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>(path);

        assetButtons[i] = createdButton.GetComponent<Button>();
        assetButtons[i].gameObject.name = assetPrefab[i].name;
        assetButtons[i].GetComponentInChildren<Text>().text = assetPrefab[i].name;
        assetButtons[i].GetComponentInChildren<Text>().color = Color.white;
        assetButtons[i].onClick.AddListener(() => SetActivePrefab(createdButton.GetComponent<Button>(), currMenu));
        Debug.Log(createdButton.name);

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
            noneMusic.GetComponentInChildren<Image>().color = Color.white;
            selectedMusic.GetComponentInChildren<Image>().color = Color.white;
            if (furnitureButton.name.Equals(Session.music))
            {
                selectedMusic = furnitureButton.gameObject;
                selectedMusic.GetComponentInChildren<Image>().color = Color.gray;
            } 


            //if (selectedMusic != null)
            //{
            //    selectedMusic.GetComponentInChildren<Image>().color = Color.white;
            //}
            //selectedMusic = furnitureButton.gameObject;
            //selectedMusic.GetComponentInChildren<Image>().color = Color.gray;

        }
        else
        {
            interaction.SetObjectToPlace(Resources.Load<GameObject>(menuName + "/" + furnitureButton.name));
            interaction.PlaceObject();
        }
    }

    public void unselectMusic()
    {
        if (selectedMusic != null)
        {
            selectedMusic.GetComponentInChildren<Image>().color = Color.white;
        }
        noneMusic.GetComponentInChildren<Image>().color = Color.gray;
    }

    void changeToUiLayer(GameObject obj)
    {
        foreach (Transform child in obj.GetComponentsInChildren<Transform>())
        {
            child.gameObject.layer = 5;
            Animation anim = child.gameObject.GetComponent<Animation>();
            if (anim != null)
            {
                anim.Stop(); // to Update
            }
        }
    }

    private void updateTransform(GameObject newObject)
    {
        switch (menuName)
        {
            case "Food":
            default: // TO UPDATE
                Vector3 scale = newObject.transform.localScale;
                scale = scale * 80;
                newObject.transform.localScale = scale;
                Vector3 pos = newObject.transform.position;
                pos = new Vector3(pos.x + 40.0f, pos.y + 15.0f, pos.z);
                newObject.transform.position = pos;
                break;
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
