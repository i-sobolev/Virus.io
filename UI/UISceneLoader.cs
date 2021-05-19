using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UISceneLoader : MonoBehaviour
{
    public GameObject loadScreen;
    public GameObject[] otherUIelements;

    public void TurnOnLoadScreen(string scene)
    {
        StartCoroutine(SmoothShowAndLoad(scene, loadScreen, 0.2f));

        otherUIelements[0].SetActive(true);
        otherUIelements[1].SetActive(scene != "MainMenu");
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public static void RevivePlayer()
    {
        SceneManager.LoadScene("MainScene");
    }

    public IEnumerator SmoothShowAndLoad(string scene, GameObject editableObject, float time)
    {
        var image = editableObject.GetComponent<Image>();

        for (float alpha = 0; alpha < 1; alpha += Time.deltaTime / time)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
            yield return null;
        }

        image.color = new Color(image.color.r, image.color.g, image.color.b, 1);

        float loadTime = scene != "MainMenu" ? Random.Range(1f, 2.5f) : 1f;
        yield return new WaitForSeconds(loadTime);

        LoadScene(scene);
    }
}
