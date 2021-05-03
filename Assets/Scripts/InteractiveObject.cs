using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractiveObject : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int count = 1;
    [SerializeField] private ItemObject requireTool;
    [SerializeField] private float dropOffset = 0.2f;
    [SerializeField] private float interactRange = 2f;
    private GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //float distant = Vector2.Distance(transform.position, player.transform.position);
        //Debug.Log($"click {gameObject.name} with {distant}");
        //if (distant >= interactRange)
        //{
        //    return;
        //}
        //if (requireTool == null || requireTool == player.GetComponent<PlayerInventory>().currentItem)
        //{
        //    for (int i = 0; i < count; i++)
        //    {
        //        Vector2 pos = new Vector2(Random.Range(transform.position.x - dropOffset, transform.position.x + dropOffset)
        //        , Random.Range(transform.position.y - dropOffset, transform.position.y + dropOffset));
        //        Instantiate(prefab, pos, Quaternion.identity);
        //    }

        //    Destroy(this.gameObject);
        //}
    }
}