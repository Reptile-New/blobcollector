# Blob Collector - Architecture du Code

## Structure des Scripts

### Core Systems
- **GameManager.cs** - Point d'entrée principal, initialise tous les systèmes
- **BlobData.cs** - Classe de données pour représenter un blob (ID, nom, sprite, rareté)
- **BlobDatabase.cs** - Singleton qui gère tous les blobs disponibles (50 au total)

### Collection System
- **BlobCollectionManager.cs** - Gère la collection du joueur (blobs possédés)
  - Sauvegarde/chargement avec PlayerPrefs
  - Vérification des doublons
  - Statistiques de collection (pourcentage, compte)

### Daily Chest System
- **DailyChestManager.cs** - Gère les coffres quotidiens
  - Coffre 1: 12h (midi)
  - Coffre 2: 17h
  - Système de timer avec DateTime
  - Vérification de disponibilité

### Gacha/Reward System
- **ChestRewardSystem.cs** - Système de récompenses
  - 3 blobs aléatoires par coffre
  - Distribution basée sur la rareté:
    - Common: 60%
    - Rare: 30%
    - Epic: 9%
    - Legendary: 1%

### UI Scripts
- **MainMenuUI.cs** - Menu principal avec boutons des coffres
- **CollectionUI.cs** - Écran de collection (grille de blobs)
- **BlobSlotUI.cs** - Slot individuel d'un blob dans la grille
- **ChestOpenUI.cs** - Popup d'ouverture de coffre avec animation

## Système de Rareté

```
BlobRarity {
  Common    (60%) - Blobs 1-30
  Rare      (30%) - Blobs 31-45
  Epic      (9%)  - Blobs 46-49
  Legendary (1%)  - Blob 50
}
```

## Flow du Jeu

1. **Démarrage**
   - GameManager initialise tous les systèmes
   - BlobDatabase charge les 50 sprites
   - BlobCollectionManager charge la progression sauvegardée

2. **Menu Principal**
   - Affiche 2 boutons de coffres avec timers
   - Affiche stats de collection (X/50, pourcentage)

3. **Ouverture de Coffre**
   - Vérification de disponibilité
   - Génération de 3 blobs aléatoires (selon rareté)
   - Ajout à la collection
   - Affichage des récompenses
   - Sauvegarde automatique
   - Mise à jour du timer

4. **Collection**
   - Grille de 50 blobs
   - Blobs collectés: affichés en couleur avec nom
   - Blobs non collectés: silhouette noire avec "???"

## Sauvegarde

Utilise PlayerPrefs pour stocker:
- Collection de blobs (liste d'IDs en JSON)
- Date/heure de la dernière réclamation de chaque coffre

## TODO / Prochaines Étapes

- [ ] Remplacer les sprites placeholder par les vrais blobs
- [ ] Créer les prefabs UI dans Unity
- [ ] Ajouter des animations pour l'ouverture des coffres
- [ ] Ajouter des sons/musique
- [ ] Système de notifications push pour les coffres
- [ ] Écran de détails d'un blob (description, statistiques)
- [ ] Système d'achievements
