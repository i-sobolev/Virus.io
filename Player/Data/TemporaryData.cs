using UnityEngine;

public class TemporaryData : MonoBehaviour
{
    public static string PlayerNickName => PersistentData.Nickname.Get();
    public static Sprite PlayerHeadSprite;
    public static Sprite PlayerBodySprite;
    public static bool IsSpeedButtonEnabled;
    public static bool IsSplashButtonEnabled;
    public static bool IsSplashAndSpeedButtonsEnabled;
    public static bool IsSecretSkinSelected;

    public static int interstitialAdCounter;
    public static bool IsRewardEarned;
    public static int SavedKills;
    public static int SavedScore;

    public static int CurrentSplashColorNumber;
    public static float BoostIncreaseSpeed;
    public static float BoostDecreaseSpeed;
    public static float VirusBoostedSpeed;
    public static float SplashIncreaseSpeed;
    public static bool IsParticlesEnabled;

    public static bool IsAdMobInitialized;
    public static bool IsBannerLoaded;

    public static bool isSubscribed = false;
    public static int lastSelectevSubSkin = 0;

    public static void SetSnakeSkin(UIShopButton button)
    {
        PlayerHeadSprite = button.snakeSprites[0];
        PlayerBodySprite = button.snakeSprites[1];
        PersistentData.VirusSkin.CurrentSelectedSkin.Set(button.skinNumber);
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void SetFrameRateTarget()
    {
        Application.targetFrameRate = 90;
    }
}