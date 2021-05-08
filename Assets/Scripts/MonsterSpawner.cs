using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private const float SpawnDelay = 5f;

    private float nextSpawn;

    private float spawnRange = 15f;

    private static int monsterCount = 0;
    
    [SerializeField]
    private List<GameObject> monster = new List<GameObject>();
    // Start is called before the first frame update
    private void LateUpdate()
    {
        if (Time.time > nextSpawn && monsterCount<5)
        {
            nextSpawn = Time.time + SpawnDelay;
            //spawn monster sequence
            monsterCount += 1;
            var pos = player.transform.position;
            var posX = Random.Range(-spawnRange, spawnRange);
            posX= CheckPos(posX);
            
            var posY = Random.Range(-spawnRange, spawnRange);
            posY = CheckPos(posY);
            var go =Instantiate(monster[Random.Range(0, monster.Count)],
                new Vector2(pos.x + posX, pos.y + posY),
                quaternion.identity);
            go.GetComponent<InterObject>().onDie.AddListener(RemoveSelf);
        }
    }

    void RemoveSelf()
    {
        monsterCount -= 1;
    }
    
    float CheckPos(float value)
    {
        if (value < 5 && value > -5)
        {
            if (Random.Range(0, 1) == 1)
            {
                return 5f;
            }

            return -5f;
        }

        return value;
    }
}
