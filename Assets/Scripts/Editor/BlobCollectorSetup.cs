using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class BlobCollectorSetup : EditorWindow
{
    [MenuItem("Tools/Blob Collector/Auto Setup Game")]
    public static void SetupGame()
    {
        if (EditorUtility.DisplayDialog("Auto Setup",
            "Cette opération va créer automatiquement:\n\n" +
            "- Les GameObjects managers\n" +
            "- L'UI du menu principal\n" +
            "- L'UI de collection\n" +
            "- Les prefabs nécessaires\n\n" +
            "Continuer?", "Oui", "Annuler"))
        {
            PerformSetup();
        }
    }

    private static void PerformSetup()
    {
        Debug.Log("=== Début du setup automatique ===");

        // 1. Configurer les sprites
        ConfigureSprites();

        // 2. Créer les managers
        CreateManagers();

        // 3. Créer les prefabs
        CreatePrefabs();

        // 4. Créer l'UI
        CreateMainMenuUI();
        CreateCollectionUI();
        CreateChestOpenUI();

        // 5. Setup de la scène
        SetupScene();

        Debug.Log("=== Setup terminé! ===");
        EditorUtility.DisplayDialog("Setup Complet",
            "Le jeu a été configuré avec succès!\n\n" +
            "Appuyez sur Play pour tester.", "OK");
    }

    private static void ConfigureSprites()
    {
        Debug.Log("Configuration des sprites...");

        string[] guids = AssetDatabase.FindAssets("t:Texture2D", new[] { "Assets/Resources/Blobs" });

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;

            if (importer != null)
            {
                importer.textureType = TextureImporterType.Sprite;
                importer.spriteImportMode = SpriteImportMode.Single;
                importer.spritePixelsPerUnit = 100;
                importer.filterMode = FilterMode.Point;
                importer.textureCompression = TextureImporterCompression.Uncompressed;

                AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
            }
        }

        Debug.Log($"✓ {guids.Length} sprites configurés");
    }

    private static void CreateManagers()
    {
        Debug.Log("Création des managers...");

        // BlobDatabase
        GameObject dbObj = new GameObject("BlobDatabase");
        dbObj.AddComponent<BlobDatabase>();
        CreatePrefab(dbObj, "Assets/Prefabs/BlobDatabase.prefab");

        // BlobCollectionManager
        GameObject collectionObj = new GameObject("BlobCollectionManager");
        collectionObj.AddComponent<BlobCollectionManager>();
        CreatePrefab(collectionObj, "Assets/Prefabs/BlobCollectionManager.prefab");

        // DailyChestManager
        GameObject chestManagerObj = new GameObject("DailyChestManager");
        chestManagerObj.AddComponent<DailyChestManager>();
        CreatePrefab(chestManagerObj, "Assets/Prefabs/DailyChestManager.prefab");

        // ChestRewardSystem
        GameObject rewardObj = new GameObject("ChestRewardSystem");
        rewardObj.AddComponent<ChestRewardSystem>();
        CreatePrefab(rewardObj, "Assets/Prefabs/ChestRewardSystem.prefab");

        // GameManager
        GameObject gmObj = new GameObject("GameManager");
        GameManager gm = gmObj.AddComponent<GameManager>();

        // Assigner les références
        SerializedObject so = new SerializedObject(gm);
        so.FindProperty("blobDatabasePrefab").objectReferenceValue =
            AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/BlobDatabase.prefab");
        so.FindProperty("blobCollectionManagerPrefab").objectReferenceValue =
            AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/BlobCollectionManager.prefab");
        so.FindProperty("dailyChestManagerPrefab").objectReferenceValue =
            AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/DailyChestManager.prefab");
        so.FindProperty("chestRewardSystemPrefab").objectReferenceValue =
            AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/ChestRewardSystem.prefab");
        so.ApplyModifiedProperties();

        CreatePrefab(gmObj, "Assets/Prefabs/GameManager.prefab");

        Debug.Log("✓ Managers créés");
    }

    private static void CreatePrefabs()
    {
        Debug.Log("Création des prefabs UI...");

        // Créer le dossier Prefabs/UI s'il n'existe pas
        if (!AssetDatabase.IsValidFolder("Assets/Prefabs/UI"))
            AssetDatabase.CreateFolder("Assets/Prefabs", "UI");

        // BlobSlot Prefab
        CreateBlobSlotPrefab();

        // RewardSlot Prefab
        CreateRewardSlotPrefab();

        Debug.Log("✓ Prefabs UI créés");
    }

    private static void CreateBlobSlotPrefab()
    {
        GameObject slotObj = new GameObject("BlobSlot");
        RectTransform slotRect = slotObj.AddComponent<RectTransform>();
        slotRect.sizeDelta = new Vector2(120, 140);

        Image bgImage = slotObj.AddComponent<Image>();
        bgImage.color = new Color(0.2f, 0.2f, 0.2f);

        BlobSlotUI slotUI = slotObj.AddComponent<BlobSlotUI>();

        // Rarity Border
        GameObject borderObj = new GameObject("RarityBorder");
        borderObj.transform.SetParent(slotObj.transform);
        RectTransform borderRect = borderObj.AddComponent<RectTransform>();
        borderRect.anchorMin = Vector2.zero;
        borderRect.anchorMax = Vector2.one;
        borderRect.sizeDelta = Vector2.zero;
        Image borderImage = borderObj.AddComponent<Image>();
        borderImage.color = Color.white;

        // Blob Image
        GameObject imageObj = new GameObject("BlobImage");
        imageObj.transform.SetParent(slotObj.transform);
        RectTransform imageRect = imageObj.AddComponent<RectTransform>();
        imageRect.anchorMin = new Vector2(0.5f, 0.5f);
        imageRect.anchorMax = new Vector2(0.5f, 0.5f);
        imageRect.anchoredPosition = new Vector2(0, 10);
        imageRect.sizeDelta = new Vector2(80, 80);
        Image blobImage = imageObj.AddComponent<Image>();

        // Blob ID Text
        GameObject idTextObj = new GameObject("BlobIdText");
        idTextObj.transform.SetParent(slotObj.transform);
        RectTransform idRect = idTextObj.AddComponent<RectTransform>();
        idRect.anchorMin = new Vector2(0, 0);
        idRect.anchorMax = new Vector2(1, 0);
        idRect.anchoredPosition = new Vector2(0, 5);
        idRect.sizeDelta = new Vector2(0, 20);
        TextMeshProUGUI idText = idTextObj.AddComponent<TextMeshProUGUI>();
        idText.alignment = TextAlignmentOptions.Center;
        idText.fontSize = 12;

        // Blob Name Text
        GameObject nameTextObj = new GameObject("BlobNameText");
        nameTextObj.transform.SetParent(slotObj.transform);
        RectTransform nameRect = nameTextObj.AddComponent<RectTransform>();
        nameRect.anchorMin = new Vector2(0, 0);
        nameRect.anchorMax = new Vector2(1, 0);
        nameRect.anchoredPosition = new Vector2(0, 25);
        nameRect.sizeDelta = new Vector2(0, 20);
        TextMeshProUGUI nameText = nameTextObj.AddComponent<TextMeshProUGUI>();
        nameText.alignment = TextAlignmentOptions.Center;
        nameText.fontSize = 14;

        // Locked Overlay
        GameObject lockedObj = new GameObject("LockedOverlay");
        lockedObj.transform.SetParent(slotObj.transform);
        RectTransform lockedRect = lockedObj.AddComponent<RectTransform>();
        lockedRect.anchorMin = Vector2.zero;
        lockedRect.anchorMax = Vector2.one;
        lockedRect.sizeDelta = Vector2.zero;
        Image lockedImage = lockedObj.AddComponent<Image>();
        lockedImage.color = new Color(0, 0, 0, 0.7f);

        // Assigner les références
        SerializedObject so = new SerializedObject(slotUI);
        so.FindProperty("blobImage").objectReferenceValue = blobImage;
        so.FindProperty("blobNameText").objectReferenceValue = nameText;
        so.FindProperty("blobIdText").objectReferenceValue = idText;
        so.FindProperty("lockedOverlay").objectReferenceValue = lockedObj;
        so.FindProperty("rarityBorder").objectReferenceValue = borderImage;
        so.ApplyModifiedProperties();

        CreatePrefab(slotObj, "Assets/Prefabs/UI/BlobSlot.prefab");
    }

    private static void CreateRewardSlotPrefab()
    {
        GameObject slotObj = new GameObject("RewardSlot");
        RectTransform slotRect = slotObj.AddComponent<RectTransform>();
        slotRect.sizeDelta = new Vector2(150, 180);

        Image bgImage = slotObj.AddComponent<Image>();
        bgImage.color = new Color(0.3f, 0.3f, 0.3f);

        // Blob Image
        GameObject imageObj = new GameObject("BlobImage");
        imageObj.transform.SetParent(slotObj.transform);
        RectTransform imageRect = imageObj.AddComponent<RectTransform>();
        imageRect.anchorMin = new Vector2(0.5f, 0.5f);
        imageRect.anchorMax = new Vector2(0.5f, 0.5f);
        imageRect.anchoredPosition = new Vector2(0, 20);
        imageRect.sizeDelta = new Vector2(100, 100);
        imageObj.AddComponent<Image>();

        // Name Text
        GameObject nameObj = new GameObject("NameText");
        nameObj.transform.SetParent(slotObj.transform);
        RectTransform nameRect = nameObj.AddComponent<RectTransform>();
        nameRect.anchorMin = new Vector2(0, 0);
        nameRect.anchorMax = new Vector2(1, 0);
        nameRect.anchoredPosition = new Vector2(0, 40);
        nameRect.sizeDelta = new Vector2(-10, 25);
        TextMeshProUGUI nameText = nameObj.AddComponent<TextMeshProUGUI>();
        nameText.alignment = TextAlignmentOptions.Center;
        nameText.fontSize = 16;

        // Rarity Text
        GameObject rarityObj = new GameObject("RarityText");
        rarityObj.transform.SetParent(slotObj.transform);
        RectTransform rarityRect = rarityObj.AddComponent<RectTransform>();
        rarityRect.anchorMin = new Vector2(0, 0);
        rarityRect.anchorMax = new Vector2(1, 0);
        rarityRect.anchoredPosition = new Vector2(0, 15);
        rarityRect.sizeDelta = new Vector2(-10, 20);
        TextMeshProUGUI rarityText = rarityObj.AddComponent<TextMeshProUGUI>();
        rarityText.alignment = TextAlignmentOptions.Center;
        rarityText.fontSize = 12;

        // New Badge
        GameObject newBadgeObj = new GameObject("NewBadge");
        newBadgeObj.transform.SetParent(slotObj.transform);
        RectTransform badgeRect = newBadgeObj.AddComponent<RectTransform>();
        badgeRect.anchorMin = new Vector2(1, 1);
        badgeRect.anchorMax = new Vector2(1, 1);
        badgeRect.anchoredPosition = new Vector2(-10, -10);
        badgeRect.sizeDelta = new Vector2(50, 30);
        Image badgeImage = newBadgeObj.AddComponent<Image>();
        badgeImage.color = Color.yellow;

        GameObject badgeTextObj = new GameObject("Text");
        badgeTextObj.transform.SetParent(newBadgeObj.transform);
        RectTransform badgeTextRect = badgeTextObj.AddComponent<RectTransform>();
        badgeTextRect.anchorMin = Vector2.zero;
        badgeTextRect.anchorMax = Vector2.one;
        badgeTextRect.sizeDelta = Vector2.zero;
        TextMeshProUGUI badgeText = badgeTextObj.AddComponent<TextMeshProUGUI>();
        badgeText.text = "NEW!";
        badgeText.alignment = TextAlignmentOptions.Center;
        badgeText.fontSize = 12;
        badgeText.color = Color.black;

        CreatePrefab(slotObj, "Assets/Prefabs/UI/RewardSlot.prefab");
    }

    private static void CreateMainMenuUI()
    {
        Debug.Log("Création du Menu Principal...");

        GameObject canvas = GetOrCreateCanvas();

        GameObject menuObj = new GameObject("MainMenu");
        menuObj.transform.SetParent(canvas.transform);
        RectTransform menuRect = menuObj.AddComponent<RectTransform>();
        menuRect.anchorMin = Vector2.zero;
        menuRect.anchorMax = Vector2.one;
        menuRect.sizeDelta = Vector2.zero;

        MainMenuUI menuUI = menuObj.AddComponent<MainMenuUI>();

        // Title
        CreateTitle(menuObj, "Blob Collector", new Vector2(0, -50));

        // Collection Info
        GameObject collectionInfo = CreatePanel(menuObj, "CollectionInfo", new Vector2(0, -120), new Vector2(400, 60));
        GameObject countText = CreateText(collectionInfo, "CountText", "0/50", new Vector2(-50, 0), new Vector2(150, 40));
        GameObject percentText = CreateText(collectionInfo, "PercentText", "0%", new Vector2(100, 0), new Vector2(150, 40));

        // Chest 1
        GameObject chest1Panel = CreateChestPanel(menuObj, "Chest1Panel", new Vector2(-150, -250), 1);
        Button chest1Btn = chest1Panel.GetComponentInChildren<Button>();
        TextMeshProUGUI chest1Timer = chest1Panel.transform.Find("TimerText").GetComponent<TextMeshProUGUI>();

        // Chest 2
        GameObject chest2Panel = CreateChestPanel(menuObj, "Chest2Panel", new Vector2(150, -250), 2);
        Button chest2Btn = chest2Panel.GetComponentInChildren<Button>();
        TextMeshProUGUI chest2Timer = chest2Panel.transform.Find("TimerText").GetComponent<TextMeshProUGUI>();

        // View Collection Button
        GameObject viewBtn = CreateButton(menuObj, "ViewCollectionButton", "Voir Collection", new Vector2(0, -450), new Vector2(300, 60));

        // Assigner les références
        SerializedObject so = new SerializedObject(menuUI);
        so.FindProperty("chest1Button").objectReferenceValue = chest1Btn;
        so.FindProperty("chest2Button").objectReferenceValue = chest2Btn;
        so.FindProperty("chest1TimerText").objectReferenceValue = chest1Timer;
        so.FindProperty("chest2TimerText").objectReferenceValue = chest2Timer;
        so.FindProperty("collectionCountText").objectReferenceValue = countText.GetComponent<TextMeshProUGUI>();
        so.FindProperty("collectionPercentText").objectReferenceValue = percentText.GetComponent<TextMeshProUGUI>();
        so.FindProperty("viewCollectionButton").objectReferenceValue = viewBtn.GetComponent<Button>();
        so.ApplyModifiedProperties();

        Debug.Log("✓ Menu Principal créé");
    }

    private static GameObject CreateChestPanel(GameObject parent, string name, Vector2 position, int chestNum)
    {
        GameObject panel = CreatePanel(parent, name, position, new Vector2(250, 200));

        CreateTitle(panel, $"Coffre {chestNum}", new Vector2(0, 60));

        GameObject button = CreateButton(panel, "OpenButton", "Ouvrir", new Vector2(0, 0), new Vector2(200, 50));

        GameObject timerText = CreateText(panel, "TimerText", "Chargement...", new Vector2(0, -60), new Vector2(200, 30));

        return panel;
    }

    private static void CreateCollectionUI()
    {
        Debug.Log("Création de l'UI Collection...");

        GameObject canvas = GetOrCreateCanvas();

        GameObject collectionObj = new GameObject("CollectionScreen");
        collectionObj.transform.SetParent(canvas.transform);
        RectTransform collectionRect = collectionObj.AddComponent<RectTransform>();
        collectionRect.anchorMin = Vector2.zero;
        collectionRect.anchorMax = Vector2.one;
        collectionRect.sizeDelta = Vector2.zero;
        collectionObj.SetActive(false);

        CollectionUI collectionUI = collectionObj.AddComponent<CollectionUI>();

        // Background
        Image bg = collectionObj.AddComponent<Image>();
        bg.color = new Color(0.1f, 0.1f, 0.1f, 0.95f);

        // Title
        GameObject titleObj = CreateTitle(collectionObj, "Ma Collection", new Vector2(0, -50));

        // Back Button
        GameObject backBtn = CreateButton(collectionObj, "BackButton", "Retour", new Vector2(-300, -50), new Vector2(150, 50));

        // Scroll View
        GameObject scrollView = CreateScrollView(collectionObj, "ScrollView", new Vector2(0, -300), new Vector2(800, 800));
        Transform gridParent = scrollView.transform.Find("Viewport/Content");

        // Assigner les références
        GameObject blobSlotPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/UI/BlobSlot.prefab");

        SerializedObject so = new SerializedObject(collectionUI);
        so.FindProperty("blobGridParent").objectReferenceValue = gridParent;
        so.FindProperty("blobSlotPrefab").objectReferenceValue = blobSlotPrefab;
        so.FindProperty("titleText").objectReferenceValue = titleObj.GetComponent<TextMeshProUGUI>();
        so.FindProperty("backButton").objectReferenceValue = backBtn.GetComponent<Button>();
        so.ApplyModifiedProperties();

        Debug.Log("✓ UI Collection créée");
    }

    private static void CreateChestOpenUI()
    {
        Debug.Log("Création de l'UI Chest Open...");

        GameObject canvas = GetOrCreateCanvas();

        GameObject chestOpenObj = new GameObject("ChestOpenScreen");
        chestOpenObj.transform.SetParent(canvas.transform);
        RectTransform chestOpenRect = chestOpenObj.AddComponent<RectTransform>();
        chestOpenRect.anchorMin = Vector2.zero;
        chestOpenRect.anchorMax = Vector2.one;
        chestOpenRect.sizeDelta = Vector2.zero;
        chestOpenObj.SetActive(false);

        ChestOpenUI chestOpenUI = chestOpenObj.AddComponent<ChestOpenUI>();
        CanvasGroup canvasGroup = chestOpenObj.AddComponent<CanvasGroup>();

        // Background
        Image bg = chestOpenObj.AddComponent<Image>();
        bg.color = new Color(0, 0, 0, 0.8f);

        // Reward Panel
        GameObject rewardPanel = CreatePanel(chestOpenObj, "RewardPanel", Vector2.zero, new Vector2(700, 500));

        // Title
        GameObject titleObj = CreateTitle(rewardPanel, "Récompenses!", new Vector2(0, 200));

        // Reward Container (Horizontal Layout)
        GameObject container = new GameObject("RewardContainer");
        container.transform.SetParent(rewardPanel.transform);
        RectTransform containerRect = container.AddComponent<RectTransform>();
        containerRect.anchorMin = new Vector2(0.5f, 0.5f);
        containerRect.anchorMax = new Vector2(0.5f, 0.5f);
        containerRect.anchoredPosition = Vector2.zero;
        containerRect.sizeDelta = new Vector2(650, 200);
        HorizontalLayoutGroup layout = container.AddComponent<HorizontalLayoutGroup>();
        layout.spacing = 20;
        layout.childAlignment = TextAnchor.MiddleCenter;
        layout.childControlWidth = false;
        layout.childControlHeight = false;

        // Close Button
        GameObject closeBtn = CreateButton(rewardPanel, "CloseButton", "Fermer", new Vector2(0, -200), new Vector2(200, 50));

        // Assigner les références
        GameObject rewardSlotPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/UI/RewardSlot.prefab");

        SerializedObject so = new SerializedObject(chestOpenUI);
        so.FindProperty("rewardPanel").objectReferenceValue = rewardPanel;
        so.FindProperty("rewardContainer").objectReferenceValue = container.transform;
        so.FindProperty("rewardSlotPrefab").objectReferenceValue = rewardSlotPrefab;
        so.FindProperty("titleText").objectReferenceValue = titleObj.GetComponent<TextMeshProUGUI>();
        so.FindProperty("closeButton").objectReferenceValue = closeBtn.GetComponent<Button>();
        so.FindProperty("canvasGroup").objectReferenceValue = canvasGroup;
        so.ApplyModifiedProperties();

        Debug.Log("✓ UI Chest Open créée");
    }

    private static void SetupScene()
    {
        Debug.Log("Setup de la scène...");

        // Ajouter GameManager à la scène
        GameObject gmPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/GameManager.prefab");
        if (gmPrefab != null && GameObject.Find("GameManager") == null)
        {
            PrefabUtility.InstantiatePrefab(gmPrefab);
        }

        // Créer EventSystem si nécessaire
        if (GameObject.FindFirstObjectByType<EventSystem>() == null)
        {
            GameObject eventSystemObj = new GameObject("EventSystem");
            eventSystemObj.AddComponent<EventSystem>();
            eventSystemObj.AddComponent<StandaloneInputModule>();
        }

        Debug.Log("✓ Scène configurée");
    }

    // Helper Methods
    private static GameObject GetOrCreateCanvas()
    {
        Canvas canvas = GameObject.FindFirstObjectByType<Canvas>();

        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("Canvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
        }

        return canvas.gameObject;
    }

    private static GameObject CreatePanel(GameObject parent, string name, Vector2 position, Vector2 size)
    {
        GameObject panel = new GameObject(name);
        panel.transform.SetParent(parent.transform);
        RectTransform rect = panel.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = position;
        rect.sizeDelta = size;
        Image image = panel.AddComponent<Image>();
        image.color = new Color(0.2f, 0.2f, 0.2f, 0.9f);
        return panel;
    }

    private static GameObject CreateTitle(GameObject parent, string text, Vector2 position)
    {
        GameObject titleObj = new GameObject("Title");
        titleObj.transform.SetParent(parent.transform);
        RectTransform rect = titleObj.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = position;
        rect.sizeDelta = new Vector2(400, 50);
        TextMeshProUGUI tmp = titleObj.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = 36;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.fontStyle = FontStyles.Bold;
        return titleObj;
    }

    private static GameObject CreateButton(GameObject parent, string name, string text, Vector2 position, Vector2 size)
    {
        GameObject btnObj = new GameObject(name);
        btnObj.transform.SetParent(parent.transform);
        RectTransform rect = btnObj.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = position;
        rect.sizeDelta = size;

        Image image = btnObj.AddComponent<Image>();
        image.color = new Color(0.3f, 0.6f, 0.9f);

        Button button = btnObj.AddComponent<Button>();

        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(btnObj.transform);
        RectTransform textRect = textObj.AddComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.sizeDelta = Vector2.zero;
        TextMeshProUGUI tmp = textObj.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = 24;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = Color.white;

        return btnObj;
    }

    private static GameObject CreateText(GameObject parent, string name, string text, Vector2 position, Vector2 size)
    {
        GameObject textObj = new GameObject(name);
        textObj.transform.SetParent(parent.transform);
        RectTransform rect = textObj.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = position;
        rect.sizeDelta = size;
        TextMeshProUGUI tmp = textObj.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = 20;
        tmp.alignment = TextAlignmentOptions.Center;
        return textObj;
    }

    private static GameObject CreateScrollView(GameObject parent, string name, Vector2 position, Vector2 size)
    {
        GameObject scrollObj = new GameObject(name);
        scrollObj.transform.SetParent(parent.transform);
        RectTransform scrollRect = scrollObj.AddComponent<RectTransform>();
        scrollRect.anchorMin = new Vector2(0.5f, 0.5f);
        scrollRect.anchorMax = new Vector2(0.5f, 0.5f);
        scrollRect.anchoredPosition = position;
        scrollRect.sizeDelta = size;

        Image scrollBg = scrollObj.AddComponent<Image>();
        scrollBg.color = new Color(0.15f, 0.15f, 0.15f);

        ScrollRect scroll = scrollObj.AddComponent<ScrollRect>();

        // Viewport
        GameObject viewportObj = new GameObject("Viewport");
        viewportObj.transform.SetParent(scrollObj.transform);
        RectTransform viewportRect = viewportObj.AddComponent<RectTransform>();
        viewportRect.anchorMin = Vector2.zero;
        viewportRect.anchorMax = Vector2.one;
        viewportRect.sizeDelta = Vector2.zero;
        viewportObj.AddComponent<Image>();
        viewportObj.AddComponent<Mask>().showMaskGraphic = false;

        // Content
        GameObject contentObj = new GameObject("Content");
        contentObj.transform.SetParent(viewportObj.transform);
        RectTransform contentRect = contentObj.AddComponent<RectTransform>();
        contentRect.anchorMin = new Vector2(0, 1);
        contentRect.anchorMax = new Vector2(0, 1);
        contentRect.pivot = new Vector2(0, 1);
        contentRect.anchoredPosition = Vector2.zero;
        contentRect.sizeDelta = new Vector2(800, 1000);

        GridLayoutGroup grid = contentObj.AddComponent<GridLayoutGroup>();
        grid.cellSize = new Vector2(120, 140);
        grid.spacing = new Vector2(10, 10);
        grid.startCorner = GridLayoutGroup.Corner.UpperLeft;
        grid.startAxis = GridLayoutGroup.Axis.Horizontal;
        grid.childAlignment = TextAnchor.UpperCenter;
        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = 5;

        ContentSizeFitter fitter = contentObj.AddComponent<ContentSizeFitter>();
        fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        scroll.content = contentRect;
        scroll.viewport = viewportRect;
        scroll.horizontal = false;
        scroll.vertical = true;

        return scrollObj;
    }

    private static void CreatePrefab(GameObject obj, string path)
    {
        // Créer les dossiers si nécessaire
        string directory = System.IO.Path.GetDirectoryName(path);
        if (!AssetDatabase.IsValidFolder(directory))
        {
            string[] folders = directory.Split('/');
            string currentPath = folders[0];
            for (int i = 1; i < folders.Length; i++)
            {
                if (!AssetDatabase.IsValidFolder(currentPath + "/" + folders[i]))
                {
                    AssetDatabase.CreateFolder(currentPath, folders[i]);
                }
                currentPath += "/" + folders[i];
            }
        }

        PrefabUtility.SaveAsPrefabAsset(obj, path);
        GameObject.DestroyImmediate(obj);
    }
}
