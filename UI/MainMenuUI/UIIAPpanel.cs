using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using VirusAnimator;

public class UIIAPpanel : MonoBehaviour
{
    public Image Background;

    public GameObject MainPanel;
    public GameObject PurchaseCompletePanel;
    public GameObject PurchaseFailedPanel;
    public GameObject NoAdsPurchaseCompletePanel;

    private GameObject _currentOpenedPanel;

    [Space]
    public ParticleSystem particles;
    public Text cyText;

    private void Awake()
    {
        Background.color = new Color(0, 0, 0, 0);
        MainPanel.GetComponent<RectTransform>().localScale = Vector3.zero;
        PurchaseCompletePanel.GetComponent<RectTransform>().localScale = Vector3.zero;
        PurchaseFailedPanel.GetComponent<RectTransform>().localScale = Vector3.zero;
    }

    public void ShowPanel(GameObject panel)
    {
        StartCoroutine(Twiner.PanelSmoothScaleChange(0, 1, panel));

        if (_currentOpenedPanel != null)
            StartCoroutine(Twiner.PanelSmoothScaleChange(1, 0, _currentOpenedPanel));

        else
            StartCoroutine(Twiner.ImageSmoothAlphaChange(0, 0.65f, Background));

        _currentOpenedPanel = panel;
    }

    public void CloseIAPPanel()
    {
        StartCoroutine(Twiner.PanelSmoothScaleChange(1, 0, _currentOpenedPanel));
        StartCoroutine(Twiner.ImageSmoothAlphaChange(0.65f, 0, Background));
        _currentOpenedPanel = null;
    }

    public void OnDNAPurchasingComplete(int value)
    {
        int currentPlayerDna = PersistentData.DNA.Get(); 
        cyText.text = currentPlayerDna.ToString();

        StartCoroutine(AddDnaEffect(currentPlayerDna + value, currentPlayerDna));

        PersistentData.DNA.Add(value);

        UIShopPanel.Singletone.RefreshDnaText();

        FacebookSDK.DnaPurchaseEventLog(value);
    }

    private IEnumerator AddDnaEffect(int target, int currentDna)
    {
        particles.Play();

        while (currentDna < target)
        {
            currentDna += target - currentDna > 30 ? 10 : 1;
            cyText.text = currentDna.ToString();
            yield return null;
        }

        particles.Stop();
    }
}