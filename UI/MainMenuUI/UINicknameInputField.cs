using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UINicknameInputField : MonoBehaviour
{
    public GameObject inputNamePanel;
    public GameObject thisPanel;
    private void Awake()
    {
        if (!PersistentData.Nickname.HasKey() || PersistentData.Nickname.Get().Length == 0)
        {
            inputNamePanel.SetActive(true);
            thisPanel.SetActive(false);
        }

        else
        {
            GetComponent<InputField>().text = PersistentData.Nickname.Get();
        }
    }

    public void SetPlayerName()
    {
        string nickname = GetComponent<Text>().text;
        PersistentData.Nickname.Set(nickname);

        PersistentData.Save();
    }
}
