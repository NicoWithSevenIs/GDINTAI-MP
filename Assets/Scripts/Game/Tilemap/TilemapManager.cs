using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;

public class TilemapManager : MonoBehaviour
{
    [SerializeField] private Tilemap tileMap;
    [SerializeField] private TileTypeSorter typedTile;

    TileType[,] tileMapTypes;

    BoundsInt? tileMapBoundsData = null;

    private void Start()
    {
        tileMap.CompressBounds();
        tileMapBoundsData = tileMap.cellBounds;
        BoundsInt tileMapBounds = tileMapBoundsData.Value;

        tileMapTypes = new TileType[tileMapBounds.size.x, tileMapBounds.size.y];

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



        //writeToTextFile();
    }

    private void writeToTextFile()
    {

        if (tileMapBoundsData == null)
            return;

        BoundsInt tileMapBounds = tileMapBoundsData.Value;


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
