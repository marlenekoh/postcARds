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
            createdButton.SetActive(true);
            createdButton.name = categories[i];
            Button button = createdButton.GetComponent<Button>();
            menuButtons.Add(button);

            Sprite[] sprites = Resources.LoadAll<Sprite>(categories[i]);
            if (sprites.Length > 0)
            {
                createdButton.GetComponentInChildren<Image>().sprite = sprites[0];
                createdButton.GetComponentInChildren<Image>().color = Color.white;
            }

            button.onClick.AddListener(() => SwitchMenu(button));
        }

    }

    void SwitchMenu(Button button)
    {
        menuManager.SubMenu(button.name);
    }
}
