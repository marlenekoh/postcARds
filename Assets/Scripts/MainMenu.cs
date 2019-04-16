using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public MenuManager menuManager;
    public string[] categories;
    public GameObject categoryButtonPrefab;
    public Image background;
    private List<Button> menuButtons;

    // Start is called before the first frame update
    void Start()
    {
        menuButtons = new List<Button>();

        for (int i = 0; i < categories.Length; i++)
        {
            GameObject createdButton = Instantiate(categoryButtonPrefab, transform);
            createdButton.GetComponentInChildren<Text>().text = categories[i];
            GameObject[] assets = Resources.LoadAll<GameObject>(categories[i]);
            Texture2D assetPreview = AssetPreview.GetAssetPreview(assets[0]);
            while (assetPreview == null)
            {
                assetPreview = AssetPreview.GetAssetPreview(assets[0]);
                System.Threading.Thread.Sleep(10);
            }
            Rect rec = new Rect(0, 0, assetPreview.width, assetPreview.height);
            Sprite.Create(assetPreview, rec, new Vector2(0, 0), 1);
            createdButton.GetComponentInChildren<Image>().sprite = Sprite.Create(assetPreview, rec, new Vector2(0, 0), .01f);
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

    private void GenerateSprites()
    {
        foreach (string currMenu in categories)
        {
            GameObject[] assetPrefab = Resources.LoadAll<GameObject>(currMenu);
            for (int i = 0; i < assetPrefab.Length; i++)
            {
                Texture2D assetPreview = AssetPreview.GetAssetPreview(assetPrefab[i]);
                while (assetPreview == null)
                {
                    assetPreview = AssetPreview.GetAssetPreview(assetPrefab[i]);
                    System.Threading.Thread.Sleep(10);
                }
            }
        }

    }
}
