using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EntityDetector : MonoBehaviour
{


    private Vector3Int lastPlayerPos = Vector3Int.zero;
    private Vector3Int lastEnemyPos = Vector3Int.zero;
    private Tilemap tilemap;

    private void Start()
    {
        tilemap = GetComponent<Tilemap>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.tag != "Player" && collision.tag != "Enemy")
            return;


        Vector2 entityPivot = new Vector2(collision.bounds.center.x, collision.bounds.min.y + collision.bounds.size.y / 8);
        Vector3Int cellPosition = tilemap.WorldToCell(entityPivot);

        if (collision.tag == "Player" &&  cellPosition != lastPlayerPos)
        {
            lastPlayerPos = cellPosition;
            TilemapManager.instance.setPlayerPos(cellPosition);
        }else if(collision.tag == "Enemy" && cellPosition != lastEnemyPos)
        {
            lastEnemyPos = cellPosition;
            TilemapManager.instance.setEnemyPos(cellPosition);
        }

    }

    //Scalable Version

    /*
    [SerializeField] private GameObject[] trackedEntities;
    private Dictionary<GameObject, Vector3Int> lastSavedPosition;




    private void Start()
    {
        tilemap = GetComponent<Tilemap>();

        foreach(var e in trackedEntities)
        {
            lastSavedPosition[e] = Vector3Int.zero;
        }
    }


    private bool isEntityTracked(GameObject key)
    {
        foreach(var saved in lastSavedPosition)
        {
            if(saved.Key == key) 
                return true;
        }
        return false;
    }
    
    private void OnTriggerStay2D(Collider2D collision)
    {

        if (!isEntityTracked(collision.gameObject))
            return;


        Vector2 entityPivot = new Vector2(collision.bounds.center.x, collision.bounds.min.y + collision.bounds.size.y/8);
        Vector3Int cellPosition = tilemap.WorldToCell(entityPivot);

        if (cellPosition != lastSavedPosition[collision.gameObject])
        {
            lastSavedPosition[collision.gameObject] = cellPosition;

            //tilemap manager must have a set tracked entity dictionary method that takes in

            //TilemapManager.instance.setEntityPosition(collision.gameObject, cellPosition);
            
            //TilemapManager.instance.setPlayerPos(cellPosition);
        }
           
    }
    */



}
