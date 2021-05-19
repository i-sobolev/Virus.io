using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuAnimator : MonoBehaviour
{
    [SerializeField] public List<GameObject> sceneElements;
    private float _pitch = 1.6f;

    private void Awake()
    {
        foreach(GameObject element in sceneElements)
           element.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
    }

    private void Start()
    {
        if (PersistentData.Nickname.HasKey() || PersistentData.Nickname.Get().Length != 0)
            ShowMenu();
    }

    private IEnumerator UIElementShowAnim(float time, int numberOfElement)
    {
        var rectTransform = sceneElements[numberOfElement].GetComponent<RectTransform>();
        var isRekursed = false;
        var buttonAudioSource = sceneElements[numberOfElement].GetComponent<AudioSource>(); 

        buttonAudioSource.pitch = _pitch += 0.2f;
        buttonAudioSource.CheckIsAudioEnabledAndPlay();

        for (float value = 0; value < 1; value +=Time.deltaTime / time)
        {
            float easeValue = Ease(value);
            rectTransform.localScale = new Vector3(easeValue, easeValue, 0);

            if (value > 0.5 && !isRekursed && numberOfElement < sceneElements.Count - 1)
            {
                isRekursed = true;
                StartCoroutine(UIElementShowAnim(time, numberOfElement + 1));
            }

            yield return null;
        }

        buttonAudioSource.pitch = 1;

        rectTransform.localScale = new Vector3(1, 1, 0);
    }

    private float Ease(float x)
    {
        return x < 0.5f ? 2 * x * x : 1 - (-2 * x + 2) * (-2 * x + 2) / 2;
    }

    public void ShowMenu()
    {
        StartCoroutine(UIElementShowAnim(0.2f, 0));
        FindObjectOfType<MainMenuVirus>().TurnOnMainMenuVirus();
    }

    public void ButtonAnim (GameObject buttonGameObject)
    {
        if (buttonGameObject.GetComponent<AudioSource>())
            buttonGameObject.GetComponent<AudioSource>().CheckIsAudioEnabledAndPlay();

        StartCoroutine(ButtonPressAnim(buttonGameObject));
    }

    IEnumerator ButtonPressAnim(GameObject buttonGameObject)
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
}
