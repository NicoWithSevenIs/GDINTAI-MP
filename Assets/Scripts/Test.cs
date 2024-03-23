using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Test : MonoBehaviour
{
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
                buffer += typeToStr[TilemapManager.instance.tileMapTypes[j, i]];
                if (j + 1 != x + 3)
                    buffer += " ";
            }
  
            buffer += "\n";
            File.AppendAllText(filename, buffer);
        }

        print(TilemapManager.instance.tileMapTypes[x-2, y]);
        print(TilemapManager.instance.tileMapTypes[x+2, y]);
        print(TilemapManager.instance.tileMapTypes[x, y-2]);
    }

}
