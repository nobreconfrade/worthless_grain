using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private float timeDuration = 5f * 60f;
    public float timer;
    private TextMeshProUGUI timerText;
    void Start()
    {
        timerText = GetComponent<TextMeshProUGUI>();
        timer = timeDuration;
    }
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            UpdateTimerDisplay(timer);
        } else {
            timer = 0;
            UpdateTimerDisplay(0);
        }

    }
    void UpdateTimerDisplay(float time)
    {
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);

        string currentTime = string.Format("{00:00}:{1:00}", minutes, seconds);
        timerText.text = currentTime;
    }
}
