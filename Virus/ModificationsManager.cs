using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModificationsManager : MonoBehaviour
{
    public GameObject speedButton;
    public GameObject splashButton;
    public GameObject[] splashAndSpeedButtons;

    public GameObject splashParticles;
    public GameObject speedParticles;
    public GameObject splashSpeedParticles;
    public GameObject secretParticles;

    private void Start()
    {
        if (TemporaryData.IsSpeedButtonEnabled)
        {
            speedButton.SetActive(true);
            
            if (TemporaryData.IsParticlesEnabled)
                speedParticles.SetActive(true);
        }

        if (TemporaryData.IsSplashButtonEnabled)
        {
            splashButton.SetActive(true);
            
            if (TemporaryData.IsParticlesEnabled)
                splashParticles.SetActive(true);
        }

        if (TemporaryData.IsSplashAndSpeedButtonsEnabled)
        {
            SetActiveSplashAndSpeedButtons();

            if (TemporaryData.IsSecretSkinSelected)
                secretParticles.SetActive(true);
            
            else
                splashSpeedParticles.SetActive(true);
        }
    }

    private void SetActiveSplashAndSpeedButtons()
    {
        foreach (GameObject btn in splashAndSpeedButtons)
            btn.SetActive(true);
    }
}
