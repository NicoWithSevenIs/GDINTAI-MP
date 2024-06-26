using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;
using System;
using System.Linq;

public class TilemapManager : MonoBehaviour
{
    [Header("Tile Type Referencing")]
    [SerializeField] private Tilemap[] tileMap;
    [SerializeField] private TileTypeSorter typedTile;

    [Header("Collider Handling")]
    [SerializeField] private Tilemap pathReference;



    public TileType[,] tileMapTypes { get; private set; }

    public BoundsInt? maxBoundsData { get; private set; } = null; //largest bounds data among all tilemap layers


    public float getCellSize()
    {
        return (tileMap[0].cellBounds.size.x + tileMap[0].cellBounds.size.y) / 2 ;
    }


    #region positionTracking

    [SerializeField] public Vector3Int playerPos { get; private set; } = Vector3Int.zero;
    [SerializeField] public Vector3Int enemyPos { get; private set; } = Vector3Int.zero;

    public event Action onPlayerChangedPos;
    
    public void setPlayerPos(Vector3Int newPos)
    {
        playerPos = newPos;
        onPlayerChangedPos?.Invoke();
    }

    public void setEnemyPos(Vector3Int newPos)
    {
        enemyPos = newPos;
    }

    public Vector2 CellToWorld(Vector2Int vec)
    {
      
        for(int i =0; i < tileMap.Length; i++)
        {
            Vector3Int cell = new Vector3Int(vec.x, vec.y, 0);
            if(tileMap[i].GetTile(cell) != null)
            {
                return tileMap[i].GetCellCenterWorld(cell);
            }
        }

        //Debug.LogException(new Exception("Tile " + vec + " does not exist in Tilemap Collection"));
       
        return new Vector2(0, 0);

    }

    public Vector3Int WorldToCell(Vector3 pos)
    {
        if(tileMap.Length == 0)
        {
            Debug.LogException(new Exception("No Tilemaps to Reference"));
            return new Vector3Int(0, 0, 0);
        }
           
        
        return tileMap[0].WorldToCell(pos);
    }

    public Vector2Int IndexToCell(int i, int j)
    {

        BoundsInt maxBounds = maxBoundsData.Value;

        int x = i + maxBounds.xMin;
        int y = maxBounds.yMax - 1 - j;

        return new Vector2Int(x,y);
    }


    public Vector2Int CellToIndex(Vector2Int cell)
    {
        Vector2Int v = cell;
        v.x = v.x - TilemapManager.instance.maxBoundsData.Value.xMin;
        v.y = TilemapManager.instance.maxBoundsData.Value.yMax - 1 - v.y;

        return v;
    }
    #endregion positionTracking



    #region mapInitialization
    private void getMaxBounds()
    {
        foreach (var t in tileMap)
        {
            t.CompressBounds();
            BoundsInt tBounds = t.cellBounds;

            if (!maxBoundsData.HasValue)
            {
                maxBoundsData = tBounds;
                continue;
            }

            if (tBounds.size.x * tBounds.size.y > maxBoundsData.Value.size.x * maxBoundsData.Value.size.y)
                maxBoundsData = tBounds;

        }
    }

    private void UpdateTileTypeAt(int index, BoundsInt maxBounds)
    {
        for (int row = 0; row < maxBounds.size.y; row++)
        {

            for (int col = 0; col < maxBounds.size.x; col++)
            {
                int x = col + maxBounds.xMin;
                int y = maxBounds.yMax - 1 - row;


                Vector3Int pos = new Vector3Int(x, y, 0);
                TileBase t = tileMap[index].GetTile(pos);

                TileType typeOfT = t != null ? typedTile.getTileType(t.name) : TileType.None;

                if (typeOfT > tileMapTypes[col, row])
                    tileMapTypes[col, row] = typeOfT;


            }

        }
    }

    private void initializeTileTypeArray()
    {
        getMaxBounds();

        if (!maxBoundsData.HasValue)
            return;


        BoundsInt maxBounds = maxBoundsData.Value;
        tileMapTypes = new TileType[maxBounds.size.x, maxBounds.size.y];


        tileMapTypes.Initialize();


        for (int i = 0; i < tileMap.Length; i++)
        {
            UpdateTileTypeAt(i, maxBounds);
        }

        writeToTextFile();

    }

    private void writeToTextFile()
    {

        if (!maxBoundsData.HasValue)
            return;

        BoundsInt tileMapBounds = maxBoundsData.Value;


        string filename = Application.dataPath + "/Output.txt";

        if (!File.Exists(filename))
        {
            print("File " + filename + "not Found!");
            return;
        }
        string buffer;

        Dictionary<TileType, string> typeToStr = new Dictionary<TileType, string>() {
            {TileType.None, " "},
            {TileType.Path, "."},
            {TileType.Wall, "W"}
        };

        File.WriteAllText(filename, "");


        for (int i = 0; i < tileMapBounds.size.y; i++)
        {
            buffer = "";
            for (int j = 0; j < tileMapBounds.size.x; j++)
            {
                buffer += typeToStr[tileMapTypes[j, i]];
                if (j + 1 != tileMapBounds.size.x)
                    buffer += " ";
            }
            if (i + 1 != tileMapBounds.size.y)
                buffer += "\n";
            File.AppendAllText(filename, buffer);
        }

    }
    #endregion mapInitialization

    #region singleton
    public static TilemapManager instance { get; private set; }
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else Destroy(gameObject);

        initializeTileTypeArray();
    }
    #endregion
}
