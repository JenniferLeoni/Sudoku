using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class ResultRetryButtonChangeColor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Button retryButton;
    [SerializeField] Image retryButtonImage;
    [SerializeField] TMP_Text retryText;
    [SerializeField] List<Sprite> buttonBGSprites;

    [SerializeField] ResultUIManager resultUIManager;

    // Start is called before the first frame update
    void Start()
    {
        ChangeSubmitButtonImageSpriteToBorder();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ChangeButtonToHovered();

        // SFX
        if (retryButton.IsInteractable())
        {
            resultUIManager.PlayButtonHoverSFX();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Change sprite to border only when enabled
        if (retryButton.IsInteractable())
        {
            retryButtonImage.sprite = buttonBGSprites[0];
            retryText.color = resultUIManager.PrimaryColor;
        }
    }

    public void ChangeSubmitButtonImageSpriteToBorder()
    {
        retryButtonImage.sprite = buttonBGSprites[0];
        retryText.color = resultUIManager.PrimaryColor;
    }

    public void ChangeButtonToHovered() {
        retryButtonImage.sprite = buttonBGSprites[1];
        retryText.color = resultUIManager.BackgroundColor;
    }
}
