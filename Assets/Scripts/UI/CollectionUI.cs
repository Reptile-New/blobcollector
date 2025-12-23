using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class CollectionUI : MonoBehaviour
{
    [Header("Grid")]
    [SerializeField] private Transform blobGridParent;
    [SerializeField] private GameObject blobSlotPrefab;

    [Header("Info")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private Button backButton;

    private List<GameObject> blobSlots = new List<GameObject>();

    private void Start()
    {
        if (backButton != null)
            backButton.onClick.AddListener(OnBackClicked);

        PopulateCollection();
    }

    private void PopulateCollection()
    {
        // Clear existing slots
        foreach (var slot in blobSlots)
        {
            if (slot != null)
                Destroy(slot);
        }
        blobSlots.Clear();

        List<BlobData> allBlobs = BlobDatabase.Instance.GetAllBlobs();

        foreach (BlobData blob in allBlobs)
        {
            GameObject slotObj = Instantiate(blobSlotPrefab, blobGridParent);
            blobSlots.Add(slotObj);

            BlobSlotUI slotUI = slotObj.GetComponent<BlobSlotUI>();
            if (slotUI != null)
            {
                bool isCollected = BlobCollectionManager.Instance.HasBlob(blob.id);
                slotUI.Setup(blob, isCollected);
            }
        }

        UpdateTitle();
    }

    private void UpdateTitle()
    {
        if (titleText != null)
        {
            int count = BlobCollectionManager.Instance.GetCollectionCount();
            int total = BlobDatabase.Instance.GetAllBlobs().Count;
            titleText.text = $"Ma Collection ({count}/{total})";
        }
    }

    private void OnBackClicked()
    {
        Debug.Log("Retour au menu principal");
        // TODO: Retourner au menu principal
        gameObject.SetActive(false);
    }

    public void RefreshCollection()
    {
        PopulateCollection();
    }
}
