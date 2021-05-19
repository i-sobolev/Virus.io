using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuVirus : MonoBehaviour
{
    private float currentSpeed = 3f;
    private Vector2 currentDirVec;

    private Sprite _headSprite;
    private Sprite _bodySprite;

    public GameObject body;
    private GameObject lastBody;

    public SpriteRenderer bodySpriteRenderer;
    public SpriteRenderer headSpriteRenderer;

    private bool isMenuLoaded = false;

    public List<SpriteRenderer> bodiesSprites;

    public static MainMenuVirus Singletone { private set; get; }

    private void Awake()
    {
        Singletone = this;

        _headSprite = TemporaryData.PlayerHeadSprite;
        _bodySprite = TemporaryData.PlayerBodySprite;

        body.AddComponent<MainMenuBody>().SetNextBody(gameObject);

        lastBody = body;

        bodiesSprites.Add(body.GetComponentsInChildren<SpriteRenderer>()[1]);

        currentDirVec = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
    }

    private void Start()
    {
        bodySpriteRenderer.sprite = _bodySprite;
        headSpriteRenderer.sprite = _headSprite;
    }

    public void TurnOnMainMenuVirus()
    {
        StartCoroutine(MoveStartDelay());
        StartCoroutine(ScaleAtStart(gameObject.transform));
        StartCoroutine(ScaleAtStart(body.transform));

        for (int i = Random.Range(5, 10); i > 0; i--)
        {
            GameObject newBody = AddBody(body);
            bodiesSprites.Add(newBody.GetComponentsInChildren<SpriteRenderer>()[1]);
            StartCoroutine(ScaleAtStart(newBody.transform));
        }

        if (TemporaryData.IsSecretSkinSelected)
        {
            headSpriteRenderer.color = SecretSkin.Colors[0];

            int i = 0;
            while (bodiesSprites[i] != null)
                bodiesSprites[i].color = SecretSkin.Colors[i++];
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        currentDirVec = ReflectDirectionVector(collision.name);
    }

    private void Update()
    {
        if (isMenuLoaded)
        {
            Vector2 moveTarget = new Vector2(transform.position.x + currentDirVec.x, transform.position.y + currentDirVec.y);

            float lookAngle = Vector2.SignedAngle(Vector2.up, currentDirVec);
            transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, lookAngle);

            transform.position = Vector2.MoveTowards(transform.position, moveTarget, currentSpeed * Time.deltaTime);
        }
    }

    private Vector2 ReflectDirectionVector(string name)
    {
        Vector2 normal = Vector2.zero;

        if (name == "BorderBottom")
            normal = Vector2.up;

        if (name == "BorderTop")
            normal = Vector2.down;

        if (name == "BorderLeft")
            normal = Vector2.right;

        if (name == "BorderRight")
            normal = Vector2.left;

        return Vector2.Reflect(currentDirVec, normal);
    }

    private GameObject AddBody(GameObject body)
    {
        GameObject newBody = Instantiate(body, transform.position, Quaternion.identity);
        newBody.GetComponent<MainMenuBody>().SetNextBody(lastBody);
        lastBody = newBody;

        return newBody;
    }

    private IEnumerator ScaleAtStart(Transform gameObjectTransform)
    {
        yield return new WaitForSeconds(0.3f);

        float scale = 0;

        while (scale < 1)
        {
            gameObjectTransform.localScale = new Vector3(scale, scale, 0);
            scale += 0.05f;
            yield return null;
        }

        gameObjectTransform.localScale = new Vector3(1, 1, 0);
    }

    private IEnumerator MoveStartDelay()
    {
        yield return new WaitForSeconds(0.5f);
        isMenuLoaded = true;
    }

    public void UpdateSprites(UIShopButton button)
    {
        headSpriteRenderer.sprite = button.snakeSprites[0];

        foreach (SpriteRenderer body in bodiesSprites)
            body.sprite = button.snakeSprites[1];

        if (TemporaryData.IsSecretSkinSelected)
        {
            headSpriteRenderer.color = SecretSkin.Colors[0];

            int i = 0;
            
            foreach (SpriteRenderer tail in bodiesSprites)
                tail.color = SecretSkin.Colors[i++];
        }

        else
        {
            Color defaultColor = new Color(1, 1, 1);
            headSpriteRenderer.color = defaultColor;

            foreach (SpriteRenderer tail in bodiesSprites)
                tail.color = defaultColor;
        }
    }
}

public class MainMenuBody : MonoBehaviour
{
    private GameObject _nextBody;

    public void SetNextBody(GameObject nextBody)
    {
        _nextBody = nextBody;
    }

    private void Update()
    {
        transform.position = Vector2.Lerp(transform.position, _nextBody.transform.position, 9 * Time.deltaTime);

        Vector2 lookVec = transform.position - _nextBody.transform.position;
        float lookAngle = Vector2.SignedAngle(Vector2.up, lookVec);
        transform.rotation = Quaternion.Euler(0, 0, lookAngle);
    }
}

