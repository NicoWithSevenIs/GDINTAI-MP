using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;

public class Game : MonoBehaviour
{
    [Header("Bases")]
    [SerializeField] private GameObject playerBase;
    [SerializeField] private GameObject enemyBase;

    public List<GameObject> playerBases { get; private set; }
    public List<GameObject> enemyBases { get; private set; }
    public bool[,] basePositions { get; private set; }

    [Header("Game Duration")]
    [SerializeField] private float gameDuration = 121f;
    public Timer gameTimer { get; private set; }


    //Scores
    public int playerScore { get; private set; }
    public int enemyScore { get; private set; }


    public event Action onGameOver;

    private void Start()
    {

        basePositions = new bool[TilemapManager.instance.maxBoundsData.Value.size.x, TilemapManager.instance.maxBoundsData.Value.size.y];
        basePositions.Initialize();

        gameTimer = new Timer(gameDuration);
        gameTimer.startTimer();

        gameTimer.onElapse += () =>
        {
            //Check Win con here
        };


        print(TilemapManager.instance.tileMapTypes[5,2]);

        SpawnBases(true,playerBases, 3);
        SpawnBases(false,enemyBases, 3);

        initializeBase(true, playerBases);
        initializeBase(false, enemyBases);

        

    }

    //Call this in chaos powerup
    private void SpawnBases(bool isPlayer, List<GameObject> parent, int amount)
    {
        for(int i =0; i < 3; i++)
        {
            List<Vector3Int> openList = getSpawnableTiles();

            if (openList.Count == 0)
                return;

            Vector3Int selectedTile = openList[UnityEngine.Random.Range(0, openList.Count)];

            GameObject b = Instantiate(isPlayer? playerBase : enemyBase);
            b.transform.position = TilemapManager.instance.getTileCoord((Vector2Int) selectedTile);

            int j = selectedTile.x - TilemapManager.instance.maxBoundsData.Value.xMin;
            int k = TilemapManager.instance.maxBoundsData.Value.yMax - 1 - selectedTile.y;

            basePositions[j - 2, k] = true;
            basePositions[j + 2, k] = true;
            basePositions[j, k - 2] = true;

            for (int r= k - 1; r <= k + 1; r++)
            {
                for (int c = j - 1; c <= j + 1; c++)
                {
                    basePositions[c,r] = true;
                }
            }


        }
    }

    private List<Vector3Int> getSpawnableTiles()
    {
        List<Vector3Int> openList = new List<Vector3Int>();

        for (int row = 0; row < basePositions.GetLength(1); row++)
        {
            for (int col = 0; col < basePositions.GetLength(0); col++)
            {
                int x = col + TilemapManager.instance.maxBoundsData.Value.xMin;
                int y = TilemapManager.instance.maxBoundsData.Value.yMax - 1 - row;


                /*
                    Base is within a 4x5 grid and its center is at index [2,2]
                    
                    When adding a tile to the open list, all tiles within this range must be within the bounds of the array and vacant
                */

                if(isTileSpawnable(new Vector2Int(row,col)))
                    openList.Add(new Vector3Int(x, y, 0));
               

            }
        }

        return openList;
    }

    private bool isTileSpawnable(Vector2Int v)
    {
        bool topleft = v.y - 2 >= 0 && v.x - 2 >= 0;
        bool bottomright = v.y + 1 < basePositions.GetLength(1) && v.x + 2 < basePositions.GetLength(0);

        //if the grid is out of bounds, return false
        if (!topleft || !bottomright)
            return false;

        //check first if "outskirt" tiles are path tiles
        Vector2Int[] cross = new Vector2Int[]
        {
           new Vector2Int(v.x-2, v.y),
           new Vector2Int(v.x+2, v.y),
           new Vector2Int(v.x, v.y-2)
        };

        foreach(var c in cross)
        {
            if (TilemapManager.instance.tileMapTypes[c.x, c.y] != TileType.Path && !basePositions[c.x, c.y])
                return false;
        }

        //finally, check within the surrounding tiles of the origin tile
        for (int i = v.y - 1; i <= v.y + 1; i++)
        {
            for (int j = v.x - 1; j <= v.x + 1; j++)
            {
                if (TilemapManager.instance.tileMapTypes[j, i] != TileType.Path && !basePositions[j,i])
                    return false;

            }
        }

                //otherwise iterate through the grid and check if none of the tiles within the bounds are walls
                //for(int row = v.y - 2; row < v.y + 3  )

        return true;
    }

    private void initializeBase(bool isPlayer, List<GameObject> bases)
    {
        foreach(var b in bases)
        {
            var baseScript = b.GetComponent<Base>();
            
            if(baseScript == null)
                continue;

            baseScript.onBaseDestroyed += () => {

                if (isPlayer)   
                    enemyScore += 100;
                else playerScore += 100;

                //Check here if any agent has run out of bases
            
            };
        }
    }

    
    private bool hasAgentLost(GameObject[] bases)
    {
        int count = 0;

        foreach(var b in bases)
        {
            var baseScript = GetComponent<Base>();
            if (baseScript == null)
                continue;

            if (!baseScript.isDestroyed)
                count++;
        }

        return count == 0;
    }
   
    private void Update()
    {
        gameTimer.TickDown(Time.deltaTime);
    }

    #region singleton
    public static Game instance { get; private set; }
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else Destroy(gameObject);
    }
    #endregion
}
