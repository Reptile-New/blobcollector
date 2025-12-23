using UnityEngine;
using System;

public class DailyChestManager : MonoBehaviour
{
    public static DailyChestManager Instance { get; private set; }

    // Horaires des coffres (en heures)
    private const int CHEST_1_HOUR = 12; // Midi
    private const int CHEST_2_HOUR = 17; // 17h

    private const string LAST_CHEST_1_KEY = "LastChest1Claim";
    private const string LAST_CHEST_2_KEY = "LastChest2Claim";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool IsChest1Available()
    {
        return IsChestAvailable(LAST_CHEST_1_KEY, CHEST_1_HOUR);
    }

    public bool IsChest2Available()
    {
        return IsChestAvailable(LAST_CHEST_2_KEY, CHEST_2_HOUR);
    }

    private bool IsChestAvailable(string prefsKey, int targetHour)
    {
        // Si jamais réclamé, disponible
        if (!PlayerPrefs.HasKey(prefsKey))
            return true;

        string lastClaimStr = PlayerPrefs.GetString(prefsKey);
        DateTime lastClaim = DateTime.Parse(lastClaimStr);
        DateTime now = DateTime.Now;

        // Vérifier si on est passé à un nouveau jour
        if (now.Date > lastClaim.Date)
        {
            // Nouveau jour, vérifier si on a atteint l'heure du coffre
            return now.Hour >= targetHour;
        }

        // Même jour, pas encore disponible
        return false;
    }

    public TimeSpan GetTimeUntilChest1()
    {
        return GetTimeUntilChest(LAST_CHEST_1_KEY, CHEST_1_HOUR);
    }

    public TimeSpan GetTimeUntilChest2()
    {
        return GetTimeUntilChest(LAST_CHEST_2_KEY, CHEST_2_HOUR);
    }

    private TimeSpan GetTimeUntilChest(string prefsKey, int targetHour)
    {
        DateTime now = DateTime.Now;
        DateTime nextChestTime;

        if (!PlayerPrefs.HasKey(prefsKey))
        {
            // Jamais réclamé
            if (now.Hour >= targetHour)
            {
                // Disponible aujourd'hui
                return TimeSpan.Zero;
            }
            else
            {
                // Disponible plus tard aujourd'hui
                nextChestTime = new DateTime(now.Year, now.Month, now.Day, targetHour, 0, 0);
                return nextChestTime - now;
            }
        }

        string lastClaimStr = PlayerPrefs.GetString(prefsKey);
        DateTime lastClaim = DateTime.Parse(lastClaimStr);

        // Calculer la prochaine occurrence
        if (now.Date > lastClaim.Date && now.Hour >= targetHour)
        {
            // Disponible maintenant
            return TimeSpan.Zero;
        }
        else if (now.Date > lastClaim.Date)
        {
            // Aujourd'hui mais pas encore l'heure
            nextChestTime = new DateTime(now.Year, now.Month, now.Day, targetHour, 0, 0);
        }
        else
        {
            // Demain
            nextChestTime = new DateTime(now.Year, now.Month, now.Day, targetHour, 0, 0).AddDays(1);
        }

        return nextChestTime - now;
    }

    public void ClaimChest1()
    {
        ClaimChest(LAST_CHEST_1_KEY);
    }

    public void ClaimChest2()
    {
        ClaimChest(LAST_CHEST_2_KEY);
    }

    private void ClaimChest(string prefsKey)
    {
        PlayerPrefs.SetString(prefsKey, DateTime.Now.ToString());
        PlayerPrefs.Save();
        Debug.Log($"Coffre réclamé: {prefsKey}");
    }

    public string GetTimeUntilChest1String()
    {
        return FormatTimeSpan(GetTimeUntilChest1());
    }

    public string GetTimeUntilChest2String()
    {
        return FormatTimeSpan(GetTimeUntilChest2());
    }

    private string FormatTimeSpan(TimeSpan time)
    {
        if (time.TotalSeconds <= 0)
            return "Disponible!";

        if (time.TotalHours >= 1)
            return $"{time.Hours}h {time.Minutes}m";
        else
            return $"{time.Minutes}m {time.Seconds}s";
    }
}
