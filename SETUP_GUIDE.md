# Guide de Setup - Blob Collector

## 🚀 Setup Automatique (RECOMMANDÉ)

### Étape 1: Ouvrir le projet dans Unity
1. Ouvrir **Unity Hub**
2. Cliquer sur **Add** → Sélectionner le dossier `BlobCollector`
3. Ouvrir le projet avec Unity 6

### Étape 2: Lancer le setup automatique
1. Dans Unity, aller dans le menu: **Tools → Blob Collector → Auto Setup Game**
2. Une fenêtre de confirmation apparaît
3. Cliquer sur **Oui**
4. Attendre quelques secondes...
5. Une fenêtre "Setup Complet" apparaît → Cliquer **OK**

### Étape 3: Tester le jeu
1. Appuyer sur le bouton **Play** ▶️
2. Le jeu devrait se lancer avec:
   - Menu principal avec 2 boutons de coffres
   - Timers affichés
   - Stats de collection (0/50)

**C'EST TOUT!** Le jeu est prêt 🎉

---

## 🎮 Comment jouer

### Ouvrir un coffre
- Cliquer sur **"Ouvrir"** sous Coffre 1 ou Coffre 2
- Une popup apparaît avec 3 blobs aléatoires
- Les blobs sont automatiquement ajoutés à ta collection
- Le coffre devient indisponible jusqu'au lendemain

### Horaires des coffres
- **Coffre 1**: Disponible à partir de **12h00** (midi)
- **Coffre 2**: Disponible à partir de **17h00**

### Voir la collection
- Cliquer sur **"Voir Collection"**
- Grille de 50 blobs
- Blobs collectés: affichés en couleur
- Blobs non collectés: silhouette noire avec "???"

---

## 🐛 Si le setup automatique ne marche pas

### Option manuelle (backup):

1. **Créer les dossiers manquants**
   ```
   Assets/Prefabs/
   Assets/Prefabs/UI/
   ```

2. **Vérifier les sprites**
   - Aller dans `Assets/Resources/Blobs/`
   - Sélectionner tous les sprites
   - Inspector → Texture Type: **Sprite (2D and UI)**
   - Appliquer

3. **Créer la scène**
   - Créer un Canvas
   - Créer un EventSystem
   - Drag & drop le script `GameManager` dans la scène

---

## 📝 Fichiers importants

| Fichier | Description |
|---------|-------------|
| `Assets/Scripts/GameManager.cs` | Point d'entrée principal |
| `Assets/Scripts/Editor/BlobCollectorSetup.cs` | Script d'auto-setup |
| `Assets/Resources/Blobs/` | Les 50 sprites de blobs |
| `Assets/Scripts/README_CODE.md` | Documentation du code |

---

## 🔧 Customisation

### Changer les horaires des coffres
Éditer `Assets/Scripts/DailyChestManager.cs`:
```csharp
private const int CHEST_1_HOUR = 12; // Changer ici
private const int CHEST_2_HOUR = 17; // Changer ici
```

### Changer le nombre de blobs par coffre
Éditer `Assets/Scripts/ChestRewardSystem.cs`:
```csharp
private const int BLOBS_PER_CHEST = 3; // Changer ici
```

### Changer les probabilités de rareté
Éditer `Assets/Scripts/BlobDatabase.cs`:
```csharp
if (roll < 60f) // Common (60%)
else if (roll < 90f) // Rare (30%)
else if (roll < 99f) // Epic (9%)
else // Legendary (1%)
```

---

## 🎨 Remplacer les sprites placeholder

1. Créer/générer les vrais sprites de blobs
2. Les nommer: `blob_001.png` à `blob_050.png`
3. Les placer dans `Assets/Resources/Blobs/`
4. Remplacer les anciens fichiers
5. Unity va auto-refresh

---

## ✅ Checklist de vérification

- [ ] Unity 6 installé
- [ ] Projet ouvert sans erreurs
- [ ] Script Editor exécuté (Tools → Auto Setup)
- [ ] GameManager présent dans la scène
- [ ] Canvas avec UI créé
- [ ] EventSystem présent
- [ ] 50 sprites dans Resources/Blobs
- [ ] Bouton Play fonctionne

---

## 🆘 Support

En cas de problème:
1. Vérifier la **Console** Unity pour les erreurs
2. Relancer le setup: **Tools → Auto Setup Game**
3. Vérifier que TextMeshPro est installé (Package Manager)

---

**Bon développement!** 🎮✨
