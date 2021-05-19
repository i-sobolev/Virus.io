using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIFinalScorePanel : MonoBehaviour
{
#if UNITY_ANDROID
    public static string[] LevelNames =
    {
        " ",
        "Infection I",
        "Infection II",
        "Infection III",
        "Infection IV",
        "Infection V",
        "Endemia I",
        "Endemia II",
        "Endemia III",
        "Endemia IV",
        "Endemia V",
        "Epidemia I",
        "Epidemia II",
        "Epidemia III",
        "Epidemia IV",
        "Epidemia V",
        "Pandemia I",
        "Pandemia II",
        "Pandemia III",
        "Pandemia IV",
        "Pandemia V",
    };
#endif

#if UNITY_IOS
    public static string[] LevelNames =
    {
        " ",
        "Newbe eater I", 
        "Newbe eater II",
        "Newbe eater III",
        "Newbe eater IV",
        "Newbe eater V",
        "Master eater I", 
        "Master eater II",
        "Master eater III",
        "Master eater IV",
        "Master eater V",
        "Legend eater I",
        "Legend eater II",
        "Legend eater III",
        "Legend eater IV",
        "Legend eater V",
        "People eater I", 
        "People eater II",
        "People eater III",
        "People eater IV",
        "People eater V",
    };
#endif

    public static int[] TargetsToUpLevel =
    {
        0,
        50, 100, 150, 200, 250,
        350, 450, 550, 650, 750,
        950, 1150, 1350, 1550, 1750,
        2000, 2500, 3000, 3500, 4000
    };

    private int _playerScore;
    private int _playerKills;

    private bool _isNewHighScore = false;

    [Header("UI")]
    public GameObject highScoreText;
    public GameObject scoreText;
    public Text playerScoreText;
    public Text prevLevelTarget, currentLevelTarget, barScore, levelNameText;
    public Text killsCounter, dnaCounter;
    public Image progressBar;

    private int _prevLvlTarget = TargetsToUpLevel[0];
    private int _currentLvlTarget = TargetsToUpLevel[1];
    private int _currentLvl = 0;
    private int _scoreToText = 0;
    private int _dnaToText = 0;
    private int _dna = 0;

    [Header("Particles")]
    public ParticleSystem particles;

    [Header("Audio")]
    public AudioSource fillBarSound;
    public AudioSource newLevelSound;

    private int _newDnaPref;
    private int _newSkinLevelPref;

    private void Awake()
    {
        SetPlayerStats();
        SetScoreOnPanel();

        particles.Stop();

        if (_isNewHighScore)
        {
            highScoreText.SetActive(true);
            scoreText.SetActive(false);
        }

        levelNameText.text = "LVL: 0";
        barScore.text = "0";

        int newLevel = 0;
        int scoreToLvl = _playerScore;
        int newDna = 0;

        TemporaryData.SavedKills = _playerKills;
        TemporaryData.SavedScore = _playerScore;

        Analytics.LogPlayerScore(_playerScore);

        if (_playerScore < TargetsToUpLevel[TargetsToUpLevel.Length - 1])
        {
            while (scoreToLvl - TargetsToUpLevel[newLevel + 1] >= 0)
                newDna = TargetsToUpLevel[++newLevel];
        }

        else
        {
            newDna = _playerScore;
            newLevel = 20;
            PersistentData.VirusSkin.SecretSkin.Unlock();
        }
        
        _newSkinLevelPref = newLevel;
        _newDnaPref = newDna;

        FacebookSDK.NewLevelEventLog(newLevel); // FACEBOOK

        StartCoroutine(FillProgressBar());
    }

    private IEnumerator FillProgressBar()
    {
        if (_currentLvl == 21)
        {
            StartCoroutine(AddDnaAtMaxLevel());

            StartCoroutine(ProgressBarMaxLevelAnim());

            levelNameText.text = "LVL: MAX";
            prevLevelTarget.text = null;
            currentLevelTarget.text = null;
        }

        else
        {
            int diffBtwLevels = _currentLvlTarget - _prevLvlTarget;

            prevLevelTarget.text = _prevLvlTarget.ToString();
            currentLevelTarget.text = _currentLvlTarget.ToString();

            int iterationsCounter = 0;

            if (_playerScore - diffBtwLevels >= 0)
            {
                float scoreAddStep = diffBtwLevels * 0.01f;
                float smoothScore = _prevLvlTarget;

                _playerScore -= diffBtwLevels;

                for (float i = 0; i < 1; i += 0.01f)
                {
                    progressBar.fillAmount = i;
                    smoothScore += scoreAddStep;

                    if (_scoreToText < _currentLvlTarget) 
                        _scoreToText = (int)smoothScore;

                    barScore.text = _scoreToText.ToString();

                    PlayFillBarSound(ref iterationsCounter, i + 1f);

                    yield return null;
                }

                particles.Clear();
                particles.Play();

                progressBar.fillAmount = 1;

                ++_currentLvl;
                levelNameText.text = "LVL: " + LevelNames[_currentLvl] + " (" + _currentLvl.ToString() + ")";
                _prevLvlTarget = TargetsToUpLevel[_currentLvl];
                _currentLvlTarget = TargetsToUpLevel[_currentLvl + 1];

                newLevelSound.pitch += 0.05f;
                newLevelSound.CheckIsAudioEnabledAndPlay();

                StartCoroutine(AddDna(diffBtwLevels));
                StartCoroutine(FillProgressBar());
            }

            else
            {
                for (float i = 0; i <= ((float)_playerScore / diffBtwLevels); i += 0.01f)
                {
                    progressBar.fillAmount = i;

                    if (_scoreToText < _prevLvlTarget + _playerScore)
                        _scoreToText += 1;

                    barScore.text = _scoreToText.ToString();

                    PlayFillBarSound(ref iterationsCounter, i + 1f);

                    yield return null;
                }

                while (_scoreToText < _prevLvlTarget + _playerScore)
                {
                    _scoreToText += 1;
                    barScore.text = _scoreToText.ToString();
                    yield return null;
                }
            }
        }
    }

    private IEnumerator AddDna(int diffBtwLevels)
    {
        _dna += diffBtwLevels;

        while (_dnaToText < _dna)
        {
            _dnaToText += !(_dnaToText % 10 > 0)? 5 : 1;
            dnaCounter.text = _dnaToText.ToString();
            yield return null;
        }
    }

    private IEnumerator AddDnaAtMaxLevel()
    {
        _dna += _playerScore;

        while (_dnaToText < _dna)
        {
            _dnaToText += !(_dnaToText % 10 > 0) ? 5 : 1;
            dnaCounter.text = _dnaToText.ToString();
            yield return null;
        }
    }

    public void SetPlayerStats()
    {
        UIPlayerScore playerScore = UIPlayerScore.Singletone;

        _playerScore = playerScore.Score;
        _playerKills = playerScore.Kills;
        _isNewHighScore = playerScore.IsPlayerBeatLastHighScore;
    }

    private void SetScoreOnPanel()
    {
        string panelText = " people were infected with ";
#if UNITY_IOS
        panelText = " people were eaten by "
#endif
        playerScoreText.text = _playerScore.ToString() + panelText + TemporaryData.PlayerNickName.ToUpper();
        killsCounter.text = "x " + _playerKills.ToString();
    }

    public void SavePrefs()
    {
        int dna = _newDnaPref;
        int lvl = _newSkinLevelPref;

        PersistentData.DNA.Add(dna);

        PersistentData.VirusSkin.Level.Set(PersistentData.VirusSkin.CurrentSelectedSkin.Get(), lvl);

        PersistentData.Save();
    }

    public void ButtonAnim(GameObject buttonGameObject)
    {
        buttonGameObject.GetComponent<AudioSource>().CheckIsAudioEnabledAndPlay();

        StartCoroutine(ButtonPressAnim(buttonGameObject));
    }

    private IEnumerator ButtonPressAnim(GameObject buttonGameObject)
    {
        RectTransform transform = buttonGameObject.GetComponent<RectTransform>();

        for (float i = 1; i > 0.8f; i -= Time.deltaTime * 3)
        {
            transform.localScale = new Vector3(i, i, 1);
            yield return null;
        }

        transform.localScale = new Vector3(0.8f, 0.8f, 1);

        for (float i = 0.8f; i < 1; i += Time.deltaTime * 4)
        {
            transform.localScale = new Vector3(i, i, 1);
            yield return null;
        }

        transform.localScale = new Vector3(1, 1, 1);
    }

    private void PlayFillBarSound(ref int iterCounter, float pitch)
    {
        iterCounter++;

        if (iterCounter == 5)
        {
            iterCounter = 0;
            fillBarSound.pitch = pitch;
            fillBarSound.CheckIsAudioEnabledAndPlay();
        }
    }

    public void DestroyParticlesOnLoad()
    {
        Destroy(particles);
    }

    private IEnumerator ProgressBarMaxLevelAnim()
    {
        progressBar.fillOrigin = 0;
        progressBar.fillAmount = 0;
        
        while (progressBar.fillAmount < 1)
        {
            progressBar.fillAmount += 0.015f;
            yield return null;
        }

        progressBar.fillOrigin = 1;
        progressBar.fillAmount = 1;

        while (progressBar.fillAmount > 0)
        {
            progressBar.fillAmount -= 0.015f;
            yield return null;
        }

        StartCoroutine(ProgressBarMaxLevelAnim());

        yield return new WaitForEndOfFrame();
    }
}