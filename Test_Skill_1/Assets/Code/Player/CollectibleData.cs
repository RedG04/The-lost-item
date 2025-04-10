using UnityEngine;

public enum CollectibleEffect
{
    None,
    Heal,
    Buff,
    ExtraPoints,
    SpeedBoost
}

[CreateAssetMenu(fileName = "NewCollectibleData", menuName = "Collectible/Collectible Data")]
public class CollectibleData : ScriptableObject
{
    public string collectibleName;
    public int pointValue;

    [Header("Effect")]
    public CollectibleEffect effect;
    public float effectStrength;  // es. quantità di cura o durata del buff
    public Sprite icon;
}
