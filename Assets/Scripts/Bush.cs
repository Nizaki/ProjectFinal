using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : MonoBehaviour
{
    [SerializeField]
    GameObject prefab;
    [SerializeField]
    int count = 1;
    [SerializeField]
    float dropOffset = 0.2f;
    // Start is called before the first frame update
    private void OnMouseDown()
    {

        Debug.Log($"click {gameObject.name}");
        for (int i = 0; i < count; i++)
        {
            Vector2 pos = new Vector2(Random.Range(transform.position.x - dropOffset, transform.position.x + dropOffset)
            , Random.Range(transform.position.y - dropOffset, transform.position.y + dropOffset));
            Instantiate(prefab, pos, Quaternion.identity);
        }

        Destroy(this.gameObject);
    }

    void OnDrawGizmos()
    {

    }
}
