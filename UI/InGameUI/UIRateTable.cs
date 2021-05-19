using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRateTable : MonoBehaviour
{
    public List<VirusScore> playersList;

    public Text[] scores;
    public Text[] names;

    public static UIRateTable Singletone { get; private set; }

    private void Awake() => Singletone = this;

    private void Start() => ListUpdate();

#if UNITY_ANDROID
    private readonly string _pointsName = " infected";
#endif
#if UNITY_IOS
    private readonly string _pointsName = " people";
#endif



    public void ListUpdate()
    {
        Sort();

        for (int i = scores.Length - 1; i >= 0; i--)
        {
            scores[i].text = playersList[i].Score.ToString() + _pointsName;
            names[i].text = playersList[i].name;
        }
    }

    public void Sort()
    {
        for (int i = 0; i < playersList.Count - 1; i++)
        {
            VirusScore scoreBus;

            if (playersList[i] == null)
                playersList.RemoveAt(i);

            for (int j = i + 1; j < playersList.Count - 1; j++)
            {
                if (playersList[j] == null)
                    j++;

                if (playersList[i].Score < playersList[j].Score)
                {
                    scoreBus = playersList[i];
                    playersList[i] = playersList[j];
                    playersList[j] = scoreBus;
                }
            }
        }
    }
}
