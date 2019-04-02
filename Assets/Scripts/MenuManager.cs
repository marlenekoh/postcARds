using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public string currMenu;
    public GameObject[] menus; // 0 - Main menu, 1 - Sub menu. Order is important
    public GameObject categoryButton;

    // Start is called before the first frame update
    void Start()
    {
        MainMenu();
    }

    public void MainMenu()
    {
        Reset();
        menus[0].SetActive(true);
        categoryButton.SetActive(false);
    }

    public void SubMenu(string menu)
    {
        Reset();
        currMenu = menu;
        menus[1].SetActive(true);
        menus[1].GetComponent<SubMenu>().init(currMenu);
        categoryButton.SetActive(true);
    }

    private void Reset()
    {
        foreach (GameObject menu in menus)
        {
            menu.SetActive(false);
        }
        currMenu = "Main";
    }
}
