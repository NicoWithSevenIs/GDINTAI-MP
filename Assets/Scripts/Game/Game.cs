using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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

    public delegate bool invalidTileChecker(int x, int y);

    private void Start()
    {

        basePositions = new bool[TilemapManager.instance.maxBoundsData.Value.size.x, TilemapManager.instance.maxBoundsData.Value.size.y];
        basePositions.Initialize();

        gameTimer = new Timer(gameDuration);
        gameTimer.startTimer();

        playerBases = new List<GameObject>();
        enemyBases = new List<GameObject>();

        gameTimer.onElapse += () =>
        {
            //Check Win con here
        };


        SpawnBases(true, playerBases, 3);
        SpawnBases(false, enemyBases, 3);

        initializeBase(true, playerBases);
        initializeBase(false, enemyBases);

        writeToTextFile();

    }

    public void resetBasePositions()
    {
        for(int i =0; i <basePositions.GetLength(0); i++)
        {
            for (int j = 0; j < basePositions.GetLength(1); j++)
            {
                basePositions[i, j] = false;
            }
        }
    }
    private void markBasePosition(Vector2Int selectedTile)
    {

        int j = selectedTile.x - TilemapManager.instance.maxBoundsData.Value.xMin;
        int k = TilemapManager.instance.maxBoundsData.Value.yMax - 1 - selectedTile.y;


        //set basePosition flags  to true based on the tiles occupied by the base
        for (int r = k - 2; r <= k + 1; r++)
        {
            for (int c = j - 2; c <= j + 2; c++)
            {
                basePositions[c, r] = true;
            }
        }
    }


    
    public void SpawnBases(bool isPlayer, List<GameObject> parent, int amount)
    {

        invalidTileChecker isTileInvalid = (int x, int y) =>
        {
            return TilemapManager.instance.tileMapTypes[x, y] != TileType.Path || basePositions[x, y];
        };


        for (int i =0; i < amount; i++)
        {
            List<Vector3Int> openList = getSpawnableTiles(isTileInvalid);

            if (openList.Count == 0)
            {
                print("No more vacant tiles");
                return;
            }
               
            Vector2Int selectedTile = (Vector2Int)openList[UnityEngine.Random.Range(0, openList.Count)];

            GameObject b = Instantiate(isPlayer ? playerBase : enemyBase);
            b.transform.position = TilemapManager.instance.CellToWorld( selectedTile);

            markBasePosition(selectedTile);

            if (isPlayer)
                playerBases.Add(b);
            else enemyBases.Add(b);
        }
    }

    public void RearrangeBases(bool isPlayer, List<GameObject> parent, invalidTileChecker isTileInvalid)
    {
        if (parent.Count == 0)
        {
            Debug.LogException(new Exception("Template Array is empty!"));
            return;
        }
            
        //reset base positions then reassign the positions of the opposing side
        resetBasePositions();

        foreach(var b in isPlayer ? enemyBases : playerBases)
        {
            Vector2Int cell = (Vector2Int)TilemapManager.instance.WorldToCell(b.transform.position);
            markBasePosition(cell);
        }

        //transfer template to a temporary list
        List<GameObject> temp = new List<GameObject>();
        temp.AddRange(parent);
        parent.Clear();
        

        for (int i = 0; i < temp.Count; i++)
        {
            List<Vector3Int> openList = getSpawnableTiles(isTileInvalid);

            if (openList.Count == 0)
            {
                print("No more vacant tiles");
                return;
            }

            Vector2Int selectedTile = (Vector2Int)openList[UnityEngine.Random.Range(0, openList.Count)];
            temp[i].transform.position = TilemapManager.instance.CellToWorld(selectedTile);

            markBasePosition(selectedTile);

            parent.Add(temp[i]);
        }

    }



    private List<Vector3Int> getSpawnableTiles(invalidTileChecker isTileInvalid)
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

                if(isTileSpawnable(new Vector2Int(col,row), isTileInvalid))
                    openList.Add(new Vector3Int(x, y, 0));
               

            }
        }

        return openList;
    }

   
    public bool isTileSpawnable(Vector2Int v, invalidTileChecker isTileInvalid)
    {
       
        bool topleft = v.y - 2 >= 0 && v.x - 2 >= 0;
        bool bottomright = v.y + 1 < basePositions.GetLength(1) && v.x + 2 < basePositions.GetLength(0);

        //if the grid is out of bounds, return false
        if (!topleft || !bottomright)
        {
            return false;
        }
           
        //check first if "outskirt" tiles are path tiles
   
        if (isTileInvalid(v.x - 2, v.y))
            return false;

        if (isTileInvalid(v.x + 2, v.y))
            return false;

        if(isTileInvalid(v.x, v.y - 2))
            return false;


        //finally, check within the surrounding tiles of the origin tile
        for (int i = v.y - 1; i <= v.y + 1; i++)
        {
            for (int j = v.x - 1; j <= v.x + 1; j++)
            {

                if (isTileInvalid(j, i))
                    return false;
        
            }
        }
      

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

    int count = 1;
    private void Update()
    {
        gameTimer.TickDown(Time.deltaTime);

        /* //Debugging - Check if Tile is Placeable
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector2Int p = (Vector2Int)TilemapManager.instance.playerPos;
            int j = p.x - TilemapManager.instance.maxBoundsData.Value.xMin;
            int k = TilemapManager.instance.maxBoundsData.Value.yMax - 1 - p.y;
            Vector2Int norm = new Vector2Int(j, k);
            print(p + "-> " + norm + " = "+ isTileSpawnable(norm));
        }*/
        invalidTileChecker isTileInvalid = (int x, int y) =>
        {
            return TilemapManager.instance.tileMapTypes[x, y] != TileType.Path || basePositions[x, y];
        };

        //Debugging - Check if spawning works correctly
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RearrangeBases(true, playerBases, isTileInvalid);
            writeToTextFile();

        }
      


    }

    private void writeToTextFile()
    {


        BoundsInt tileMapBounds = TilemapManager.instance.maxBoundsData.Value;


        string filename = Application.dataPath + "/Test.txt";


        string buffer;
        File.WriteAllText(filename, "");



        for (int i = 0; i < tileMapBounds.size.y; i++)
        {
            buffer = "";
            for (int j = 0; j < tileMapBounds.size.x; j++)
            {
                buffer += TilemapManager.instance.tileMapTypes[j,i] != TileType.Path ? "X " : basePositions[j, i] ? "O " : ". ";
                if (j + 1 != tileMapBounds.size.x)
                    buffer += " ";
            }
            if (i + 1 != tileMapBounds.size.y)
                buffer += "\n";
            File.AppendAllText(filename, buffer);
        }

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
