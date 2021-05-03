using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DropManager : MonoBehaviour
{
    public static DropManager Instance { get; private set; }

    [SerializeField] private GameObject dropTemplate;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        if (Instance == null) Instance = this;
    }

    public static void RandomDrop(IEnumerable<ChanceItem> drop, Vector2 pos)
    {
        foreach (var item in drop)
        {
            Debug.Log(item.item.name);
            Instance.RandomDrop(item, pos);
        }
    }

    private void RandomDrop(ChanceItem drop, Vector2 pos)
    {
        if (drop.chance <= Random.Range(0, 100)) return;
        var obj = Instantiate(dropTemplate, pos, Quaternion.identity);
        obj.GetComponent<DropItem>().item = drop.item.Item;
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}

[System.Serializable]
public class ChanceItem
{
    [SerializeField] [Range(0, 100)] public int chance;
    [SerializeField] public ItemStack item;
}