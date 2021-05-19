using System.Collections;
using UnityEngine;
using VirusAnimator;

public class VirusHead : MonoBehaviour
{
    [Header("Components")]
    public Spawner Spawner;
    public VirusScore Score;
    public SpriteRenderer spriteRenderer;
    public Sprite BodySprite;

    [Header("Tail")]
    internal GameObject LastTail;
    public GameObject TailPrefab;

    internal int TailCounter { get; private set; }
    internal int Wide { get; private set; }
    internal int NumberOfTailsToGrowUp { get; set; }
    public int ScoreToAddTail { get; internal set; }

    public bool IsNotFlicker;

    [Header("Moving")]
    internal Vector3 CurrentDirVec;
    internal Vector2 ReflectedVector;

    [Space]
    private bool _isStunned;

    [Space]
    public float Speed;
    private float _rotationSpeed;

    private float _generalTailDestroyDelay = 0;
    private int _evenTailCounter = 0;

    public VirusHead()
    {
        TailCounter = -5;
        _rotationSpeed = 500f;
        NumberOfTailsToGrowUp = 10;
        ScoreToAddTail = 5;
    }

    internal virtual void Awake()
    {
        LastTail = gameObject;
        Score = gameObject.AddComponent<VirusScore>();
    }

    IEnumerator StunCooldown()
    {
        yield return new WaitForSeconds(0.75f);
        _isStunned = false;
    }

    internal void Stun(Collider2D collision)
    {
        StopCoroutine(StunCooldown());
        _isStunned = true;
        ReflectedVector = ReflectDirectionVector(collision.transform);
        CurrentDirVec = ReflectedVector;
        StartCoroutine(StunCooldown());
    }

    internal virtual void AddTail()
    {
        _generalTailDestroyDelay += 0.02f;
        Tail newTail = Instantiate(TailPrefab, new Vector2(LastTail.transform.position.x, LastTail.transform.position.y), Quaternion.identity).GetComponent<Tail>();

        newTail.SetFields(LastTail, this, _generalTailDestroyDelay, gameObject.name, BodySprite);

        if (_evenTailCounter == 1)
        {
            newTail.GetComponent<CircleCollider2D>().enabled = false;
            _evenTailCounter = -1;
        }

        _evenTailCounter++;
        TailCounter++;

        LastTail = newTail.gameObject;

        if (TailCounter == NumberOfTailsToGrowUp)
            AddWide();
    }

    internal void AddTailsOnStart(int numberOfTails)
    {
        for (int i = numberOfTails; i >= 0; i--)
            AddTail();
    }

    internal virtual void AddWide()
    {
        StartCoroutine(Twiner.GrowVirus(gameObject, gameObject));
        LastTail.GetComponent<Tail>().GrowVirus();

        NumberOfTailsToGrowUp += 2;
        ScoreToAddTail += 8;

        TailCounter = 0;
        Wide++;
    }

    public void AddScore()
    {
        Score.Score++;
        Score.RateTable.ListUpdate();

        if (Score.Score - Score.LastScore == ScoreToAddTail)
        {
            AddTail();

            LastTail.GetComponent<Tail>().AddTailEffect();

            Score.LastScore = Score.Score;
        }
    }

    public Vector2 ReflectDirectionVector(Transform objectTransform)
    {
        float x = objectTransform.position.x, y = objectTransform.position.y;
        Vector2 normal = Vector2.zero;

        if (x > 0 && y < 0)
            normal = Vector2.up;

        if (x < 0 && y > 0)
            normal = Vector2.down;

        if (x < 0 && y < 0)
            normal = Vector2.right;

        if (x > 0 && y > 0)
            normal = Vector2.left;

        return Vector2.Reflect(CurrentDirVec, normal);
    }

    internal void Move(Vector2 moveTo)
    {
        Vector2 moveTarget;

        if (_isStunned)
        {
            moveTarget = transform.position + (Vector3)ReflectedVector;
        }

        else
        {
            float vecDelta = Vector2.SignedAngle(moveTo, CurrentDirVec);

            float newAngle = 0;

            if (vecDelta > 0.1f)
            {
                if (vecDelta > 20)
                    newAngle = -_rotationSpeed;

                else
                    newAngle = -vecDelta * vecDelta;
            }

            if (vecDelta < -0.1f)
            {
                if (vecDelta < -20)
                    newAngle = _rotationSpeed;

                else
                    newAngle = vecDelta * vecDelta;
            }

            CurrentDirVec = Quaternion.Euler(0, 0, CurrentDirVec.z + newAngle * Time.deltaTime) * CurrentDirVec;

            moveTarget = new Vector2(transform.position.x + CurrentDirVec.x, transform.position.y + CurrentDirVec.y);
        }

        RotateToMoveDirection();

        transform.position = Vector2.MoveTowards(transform.position, moveTarget, Speed * Time.deltaTime);
    }

    internal virtual void RotateToMoveDirection()
    {
        float lookAngle = Vector2.SignedAngle(Vector2.up, CurrentDirVec);
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, lookAngle);
    }

    internal void Death()
    {
        GetComponent<CircleCollider2D>().enabled = false;
        Score.RateTable.playersList.Remove(Score);

        if (Score.RateTable.playersList.Count < 12)
            Spawner.SpawnBot();

        Tail lastBodyComponent = LastTail.GetComponent<Tail>();
        lastBodyComponent.StartCoroutine(lastBodyComponent.Death());
    }

    public IEnumerator FlickVirusOnStart()
    {
        StartCoroutine(Twiner.VirusFlicker(gameObject));
        LastTail.GetComponent<Tail>().FlickOnAwake();
        IsNotFlicker = false;

        yield return null;
    }

    internal void FlickVirus()
    {
        StartCoroutine(FlickVirusOnStart());
    }
}