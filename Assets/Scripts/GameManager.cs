using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Managers")]
    [SerializeField] private GameObject blobDatabasePrefab;
    [SerializeField] private GameObject blobCollectionManagerPrefab;
    [SerializeField] private GameObject dailyChestManagerPrefab;
    [SerializeField] private GameObject chestRewardSystemPrefab;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeGame();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeGame()
    {
        Debug.Log("=== Initialisation du jeu ===");

        // Instancier les managers s'ils n'existent pas déjà
        if (BlobDatabase.Instance == null && blobDatabasePrefab != null)
            Instantiate(blobDatabasePrefab);

        if (BlobCollectionManager.Instance == null && blobCollectionManagerPrefab != null)
            Instantiate(blobCollectionManagerPrefab);

        if (DailyChestManager.Instance == null && dailyChestManagerPrefab != null)
            Instantiate(dailyChestManagerPrefab);

        if (ChestRewardSystem.Instance == null && chestRewardSystemPrefab != null)
            Instantiate(chestRewardSystemPrefab);

        Debug.Log("=== Jeu initialisé ===");
    }

    private void Start()
    {
        // Afficher les infos de debug au démarrage
        LogGameStatus();
    }

    private void LogGameStatus()
    {
        Debug.Log("--- État du jeu ---");
        Debug.Log($"Blobs dans la base: {BlobDatabase.Instance?.GetAllBlobs().Count ?? 0}");
        Debug.Log($"Blobs collectés: {BlobCollectionManager.Instance?.GetCollectionCount() ?? 0}");
        Debug.Log($"Coffre 1 disponible: {DailyChestManager.Instance?.IsChest1Available()}");
        Debug.Log($"Coffre 2 disponible: {DailyChestManager.Instance?.IsChest2Available()}");
        Debug.Log("-------------------");
    }
}
