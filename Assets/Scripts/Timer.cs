using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public float currentTime = 0f;
    [SerializeField] public float startingTime = 800f;
    [SerializeField] public TMP_Text countdownText;
    [SerializeField] public TMP_Text mistakesText;

    void Start()
    {
        currentTime = startingTime;
    }
    void Update()
    {
        currentTime -= 1 * Time.deltaTime;
        PlayerPrefs.SetInt("timerCountdown", (int)currentTime);
        // countdownText.text = currentTime.ToString("Time: 0");

        int minutes = ((int)currentTime / 60);
        int seconds = ((int)currentTime % 60);
        int mistakes = PlayerPrefs.GetInt("mistakesValue");
        countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        mistakesText.text = string.Format("chances: {0}/5", mistakes);


        if (currentTime <= 0)
        {
            currentTime = 0;
            SceneManager.LoadScene("EndScene");
        }
    }
}