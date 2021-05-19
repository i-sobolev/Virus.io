using System.Collections;
using UnityEngine;
using VirusAnimator;

public class Tail : MonoBehaviour
{
    public new CircleCollider2D collider;
    public SpriteRenderer spriteRenderer;
    public GameObject nextBody;
    public VirusHead head;
    private Transform _nextBodyTransform;
    public float _deathDelay;
    private bool _isAlive = true;

    public void SetFields(GameObject _nextBody, VirusHead @this, float deathDelay, string name, Sprite sprite)
    {
        transform.localScale = @this.transform.localScale;
        head = @this;
        nextBody = _nextBody;
        _nextBodyTransform = _nextBody.transform;
        gameObject.name = name;
        _deathDelay = deathDelay;
        spriteRenderer.sprite = sprite;
    }

    private void Update()
    {
        if (_isAlive)
        {
            MoveBody();

            Vector2 lookVec = transform.position - _nextBodyTransform.position;
            float lookAngle = Vector2.SignedAngle(Vector2.up, lookVec);
            transform.rotation = Quaternion.Euler(0, 0, lookAngle);
        }
    }

    public IEnumerator Death()
    {
        _isAlive = false;
        bool isEvenBody = collider.enabled;
        collider.enabled = false;
        Tail nextBodyComponent = nextBody.GetComponent<Tail>();

        if (nextBody != head.gameObject)
            nextBodyComponent.StartCoroutine(nextBodyComponent.Death());

        else
            Destroy(head.gameObject);

        yield return new WaitForSeconds(_deathDelay);

        if (isEvenBody)
            Spawner.Singletone.SpawnPeopleOnVirusDeath(transform.position, 1);

        StopAllCoroutines();
        Destroy(gameObject);
    }

    private IEnumerator StartAddBodyEffect()
    {
        if (nextBody != head.gameObject)
            nextBody.GetComponent<Tail>().AddTailEffect();

        yield return new WaitForSeconds(_deathDelay * 5);
        StartCoroutine(Twiner.GrowBodyAtNewBody(gameObject, 0.25f, 0.4f));
    }

    private IEnumerator StartGrowVirus()
    {
        if (nextBody != head.gameObject)
            nextBody.GetComponent<Tail>().GrowVirus();

        yield return null;
        StartCoroutine(Twiner.GrowVirus(gameObject, head.gameObject));
    }

    private IEnumerator StartFlickOnAwake()
    {
        StartCoroutine(Twiner.VirusFlicker(gameObject));

        if (nextBody != head.gameObject)
            nextBody.GetComponent<Tail>().FlickOnAwake();
        
        yield return null;
    }

    public void FlickOnAwake() => StartCoroutine(StartFlickOnAwake());

    public void GrowVirus() => StartCoroutine(StartGrowVirus());

    public void AddTailEffect() => StartCoroutine(StartAddBodyEffect());    

    private void MoveBody()
    {
        if (head != null)
            transform.position = Vector2.Lerp(transform.position, _nextBodyTransform.position, 9 * Time.deltaTime);
    }
}
