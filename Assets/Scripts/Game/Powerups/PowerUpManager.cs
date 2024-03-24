
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;


public class PowerUpManager : MonoBehaviour
{
    [Header("Power Ups")]
    [SerializeField] private GameObject[] powerUps;
    [SerializeField] private float spawnInterval = 15f;
    [SerializeField] private int sizePerPool = 10;
    private Timer potionSpawnTimer;
    private bool[,] potionPosTracker;

    private Dictionary<string, List<GameObject>> potionPool;

    private void Start()
    {

        initializePool();

        Vector3Int size = TilemapManager.instance.maxBoundsData.Value.size;
        potionPosTracker = new bool[size.x, size.y];

        potionPosTracker.Initialize();

        potionSpawnTimer = new Timer(spawnInterval);

        potionSpawnTimer.onElapse += spawnPotion;

        potionSpawnTimer.startTimer();
    }


    private void spawnPotion()
    {
        potionSpawnTimer.restartTimer();
        
        List<Vector3Int> openList = getEmptyTiles();

        if (openList.Count == 0)
        {
            print("Board is Full");
            return;
        }

        Vector3Int randomTile = openList[Random.Range(0, openList.Count)];
      
        string key = powerUps[Random.Range(0, powerUps.Length)].name;

        int i = randomTile.x - TilemapManager.instance.maxBoundsData.Value.xMin;
        int j = TilemapManager.instance.maxBoundsData.Value.yMax - 1 - randomTile.y;

        potionPosTracker[i,j] = true;
   
        GameObject poolable = getFromPool(key);
        poolable.transform.position = TilemapManager.instance.CellToWorld((Vector2Int)randomTile);


        PowerUp powerup = poolable.GetComponent<PowerUp>();

        System.Action e = null;

        e = () =>
        {
            Vector3Int v = TilemapManager.instance.WorldToCell(poolable.transform.position);
      
            v.x = v.x - TilemapManager.instance.maxBoundsData.Value.xMin;
            v.y = TilemapManager.instance.maxBoundsData.Value.yMax - 1 - v.y;

            potionPosTracker[v.x, v.y] = false;
            powerup.onConsumption -= e;
        };

        powerup.onConsumption += e;
        
    }

 

    private void initializePool()
    {
        potionPool = new Dictionary<string, List<GameObject>>();

        for (int i = 0; i < powerUps.Length; i++)
        {
            GameObject container = new GameObject();
            container.name = powerUps[i].name + " Pool";
            container.transform.SetParent(gameObject.transform);

            potionPool.Add(powerUps[i].name, new List<GameObject>());
            for(int j =0; j < sizePerPool; j++)
            {
                GameObject poolable = Instantiate(powerUps[i]);
                poolable.transform.parent = container.transform;
                poolable.SetActive(false);
                potionPool[powerUps[i].name].Add(poolable);
            }
        }

    }

    private GameObject getFromPool(string name)
    {
        if (!potionPool.ContainsKey(name))
            return null;

        for(int i = 0; i < potionPool[name].Count; i++)
        {
            if (!potionPool[name][i].activeInHierarchy)
            {
                potionPool[name][i].SetActive(true);
                return potionPool[name][i];
            }
                
        }
        GameObject poolable = Instantiate(getPowerUpByName(name));
        poolable.transform.parent = gameObject.transform.Find(name + " Pool");
        potionPool[name].Add(poolable); 

        return poolable;
    }


    private GameObject getPowerUpByName(string name)
    {
        foreach(var p in powerUps)
        {
            if (p.name == name)
                return p;
        }
        return null;
    }

    private void Update()
    {
        potionSpawnTimer.TickDown(Time.deltaTime);
    }


    private List<Vector3Int> getEmptyTiles()
    {
        List<Vector3Int> openList = new List<Vector3Int>();

        for(int row =0; row < potionPosTracker.GetLength(1); row++)
        {
            for (int col = 0; col < potionPosTracker.GetLength(0); col++)
            {
                int x = col + TilemapManager.instance.maxBoundsData.Value.xMin;
                int y = TilemapManager.instance.maxBoundsData.Value.yMax - 1 - row;

                if (TilemapManager.instance.tileMapTypes[col,row] == TileType.Path && !potionPosTracker[col, row] && !Game.instance.basePositions[col,row])
                {
                    openList.Add(new Vector3Int(x, y, 0));
                }
                    

            }
        }

        return openList;
    }
    #region singleton
    public static PowerUpManager instance { get; private set; }
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else Destroy(gameObject);
    }
    #endregion
}
