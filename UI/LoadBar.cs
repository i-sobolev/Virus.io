using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadBar : MonoBehaviour
{
    public Image image;
    public RectTransform rectTransform;
    private float fillValue = 0;
    public Text text;

    private void Awake()
    {
        StartCoroutine(SmoothShow());
        image.fillAmount = 0;
    }

    private void Update()
    {
        rectTransform.Rotate(0, 0, 4);
        image.fillAmount = Mathf.Sin(fillValue) / 2 + 0.5f;
        fillValue += Time.deltaTime * 2;
    }

    private IEnumerator SmoothShow()
    {
        for (float alpha = 0; alpha < 1; alpha += Time.deltaTime / 0.2f)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
            text.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
            yield return null;
        }

        image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
        text.color = new Color(image.color.r, image.color.g, image.color.b, 1);
    }
}
