using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VirusAnimator
{
    public class Twiner : MonoBehaviour
    {
        public static IEnumerator GrowBodyAtNewBody(GameObject editableObject, float time, float scaleTarget)
        {
            Vector3 baseScale = editableObject.transform.localScale;
            Vector3 targetScale = new Vector3(baseScale.x + scaleTarget, baseScale.y + scaleTarget);

            for (float i = 0; i < 1; i += Time.deltaTime / time)
            {
                editableObject.transform.localScale = Vector3.Lerp(baseScale, targetScale, i);
                yield return null;
            }

            if (editableObject != null && baseScale != editableObject.GetComponent<Tail>().head.transform.localScale)
                baseScale = editableObject.GetComponent<Tail>().head.transform.localScale;

            for (float i = 0; i < 1; i += Time.deltaTime / time)
            {
                editableObject.transform.localScale = Vector3.Lerp(targetScale, baseScale, i);
                yield return null;
            }

            editableObject.transform.localScale = baseScale;
        }

        public static IEnumerator GrowVirus(GameObject editableObject, GameObject head)
        {
            Vector3 baseScale = editableObject.transform.localScale;
            Vector3 targetScale = new Vector3(head.transform.localScale.x + 0.15f, head.transform.localScale.y + 0.15f);

            for (float i = 0; i < 1; i += Time.deltaTime / 0.5f)
            {
                editableObject.transform.localScale = Vector3.Lerp(baseScale, targetScale, i);
                yield return null;
            }

            editableObject.transform.localScale = targetScale;
        }

        public static float SmoothSquarer(float x)
        {
            return x < 0.5f ? x * x * 2 : (1 - (1 - x) * (1 - x) * 2);
        }

        public static IEnumerator VirusFlicker(GameObject virusGameObject)
        {
            var sprite = virusGameObject.GetComponent<SpriteRenderer>();

            if (virusGameObject.GetComponent<VirusHead>())
                virusGameObject.GetComponent<VirusHead>().IsNotFlicker = false;

            for (int i = 5; i > 0; i--)
            {
                for (float alpha = 1; alpha > 0.1f; alpha -= Time.deltaTime * 5)
                {
                    sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, alpha);
                    yield return null;
                }

                for (float alpha = 0.1f; alpha < 1; alpha += Time.deltaTime * 5)
                {
                    sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, alpha);
                    yield return null;
                }

                yield return null;
            }

            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1f);

            if (virusGameObject.GetComponent<VirusHead>())
            {
                virusGameObject.GetComponent<VirusHead>().IsNotFlicker = true;
                var virusCollider = virusGameObject.GetComponent<CircleCollider2D>();

                // OnTriggerEnter refresh
                virusCollider.enabled = false;
                yield return null;
                virusCollider.enabled = true;
            }
        }

        public static IEnumerator UnsizeAndDestroy(GameObject gameObj)
        {
            Vector3 baseScale = new Vector3(gameObj.transform.localScale.x, gameObj.transform.localScale.y, 0);
            float unsizeStep = 0.0001f;

            while (unsizeStep < 0.4f)
            {
                gameObj.transform.localScale = baseScale - new Vector3(unsizeStep, unsizeStep, 0);
                unsizeStep *= 2;
                yield return null;
            }

            Destroy(gameObj);
        }

        public static IEnumerator UnsizeAndReposition(Vector2 newPosition, GameObject gameObj)
        {

            Vector3 baseScale = new Vector3(gameObj.transform.localScale.x, gameObj.transform.localScale.y, 0);
            float unsizeStep = 0.0001f;

            while (unsizeStep < 0.4f)
            {
                gameObj.transform.localScale = baseScale - new Vector3(unsizeStep, unsizeStep, 0);
                unsizeStep *= 2;
                yield return null;
            }

            gameObj.transform.position = newPosition;
            gameObj.transform.localScale = baseScale;
        }

        public static IEnumerator UISigner(GameObject UIElement)
        {
            RectTransform elementTransform = UIElement.GetComponent<RectTransform>();

            Vector3 baseScale = new Vector3(1, 1, 0);
            Vector3 targetScale = new Vector3(0.8f, 0.8f, 0);

            for (int i = 0; i < 2; i++)
            {
                for (float scaleValue = 0; scaleValue < 1; scaleValue += Time.deltaTime * 3)
                {
                    elementTransform.localScale = Vector3.Lerp(baseScale, targetScale, SmoothSquarer(scaleValue));
                    yield return null;
                }

                for (float scaleValue = 1; scaleValue > 0; scaleValue -= Time.deltaTime * 3)
                {
                    elementTransform.localScale = Vector3.Lerp(baseScale, targetScale, SmoothSquarer(scaleValue));
                    yield return null;
                }
            }

            elementTransform.localScale = baseScale;
        }

        public static IEnumerator PanelSmoothScaleChange(float from, float to, GameObject panel)
        {
            RectTransform PanelRectTransform = panel.GetComponent<RectTransform>();
            var scale = from;

            if (from > to)
            {
                while (scale > to)
                {
                    PanelRectTransform.localScale = new Vector3(SmoothSquarer(scale), SmoothSquarer(scale), 1);
                    scale -= Time.deltaTime * 2;

                    yield return new WaitForEndOfFrame();
                }

                panel.SetActive(false);
            }

            else
            {
                panel.SetActive(true);

                while (scale < to)
                {
                    PanelRectTransform.localScale = new Vector3(SmoothSquarer(scale), SmoothSquarer(scale), 1);
                    scale += Time.deltaTime * 2;

                    yield return new WaitForEndOfFrame();
                }
            }

            PanelRectTransform.localScale = new Vector3(to, to, 1);
        }

        public static IEnumerator ImageSmoothAlphaChange(float from, float to, Image image)
        {
            var alpha = from;

            if (from > to)
            {
                while (alpha > to)
                {
                    image.color = new Color(0, 0, 0, alpha);
                    alpha -= Time.deltaTime * 2;

                    yield return new WaitForEndOfFrame();
                }

                image.gameObject.SetActive(false);
            }

            else
            {
                image.gameObject.SetActive(true);

                while (alpha < to)
                {
                    image.color = new Color(0, 0, 0, alpha);
                    alpha += Time.deltaTime * 2;

                    yield return new WaitForEndOfFrame();
                }
            }

            image.color = new Color(0, 0, 0, to);
        }
    }
}
