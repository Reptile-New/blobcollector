using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class BlobDatabase : MonoBehaviour
{
    public static BlobDatabase Instance { get; private set; }

    [SerializeField] private Sprite[] blobSprites;
    private List<BlobData> allBlobs = new List<BlobData>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeDatabase();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeDatabase()
    {
        // Charger tous les sprites des blobs depuis Resources
        blobSprites = Resources.LoadAll<Sprite>("Blobs");

        if (blobSprites.Length == 0)
        {
            Debug.LogWarning("Aucun sprite de blob trouvé dans Resources/Blobs");
            return;
        }

        // Créer les données pour chaque blob
        for (int i = 0; i < blobSprites.Length; i++)
        {
            BlobRarity rarity = AssignRarity(i);
            BlobData blob = new BlobData(
                id: i + 1,
                name: $"Blob #{i + 1}",
                sprite: blobSprites[i],
                rarity: rarity
            );
            allBlobs.Add(blob);
        }

        Debug.Log($"Base de données initialisée avec {allBlobs.Count} blobs");
    }

    private BlobRarity AssignRarity(int index)
    {
        // Répartition des raretés sur les 50 blobs
        // Blobs 1-30: Common (60%)
        // Blobs 31-45: Rare (30%)
        // Blobs 46-49: Epic (8%)
        // Blob 50: Legendary (2%)

        if (index < 30) return BlobRarity.Common;
        if (index < 45) return BlobRarity.Rare;
        if (index < 49) return BlobRarity.Epic;
        return BlobRarity.Legendary;
    }

    public BlobData GetBlobById(int id)
    {
        return allBlobs.FirstOrDefault(b => b.id == id);
    }

    public List<BlobData> GetAllBlobs()
    {
        return new List<BlobData>(allBlobs);
    }

    public BlobData GetRandomBlobByRarity()
    {
        float roll = Random.Range(0f, 100f);
        BlobRarity targetRarity;

        if (roll < 60f) // 60% Common
            targetRarity = BlobRarity.Common;
        else if (roll < 90f) // 30% Rare
            targetRarity = BlobRarity.Rare;
        else if (roll < 99f) // 9% Epic
            targetRarity = BlobRarity.Epic;
        else // 1% Legendary
            targetRarity = BlobRarity.Legendary;

        var blobsOfRarity = allBlobs.Where(b => b.rarity == targetRarity).ToList();

        if (blobsOfRarity.Count == 0)
            return allBlobs[Random.Range(0, allBlobs.Count)];

        return blobsOfRarity[Random.Range(0, blobsOfRarity.Count)];
    }
}
