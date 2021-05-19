using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubTimer : MonoBehaviour
{
    public DateTime ExpiredDate = new DateTime();
    public Text Text;
    public string subId;

    void FixedUpdate()
    {
        if (ExpiredDate == new DateTime())
            return;

        TimeSpan timeSpan = ExpiredDate - DateTime.Now;
        Text.text = timeSpan.ToString("hh\\:mm\\:ss");
    }
}
