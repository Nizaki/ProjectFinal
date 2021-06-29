using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheater : MonoBehaviour
{
    private Player player;

    [SerializeField] private List<ItemStack> itemList;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
#if UNITY_EDITOR
        StartCoroutine(nameof(AddItem));
#endif
    }

    IEnumerator AddItem()
    {
        foreach (var itemStack in itemList)
        {
            player.inventory.AddItem(itemStack.Item, itemStack.count);
            yield return new WaitForEndOfFrame();
        }
    }
}
