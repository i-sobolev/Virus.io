using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UISpeedUpButton : MonoBehaviour
{
    [Header("UI")]
    public Image bar;
    public bool isPressed;
    
    private float _increaseSpeed;
    private float _decreaseSpeed;

    private float _boostedSpeed;
    private const float _standartSpeed = 4f;

    public UnityEvent particlesPlay;
    public UnityEvent particlesStop;

    public AudioSource soundEffect;

    private PlayerVirusHead playerHead;

    private void Start()
    {
        playerHead = PlayerVirusHead.Singletone;

        _boostedSpeed = TemporaryData.VirusBoostedSpeed;

        _increaseSpeed = TemporaryData.BoostIncreaseSpeed;
        _decreaseSpeed = TemporaryData.BoostDecreaseSpeed;
        bar.fillAmount = 0;
    }

    public void SpeedUp()
    {
        if (bar.fillAmount > 0.15f)
        {
            soundEffect.pitch = 1;
            soundEffect.CheckIsAudioEnabledAndPlay();

            isPressed = true;
            playerHead.Speed = _boostedSpeed;
            particlesPlay.Invoke();
        }
    }

    public void SpeedDown()
    {
        soundEffect.Stop();

        isPressed = false;
        playerHead.Speed = _standartSpeed;
        particlesStop.Invoke();
    }

    private void Update()
    {
        soundEffect.pitch += 0.001f;

        bar.fillAmount += _increaseSpeed * Time.deltaTime;

        if (isPressed == true)
            bar.fillAmount -= _decreaseSpeed* Time.deltaTime;

        if (bar.fillAmount <= 0.01f)
            SpeedDown();
    }
}