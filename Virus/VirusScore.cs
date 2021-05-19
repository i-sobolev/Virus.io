using UnityEngine;

public class VirusScore : MonoBehaviour
{
    public int Score;
    public int LastScore;
    public UIRateTable RateTable;

    private void Awake()
    {
        RateTable = UIRateTable.Singletone;
        RateTable.playersList.Add(this);
    }
}
