using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class BlobCollectionManager : MonoBehaviour
{
    public static BlobCollectionManager Instance { get; private set; }

    private HashSet<int> collectedBlobIds = new HashSet<int>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadCollection();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddBlobToCollection(int blobId)
    {
        if (!collectedBlobIds.Contains(blobId))
        {
            collectedBlobIds.Add(blobId);
            Debug.Log($"Nouveau blob collecté! ID: {blobId}");
            SaveCollection();
        }
        else
        {
            Debug.Log($"Blob {blobId} déjà dans la collection (doublon)");
        }
    }

    public bool HasBlob(int blobId)
    {
        return collectedBlobIds.Contains(blobId);
    }

    public List<int> GetCollectedBlobIds()
    {
        return collectedBlobIds.ToList();
    }

    public int GetCollectionCount()
    {
        return collectedBlobIds.Count;
    }

    public float GetCompletionPercentage()
    {
        int totalBlobs = BlobDatabase.Instance.GetAllBlobs().Count;
        if (totalBlobs == 0) return 0f;
        return (float)collectedBlobIds.Count / totalBlobs * 100f;
    }

    private void SaveCollection()
    {
        string json = JsonUtility.ToJson(new SerializableCollection { blobIds = collectedBlobIds.ToList() });
        PlayerPrefs.SetString("BlobCollection", json);
        PlayerPrefs.Save();
    }

    private void LoadCollection()
    {
        if (PlayerPrefs.HasKey("BlobCollection"))
        {
            string json = PlayerPrefs.GetString("BlobCollection");
            SerializableCollection collection = JsonUtility.FromJson<SerializableCollection>(json);
            collectedBlobIds = new HashSet<int>(collection.blobIds);
            Debug.Log($"Collection chargée: {collectedBlobIds.Count} blobs");
        }
        else
        {
            Debug.Log("Nouvelle collection créée");
        }
    }

    [System.Serializable]
    private class SerializableCollection
    {
        public List<int> blobIds;
    }
}
