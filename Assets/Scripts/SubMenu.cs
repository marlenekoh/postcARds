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
        Reset();

        GameObject[] assetPrefab = Resources.LoadAll<GameObject>(currMenu);
        assetButtons = new Button[assetPrefab.Length];

        // replace text with name of furniture prefab in Resources folder
        for (int i = 0; i < assetPrefab.Length; i++)
        {

            // instantiate a new button based on prefabs loaded from Resources folder
            GameObject createdButton = Instantiate(buttonPrefab, transform);
            assetButtons[i] = createdButton.GetComponent<Button>();

            assetButtons[i].gameObject.name = assetPrefab[i].name;
            createdButton.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>(currMenu + "/" + createdButton.name);
            assetButtons[i].GetComponentInChildren<Text>().text = assetPrefab[i].name;

            assetButtons[i].onClick.AddListener(() => SetActivePrefab(createdButton.GetComponent<Button>()));

        }

        RectTransform rt = gameObject.transform.parent.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(buttonPrefab.GetComponent<RectTransform>().sizeDelta.x * assetPrefab.Length + 5.0f * (assetPrefab.Length-1), rt.sizeDelta.y);
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
        Reset();
        //furnitureButton.GetComponent<Image>().color = Color.grey;
        interaction.SetObjectToPlace(Resources.Load<GameObject>(menuName + "/" + furnitureButton.name));
        interaction.PlaceObject();
    }

    private void Reset()
    {
        if (assetButtons.Length > 0)
        {
            foreach (Button button in assetButtons)
            {
                //button.GetComponent<Image>().color = Color.white;
            }
        }
    }
}
