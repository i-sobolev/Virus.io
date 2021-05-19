using System.Collections;
using UnityEngine;
using VirusAnimator;

public class Splash : MonoBehaviour
{
    public VirusHead ParentHead;

    private CircleCollider2D _circleCollider;
    private int _numOfPeopleUnderSplash;

    private void OnEnable()
    {
        _circleCollider.GetComponent<CircleCollider2D>();
        StartCoroutine(SplashDelay());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("People") && collision.name != "D_People")
        {
            _numOfPeopleUnderSplash++;
            StartCoroutine(Twiner.UnsizeAndReposition(Spawner.NewPeoplePosition(), collision.gameObject));
        }

        if (collision.CompareTag("People") && collision.name == "D_People")
        {
            _numOfPeopleUnderSplash++;
            StartCoroutine(Twiner.UnsizeAndDestroy(collision.gameObject));
        }
    }

    private IEnumerator SplashDelay()
    {
        yield return new WaitForSeconds(0.1f);
        AddScoreAfterSplash(ParentHead);
        _circleCollider.enabled = false;
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    public void AddScoreAfterSplash(VirusHead virusHead)
    {
        for (int i = _numOfPeopleUnderSplash; i > 0; i--)
        {
            virusHead.AddScore();
        }
    }
}