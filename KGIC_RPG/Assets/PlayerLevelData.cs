using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class PlayerLevelData : ScriptableObject
    {
        public List<Attribute> list = new List<Attribute>();

        [System.Serializable]
        public class Attribute
        {
            public int level;
            public int maxHP;
            public int baseAttack;
            public int baseDef;
            public int requireNextLevelExp;
            public int moveSpeed;
            public int turnSpeed;
            public float attackRange;
        }

    }


    public class EnemyData : ScriptableObject
    {
        public List<Attribute> list = new List<Attribute>();

        [System.Serializable]
        public class Attribute
        {
            public int level;
            public int maxHP;
            public int baseAttack;
            public int baseDef;
            public int moveSpeed;
            public int turnSpeed;
            public float attackRange;

        }
    }
}