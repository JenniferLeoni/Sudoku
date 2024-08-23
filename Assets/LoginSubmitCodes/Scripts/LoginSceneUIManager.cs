using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginSceneUIManager : MonoBehaviour
{
    // Login Form
    [Header("Login Form")]
    [SerializeField] private GameObject loginFormGameObjects;
    [SerializeField] private TMP_Text verifyingIdentityText;
    [SerializeField] private Button loginFormButton;
    [SerializeField] private TMP_Text loginFormButtonText;
    [SerializeField] private LoadingTextLoginScene loadingTextAnimation;
    [SerializeField] private GetUserLogin getUserLoginScript;
    [SerializeField] private RetryButtonChangeColor retryButtonChangeColorScript;
    [SerializeField] private string rouletteLink;

    // Overlay
    [Header("Animators")]
    [SerializeField] private Animator loginAnimator;
    [SerializeField] private Animator blackOverlayAnimator;

    // SFX
    [Header("Sounds")]
    [SerializeField] private AudioSource bgmAudioSource;
    [SerializeField] private AudioSource sfxAudioSource;
    [SerializeField] private AudioClip buttonHoverSFX;
    [SerializeField] private AudioClip selectSFX;
    [SerializeField] private AudioClip typingSFX;
    [SerializeField] private AudioClip receiveRespondSFX;

    // PAM
    public Button LoginFormButton { get => loginFormButton; set => loginFormButton = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Login Form
    public void LoginFormGameObjectsSetActive(bool value) {
        loginFormGameObjects.SetActive(value);
    }

    public void SetVerificationToFailed() {
        verifyingIdentityText.text = "Verification Failed";
        loginFormButton.onClick.RemoveListener(() => getUserLoginScript.FetchInfoOfUsername());
        loginFormButton.onClick.AddListener(() => {
            OpenLinks.OpenURL(rouletteLink);
        });
        loginFormButtonText.text = "Back To Roulette";
        loginFormButton.interactable = true;
        retryButtonChangeColorScript.ChangeSubmitButtonImageSpriteToBorder();
    }

    public void PlayLoadingAnimation(bool value) {
        if (value) {
            loadingTextAnimation.PlayAnimation();
        } else {
            loadingTextAnimation.StopAnimation();
        }   
    }

    public void ChangeLoadingText(long responseCode, string teamname) {
        PlayReceiveRespondSFX();
        loadingTextAnimation.ChangeTextThenFadeOut(responseCode, teamname);
    }

    // Animation
    public void LoginTrigger() {
        loginAnimator.SetTrigger("FadeOut");
    }
    public void BlackOverlayTrigger() {
        blackOverlayAnimator.SetTrigger("FadeIn");
    }
    public void PlayAnimationThenChangeScene() {
        Invoke("LoginTrigger", 2);
        Invoke("BlackOverlayTrigger", 2);
        Invoke("AudioFadeOut", 2);
        StartCoroutine(WaitThenLoadScene(2));
    }
    IEnumerator WaitThenLoadScene(float additionalDelay) {
        yield return new WaitForSeconds(2 + additionalDelay);
        SceneManager.LoadScene(1);
        yield break;
    }

    // SFX
    public void AudioFadeOut() {
        StartCoroutine(AllAudioFadeOut());
    }
    IEnumerator AllAudioFadeOut() {
        while (bgmAudioSource.volume > 0 ||
        sfxAudioSource.volume > 0) {
            bgmAudioSource.volume -= 0.005f;
            sfxAudioSource.volume -= 0.005f;
            yield return null;
        }
    }
    public void PlayButtonHoverSFX() {
        sfxAudioSource.PlayOneShot(buttonHoverSFX);
    }

    public void PlaySelectSFX() {
        sfxAudioSource.PlayOneShot(selectSFX);
    }

    public void PlayReceiveRespondSFX() {
        sfxAudioSource.PlayOneShot(receiveRespondSFX);
    }
}
