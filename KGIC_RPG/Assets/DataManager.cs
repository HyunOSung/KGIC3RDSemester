using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    [SerializeField]
    PlayerLevelData playerLevelData;
    EnemyData enemyData;
    Player player;

    //싱글톤 기법 
    static DataManager instance = null;

    public static DataManager Instance
    {
        get
        {
            return instance;
        }
    }


    private void Awake()
    {
        //instance = GetComponent<DataManager>();
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }


        playerLevelData = Resources.Load("Data/PlayerLevelData") as PlayerLevelData;
        enemyData = Resources.Load("Data/EnemyData") as EnemyData;


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

    public PlayerLevelData.Attribute GetPlayerDB(int level)
    {
        return playerLevelData.list[level - 1];
    }
    public EnemyData.Attribute GetEnemyDB(int level)
    {
        return enemyData.list[level - 1];
    }
}
