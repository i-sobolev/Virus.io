using System.Collections;
using UnityEngine;
using VirusAnimator;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UILostConnectionNotification : MonoBehaviour
{
    public Image Background;
    public GameObject Panel;
    public Image LostConnectinoIcon;

    private bool _isPanelActive;
    public void TurnOnNotificationPanel()
    {
        if (!_isPanelActive)
        {
            _isPanelActive = true;
            StartCoroutine(Twiner.ImageSmoothAlphaChange(0, 0.65f, Background));
            Panel.GetComponent<RectTransform>().localScale = Vector2.zero;
            StartCoroutine(Twiner.PanelSmoothScaleChange(0, 1, Panel));
            StartCoroutine(FlickConnectionIcon());
            StartCoroutine(MainMenuLoadDelay());
        }
    }

    private IEnumerator FlickConnectionIcon()
    {
        while(true)
        {
            LostConnectinoIcon.color = new Color(
                LostConnectinoIcon.color.r, 
                LostConnectinoIcon.color.g, 
                LostConnectinoIcon.color.b, 
                Mathf.Sin(Time.time * 4) / 4 + 0.75f
                );

            yield return null;
        }
    }

    private IEnumerator MainMenuLoadDelay()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("MainMenu");
    }
}
