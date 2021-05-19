using System;
using UnityEngine;

public class PersistentData : MonoBehaviour
{
    public static class HighScore
    {
        private static readonly string key = "player_high_score";
        public static bool HasKey() => PlayerPrefs.HasKey(key);
        public static int Get() => PlayerPrefs.GetInt(key);
        public static void Set(int value) => PlayerPrefs.SetInt(key, value);

    }

    public static class DNA
    {
        private static readonly string key = "player_dna";
        public static bool HasKey() => PlayerPrefs.HasKey(key);
        public static int Get() => PlayerPrefs.GetInt(key);
        public static void Set(int value) => PlayerPrefs.SetInt(key, value);
        public static void Add(int value) => PlayerPrefs.SetInt(key, Get() + value);
        public static void Remove(int value) => PlayerPrefs.SetInt(key, Get() - value);
    }


    public static class VirusSkin
    {
        public static class SecretSkin
        {
            private static readonly string key = "secret_skin_unlocked";
            public static void Unlock() => PlayerPrefs.SetInt(key, 1);
            public static bool IsUnlocked() => PlayerPrefs.HasKey(key) && PlayerPrefs.GetInt(key) == 1;
        }

        public static class Level
        {
            public static void Set(int skinNumber, int newSkinLevel)
            {
                if (Get(skinNumber) > newSkinLevel)
                    PlayerPrefs.SetInt("skin_" + skinNumber + "_lvl", skinNumber);
            }

            public static int Get(int skinNumber) => PlayerPrefs.GetInt("skin_" + skinNumber + "_lvl");
        }

        public static class CurrentSelectedSkin
        {
            public static void Set(int number)
            {
                PlayerPrefs.SetInt("current_selected_skin_number", number);
                PlayerPrefs.Save();
            }

            public static int Get()
            {
                return PlayerPrefs.GetInt("current_selected_skin_number");
            }
        }



        public static void Unlock(int skinNumber) => PlayerPrefs.SetInt("is_skin_" + skinNumber + "_bought", 1);
        public static bool IsUnlocked(int skinNumber) => PlayerPrefs.GetInt("is_skin_" + skinNumber + "_bought") == 1;
    }

    public static class Nickname
    {
        public static readonly string key = "nickname";
        public static bool HasKey() => PlayerPrefs.HasKey(key);
        public static void Set(string name) => PlayerPrefs.SetString(key, name);
        public static string Get() => PlayerPrefs.GetString(key);
    }

    public static class AudioSettigns
    {
        public static readonly string key = "audio";
        public static bool HasKey() => PlayerPrefs.HasKey(key);
        public static void SetState(bool value) => PlayerPrefs.SetInt(key, value ? 1 : 0);
        public static bool IsEnabled() => PlayerPrefs.GetInt(key) == 1;
    }

    public static class HelpAnimationCounter
    {
        public static readonly string key = "help_animation_counter";
        public static void Set()
        {
            if (PlayerPrefs.HasKey(key))
                PlayerPrefs.SetInt(key, 2);
        }

        public static void Get() => PlayerPrefs.GetInt(key);
        public static void CountOut() => PlayerPrefs.SetInt(key, PlayerPrefs.GetInt(key) - 1);
    }

    public static class Ads
    {
        public static readonly string key = "no_ads";
        public static void TurnOffAds()
        {
            PlayerPrefs.SetInt(key, 1);
            Save();
        }
        public static bool IsEnabled()
        {
            if (!PlayerPrefs.HasKey(key))
                PlayerPrefs.SetInt(key, 0);

            return PlayerPrefs.GetInt(key) == 0;
        }
    }

    public static class RewardDate
    {
        public static readonly string D = "D", M = "M", Y = "Y", h = "h", m = "m", s = "s";
        public static void Set(DateTime dateTime)
        {
            PlayerPrefs.SetInt(D, dateTime.Day);
            PlayerPrefs.SetInt(M, dateTime.Month);
            PlayerPrefs.SetInt(Y, dateTime.Year);
            PlayerPrefs.SetInt(h, dateTime.Hour);
            PlayerPrefs.SetInt(m, dateTime.Minute);
            PlayerPrefs.SetInt(s, dateTime.Second);

            Save();
        }

        public static DateTime Get()
        {
            return new DateTime
                (
                PlayerPrefs.GetInt(Y),
                PlayerPrefs.GetInt(M),
                PlayerPrefs.GetInt(D),
                PlayerPrefs.GetInt(h),
                PlayerPrefs.GetInt(m),
                PlayerPrefs.GetInt(s)
                );
        }

        public static bool HasKey() => PlayerPrefs.HasKey(D);
    }

    public static void Save() => PlayerPrefs.Save();
}