using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoAdsButton : MonoBehaviour
{
    private void Awake()
    {
        if (!PersistentData.Ads.IsEnabled())
            gameObject.SetActive(false);
    }

    public void OnPurchaseComplete()
    {
        PersistentData.Ads.TurnOffAds();
    }
}
