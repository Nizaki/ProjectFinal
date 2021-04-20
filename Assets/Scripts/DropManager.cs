using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropManager : MonoBehaviour
{
  public static DropManager Instance { get { return _instance; } }
  private static DropManager _instance;

  [SerializeField]
  private GameObject dropTemplate;

  private void Awake()
  {
    if (_instance != null && _instance != this)
    {
      Destroy(this.gameObject);
      return;
    }
    if (_instance == null)
    {
      _instance = this;
    }
  }

  public void RandomDrop(List<ChanceItem> drop, Vector2 pos)
  {
    foreach (ChanceItem item in drop)
    {
      DropManager.Instance.RandomDrop(item, pos);
    }
  }

  public void RandomDrop(ChanceItem drop, Vector2 pos)
  {
    if (drop.chance > Random.Range(0, 100))
    {
      var obj = Instantiate(dropTemplate, pos, Quaternion.identity);
      obj.GetComponent<DropItem>().item = drop.item.Item;
    }
  }

}

[System.Serializable]
public class ChanceItem
{
  [SerializeField, Range(0, 100)]
  public int chance;
  [SerializeField]
  public ItemStack item;
}