using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VirusAnimator;

public class UIPlayButton : MonoBehaviour
{
    private UISceneLoader _sceneLoader => GetComponent<UISceneLoader>();
    public Image[] NetworkErrorIcon;
    public bool isNetworkConnected;

    [Space]
    public Image Background;
    public GameObject LostConnectionPanel;

    public void SwitchNetworkConnectionStatus(bool switchTo)
    {
        foreach (Image image in NetworkErrorIcon)
            image.enabled = !switchTo;

        isNetworkConnected = switchTo;
    }

    public void Update()
    {
        if (NetworkErrorIcon[0].enabled)
            NetworkErrorIcon[1].color = new Color(1, 1, 1, Mathf.Sin(Time.time * 4) / 4 + 0.75f);
    }

    public void StartGame()
    {
        if (isNetworkConnected)
            _sceneLoader.TurnOnLoadScreen("MainScene");

        else
            TurnOnLostConnectionNotification();
    }

    private void TurnOnLostConnectionNotification()
    {
        StopAllCoroutines();
        StartCoroutine(Twiner.ImageSmoothAlphaChange(0, 0.65f, Background));

        LostConnectionPanel.GetComponent<RectTransform>().localScale = Vector2.zero;
        StartCoroutine(Twiner.PanelSmoothScaleChange(0, 1, LostConnectionPanel));
    }

    public void TurnOffLostConnectionNotification()
    {
        StopAllCoroutines();
        StartCoroutine(Twiner.ImageSmoothAlphaChange(0.65f, 0,Background));
        StartCoroutine(Twiner.PanelSmoothScaleChange(1, 0, LostConnectionPanel));
    }
}
