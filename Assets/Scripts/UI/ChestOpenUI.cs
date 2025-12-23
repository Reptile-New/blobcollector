using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class ChestOpenUI : MonoBehaviour
{
    [Header("Reward Display")]
    [SerializeField] private GameObject rewardPanel;
    [SerializeField] private Transform rewardContainer;
    [SerializeField] private GameObject rewardSlotPrefab;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private Button closeButton;
    [SerializeField] private CanvasGroup canvasGroup;

    private void Start()
    {
        if (closeButton != null)
            closeButton.onClick.AddListener(OnCloseClicked);

        // S'abonner à l'événement d'ouverture de coffre
        ChestRewardSystem.Instance.OnChestOpenedEvent += ShowRewards;

        // Cacher au démarrage
        gameObject.SetActive(false);
    }

    private void ShowRewards(List<BlobData> rewards, int chestNumber)
    {
        gameObject.SetActive(true);

        // Update title
        if (titleText != null)
            titleText.text = $"Coffre {chestNumber} - Récompenses!";

        // Clear previous rewards
        foreach (Transform child in rewardContainer)
        {
            Destroy(child.gameObject);
        }

        // Create reward slots
        foreach (BlobData blob in rewards)
        {
            GameObject slotObj = Instantiate(rewardSlotPrefab, rewardContainer);

            // Setup the reward slot
            Image blobImage = slotObj.transform.Find("BlobImage")?.GetComponent<Image>();
            if (blobImage != null)
                blobImage.sprite = blob.sprite;

            TextMeshProUGUI nameText = slotObj.transform.Find("NameText")?.GetComponent<TextMeshProUGUI>();
            if (nameText != null)
                nameText.text = blob.name;

            TextMeshProUGUI rarityText = slotObj.transform.Find("RarityText")?.GetComponent<TextMeshProUGUI>();
            if (rarityText != null)
            {
                rarityText.text = blob.rarity.ToString();
                rarityText.color = GetRarityColor(blob.rarity);
            }

            // Check if it's a new blob
            bool isNew = !BlobCollectionManager.Instance.HasBlob(blob.id);
            GameObject newBadge = slotObj.transform.Find("NewBadge")?.gameObject;
            if (newBadge != null)
                newBadge.SetActive(isNew);
        }

        // Animate in
        StartCoroutine(AnimateIn());
    }

    private IEnumerator AnimateIn()
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            float duration = 0.3f;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / duration);
                yield return null;
            }

            canvasGroup.alpha = 1f;
        }
    }

    private Color GetRarityColor(BlobRarity rarity)
    {
        switch (rarity)
        {
            case BlobRarity.Common:
                return Color.gray;
            case BlobRarity.Rare:
                return new Color(0.3f, 0.5f, 1f); // Blue
            case BlobRarity.Epic:
                return new Color(0.8f, 0.2f, 0.8f); // Purple
            case BlobRarity.Legendary:
                return new Color(1f, 0.8f, 0f); // Gold
            default:
                return Color.white;
        }
    }

    private void OnCloseClicked()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (ChestRewardSystem.Instance != null)
            ChestRewardSystem.Instance.OnChestOpenedEvent -= ShowRewards;
    }
}
