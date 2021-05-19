using UnityEngine;
using System.Collections;

public class AIVirusHead : VirusHead
{
    private readonly string _virusHeadTag = "VirusHead";
    private readonly string _virusTailTag = "VirusTail";

    public GameObject peopleFinderFab;
    private PeopleFinder _peopleFinder;

    private Vector3 moveDirection;
    private Vector3 nearPeoplePosition;

    private enum Behavior { Passive, DefRight, DefLeft, DefForward };
    private Behavior currentBehavior;

    public bool isInstantietedOnStart;

    private AudioSource _deathSound;

    private bool _isDefending = true;

    internal override void Awake()
    {
        base.Awake();
        _deathSound = GameObject.Find("GameController").GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        Spawner = Spawner.Singletone;
        gameObject.name = NicknameGenerator.GenerateOneWordNick();

        CurrentDirVec = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        currentBehavior = Behavior.Passive;

        _peopleFinder = Instantiate(peopleFinderFab, transform.position, Quaternion.identity).GetComponent<PeopleFinder>();
        _peopleFinder.SetParent(transform);
        nearPeoplePosition = _peopleFinder.FindNearestObject();
    }

    private void Start()
    {
        Spawn();
        FindNewObjects();
    }

    private void OnDestroy()
    {
        if (_peopleFinder != null)
            Destroy(_peopleFinder.gameObject);
    }

    private void Update()
    {
        Move(moveDirection);
    }

    private void FixedUpdate()
    {
        BehaviourUpdate();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsNotFlicker)
        {
            #region PeopleCollision
            if (collision.CompareTag("People"))
            {
                AddScore();

                if (collision.name != "D_People")
                    collision.transform.position = Spawner.NewPeoplePosition();

                if (collision.name == "D_People")
                    Destroy(collision.gameObject);

                nearPeoplePosition = _peopleFinder.FindNearestObject();
            }
            #endregion

            #region VirusCollision
            if (collision.GetComponent<Tail>() != null && collision.name != gameObject.name)
            {
                var colliedTail = collision.GetComponent<Tail>();

                if (colliedTail.head.IsNotFlicker)
                {
                    if (colliedTail.head is PlayerVirusHead)
                    {
                        DeathSoundPlay();
                        PlayerVirusHead.Singletone.OnKillingVirusInvoke();
                    }

                    Death();
                }
            }

            if (collision.GetComponent<VirusHead>() != null && collision.name != gameObject.name)
            {
                var colliedVirusHead = collision.GetComponent<VirusHead>();

                if (colliedVirusHead.Wide >= Wide && colliedVirusHead.IsNotFlicker)
                {
                    if (colliedVirusHead is PlayerVirusHead)
                    {
                        if (colliedVirusHead.Wide != Wide)
                            DeathSoundPlay();

                        PlayerVirusHead.Singletone.OnKillingVirusInvoke();
                    }

                    Death();
                }
            }
            #endregion
        }

        if (collision.name == "PlayArea")
        {
            Stun(collision);
            nearPeoplePosition = _peopleFinder.FindNearestObject();
        }
    }

    private void BehaviourUpdate()
    {
        #region RayInst & beh def
        Vector3 vecLeft = Quaternion.Euler(0, 0, CurrentDirVec.z + 70) * CurrentDirVec;
        Vector3 vecRight = Quaternion.Euler(0, 0, CurrentDirVec.z - 70) * CurrentDirVec;

        Vector3 position = new Vector3(transform.position.x, transform.position.y);

        RaycastHit2D rayForward = Physics2D.Raycast(position + CurrentDirVec * 1.5f, CurrentDirVec, 6f);
        RaycastHit2D rayLeft = Physics2D.Raycast(position + vecLeft * 1.5f, vecLeft, 6f);
        RaycastHit2D rayRight = Physics2D.Raycast(position + vecRight * 1.5f, vecRight, 6f);

        bool right = true, left = true, forward = true;

        if (rayForward.collider != null)
        {
            if ((rayForward.collider.CompareTag(_virusTailTag) && rayForward.collider.name != gameObject.name) || (rayForward.collider.CompareTag(_virusHeadTag) && rayForward.collider.name != gameObject.name))
            {
                currentBehavior = Behavior.DefForward;
                StartCoroutine(Defend());
            }

            else
                forward = false;
        }

        else
        {
            forward = false;
        }

        if (rayLeft.collider != null)
        {
            if ((rayLeft.collider.CompareTag(_virusTailTag) && rayLeft.collider.name != gameObject.name) || (rayLeft.collider.CompareTag(_virusHeadTag) && rayLeft.collider.name != gameObject.name))
            {
                currentBehavior = Behavior.DefLeft;
                StartCoroutine(Defend());
            }

            else
                left = false;
        }

        else
        {
            left = false;
        }

        if (rayRight.collider != null)
        {
            if ((rayRight.collider.CompareTag(_virusTailTag) && rayRight.collider.name != gameObject.name) || (rayRight.collider.CompareTag(_virusHeadTag) && rayRight.collider.name != gameObject.name))
            {
                currentBehavior = Behavior.DefRight;
                StartCoroutine(Defend());
            }

            else
                right = false;
        }

        else
        {
            right = false;
        }

        if (!right && !left && !forward && !_isDefending)
        {
            currentBehavior = Behavior.Passive;
        }
        #endregion

        switch (currentBehavior)
        {
            case Behavior.DefForward:
                moveDirection = Quaternion.Euler(0, 0, CurrentDirVec.z + 30) * CurrentDirVec;
                break;

            case Behavior.DefRight:
                moveDirection = Quaternion.Euler(0, 0, CurrentDirVec.z + 20) * CurrentDirVec;
                break;

            case Behavior.DefLeft:
                moveDirection = Quaternion.Euler(0, 0, CurrentDirVec.z - 20) * CurrentDirVec;
                break;

            case Behavior.Passive:
                FindNewObjects();
                break;
        }
    }

    private void FindNewObjects()
    {
        moveDirection = nearPeoplePosition - transform.position;
        moveDirection.Normalize();

        var distanceToNearPeople = Vector2.Distance(transform.position, nearPeoplePosition);

        if (distanceToNearPeople < 1f)
            nearPeoplePosition = _peopleFinder.FindNewObject();
    }

    public void SetSprites(Sprite head, Sprite body)
    {
        spriteRenderer.sprite = head;
        BodySprite = body;
    }

    public void Spawn()
    {
        if (isInstantietedOnStart)
        {
            int newScore = Random.Range(37, 73);
            AddTailsOnStart(5 + newScore / 5);
            Score.Score = newScore + Random.Range(0, 10);
            IsNotFlicker = true;
        }

        else
        {
            AddTailsOnStart(5);
            FlickVirus();
        }
    }

    private void DeathSoundPlay()
    {
        _deathSound.pitch = Random.Range(1.0f, 1.3f);
        _deathSound.CheckIsAudioEnabledAndPlay();
    }

    private IEnumerator Defend()
    {
        _isDefending = true;

        yield return new WaitForSeconds(0.4f);

        nearPeoplePosition = transform.position + CurrentDirVec * Random.Range(4, 8);
        _isDefending = false;
    }
}
