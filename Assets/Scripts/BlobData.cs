using UnityEngine;

[System.Serializable]
public class BlobData
{
    public int id;
    public string name;
    public Sprite sprite;
    public BlobRarity rarity;

    public BlobData(int id, string name, Sprite sprite, BlobRarity rarity)
    {
        this.id = id;
        this.name = name;
        this.sprite = sprite;
        this.rarity = rarity;
    }
}

public enum BlobRarity
{
    Common,    // 60% chance
    Rare,      // 30% chance
    Epic,      // 9% chance
    Legendary  // 1% chance
}
