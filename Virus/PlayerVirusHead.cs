using System.Collections;
using UnityEngine;
using VirusAnimator;

public class PlayerVirusHead : VirusHead
{
    [Header("Player special components")]
    public PlayerInput PlayerInput;
    public SpriteRenderer SpriteRenderer;
    public GameObject MoveDirectionArrow;
    public static PlayerVirusHead Singletone { get; private set; }

    [Header("Audio")]
    public AudioSource InfectPeopleSound;
    private float _pitch = 1;
    private float _pitchCoolDown = 0;

    public event Action OnPeopleEat;
    public event Action OnKillingVirus;
    public event Action OnNewTail;
    public event Action OnVirusGrow;
    public delegate void Action();

    internal override void Awake()
    {
        base.Awake();

        Singletone = this;

        BodySprite = TemporaryData.PlayerBodySprite;
        SpriteRenderer.sprite = TemporaryData.PlayerHeadSprite;
        gameObject.name = TemporaryData.PlayerNickName;
    }

    private void Start()
    {
        Spawner = Spawner.Singletone;
        CurrentDirVec = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));

        if (TemporaryData.IsSecretSkinSelected)
            gameObject.AddComponent<SecretSkin>();

        if (TemporaryData.IsRewardEarned)
            Revive();

        else
            AddTailsOnStart(5);

        FlickVirus();
    }

    private void Update()
    {
        Move(PlayerInput.MoveDirection);

        float lookAngleForArrow = Vector2.SignedAngle(Vector2.up, PlayerInput.MoveDirection);
        MoveDirectionArrow.transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, lookAngleForArrow);
        MoveDirectionArrow.transform.position = transform.position + (Vector3)PlayerInput.MoveDirection.normalized * PlayerInput.TouchDistance;
        MoveDirectionArrow.transform.localScale = new Vector3(PlayerInput.TouchDistance, PlayerInput.TouchDistance, 0);
    }

    private void FixedUpdate()
    {
        _pitchCoolDown -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsNotFlicker)
        {
            #region PeopleCollision
            if (collision.CompareTag("People"))
            {
                AddScore();
                PlayPeopleEatSound();

                OnPeopleEat?.Invoke();

                if (collision.name != "D_People")
                    StartCoroutine(Twiner.UnsizeAndReposition(Spawner.NewPeoplePosition(), collision.gameObject));

                if (collision.name == "D_People")
                    StartCoroutine(Twiner.UnsizeAndDestroy(collision.gameObject));
            }
            #endregion

            #region VirusCollision
            if (collision.GetComponent<Tail>() != null && collision.name != gameObject.name)
            {
                if (collision.GetComponent<Tail>().head.IsNotFlicker)
                {
                    PlayerCamera.Singletone.SetMoveTarget(collision.gameObject);
                    PlayDeathSound();
                    Death();
                }
            }

            if (collision.GetComponent<VirusHead>() != null && collision.name != gameObject.name)
            {
                VirusHead head = collision.GetComponent<VirusHead>();

                if (head.IsNotFlicker)
                {
                    if (head.Wide >= Wide)
                    {
                        PlayerCamera.Singletone.SetMoveTarget(collision.gameObject);
                        PlayDeathSound();
                        Death();
                    }
                }
            }
            #endregion
        }

        if (collision.name == "PlayArea")
            Stun(collision);
    }

    internal void OnKillingVirusInvoke() => OnKillingVirus?.Invoke();

    private void PlayPeopleEatSound()
    {
        if (_pitchCoolDown < 0)
            _pitch = 1;

        _pitchCoolDown = 1f;

        InfectPeopleSound.pitch = _pitch;
        InfectPeopleSound.CheckIsAidioEnabledAndPlayOneShot();

        if (_pitch < 3)
            _pitch += 0.1f;
    }

    private void PlayDeathSound() => UIPlayerScore.Singletone.PlayDeathSound();

    internal override void AddTail()
    {
        base.AddTail();
        OnNewTail?.Invoke();
    }

    internal override void AddWide()
    {
        base.AddWide();
        OnVirusGrow?.Invoke();
    }

    private IEnumerator AddTailsOnRevive()
    {
        int savedScore = TemporaryData.SavedScore;
        int numberOfTails = 5;
        int scoreToAddTail = 5;
        int tailCounter = 0;
        int tailsToGrowUp = 10;

        while (savedScore > 0)
        {
            savedScore -= scoreToAddTail;
            numberOfTails++;
            tailCounter++;

            if (tailCounter == tailsToGrowUp)
            {
                tailsToGrowUp += 2;
                scoreToAddTail += 8;
                tailCounter = 0;
            }
        }

        while (numberOfTails > 0)
        {
            AddTail();
            numberOfTails--;

            for (int i = 8; i > 0; i--)
                yield return null;
        }
    }

    private void Revive()
    {
        StartCoroutine(AddTailsOnRevive());
        Score.Score = TemporaryData.SavedScore;
        Score.LastScore = Score.Score;
        UIPlayerScore.Singletone.Kills = TemporaryData.SavedKills;
    }
}