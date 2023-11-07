using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GamePause : MonoBehaviour
{
    private VisualElement pauseMenu;
    private VisualElement resume;
    private VisualElement quit;
    void Start()
    {
        UIDocument pauseDoc = GetComponent<UIDocument>();
        pauseMenu = pauseDoc.rootVisualElement.Q("PauseMenu");
        resume = pauseDoc.rootVisualElement.Q("Resume");
        quit = pauseDoc.rootVisualElement.Q("Quit");

        resume.RegisterCallback<ClickEvent>(onResumeClick);
        quit.RegisterCallback<ClickEvent>(onQuitClick);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    void PauseGame()
    {
        if(Time.timeScale == 0)
        {
            pauseMenu.style.visibility = Visibility.Hidden;
            pauseMenu.SetEnabled(false);
            Time.timeScale = 1;
        } else {
            pauseMenu.SetEnabled(true);
            pauseMenu.style.visibility = Visibility.Visible;
            Time.timeScale = 0;
        }
    }

    void onResumeClick(ClickEvent clk)
    {
        PauseGame();
    }
    void onQuitClick(ClickEvent clk)
    {
        PauseGame();
        SceneManager.LoadScene("MainMenu");
    }

}
