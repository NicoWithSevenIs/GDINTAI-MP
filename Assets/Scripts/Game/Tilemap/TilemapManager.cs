using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;

public class TilemapManager : MonoBehaviour
{
    [SerializeField] private Tilemap[] tileMap;
    [SerializeField] private TileTypeSorter typedTile;

    TileType[,] tileMapTypes;

    BoundsInt? maxBoundsData = null; //largest bounds data among all tilemap layers



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

 
    private void UpdateTileTypeArray(int index, BoundsInt maxBounds)
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

                if(typeOfT > tileMapTypes[col, row])
                    tileMapTypes[col, row] = typeOfT;


            }

        }
    }

    private void Start()
    {
        getMaxBounds();

        if (!maxBoundsData.HasValue)
            return;

        tileMapTypes.Initialize();
        writeToTextFile();
        BoundsInt maxBounds = maxBoundsData.Value;
        tileMapTypes = new TileType[maxBounds.size.x, maxBounds.size.y];


        for(int i = 0; i < tileMap.Length; i++)
        {
            UpdateTileTypeArray(i, maxBounds);
        }

        /*
        tileMap.CompressBounds();
        tileMapBoundsData = tileMap.cellBounds;
        BoundsInt tileMapBounds = tileMapBoundsData.Value;

      

        //row is y traversal, y goes down as you go down
        for (int row = 0; row < tileMapBounds.size.y; row++)
        {

            for (int col = 0; col < tileMapBounds.size.x; col++)
            {
                int x = col + tileMapBounds.xMin;
                int y = tileMapBounds.yMax - 1 - row;


                Vector3Int pos = new Vector3Int(x, y, 0);
                TileBase t = tileMap.GetTile(pos);

                tileMapTypes[col, row] = t != null ? typedTile.getTileType(t.name) : TileType.None;

            }

        }


       
        writeToTextFile();
         */
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

    #region singleton
    public static TilemapManager instance { get; private set; }
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else Destroy(gameObject);
    }
    #endregion
}
