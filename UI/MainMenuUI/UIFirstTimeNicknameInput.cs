using UnityEngine;
using UnityEngine.UI;

public class UIFirstTimeNicknameInput : MonoBehaviour
{
    public GameObject menuNicknamePanel;
    public MainMenuAnimator menuAnimator;
    public GameObject thisInputField;

    private void Awake()
    {
        GetComponent<InputField>().text = "Enter your name";

    }

    public void SetPlayerName()
    {
        string nickname = GetComponent<Text>().text;
        PersistentData.Nickname.Set(nickname);
        PersistentData.Save();

        menuAnimator.ShowMenu();

        menuNicknamePanel.SetActive(true);
        menuNicknamePanel.GetComponentInChildren<InputField>().text = nickname;
        thisInputField.SetActive(false);

        FacebookSDK.FirstOpenEventLog(); // FACEBOOK
    }
}