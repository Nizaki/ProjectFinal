using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Game/Item")]
public class ItemObject : ScriptableObject
{
    public string itemName;
    public Sprite sprite;
    public ItemType type;
    public EquipType equipType = EquipType.None;

    [Tooltip("ตัวเลขตามผลของไอเท็มเช่น ถ้าเป็นอาหารจะเพิ่มความหิว")]
    public float data; //damge amount of hp hung bra bra

    public bool fuelable = false;
    public int fuelAmount = 10;
    [Multiline] public string description;
    public GameObject prefab;
}

public enum ItemType
{
    None,
    Food,
    Water,
    Health,
    Equip,
    Build
}

public enum EquipType
{
    None,
    Axe,
    Sword
}