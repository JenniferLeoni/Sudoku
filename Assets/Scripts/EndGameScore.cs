using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EndGameScore : MonoBehaviour
{
    public int score = 0;
    [SerializeField] public TMP_Text scoreText;
    [SerializeField] ResultUIManager resultUIManagerScript;

    public void TriggerGameWin()
    {
        resultUIManagerScript.IsGameWin = true;
        resultUIManagerScript.GameScore = score;
        resultUIManagerScript.ShowSubmittingUI();
    }

    public void TriggerGameLose()
    {
        resultUIManagerScript.IsGameWin = false;
        resultUIManagerScript.GameScore = score;
        resultUIManagerScript.ShowSubmittingUI();
    }

    // Start is called before the first frame update
    void Start()
    {
        score = PlayerPrefs.GetInt("scoreValue");
        if (PlayerPrefs.GetInt("mistakesValue") <= 5)
        {
            int timeRemaining = PlayerPrefs.GetInt("timerCountdown");
            score += (int)(timeRemaining * 0.5);
            TriggerGameWin();
        }
        else
        {
            score = 0;
            TriggerGameLose();
        }
        scoreText.text = string.Format("Score: {0}", score);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
