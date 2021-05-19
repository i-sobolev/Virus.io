using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIRevivePanel : MonoBehaviour
{
    public Image timeBar;
    public Text text;

    public GameObject finalScorePanel;

    public Sprite NoAdsIcon;
    public Image Icon;
    public GameObject Fixer;

    private void Start()
    {
        if (!PersistentData.Ads.IsEnabled())
        {
            Icon.sprite = NoAdsIcon;
            Fixer.SetActive(false);
        }
    
        text.text += TemporaryData.PlayerNickName.ToUpper();
        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        while (timeBar.fillAmount > 0.09f)
        {
            timeBar.fillAmount -= Time.deltaTime / 10;
            yield return null;
        }

        DestroyAndTurnOnFinalScorePanel();
    }

    public void DestroyAndTurnOnFinalScorePanel()
    {
        StopAllCoroutines();
        finalScorePanel.SetActive(true);
        Destroy(gameObject);
    }
}
