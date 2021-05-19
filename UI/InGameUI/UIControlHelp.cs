using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIControlHelp : MonoBehaviour
{
    [Header("UI")]
    public GameObject DoubleTapHand;
    public Image Background;
    public Text Text;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("HelpCount"))
        {
            PlayerPrefs.SetInt("HelpCount", 2);
        }

        if (PlayerPrefs.GetInt("HelpCount") > 0 && (TemporaryData.IsSpeedButtonEnabled || TemporaryData.IsSplashAndSpeedButtonsEnabled))
        {
            StartCoroutine(HelpAnimation());
            PlayerPrefs.SetInt("HelpCount", PlayerPrefs.GetInt("HelpCount") - 1);
        }
        
        else
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator HelpAnimation()
    {
        yield return new WaitForSeconds(1f);

        DoubleTapHand.SetActive(true);

        for (float alpha = 0; alpha < 0.9f; alpha += Time.deltaTime * 5)
        {
            yield return null;
            Background.color = new Color(Background.color.b, Background.color.g, Background.color.b, alpha);
            Text.color = new Color(Text.color.b, Text.color.g, Text.color.b, alpha);
            
        }

        Text.color = new Color(Text.color.b, Text.color.g, Text.color.b, 0.9f);
        Background.color = new Color(Background.color.b, Background.color.g, Background.color.b, 0.9f);

        yield return new WaitForSeconds(2.21f);

        for (float alpha = 0.9f; alpha > 0.05f; alpha -= Time.deltaTime * 5)
        {
            yield return null;
            Background.color = new Color(Background.color.b, Background.color.g, Background.color.b, alpha);
            Text.color = new Color(Text.color.b, Text.color.g, Text.color.b, alpha);

        }

        Text.color = new Color(Text.color.b, Text.color.g, Text.color.b, 0);
        Background.color = new Color(Background.color.b, Background.color.g, Background.color.b, 0);

        yield return new WaitForSeconds(2.6f);

        Destroy(gameObject);
    }
}
