using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using VirusAnimator;

public class UIShopButton : MonoBehaviour
{
    public Sprite[] snakeSprites;
    public Image selectIcon;
    public Image lockIcon;
    public int skinNumber;
    public int cost;
    public Text costText;
    public bool isBought;
    public GameObject costGameObject;
    public UnityEvent selectEvent;//
    public enum ModificationType { None, Splash, SpeedUp, SplashAndSpeedUp };
    public ModificationType modificationType;
    //public bool LvlSkin;
    //public UIShopButton prevSkinLvl;
    public UIShopPanel shopPanel;
    public int splashColorNumber; //0 - green, 1 - purple, 2 - secret

    public float boostIncreaseSpeed, boostDecreaseSpeed, boostedVirusSpeed;
    public float splashIncreaseSpeed;
    public bool isParticlesEnabled;

    public GameObject skinMaxLevel;
    public int skinMaxLevelInt;

    public bool isSecretSkin;
    public GameObject[] classIcons;
    public Sprite[] secretVirusSkin;
    public Image buttonIcon;

    public bool isSelected = false;

    private void Awake()
    {
        if (isSecretSkin)
        {
            if (PersistentData.VirusSkin.SecretSkin.IsUnlocked())
            {
                PersistentData.VirusSkin.Unlock(skinNumber);

                foreach (GameObject gameObject in classIcons)
                    gameObject.SetActive(true);

                for (int i = 0; i <= 1; i++)
                    snakeSprites[i] = secretVirusSkin[i];

                buttonIcon.sprite = secretVirusSkin[2];
            }
        }

        if (skinNumber != 0)
            isBought = PersistentData.VirusSkin.IsUnlocked(skinNumber);  

        if (PersistentData.VirusSkin.CurrentSelectedSkin.Get() == skinNumber)
        {
            selectIcon.enabled = true;
            TemporaryData.SetSnakeSkin(this);
        }

        costText.text = cost.ToString();

        if (isBought)
        {
            costGameObject.SetActive(false);
            skinMaxLevel.SetActive(true);

            lockIcon.enabled = false;

            skinMaxLevelInt = PersistentData.VirusSkin.Level.Get(skinNumber);

            skinMaxLevel.GetComponentInChildren<Text>().text = skinMaxLevelInt.ToString();
        }

        else
        {
            skinMaxLevel.SetActive(false);
            lockIcon.enabled = true;
        }
    }

    //private void Start()
    //{
        //if (LvlSkin && prevSkinLvl.isBought == false)
        //{
        //    lockIcon.enabled = true;
        //    costGameObject.SetActive(false);
        //}
    //}

    public void SetSprites()
    {
        shopPanel.preview.ShowSkinOnPreviewTable(this);

        if (!isBought && isSelected)
        {
            //if (LvlSkin)
            //{
            //    if (prevSkinLvl.isBought)
            //        SellSkin();

            //    else
            //        prevSkinLvl.SingOnCost();
            //}

            //else
            //{
                SellSkin();
            //}
        }

        shopPanel.UpdateCurrentSelectedButton(this);

        if (isBought)
        {
            TemporaryData.SetSnakeSkin(this);
            
            selectEvent.Invoke();

            TemporaryData.IsSecretSkinSelected = isSecretSkin ? true : false;

            FindObjectOfType<MainMenuVirus>().UpdateSprites(this);

            switch (modificationType)
            {
                case ModificationType.None:
                    TemporaryData.IsSpeedButtonEnabled = false;
                    TemporaryData.IsSplashButtonEnabled = false;
                    TemporaryData.IsSplashAndSpeedButtonsEnabled = false;
                    break;

                case ModificationType.Splash:
                    TemporaryData.IsSpeedButtonEnabled = false;
                    TemporaryData.IsSplashButtonEnabled = true;
                    TemporaryData.IsSplashAndSpeedButtonsEnabled = false;
                    break;

                case ModificationType.SpeedUp:
                    TemporaryData.IsSpeedButtonEnabled = true;
                    TemporaryData.IsSplashButtonEnabled = false;
                    TemporaryData.IsSplashAndSpeedButtonsEnabled = false;
                    break;

                case ModificationType.SplashAndSpeedUp:
                    TemporaryData.IsSpeedButtonEnabled = false;
                    TemporaryData.IsSplashButtonEnabled = false;
                    TemporaryData.IsSplashAndSpeedButtonsEnabled = true;

                    break;
            }

            TemporaryData.CurrentSplashColorNumber = splashColorNumber;
            TemporaryData.SplashIncreaseSpeed = splashIncreaseSpeed;
            TemporaryData.VirusBoostedSpeed = boostedVirusSpeed;
            TemporaryData.BoostIncreaseSpeed = boostIncreaseSpeed;
            TemporaryData.BoostDecreaseSpeed = boostDecreaseSpeed;
            TemporaryData.IsParticlesEnabled = isParticlesEnabled;
        }
    }
    
    public void SellSkin()
    {
        if (cost <= PersistentData.DNA.Get() && !isBought)
        {
            PersistentData.DNA.Remove(cost);

            //isBought = true;
            //costGameObject.SetActive(false);
            PersistentData.VirusSkin.Unlock(skinNumber);

            shopPanel.preview.ShowSkinOnPreviewTable(this);
            UnlockSkin();
            //skinMaxLevelInt = 0;
            //skinMaxLevel.SetActive(true);

            //lockIcon.enabled = false;

            FacebookSDK.SkinPurchaseEventLog(skinNumber, cost); // FACEBOOK
        }

        else if (!isBought /*&& !isSecretSkin*/)
        {
            shopPanel.SignOnPurse();
        }
    }

    public void UnlockSkin()
    {
        isBought = true;
        costGameObject.SetActive(false);

        skinMaxLevelInt = 0;
        skinMaxLevel.SetActive(true);

        lockIcon.enabled = false;
    }

    internal void SingOnCost()
    {
        shopPanel.declineSellSkinSound.Play();
        GetComponent<RectTransform>().localScale = new Vector3(1, 1, 0);
        StartCoroutine(Twiner.UISigner(gameObject));
    }

    public void LockRefresh()
    {
        //if (LvlSkin && prevSkinLvl.isBought)
        //{
        //    lockIcon.enabled = false;

        //    if (!isBought)
        //        costGameObject.SetActive(true);
        //}
    }
}