using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public MenuManager menuManager;
    public string[] categories;
    public GameObject categoryButtonPrefab;
    private List<Button> menuButtons;

    // Start is called before the first frame update
    void Start()
    {
        menuButtons = new List<Button>();

        for (int i = 0; i < categories.Length; i++)
        {
            GameObject createdButton = Instantiate(categoryButtonPrefab, transform);
            createdButton.GetComponentInChildren<Text>().text = categories[i];
            createdButton.name = categories[i];
            Button button = createdButton.GetComponent<Button>();
            menuButtons.Add(button);
            button.onClick.AddListener(() => SwitchMenu(button));
        }

        RectTransform rt = gameObject.transform.parent.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(categoryButtonPrefab.GetComponent<RectTransform>().sizeDelta.x * categories.Length + 5.0f * (categories.Length - 1), rt.sizeDelta.y);
    }

    void SwitchMenu(Button button)
    {
        menuManager.SubMenu(button.name);
    }
}
