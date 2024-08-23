using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ResultUIManager : MonoBehaviour
{
    private bool isGameWin; // TODO: Jangan lupa set isGameWin ketika game berakhir
    private float gameScore; // TODO: Jangan lupa set gameScore ketika game berakhir
    public bool IsGameWin { get => isGameWin; set => isGameWin = value; }
    public float GameScore { get => gameScore; set => gameScore = value; }

    private bool hasShowedSubmittingUI;

    public uploadScore uploadScoreScript;

    // Submit
    [Header("Retry Submit Button")]
    [SerializeField] private Button retrySubmitScoreButton;
    [SerializeField] private ResultRetryButtonChangeColor resultRetryButtonChangeColorScript;
    public Button RetrySubmitScoreButton { get => retrySubmitScoreButton; set => retrySubmitScoreButton = value; }
    public ResultRetryButtonChangeColor RetryButtonChangeColorScript { get => resultRetryButtonChangeColorScript; set => resultRetryButtonChangeColorScript = value; }

    // UI
    [Header("UI")]
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject gameWinUI;
    [SerializeField] private GameObject submittingUI;
    [SerializeField] private LoadingTextSubmitScene loadingTextSubmitScene;

    // Overlay
    [Header("Animators")]
    [SerializeField] private Animator resultAnimator;

    // SFX
    [Header("Sounds")]
    [SerializeField] private AudioSource sfxAudioSource;
    [SerializeField] private AudioClip buttonHoverSFX;
    [SerializeField] private AudioClip selectSFX;
    [SerializeField] private AudioClip typingSFX;
    [SerializeField] private AudioClip receiveRespondSFX;

    // Color
    [Header("Color")]
    [SerializeField] private Color32 backgroundColor;
    [SerializeField] private Color32 primaryColor;
    public Color32 PrimaryColor { get => primaryColor; set => primaryColor = value; }
    public Color32 BackgroundColor { get => backgroundColor; set => backgroundColor = value; }

    // UI Color Change
    [Header("UI Color Change Targets (Jangan diubah)")]
    [SerializeField] private Image backgroundImage;
    [SerializeField] private List<Image> resultButtonImages;
    [SerializeField] private List<TMP_Text> resultTexts;

    // Start is called before the first frame update
    void Start()
    {
        // Ubah warna sesuai theme
        backgroundImage.color = BackgroundColor;

        foreach (Image image in resultButtonImages)
        {
            image.color = PrimaryColor;
        }
        foreach (TMP_Text text in resultTexts)
        {
            text.color = PrimaryColor;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void HideAllUI()
    {
        gameOverUI.SetActive(false);
        gameWinUI.SetActive(false);
        submittingUI.SetActive(false);
    }

    public void ShowSubmittingUI()
    {
        if (!hasShowedSubmittingUI)
        {
            submittingUI.SetActive(true);
            resultAnimator.SetTrigger("SubmittingTrigger");
            // TODO: Mulai proses upload
            retrySubmitScoreButton.interactable = false;
            loadingTextSubmitScene.PlayAnimation();
            uploadScoreScript.UploadScore(Mathf.RoundToInt(gameScore).ToString(), isGameWin);
            hasShowedSubmittingUI = true;
        }
    }

    public void RetrySubmit()
    {
        retrySubmitScoreButton.interactable = false;
        loadingTextSubmitScene.PlayAnimation();
        uploadScoreScript.UploadScore(Mathf.RoundToInt(gameScore).ToString(), isGameWin);
    }

    public void ShowResultUI()
    {
        if (IsGameWin)
        {
            gameWinUI.SetActive(true);
            WinTrigger();
        }
        else
        {
            gameOverUI.SetActive(true);
            LoseTrigger();
        }
    }

    public void HideResultUI()
    {
        if (IsGameWin)
        {
            gameWinUI.SetActive(false);
        }
        else
        {
            gameOverUI.SetActive(false);
        }
    }

    public void PlayLoadingAnimation(bool value)
    {
        if (value)
        {
            loadingTextSubmitScene.PlayAnimation();
        }
        else
        {
            loadingTextSubmitScene.StopAnimation();
        }
    }

    public void ChangeLoadingText(long responseCode)
    {
        if (responseCode != 200) {
            retrySubmitScoreButton.interactable = true;
        }
        PlayReceiveRespondSFX();
        loadingTextSubmitScene.ChangeTextThenFadeOut(responseCode);
    }

    // Animation
    public void WinTrigger()
    {
        resultAnimator.SetTrigger("WinTrigger");
    }

    public void LoseTrigger()
    {
        resultAnimator.SetTrigger("LoseTrigger");
    }

    // public void PlayAnimationThenChangeScene() {
    //     Invoke("LoginTrigger", 2);
    //     Invoke("BlackOverlayTrigger", 2);
    //     Invoke("AudioFadeOut", 2);
    //     StartCoroutine(WaitThenLoadScene(2));
    // }
    // IEnumerator WaitThenLoadScene(float additionalDelay) {
    //     yield return new WaitForSeconds(2 + additionalDelay);
    //     SceneManager.LoadScene(1);
    //     yield break;
    // }

    // SFX
    public void PlayButtonHoverSFX()
    {
        sfxAudioSource.PlayOneShot(buttonHoverSFX);
    }

    public void PlaySelectSFX()
    {
        sfxAudioSource.PlayOneShot(selectSFX);
    }

    public void PlayReceiveRespondSFX()
    {
        sfxAudioSource.PlayOneShot(receiveRespondSFX);
    }
}
