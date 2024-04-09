using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathfindingTester : MonoBehaviour
{

    [SerializeField] private GameObject prefab;

    private Vector3Int enemyPos;
    private Vector3Int targetPos;

    // Start is called before the first frame update
    void Start()
    {
        enemyPos = TilemapManager.instance.enemyPos;
        targetPos = TilemapManager.instance.playerPos;

        BoundsInt bounds = TilemapManager.instance.maxBoundsData.Value;

        for (int row = 0; row < bounds.size.y; row++)
        {
            for (int col = 0; col < bounds.size.x; col++)
            {


                if (TilemapManager.instance.tileMapTypes[col, row] != TileType.Path)
                    continue;


                Vector2Int cell = TilemapManager.instance.IndexToCell(col, row);
                Vector2 worldPos = TilemapManager.instance.CellToWorld(cell);
            
                Instantiate(prefab, new Vector3(worldPos.x, worldPos.y, 0), Quaternion.identity, gameObject.transform);


            }

        }

    }

    // Update is called once per frame
    void Update()
    {
        enemyPos = TilemapManager.instance.enemyPos;


        Vector2 eP = TilemapManager.instance.CellToWorld(new Vector2Int(enemyPos.x, enemyPos.y));
        Vector2 tP = TilemapManager.instance.CellToWorld(new Vector2Int(targetPos.x, targetPos.y));

        Debug.DrawLine(tP, eP);
    }

    public void changeTarget(Vector3Int targetPos)
    {
        print(targetPos);
        this.targetPos = targetPos;
    }

    #region singleton
    public static PathfindingTester instance { get; private set; }
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else Destroy(gameObject);
    }
    #endregion
}
