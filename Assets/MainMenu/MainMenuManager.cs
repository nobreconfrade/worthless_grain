using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuManager : MonoBehaviour
{
    private VisualElement level1;
    private VisualElement quit;
    private UIDocument mainMenu;
    void Start()
    {
        mainMenu = GetComponent<UIDocument>();
        level1 = mainMenu.rootVisualElement.Q("Level1");
        quit = mainMenu.rootVisualElement.Q("Quit");

        level1.RegisterCallback<ClickEvent>(onLevel1Click);
        quit.RegisterCallback<ClickEvent>(onQuitClick);
    }

    void onLevel1Click(ClickEvent clk)
    {
        SceneManager.LoadScene("Dinner");
    }
    void onQuitClick(ClickEvent clk)
    {
        Application.Quit();
    }
}
