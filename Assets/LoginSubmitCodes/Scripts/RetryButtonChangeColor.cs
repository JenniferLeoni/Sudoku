using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class RetryButtonChangeColor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Button retryButton;
    [SerializeField] Image retryButtonImage;
    [SerializeField] TMP_Text retryText;
    [SerializeField] List<Sprite> buttonBGSprites;

    [SerializeField] LoginSceneUIManager loginSceneUIManager;

    // Start is called before the first frame update
    void Start()
    {
        // retryButtonImage.sprite = buttonBGSprites[0];
        // // submitText.color = new Color32(0x05, 0xFD, 0x47, 0xFF); // Equivalent to 05FD47
        // retryText.color = new Color32(0x00, 0xB7, 0xFA, 0xFF); // Equivalent to 00B7FA
        ChangeButtonToHovered();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ChangeButtonToHovered();

        // SFX
        if (retryButton.IsInteractable())
        {
            loginSceneUIManager.PlayButtonHoverSFX();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Change sprite to border only when enabled
        if (retryButton.IsInteractable())
        {
            retryButtonImage.sprite = buttonBGSprites[0];
            retryText.color = new Color32(0x00, 0xB7, 0xFA, 0xFF); // Equivalent to 00B7FA
        }
    }

    public void ChangeSubmitButtonImageSpriteToBorder()
    {
        retryButtonImage.sprite = buttonBGSprites[0];
        // submitText.color = new Color32(0x05, 0xFD, 0x47, 0xFF); // Equivalent to 05FD47
        retryText.color = new Color32(0x00, 0xB7, 0xFA, 0xFF); // Equivalent to 00B7FA
    }

    public void ChangeButtonToHovered() {
        retryButtonImage.sprite = buttonBGSprites[1];
        retryText.color = new Color(0, 0, 0, 255); // Equivalent to black
    }
}
