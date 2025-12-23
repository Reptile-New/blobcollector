using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuUI : MonoBehaviour
{
    [Header("Chest Buttons")]
    [SerializeField] private Button chest1Button;
    [SerializeField] private Button chest2Button;

    [Header("Chest Timers")]
    [SerializeField] private TextMeshProUGUI chest1TimerText;
    [SerializeField] private TextMeshProUGUI chest2TimerText;

    [Header("Collection Info")]
    [SerializeField] private TextMeshProUGUI collectionCountText;
    [SerializeField] private TextMeshProUGUI collectionPercentText;

    [Header("Navigation")]
    [SerializeField] private Button viewCollectionButton;

    private void Start()
    {
        // Setup button listeners
        if (chest1Button != null)
            chest1Button.onClick.AddListener(OnChest1Clicked);

        if (chest2Button != null)
            chest2Button.onClick.AddListener(OnChest2Clicked);

        if (viewCollectionButton != null)
            viewCollectionButton.onClick.AddListener(OnViewCollectionClicked);

        // Update UI every second
        InvokeRepeating(nameof(UpdateUI), 0f, 1f);
    }

    private void UpdateUI()
    {
        UpdateChestButtons();
        UpdateCollectionInfo();
    }

    private void UpdateChestButtons()
    {
        // Chest 1
        bool chest1Available = DailyChestManager.Instance.IsChest1Available();
        if (chest1Button != null)
            chest1Button.interactable = chest1Available;

        if (chest1TimerText != null)
        {
            if (chest1Available)
                chest1TimerText.text = "DISPONIBLE!";
            else
                chest1TimerText.text = DailyChestManager.Instance.GetTimeUntilChest1String();
        }

        // Chest 2
        bool chest2Available = DailyChestManager.Instance.IsChest2Available();
        if (chest2Button != null)
            chest2Button.interactable = chest2Available;

        if (chest2TimerText != null)
        {
            if (chest2Available)
                chest2TimerText.text = "DISPONIBLE!";
            else
                chest2TimerText.text = DailyChestManager.Instance.GetTimeUntilChest2String();
        }
    }

    private void UpdateCollectionInfo()
    {
        int count = BlobCollectionManager.Instance.GetCollectionCount();
        int total = BlobDatabase.Instance.GetAllBlobs().Count;
        float percent = BlobCollectionManager.Instance.GetCompletionPercentage();

        if (collectionCountText != null)
            collectionCountText.text = $"{count}/{total}";

        if (collectionPercentText != null)
            collectionPercentText.text = $"{percent:F1}%";
    }

    private void OnChest1Clicked()
    {
        Debug.Log("Coffre 1 cliqué!");
        ChestRewardSystem.Instance.OpenChest1();
    }

    private void OnChest2Clicked()
    {
        Debug.Log("Coffre 2 cliqué!");
        ChestRewardSystem.Instance.OpenChest2();
    }

    private void OnViewCollectionClicked()
    {
        Debug.Log("Voir la collection!");
        // TODO: Ouvrir l'écran de collection
    }

    private void OnDestroy()
    {
        CancelInvoke(nameof(UpdateUI));
    }
}
