using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class RaidManager : MonoBehaviour
{
    public GameTime gt;
    public GameObject player;
    public List<GameObject> currentMonsters;
    private float spawnRange = 25f;
    public List<WaveObject> wave;
    //require raid monster to work normal monster not working because their have uniq ai
    // Start is called before the first frame update
    private void Start()
    {
        gt.onTimeChange.AddListener(OnNight);
    }

    private void OnNight(TimeState time)
    {
        if (time != TimeState.Night)
            return;
        //invoke every night
        //check monster range
        var currentDay = gt.day;
        var monsterTemplate =wave[Random.Range(0, wave.Count)];
        
        foreach (var go in monsterTemplate.MonsterList)
        {
            var count = currentDay / monsterTemplate.MonsterList.Count;
            for (var i = 0; i < count; i++)
            {
                var pos = player.transform.position;
                var posX = Random.Range(-spawnRange, spawnRange);
                var posY = Random.Range(-spawnRange, spawnRange);
                posX = MonsterSpawner.CheckPos(posX);
                posY = MonsterSpawner.CheckPos(posY);
                var o = Instantiate(go, new Vector2(pos.x + posX, pos.y + posY), quaternion.identity);
            }
        }
        
        // currentMonsters.Add();

        // var go =Instantiate(monster[Random.Range(0, monster.Count)],
        //     new Vector2(pos.x + posX, pos.y + posY),
        //     quaternion.identity);
        // go.GetComponent<InterObject>().onDie.AddListener(RemoveSelf);
    }
}