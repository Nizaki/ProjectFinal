
    using System.Collections.Generic;
    using UnityEngine;
    [CreateAssetMenu(fileName = "New wave", menuName = "Game/Wave")]
    public class WaveObject : ScriptableObject
    {
        public List<GameObject> MonsterList;
    }