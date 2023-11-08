using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private float timeDuration = 5f * 60f;
    public float timer;
    private bool gameOver;
    private TextMeshProUGUI timerText;
    private GamePause gamePause;
    void Start()
    {
        timerText = GetComponent<TextMeshProUGUI>();
        timer = timeDuration;
        gamePause = GameObject.FindGameObjectWithTag("PauseMenu").GetComponent<GamePause>();
    }
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            UpdateTimerDisplay(timer);
        } else if (timer <= 0 && !gameOver) {
            timer = 0;
            UpdateTimerDisplay(0);
            gameOver = true;
            StartCoroutine(EndSequence());
        }

    }
    void UpdateTimerDisplay(float time)
    {
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);

        string currentTime = string.Format("{00:00}:{1:00}", minutes, seconds);
        timerText.text = currentTime;
    }

    IEnumerator EndSequence()
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(3f);
        Debug.Log("past 3 seconds, initialize endgamesequence");
        gamePause.EndGameSequence();
    }
}
