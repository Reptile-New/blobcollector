using UnityEngine;
using System.Collections.Generic;

public class ChestRewardSystem : MonoBehaviour
{
    public static ChestRewardSystem Instance { get; private set; }

    private const int BLOBS_PER_CHEST = 3;

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

    public List<BlobData> OpenChest()
    {
        List<BlobData> rewards = new List<BlobData>();

        for (int i = 0; i < BLOBS_PER_CHEST; i++)
        {
            BlobData blob = BlobDatabase.Instance.GetRandomBlobByRarity();
            rewards.Add(blob);

            // Ajouter à la collection
            BlobCollectionManager.Instance.AddBlobToCollection(blob.id);
        }

        Debug.Log($"Coffre ouvert! {BLOBS_PER_CHEST} blobs obtenus:");
        foreach (var blob in rewards)
        {
            Debug.Log($"  - {blob.name} ({blob.rarity})");
        }

        return rewards;
    }

    public void OpenChest1()
    {
        if (!DailyChestManager.Instance.IsChest1Available())
        {
            Debug.LogWarning("Coffre 1 pas encore disponible!");
            return;
        }

        List<BlobData> rewards = OpenChest();
        DailyChestManager.Instance.ClaimChest1();

        // TODO: Afficher l'UI des récompenses
        OnChestOpened(rewards, 1);
    }

    public void OpenChest2()
    {
        if (!DailyChestManager.Instance.IsChest2Available())
        {
            Debug.LogWarning("Coffre 2 pas encore disponible!");
            return;
        }

        List<BlobData> rewards = OpenChest();
        DailyChestManager.Instance.ClaimChest2();

        // TODO: Afficher l'UI des récompenses
        OnChestOpened(rewards, 2);
    }

    private void OnChestOpened(List<BlobData> rewards, int chestNumber)
    {
        Debug.Log($"=== Coffre {chestNumber} ouvert! ===");
        Debug.Log($"Collection: {BlobCollectionManager.Instance.GetCollectionCount()}/50 ({BlobCollectionManager.Instance.GetCompletionPercentage():F1}%)");

        // Appeler l'événement pour l'UI
        if (OnChestOpenedEvent != null)
            OnChestOpenedEvent.Invoke(rewards, chestNumber);
    }

    // Événement pour notifier l'UI
    public delegate void ChestOpenedDelegate(List<BlobData> rewards, int chestNumber);
    public event ChestOpenedDelegate OnChestOpenedEvent;
}
