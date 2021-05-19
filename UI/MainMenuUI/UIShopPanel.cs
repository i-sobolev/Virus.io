using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VirusAnimator;

public class UIShopPanel : MonoBehaviour
{
    public UIShopButton[] virusesButtons;
    public Text dnaCount;
    public Text mainMenuDnaCount;
    public Image shopButtonIcon;
    public GameObject purseGameObject;
    public UISkinPreview preview;

    public AudioSource declineSellSkinSound;
    public AudioSource newSkinSeledtedSound;

    public static UIShopPanel Singletone { get; private set; }

    private void Awake() => Singletone = this;

    private void Start()
    {
        SetLastSelectedSkin();
        RefreshDnaText();
    }

    public void SetLastSelectedSkin()
    {
        foreach (UIShopButton button in virusesButtons)
        {
            if (button.skinNumber == PersistentData.VirusSkin.CurrentSelectedSkin.Get())
            {
                UIShopButton currentButton = button;

                if (!button.isBought)
                    currentButton = virusesButtons[0];

                currentButton.selectIcon.enabled = true;
                shopButtonIcon.sprite = currentButton.snakeSprites[0];

                if (currentButton.isSecretSkin)
                    shopButtonIcon.sprite = currentButton.secretVirusSkin[2];

                currentButton.SetSprites();
                newSkinSeledtedSound.Stop();
                preview.ShowSkinOnPreviewTable(currentButton);
            }

            else
            {
                button.selectIcon.enabled = false;
            }
        }

    }

    public void UpdateSelectImage(UIShopButton thisButton)
    {
        newSkinSeledtedSound.CheckIsAudioEnabledAndPlay();

        foreach (UIShopButton button in virusesButtons)
        {
            if (button != thisButton)
            {
                button.selectIcon.enabled = false;
            }

            else
            {
                shopButtonIcon.sprite = button.snakeSprites[0];

                if (button.isSecretSkin)
                    shopButtonIcon.sprite = button.secretVirusSkin[2];

                button.selectIcon.enabled = true;
            }

            button.LockRefresh();
        }
    }

    public void UpdateCurrentSelectedButton(UIShopButton selectedButton)
    {
        foreach (UIShopButton button in virusesButtons)
            button.isSelected = button == selectedButton;
    }

    public void RefreshDnaText()
    {
        dnaCount.text = PersistentData.DNA.Get().ToString();
        mainMenuDnaCount.text = PersistentData.DNA.Get().ToString();
        PersistentData.Save();
    }

    public void SignOnPurse()
    {
        declineSellSkinSound.CheckIsAudioEnabledAndPlay();
        StartCoroutine(Twiner.UISigner(purseGameObject));
    }

    public void SubUnlockSkins(params int[] skinIds)
    {
        List<int> skinIdsToUnlock = new List<int>();
        skinIdsToUnlock.AddRange(skinIds);

        foreach (var button in virusesButtons)
        {
            if (skinIdsToUnlock.Contains(button.skinNumber))
                button.UnlockSkin();
        }
    }
}