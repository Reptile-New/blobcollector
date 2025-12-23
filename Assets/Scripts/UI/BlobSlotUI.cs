using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BlobSlotUI : MonoBehaviour
{
    [SerializeField] private Image blobImage;
    [SerializeField] private TextMeshProUGUI blobNameText;
    [SerializeField] private TextMeshProUGUI blobIdText;
    [SerializeField] private GameObject lockedOverlay;
    [SerializeField] private Image rarityBorder;

    [Header("Rarity Colors")]
    [SerializeField] private Color commonColor = Color.gray;
    [SerializeField] private Color rareColor = Color.blue;
    [SerializeField] private Color epicColor = Color.magenta;
    [SerializeField] private Color legendaryColor = Color.yellow;

    private BlobData blobData;
    private bool isCollected;

    public void Setup(BlobData blob, bool collected)
    {
        blobData = blob;
        isCollected = collected;

        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        if (isCollected)
        {
            // Blob collecté - montrer toutes les infos
            if (blobImage != null)
            {
                blobImage.sprite = blobData.sprite;
                blobImage.color = Color.white;
            }

            if (blobNameText != null)
                blobNameText.text = blobData.name;

            if (blobIdText != null)
                blobIdText.text = $"#{blobData.id}";

            if (lockedOverlay != null)
                lockedOverlay.SetActive(false);

            if (rarityBorder != null)
                rarityBorder.color = GetRarityColor(blobData.rarity);
        }
        else
        {
            // Blob non collecté - montrer silhouette
            if (blobImage != null)
            {
                blobImage.sprite = blobData.sprite;
                blobImage.color = Color.black;
            }

            if (blobNameText != null)
                blobNameText.text = "???";

            if (blobIdText != null)
                blobIdText.text = $"#{blobData.id}";

            if (lockedOverlay != null)
                lockedOverlay.SetActive(true);

            if (rarityBorder != null)
                rarityBorder.color = Color.gray;
        }
    }

    private Color GetRarityColor(BlobRarity rarity)
    {
        switch (rarity)
        {
            case BlobRarity.Common:
                return commonColor;
            case BlobRarity.Rare:
                return rareColor;
            case BlobRarity.Epic:
                return epicColor;
            case BlobRarity.Legendary:
                return legendaryColor;
            default:
                return Color.white;
        }
    }
}
