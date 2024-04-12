using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathfindingTester : MonoBehaviour
{

    [SerializeField] private GameObject prefab;

    [SerializeField] private GameObject enemy;
    private Vector3Int enemyPos;
    private Vector3Int? targetPos;



    private Pathfinding pathfinder;
    private List<Node> path;

    // Start is called before the first frame update
    void Start()
    {
        enemyPos = TilemapManager.instance.enemyPos;
        pathfinder = new Pathfinding();
        targetPos = null;
      
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


        if (targetPos == null)
            return;

        Vector2Int eP = new Vector2Int(enemyPos.x, enemyPos.y);
        Vector2Int tP = new Vector2Int(targetPos.Value.x, targetPos.Value.y);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            path = pathfinder.getPathAStar(eP, tP);
            print("Target: " + tP.ToString());
            print("Path End: " + path[path.Count - 1].toVector2i().ToString());
        }


        if (path == null)
            return;

 
        for (int i = 1; i < path.Count; i++)
        {
            Debug.DrawLine(
                (Vector3)TilemapManager.instance.CellToWorld(path[i - 1].toVector2i()),
                (Vector3)TilemapManager.instance.CellToWorld(path[i].toVector2i()),
                Color.blue
            );
        }

        

       
    }

    public void changeTarget(Vector3Int targetPos)
    {
       // print(targetPos);
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
