using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GamePause : MonoBehaviour
{
    private CoinCounter coinCounter;
    private VisualElement pauseMenu;
    private VisualElement endMenu;
    private VisualElement resume;
    private List<VisualElement> quits;
    private Label coinValue;
    private Label coinTarget;
    private bool gameEnded;
    void Start()
    {
        UIDocument pauseDoc = GetComponent<UIDocument>();
        coinCounter = GameObject.FindGameObjectWithTag("MainUI").GetComponent<CoinCounter>();
        pauseMenu = pauseDoc.rootVisualElement.Q("PauseMenu");
        endMenu = pauseDoc.rootVisualElement.Q("EndMenu");
        resume = pauseDoc.rootVisualElement.Q("Resume");
        quits = pauseDoc.rootVisualElement.Query<VisualElement>("Quit").ToList();
        coinTarget = pauseDoc.rootVisualElement.Q<Label>("CoinTarget");
        coinValue = pauseDoc.rootVisualElement.Q<Label>("CoinValue");

        resume.RegisterCallback<ClickEvent>(onResumeClick);
        foreach (var el in quits)
        {
            el.RegisterCallback<ClickEvent>(onQuitClick);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !gameEnded)
        {
            PauseGame();
        }
    }

    void PauseGame()
    {
        if(Time.timeScale == 0)
        {
            pauseMenu.style.display = DisplayStyle.None;
            pauseMenu.SetEnabled(false);
            if (endMenu.style.display == DisplayStyle.Flex)
            {
                endMenu.style.display = DisplayStyle.None;
            }
            Time.timeScale = 1;
        } else {
            pauseMenu.SetEnabled(true);
            pauseMenu.style.display = DisplayStyle.Flex;
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

    public void EndGameSequence()
    {
        int[] coinData = coinCounter.getFinalScore();
        coinValue.text = coinData[0].ToString();
        coinTarget.text = coinData[1].ToString();
        endMenu.style.display = DisplayStyle.Flex;
    }

}
