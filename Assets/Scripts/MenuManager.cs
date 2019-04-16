using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public string currMenu;
    public GameObject[] menus; // 0 - Main menu, 1 - Sub menu. Order is important
    public GameObject backButton;

    // Start is called before the first frame update
    void Start()
    {
        MainMenu();
    }

    public void MainMenu()
    {
        Reset();
        menus[0].transform.parent.gameObject.SetActive(true);
        backButton.SetActive(false);
    }

    public void SubMenu(string menu)
    {
        Reset();
        currMenu = menu;
        menus[1].transform.parent.gameObject.SetActive(true);
        menus[1].GetComponentInChildren<SubMenu>().init(currMenu);
        backButton.SetActive(true);
    }

    private void Reset()
    {
        foreach (GameObject menu in menus)
        {
            menu.transform.parent.gameObject.SetActive(false);
        }
        currMenu = "Main";
    }
}
