using System;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class SubMenu : MonoBehaviour
{
    public MenuManager menuManager;
    public Placement interaction;
    public GameObject buttonPrefab;
    private string menuName;

    private Button[] assetButtons = new Button[0];

    public void init(string currMenu)
    {
        menuName = currMenu;

        // disable all furniture button on startup
        //disableAllButtons();

        // delete existing buttons
        deleteAllButtons();

        // clear all selected buttons
        //Reset();

        GameObject[] assetPrefab = Resources.LoadAll<GameObject>(currMenu);
        assetButtons = new Button[assetPrefab.Length];

        // replace text with name of furniture prefab in Resources folder
        for (int i = 0; i < assetPrefab.Length; i++)
        {

            // instantiate a new button based on prefabs loaded from Resources folder
            GameObject createdButton = Instantiate(buttonPrefab, transform);
            assetButtons[i] = createdButton.GetComponent<Button>();

            assetButtons[i].gameObject.name = assetPrefab[i].name;
            createdButton.GetComponentInChildren<Image>().enabled = false;
            //createdButton.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>(currMenu + "/" + createdButton.name);

            // instantiate gameobject for SS
            GameObject newObject = Instantiate(assetPrefab[i], createdButton.transform);
            newObject.tag = "3DButton";
            changeToUiLayer(newObject); // UI Layer
            updateTransform(newObject);

            assetButtons[i].GetComponentInChildren<Text>().text = assetPrefab[i].name;
            assetButtons[i].GetComponentInChildren<Text>().color = Color.white;

            assetButtons[i].onClick.AddListener(() => SetActivePrefab(createdButton.GetComponent<Button>()));

        }

        RectTransform rt = gameObject.transform.parent.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(buttonPrefab.GetComponent<RectTransform>().sizeDelta.x * assetPrefab.Length + 5.0f * (assetPrefab.Length-1), rt.sizeDelta.y);
    }

    private void updateTransform(GameObject newObject)
    {
        switch(menuName)
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

    void SetActivePrefab(Button furnitureButton)
    {
        //Reset();
        //furnitureButton.GetComponent<Image>().color = Color.grey;
        interaction.SetObjectToPlace(Resources.Load<GameObject>(menuName + "/" + furnitureButton.name));
        interaction.PlaceObject();
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
