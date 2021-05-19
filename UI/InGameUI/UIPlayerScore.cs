using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIPlayerScore : MonoBehaviour
{
    private VirusScore PlayerScoreComponent;

    [Header("UI")]
    public Text PlayerNameText;
    public Text PlayerScoreText;
    public Text PlayerKillsText;
    public Text PlayerHighScore;

    public int Kills { get; set; }
    public int Score { get; private set; }
    public bool IsPlayerBeatLastHighScore { get; private set; }

    public UnityEvent PlayerKillsOtherVirus;
    public UnityEvent TurnOffHighScorePanel;

    public static UIPlayerScore Singletone { get; private set; }
    private AudioSource _virusDeathSound;

    private void Awake()
    {
        _virusDeathSound = GetComponent<AudioSource>();

        Singletone = this;

        PlayerNameText.text = TemporaryData.PlayerNickName;

        string pointsName = " infected"; // POINTS NAME 

#if UNITY_IOS
        pointsName = " people";
#endif

        PlayerScoreText.text = 0 + pointsName;


        if (PersistentData.HighScore.HasKey())
        {
            PlayerHighScore.text = "High score: " + PersistentData.HighScore.Get().ToString();
        }

        else
        {
            TurnOffHighScorePanel.Invoke();
            PersistentData.HighScore.Set(0);
        }
    }

    private void OnEnable()
    {
        PlayerVirusHead.Singletone.OnPeopleEat += UpdatePlayerScore;
        PlayerVirusHead.Singletone.OnKillingVirus += UpdatePlayerKillsCounter;

        PlayerScoreComponent = PlayerVirusHead.Singletone.Score;
    }

    private void UpdatePlayerScore()
    {
        string pointsName = " infected"; // POINTS NAME 

#if UNITY_IOS
        pointsName = " people";
#endif

        PlayerScoreText.text = PlayerScoreComponent.Score.ToString() + pointsName;

        Score = PlayerScoreComponent.Score;

        if (PlayerScoreComponent.Score > PersistentData.HighScore.Get())
        {
            PersistentData.HighScore.Set(PlayerScoreComponent.Score);
            PersistentData.Save();
            IsPlayerBeatLastHighScore = true;
        }
    }

    private void UpdatePlayerKillsCounter()
    {
        PlayerKillsOtherVirus.Invoke();
        Kills++;
        PlayerKillsText.text = "x " + Kills.ToString();
    }

    public void PlayDeathSound() => _virusDeathSound.CheckIsAudioEnabledAndPlay();
}
