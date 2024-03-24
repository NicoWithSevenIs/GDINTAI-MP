using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Test : MonoBehaviour
{

    private Game.invalidTileChecker isTileInvalid = (int x, int y) =>
    {

        bool tileNonPath = TilemapManager.instance.tileMapTypes[x, y] != TileType.Path;
        bool tileHasBase = Game.instance.basePositions[x, y];
        bool tileHasPotion = PowerUpManager.instance.potionPosTracker[x, y];
        bool tileHasPlayer = TilemapManager.instance.playerPos.x == x && TilemapManager.instance.playerPos.y == y;
        bool tileHasEnemy = TilemapManager.instance.enemyPos.x == x && TilemapManager.instance.enemyPos.y == y;

        return tileNonPath || tileHasBase || tileHasPotion || tileHasPlayer || tileHasEnemy;

    };


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Game.instance.RearrangeBases(true, Game.instance.playerBases, isTileInvalid);
    }




    /*For testing base placement
    [SerializeField] private int x;
    [SerializeField] private int y;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            writeToTextFile();
            
    }

    private void writeToTextFile()
    {


        BoundsInt tileMapBounds = TilemapManager.instance.maxBoundsData.Value;


        string filename = Application.dataPath + "/Test.txt";

 
        string buffer;

        Dictionary<TileType, string> typeToStr = new Dictionary<TileType, string>() {
            {TileType.None, " "},
            {TileType.Path, "."},
            {TileType.Wall, "W"}
        };

        File.WriteAllText(filename, "");

        Vector3Int size = TilemapManager.instance.maxBoundsData.Value.size;

        bool topleft = y - 2 >= 0 && x - 2 >= 0;
        bool bottomright = y + 1 < size.y && x + 2 < size.x;
        if (!topleft)
        {
            print("Top Left out of bounds");
            return;
        }

        if (!bottomright)
        {
            print("Bottom Right out of bounds");
            return;
        }

        for (int i = y - 2; i <= y + 1; i++)
        {
            buffer = "";
            for (int j = x - 2; j <= x + 2; j++)
            {

                if (j == x - 2 && i == y || j == x + 2 && i == y || j == x && i == y - 2)
                    buffer += "O ";
                else if (j == x && i == y)
                    buffer += "X ";
                else buffer += typeToStr[TilemapManager.instance.tileMapTypes[j, i]] + " ";
            }
  
            buffer += "\n";
            File.AppendAllText(filename, buffer);
        }

    }
    */
}
