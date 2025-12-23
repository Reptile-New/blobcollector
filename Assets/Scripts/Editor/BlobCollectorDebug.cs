using UnityEngine;
using UnityEditor;

public class BlobCollectorDebug : EditorWindow
{
    [MenuItem("Tools/Blob Collector/Debug Info")]
    public static void ShowDebugInfo()
    {
        if (!Application.isPlaying)
        {
            EditorUtility.DisplayDialog("Debug Info",
                "Le jeu doit être en mode Play pour afficher les infos de debug.\n\n" +
                "Appuyez sur Play puis relancez cette commande.", "OK");
            return;
        }

        string info = "=== Blob Collector - Debug Info ===\n\n";

        // BlobDatabase
        if (BlobDatabase.Instance != null)
        {
            int totalBlobs = BlobDatabase.Instance.GetAllBlobs().Count;
            info += $"✓ BlobDatabase: {totalBlobs} blobs chargés\n";
        }
        else
        {
            info += "✗ BlobDatabase: NON INITIALISÉ\n";
        }

        // BlobCollectionManager
        if (BlobCollectionManager.Instance != null)
        {
            int collected = BlobCollectionManager.Instance.GetCollectionCount();
            float percent = BlobCollectionManager.Instance.GetCompletionPercentage();
            info += $"✓ Collection: {collected} blobs ({percent:F1}%)\n";
        }
        else
        {
            info += "✗ BlobCollectionManager: NON INITIALISÉ\n";
        }

        // DailyChestManager
        if (DailyChestManager.Instance != null)
        {
            bool chest1 = DailyChestManager.Instance.IsChest1Available();
            bool chest2 = DailyChestManager.Instance.IsChest2Available();
            info += $"✓ Coffre 1: {(chest1 ? "DISPONIBLE" : DailyChestManager.Instance.GetTimeUntilChest1String())}\n";
            info += $"✓ Coffre 2: {(chest2 ? "DISPONIBLE" : DailyChestManager.Instance.GetTimeUntilChest2String())}\n";
        }
        else
        {
            info += "✗ DailyChestManager: NON INITIALISÉ\n";
        }

        // ChestRewardSystem
        if (ChestRewardSystem.Instance != null)
        {
            info += "✓ ChestRewardSystem: PRÊT\n";
        }
        else
        {
            info += "✗ ChestRewardSystem: NON INITIALISÉ\n";
        }

        Debug.Log(info);
        EditorUtility.DisplayDialog("Debug Info", info, "OK");
    }

    [MenuItem("Tools/Blob Collector/Reset Save Data")]
    public static void ResetSaveData()
    {
        if (EditorUtility.DisplayDialog("Reset Save",
            "ATTENTION: Cette action va supprimer:\n\n" +
            "- Toute la collection de blobs\n" +
            "- Les timers des coffres\n\n" +
            "Cette action est IRREVERSIBLE!\n\n" +
            "Continuer?", "Oui, supprimer", "Annuler"))
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();

            Debug.Log("=== Save Data Reset ===");
            EditorUtility.DisplayDialog("Reset Complete",
                "Toutes les données de sauvegarde ont été supprimées.\n\n" +
                "Redémarrez le jeu pour voir les changements.", "OK");
        }
    }

    [MenuItem("Tools/Blob Collector/Unlock All Chests (Debug)")]
    public static void UnlockAllChests()
    {
        if (!Application.isPlaying)
        {
            EditorUtility.DisplayDialog("Unlock Chests",
                "Le jeu doit être en mode Play pour débloquer les coffres.", "OK");
            return;
        }

        // Reset les timers des coffres
        PlayerPrefs.DeleteKey("LastChest1Claim");
        PlayerPrefs.DeleteKey("LastChest2Claim");
        PlayerPrefs.Save();

        Debug.Log("✓ Tous les coffres sont maintenant disponibles!");
        EditorUtility.DisplayDialog("Coffres Débloqués",
            "Les deux coffres sont maintenant disponibles!", "OK");
    }

    [MenuItem("Tools/Blob Collector/Add 10 Random Blobs (Debug)")]
    public static void Add10RandomBlobs()
    {
        if (!Application.isPlaying)
        {
            EditorUtility.DisplayDialog("Add Blobs",
                "Le jeu doit être en mode Play pour ajouter des blobs.", "OK");
            return;
        }

        if (BlobDatabase.Instance == null || BlobCollectionManager.Instance == null)
        {
            EditorUtility.DisplayDialog("Erreur",
                "Les managers ne sont pas initialisés!", "OK");
            return;
        }

        for (int i = 0; i < 10; i++)
        {
            BlobData blob = BlobDatabase.Instance.GetRandomBlobByRarity();
            BlobCollectionManager.Instance.AddBlobToCollection(blob.id);
        }

        int count = BlobCollectionManager.Instance.GetCollectionCount();
        float percent = BlobCollectionManager.Instance.GetCompletionPercentage();

        Debug.Log($"✓ 10 blobs ajoutés! Collection: {count}/50 ({percent:F1}%)");
        EditorUtility.DisplayDialog("Blobs Ajoutés",
            $"10 blobs aléatoires ajoutés!\n\n" +
            $"Collection: {count}/50 ({percent:F1}%)", "OK");
    }
}
