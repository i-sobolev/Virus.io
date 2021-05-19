using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScorePanelSwitcher : MonoBehaviour
{
    public Image Background;
    public GameObject FinalScorePanel;
    public GameObject RevivePanel;

    private void OnDestroy()
    {
        int PlayerFinalScore = TemporaryData.SavedScore = UIPlayerScore.Singletone.Score;
        TemporaryData.SavedKills = UIPlayerScore.Singletone.Kills;

        Background.gameObject.AddComponent<BackgroundAnimation>();
        Background.gameObject.SetActive(true);

        if (PlayerFinalScore > 45 && !TemporaryData.IsRewardEarned)
        {
            RevivePanel.AddComponent<PanelAnimation>();
            RevivePanel.SetActive(true);
        }

        else
        {
            FinalScorePanel.AddComponent<PanelAnimation>();
            FinalScorePanel.SetActive(true);
            TemporaryData.IsRewardEarned = false;
        }
    }
}

public class BackgroundAnimation : MonoBehaviour
{
    private Image _background;

    private void OnEnable()
    {
        _background = GetComponent<Image>();

        _background.color = new Color(0, 0, 0, 0);

        StartCoroutine(BackgroundSmoothAlphaChanger(0, 0.65f));
    }

    private IEnumerator BackgroundSmoothAlphaChanger(float from, float to)
    {
        var alpha = from;

        if (from > to)
        {
            while (alpha > to)
            {
                _background.color = new Color(0, 0, 0, alpha);
                alpha -= Time.deltaTime * 2;

                yield return new WaitForEndOfFrame();
            }
        }

        else
        {
            while (alpha < to)
            {
                _background.color = new Color(0, 0, 0, alpha);
                alpha += Time.deltaTime * 2;

                yield return new WaitForEndOfFrame();
            }
        }

        _background.color = new Color(0, 0, 0, to);

        Destroy(this);
    }
}

public class PanelAnimation : MonoBehaviour
{
    private RectTransform _panelRectTransform;

    private void OnEnable()
    {
        _panelRectTransform = GetComponent<RectTransform>();
        
        _panelRectTransform.localScale = new Vector3(0, 0, 1);
        
        StartCoroutine(PanelSmoothScaleChanger(0, 1, _panelRectTransform));
    }

    private IEnumerator PanelSmoothScaleChanger(float from, float to, RectTransform panelRectTransform)
    {
        var scale = from;

        if (from > to)
        {
            while (scale > to)
            {
                panelRectTransform.localScale = new Vector3(SmoothSquarer(scale), SmoothSquarer(scale), 1);
                scale -= Time.deltaTime * 2;

                yield return new WaitForEndOfFrame();
            }

            gameObject.SetActive(false);
        }

        else
        {
            while (scale < to)
            {
                panelRectTransform.localScale = new Vector3(SmoothSquarer(scale), SmoothSquarer(scale), 1);
                scale += Time.deltaTime * 2;

                yield return new WaitForEndOfFrame();
            }
        }

        panelRectTransform.localScale = new Vector3(to, to, 1);

        Destroy(this);
    }

    private static float SmoothSquarer(float x)
    {
        return x < 0.5f ? x * x * 2 : (1 - (1 - x) * (1 - x) * 2);
    }
}