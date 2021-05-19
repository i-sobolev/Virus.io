using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIGrowBar : MonoBehaviour
{
    [Header("UI")]
    public Image barImage;
    public Animator filledBar;

    private float _fillStep;
    private float _targetFillAmount;
    private float _lastFillAmount;
    private int _counter = 0;
    private int _stepsToFill;

    public static UIGrowBar Singletone { get; private set; }

    private void Awake() => Singletone = this;

    private void Start()
    {
        SetStep();
        PlayerVirusHead.Singletone.OnPeopleEat += FillBar;
        PlayerVirusHead.Singletone.OnVirusGrow += SetStep;
        PlayerVirusHead.Singletone.OnNewTail += PlayFilledAnimation;
    }

    private void FillBar() => StartCoroutine(SmoothFillAmount());

    private IEnumerator SmoothFillAmount()
    {
        if (_counter != 0)
        {
            _targetFillAmount += _fillStep;
            _lastFillAmount = barImage.fillAmount;

            var lerp = 0f;

            while (barImage.fillAmount < _targetFillAmount)
            {
                barImage.fillAmount = Mathf.Lerp(_lastFillAmount, _targetFillAmount, VirusAnimator.Twiner.SmoothSquarer(lerp));
                lerp += 0.05f;
                yield return null;
            }

            barImage.fillAmount = _targetFillAmount;
        }
     
        _counter++;
    }

    public void SetStep()
    {
        _stepsToFill = PlayerVirusHead.Singletone.ScoreToAddTail - 1;
        _fillStep = 1f / (_stepsToFill);
    }

    private void PlayFilledAnimation()
    {
        _counter = 0;
        barImage.fillAmount = 0;
        _targetFillAmount = 0;

        filledBar.Play("Filled");
    }
}