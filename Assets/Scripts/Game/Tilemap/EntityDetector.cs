using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EntityDetector : MonoBehaviour
{


    private Tilemap tilemap;
    private Vector3Int lastPos = Vector3Int.zero;


    private void Start()
    {
        tilemap = GetComponent<Tilemap>();
    }


    
    
    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.tag != "Player")
            return;


        Vector2 entityPivot = new Vector2(collision.bounds.center.x, collision.bounds.min.y + collision.bounds.size.y/8);

        Vector3Int cellPosition = tilemap.WorldToCell(entityPivot);

        if (cellPosition != lastPos)
        {
            lastPos = cellPosition;
            TilemapManager.instance.setPlayerPos(cellPosition);
        }
           
    }

    

 
}
