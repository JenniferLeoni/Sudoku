using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadingTextLoginScene : MonoBehaviour
{
    [SerializeField] TMP_Text loadingText;
    [SerializeField] LoginSceneUIManager loginSceneUIManager;
    List<string> loadingTextSequence = new List<string> { "-", "\\", "|", "/" };
    int currentLoadingText = 0;
    private IEnumerator textFadeOutCoroutine;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void PlayAnimation()
    {
        gameObject.SetActive(true);
        loadingText.alpha = 1;
        if (textFadeOutCoroutine != null)
        {
            StopCoroutine(textFadeOutCoroutine);
        }
        InvokeRepeating("AnimationLoop", 0, 0.2f);
    }

    public void StopAnimation()
    {
        CancelInvoke("AnimationLoop");
        loadingText.text = "";
    }

    void AnimationLoop()
    {
        loadingText.text = loadingTextSequence[currentLoadingText];
        currentLoadingText++;

        if (currentLoadingText >= loadingTextSequence.Count)
        {
            currentLoadingText = 0;
        }
    }

    public void ChangeTextThenFadeOut(long responseCode, string teamname)
    {
        switch (responseCode)
        {
            case 200:
                loadingText.text = "Welcome, " + teamname;
                break;
            case 404:
                loadingText.text = "Wrong teamname or password";
                break;
            case 400:
                loadingText.text = "Teamname missing";
                break;
            case 405:
                loadingText.text = "Wrong Method";
                break;
            case 403:
                loadingText.text = "ID Error, please go back to Roulette";
                loginSceneUIManager.SetVerificationToFailed();
                break;
            case 599:
                loadingText.text = "SQL Server Down. Try again later.";
                break;
            default:
                loadingText.text = "Connection Error";
                break;
        }

        if (responseCode != 200)
        {
            if (responseCode != 403)
            {
                textFadeOutCoroutine = TextFadeOut();
                StartCoroutine(textFadeOutCoroutine);
            }
        }
    }

    IEnumerator TextFadeOut()
    {
        yield return new WaitForSeconds(3f);
        while (loadingText.alpha > 0)
        {
            loadingText.alpha -= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        yield break;
    }
}
