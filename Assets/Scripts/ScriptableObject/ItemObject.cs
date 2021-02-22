using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Game/Item")]
public class ItemObject : ScriptableObject
{
    public string itemName;
    public Sprite sprite;
    public ItemType type;
    [Tooltip("ตัวเลขตามผลของไอเท็มเช่น ถ้าเป็นอาหารจะเพิ่มความหิว")]
    public float data; //damge amount of hp hung bra bra
    public bool fuelable = false;
    public int fuelAmount = 10;
    [Multiline]
    public string description;
}


public enum ItemType
{
    None, Food, Water, Health, Equip, Build
}