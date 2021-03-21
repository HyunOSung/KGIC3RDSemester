using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    //싱글톤 기법 
    static DataManager instance = null;

    public static DataManager Instance
    {
        get
        {
            return instance;
        }
    }

    [SerializeField]
    Data.PlayerLevelData playerLevelData;
    Data.EnemyData enemyData;
    Player player;

    private void Awake()
    {
        //instance = GetComponent<DataManager>();

        playerLevelData = Resources.Load("Data/PlayerLevelData.Asset") as Data.PlayerLevelData;
        enemyData = Resources.Load("Data/EnemyData.Asset") as Data.EnemyData;
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        GameObject go = GameObject.FindWithTag("Player");
        player = go.GetComponent<Player>();

    }

    // Start is called before the first frame update
    private void Start()
    {

    }

    public Player GetPlayer()
    {
        return player;
    }

    public Data.PlayerLevelData.Attribute GetPlayerDB(int level)
    {
        return playerLevelData.list[level - 1];
    }
    public Data.EnemyData.Attribute GetEnemyDB(int level)
    {
        return enemyData.list[level - 1];
    }
}
