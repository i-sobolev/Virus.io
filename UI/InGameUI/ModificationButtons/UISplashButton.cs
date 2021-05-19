using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISplashButton : MonoBehaviour
{
    [Header("UI")]
    public Image bar;

    [Header("Audio")]
    public AudioSource soundEffect;

    [Header("Fabs")]
    public GameObject[] splash;
    private GameObject _splashFab;

    private float _increseSpeed;

    private PlayerVirusHead _playerHead;

    private void Start()
    {
        _playerHead = PlayerVirusHead.Singletone;
        _increseSpeed = TemporaryData.SplashIncreaseSpeed;
        _splashFab = splash[TemporaryData.CurrentSplashColorNumber];
        bar.fillAmount = 0;
    }

    private void Update()
    {
        bar.fillAmount += _increseSpeed * Time.deltaTime;
    }

    public void Splash()
    {
        if (bar.fillAmount == 1)
        {
            soundEffect.pitch = Random.Range(1f, 1.3f);
            soundEffect.CheckIsAudioEnabledAndPlay();

            Instantiate(_splashFab, _playerHead.transform.position, Quaternion.identity).GetComponent<Splash>().ParentHead = _playerHead;
            bar.fillAmount = 0;
        }
    }
}
