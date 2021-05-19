using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DailyReward : MonoBehaviour
{
    private DateTime RewardDate;
    private bool IsRewardReady = false;

    [Header("Timer text component")]
    [SerializeField] private Text TimerText;
    [SerializeField] private GameObject TimerGameobject;
    
    [Header("Icons")]
    [SerializeField] private Image ButtonIconComponent;
    [SerializeField] private Sprite RewardIco;
    [SerializeField] private Sprite TimerIco;

    public UnityEvent OnButtonClick;

    private void Awake()
    {
        if (PersistentData.RewardDate.HasKey())
            RewardDate = PersistentData.RewardDate.Get();

        else
            RewardDate = DateTime.Now;

        IsRewardReady = RewardDate - DateTime.Now < new TimeSpan();

        if (!IsRewardReady)
            ButtonIconComponent.sprite = TimerIco;
    }

    public void ButtonClick()
    {
        if (!IsRewardReady)
            return;

        OnButtonClick?.Invoke();
    }

    public void SetRewardDate()
    {
        TimerGameobject.SetActive(true);

        RewardDate = DateTime.Now.AddHours(2);
        PersistentData.RewardDate.Set(RewardDate);

        IsRewardReady = false;
        ButtonIconComponent.sprite = TimerIco;
    }

    private void FixedUpdate()
    {
        if (IsRewardReady)
        {
            ButtonIconComponent.sprite = RewardIco;
            TimerGameobject.SetActive(false);
            return;
        }

        TimeSpan toRewardTime = RewardDate - DateTime.Now;
        IsRewardReady = toRewardTime < new TimeSpan();
        TimerText.text = toRewardTime.ToString("hh\\:mm\\:ss");
    }
}
