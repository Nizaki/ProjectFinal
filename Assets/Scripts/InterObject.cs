using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterObject : MonoBehaviour, IDamagable
{


    public float Hp = 10;
    public float MaxHP = 10;
    public EquipType EffectiveType = EquipType.None;
    [SerializeField]
    public List<DropItem> DropList;
    private void Start()
    {
        Hp = MaxHP;
    }
    public void TakeDamage(int damage, EquipType sourceEquip = EquipType.None)
    {
        if(sourceEquip == EffectiveType)
        {
            Hp -= 5;
        }
        else
        {
            Hp -= 2;
        }
        Debug.Log($"Take hit HP ramin {Hp}/{MaxHP}");
        if (Hp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Die");

        foreach (DropItem item in DropList)
        {
            if(item.chance > Random.Range(0, 100))
            {
                //TODO : spawn item that player can pickup;
                Debug.Log($"Drop {item.item.name}");
            }
        }

        Destroy(this.gameObject);
    }
}

[System.Serializable]
public class DropItem
{
    [SerializeField,Range(0,100)]
    public int chance;
    [SerializeField]
    public ItemStack item;
}
